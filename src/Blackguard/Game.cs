using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Blackguard.UI;
using Blackguard.UI.Menus;
using Blackguard.UI.Scenes;
using Blackguard.Utilities;
using Mindmagma.Curses;

namespace Blackguard;

public class Game {
    public static string PlayerPath { get; } = Path.Combine(Program.Platform.DataPath(), "Players");
    public static string WorldPath { get; } = Path.Combine(Program.Platform.DataPath(), "World");

    // These are set by their respective Selection Scenes
    public Player Player { get; set; } = null!;
    public World World { get; set; } = null!;

    public Window CurrentWin;

    private readonly List<Scene> scenes = new();
    private int sceneIdx = 0;
    private Scene CurrentScene => scenes[sceneIdx];
    private bool nextQueued = false;
    private bool backQueued = false;
    private readonly List<Menu> menus = new();

    public InputHandler Input { private set; get; }

    private (int, int) oldSize = (0, 0);
    public uint ticks = 0;

    private Stopwatch gameTimer = null!;
    public TimeSpan totalElapsedTime = TimeSpan.Zero;
    private TimeSpan accumulatedElapsedTime;
    private long previousTicks = 0;
    private readonly TimeSpan targetElapsedTime = TimeSpan.FromTicks(166667); // 60 fps. 1000/60 ms
    private const int PREVIOUS_SLEEP_TIME_COUNT = 128;
    private const int SLEEP_TIME_MASK = PREVIOUS_SLEEP_TIME_COUNT - 1;
    private readonly TimeSpan[] previousSleepTimes = new TimeSpan[PREVIOUS_SLEEP_TIME_COUNT];
    private int sleepTimeIndex = 0;
    private TimeSpan worstCaseSleepPrecision = TimeSpan.FromMilliseconds(1);
    private static readonly TimeSpan maxElapsedTime = TimeSpan.FromMilliseconds(500);

    public Game() {
        InitializeDirectories();
        Input = new InputHandler();
        CurrentWin = Window.NewFullScreenWindow("Base Window", Highlight.Text);
        scenes.Add(new MainMenuScene());
        oldSize = (NCurses.Lines, NCurses.Columns);
    }

    public static void InitializeDirectories() {
        Directory.CreateDirectory(PlayerPath);
        Directory.CreateDirectory(WorldPath);
    }

    public void Run() {
        bool shouldExit = false;

        while (!shouldExit) {
            gameTimer = Stopwatch.StartNew();

            // If input isn't checked, resizing doesn't work. Don't know why...
            Input.PollInput(CurrentWin.WHandle);

            // Only run anything if a resize is successful
            if (HandleResize()) {
                shouldExit = !CurrentScene.RunTick(this);
                CurrentScene.Render(this);

                foreach (Menu menu in menus) {
                    shouldExit = shouldExit && menu.RunTick(this);
                    menu.Render(this);
                }

                if (menus.Count > 0)
                    NCurses.UpdatePanels();

                MainInputHandler();

                // By switching to the scene at the end, we avoid memory leaks and crashes from killing the scene while it's active.
                SwitchToQueuedScene();
            }

            if (!shouldExit)
                Tick();

            ticks++;
        }

        // Exit the game
        CurrentScene.Finish();
    }

    private const int MIN_WIDTH = 100;
    private const int MIN_HEIGHT = 40;
    private readonly string SIZE_WARNING = $"Minimum screen size is {MIN_WIDTH} x {MIN_HEIGHT}";
    private bool HandleResize() {
        if ((NCurses.Lines, NCurses.Columns) != oldSize) {
            // TODO: Implement resize on a scene-by-scene, menu-by-menu basis in the event they want to control spacing and the like
            CurrentWin.HandleTermResize();
        }

        oldSize = (NCurses.Lines, NCurses.Columns);

        if (NCurses.Lines < MIN_HEIGHT || NCurses.Columns < MIN_WIDTH) {

            int starty = (NCurses.Lines / 2) - 3;

            string curWidth = "Width: ";
            string curHeight = $"Height: ";
            string width = NCurses.Columns.ToString();
            string height = NCurses.Lines.ToString();
            int startx = (NCurses.Columns - (curWidth.Length + curHeight.Length + width.Length + height.Length)) / 2;

            try {
                CurrentWin.AddLinesWithHighlight(
                    (Highlight.Text, (NCurses.Columns - SIZE_WARNING.Length) / 2, starty, SIZE_WARNING),
                    (Highlight.Text, startx, starty + 1, curWidth),
                    (Highlight.Text, startx += curWidth.Length, starty + 1, width), // Red or green eventually
                    (Highlight.Text, startx += width.Length + 1, starty + 1, curHeight),
                    (Highlight.Text, startx += curHeight.Length, starty + 1, height) // Red or green eventually
                );
            }
            catch { }

            return false;
        }

        return true;
    }

    // Handles input independent of any scenes (for things like the debug menu, etc).
    private void MainInputHandler() {
        if (Input.KeyPressed(CursesKey.KEY_F(6))) {
            Menu? debugMenu = menus.FirstOrDefault((m) => m?.Panel.Name == "Debug", null);

            if (debugMenu != null) {
                debugMenu.Panel.Clear();
                debugMenu.Delete();
                menus.Remove(debugMenu);
                NCurses.UpdatePanels();
            }
            else
                menus.Add(new DebugMenu());
        }
    }

    // Implementation for fixed step borrowed from FNA

    // Wait for the full frame interval to finish
    private void Tick() {
        AdvanceElapsedTime();

        while (accumulatedElapsedTime + worstCaseSleepPrecision < targetElapsedTime) {
            System.Threading.Thread.Sleep(1);
            UpdateEstimatedSleepPrecision(AdvanceElapsedTime());
        }

        while (accumulatedElapsedTime < targetElapsedTime) {
            System.Threading.Thread.SpinWait(1);
            AdvanceElapsedTime();
        }

        // Do not allow any update to take longer than our maximum.
        if (accumulatedElapsedTime > maxElapsedTime) {
            accumulatedElapsedTime = maxElapsedTime;
        }

        totalElapsedTime += targetElapsedTime;
    }

    private TimeSpan AdvanceElapsedTime() {
        long currentTicks = gameTimer.Elapsed.Ticks;
        TimeSpan timeAdvanced = TimeSpan.FromTicks(currentTicks - previousTicks);
        accumulatedElapsedTime += timeAdvanced;
        previousTicks = currentTicks;
        return timeAdvanced;
    }

    /* To calculate the sleep precision of the OS, we take the worst case
     * time spent sleeping over the results of previous requests to sleep 1ms.
     */
    private void UpdateEstimatedSleepPrecision(TimeSpan timeSpentSleeping) {
        /* It is unlikely that the scheduler will actually be more imprecise than
         * 4ms and we don't want to get wrecked by a single long sleep so we cap this
         * value at 4ms for sanity.
         */
        TimeSpan upperTimeBound = TimeSpan.FromMilliseconds(4);

        if (timeSpentSleeping > upperTimeBound)
            timeSpentSleeping = upperTimeBound;

        /* We know the previous worst case - it's saved in worstCaseSleepPrecision.
         * We also know the current index. So the only way the worst case changes
         * is if we either 1) just got a new worst case, or 2) the worst case was
         * the oldest entry on the list.
         */
        if (timeSpentSleeping >= worstCaseSleepPrecision)
            worstCaseSleepPrecision = timeSpentSleeping;

        else if (previousSleepTimes[sleepTimeIndex] == worstCaseSleepPrecision) {
            TimeSpan maxSleepTime = TimeSpan.MinValue;
            for (int i = 0; i < previousSleepTimes.Length; i += 1)
                if (previousSleepTimes[i] > maxSleepTime)
                    maxSleepTime = previousSleepTimes[i];

            worstCaseSleepPrecision = maxSleepTime;
        }

        previousSleepTimes[sleepTimeIndex] = timeSpentSleeping;
        sleepTimeIndex = (sleepTimeIndex + 1) & SLEEP_TIME_MASK;
    }

    public void ForwardScene<T>(Action<object?>? callback = null) where T : Scene, new() {
        if (backQueued) // Make sure there isn't a back and a forward at the same time
            return;

        nextQueued = true;

        if (sceneIdx < scenes.Count - 1 && scenes[sceneIdx + 1].GetType() == typeof(T)) {
            scenes[sceneIdx + 1].callback = callback;
            return;
        }

        // Initalize the next scene if there isn't already one
        Scene nextScene = new T();

        for (int i = sceneIdx + 1; i < scenes.Count; i++) {
            scenes[i].Finish();
            scenes.RemoveAt(i);
        }

        nextScene.callback = callback;
        scenes.Add(nextScene);
        return;
    }

    public void PrevScene() {
        if (nextQueued) // Make sure there isn't a back and a forward at the same time
            return;

        if (sceneIdx > 0)
            backQueued = true;
        else
            backQueued = false;
    }

    private void SwitchToQueuedScene() {
        int changeIdx;

        if (nextQueued) {
            changeIdx = sceneIdx + 1;
            nextQueued = false;
        }
        else if (backQueued) {
            changeIdx = sceneIdx - 1;
            backQueued = false;
        }
        else
            return;

        sceneIdx = changeIdx;
        CurrentWin.Clear();
    }

    public class InputHandler() {
        private readonly List<int> keys = new();

        public void PollInput(nint windowHandle) {
            keys.Clear();

            int c;
            try {
                while ((c = NCurses.WindowGetChar(windowHandle)) != -1)
                    keys.Add(c);
            }
            catch { } // Empty catch block because WindowGetChar throws if there is not a currently pressed key
        }

        public IEnumerable<string> Keynames() => keys.Select((k) => NCurses.Keyname(k));

        public IEnumerable<int> Keycodes() => keys;

        public bool HasInputThisTick() => keys.Count > 0;

        public bool KeyPressed(int keyCode) => keys.Contains(keyCode);

        public bool IsEnterPressed() => keys.Contains(CursesKey.ENTER) || keys.Contains(10) || keys.Contains(13);
    }
}

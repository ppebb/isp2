using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Blackguard.UI.Menus;
using Blackguard.UI.Scenes;
using Mindmagma.Curses;

namespace Blackguard;

public class Game {
    private static Scene scene = null!;
    private static Scene queuedScene = null!;
    private static readonly List<Menu> menus = new();

    private (int, int) oldSize = (0, 0);

    private Stopwatch gameTimer = null!;
    private TimeSpan totalElapsedTime = TimeSpan.Zero;
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
        scene = new MainMenuScene();
        oldSize = (NCurses.Lines, NCurses.Columns);
    }

    public void Run() {
        bool shouldExit = false;

        while (!shouldExit) {
            gameTimer = Stopwatch.StartNew();

            InputHandler.PollInput(scene.CurrentWin.handle);

            if ((NCurses.Lines, NCurses.Columns) != oldSize)
                scene.CurrentWin.HandleTermResize();

            shouldExit = !scene.RunTick();
            scene.Render();

            foreach (Menu menu in menus) {
                shouldExit = shouldExit && menu.RunTick();
                menu.Render();
            }

            SceneOmnipresentInputHandler();

            if (!shouldExit)
                Tick();

            // By switching to the scene at the end, we avoid memory leaks and crashes from killing the scene while it's active.
            SwitchToQueuedScene();

            oldSize = (NCurses.Lines, NCurses.Columns);
        }

        // Exit the game
        scene.Finish();
    }

    // Handles input independent of any scenes (for things like the debug menu, etc). I thought naming it this would be funny
    private static void SceneOmnipresentInputHandler() {
        if (InputHandler.KeyPressed(CursesKey.KEY_F(6))) {
            Menu? debugMenu = menus.FirstOrDefault((m) => m?.Panel.Name == "Debug", null);

            if (debugMenu != null) {
                debugMenu.Delete();
                menus.Remove(debugMenu);
            }
            else
                menus.Add(new DebugMenu());

            NCurses.UpdatePanels();
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

    public static void SwitchScene(Scene nextScene) {
        queuedScene = nextScene;
    }

    private static void SwitchToQueuedScene() {
        if (queuedScene == null)
            return;

        scene?.Finish();
        scene = queuedScene;
    }
}

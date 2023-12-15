using System;
using System.Collections.Generic;
using System.Diagnostics;
using Blackguard.UI;
using Mindmagma.Curses;

namespace Blackguard;

public class Game {
    private static Scene scene;

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
        // TODO: Mock console setup so we can test ncurses output
#if !DEBUG
        if (!NCurses.HasColors() || !NCurses.CanChangeColor()) {
            NCurses.AddString("Your console does not support true-color, please enable it before running Blackguard");
            NCurses.EndWin();
            Environment.Exit(1);
        }

        NCurses.NoEcho();

        scene = new MainMenuScene();
#endif
    }

    private static readonly List<int> input = new();

    private void PollInput() {
        input.Clear();
        int c;
        try {
            while ((c = NCurses.WindowGetChar(scene.CurrentWin)) != -1)
                input.Add(c);
        }
        catch { } // Empty catch block because WindowGetChar throws if there is not a currently pressed key
    }

    public static bool KeyPressed(int keyCode) => input?.Contains(keyCode) ?? false;

    public void Run() {
        bool shouldExit = false;

        while (!shouldExit) {
            gameTimer = Stopwatch.StartNew();

            PollInput();

            shouldExit = !scene.RunTick();
            scene.Render();

            if (!shouldExit)
                Tick();
        }

        // Exit the game
        scene.Finish();
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

    public void SwitchScene(Scene nextScene) {
        scene?.Finish();

        scene = nextScene;
    }
}

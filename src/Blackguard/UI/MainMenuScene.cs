using Blackguard.Utils;
using Mindmagma.Curses;

namespace Blackguard.UI;

public class MainMenuScene : Scene {
    public override nint CurrentWin { get; protected set; }

    public MainMenuScene() {
        CurrentWin = CursesUtils.FullScreenWindow();
    }

    int tick = 0;
    public override bool RunTick() {
        tick++;
        return true;
    }

    public override void Render() {
        if (tick % 60 == 0)
            NCurses.MoveWindowAddString(CurrentWin, 0, 0, $"ticks {tick}, seconds {tick / 60}");

        NCurses.WindowRefresh(CurrentWin);
    }

    public override void Finish() {
        NCurses.DeleteWindow(CurrentWin);
    }
}

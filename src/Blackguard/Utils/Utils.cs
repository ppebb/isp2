using Mindmagma.Curses;

namespace Blackguard.Utils;

public static class CursesUtils {
    // Full screen windows are used rather than applying actions to stdScreen because mixing stdScreen and smaller windows will create undefined behavior
    public static nint FullScreenWindow() {
        nint window = NCurses.NewWindow(NCurses.Lines, NCurses.Columns, 0, 0);
        NCurses.NoDelay(window, true);
        return window;
    }
}

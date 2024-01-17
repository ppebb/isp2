using Mindmagma.Curses;

namespace Blackguard.Utilities;

public static class CursesUtils {
    // Full screen windows are used rather than applying actions to stdScreen because mixing stdScreen and smaller windows will create undefined behavior
    public static nint FullScreenWindow() {
        nint window = NCurses.NewWindow(NCurses.Lines, NCurses.Columns, 0, 0);
        NCurses.NoDelay(window, true);
        return window;
    }

    public static void WindowAddLines(nint window, string[] lines) {
        for (int i = 0; i < lines.Length; i++) {
            NCurses.WindowAddString(window, lines[i]);
        }
    }
}

public static class Utils {
    public static string[] CenterText(string[] text, int width) {
        string[] ret = new string[text.Length];
        for (int i = 0; i < text.Length; i++) {
            string padding = new string(' ', (width - text[i].Length) / 2);
            ret[i] = padding + text[i] + padding;
        }

        return ret;
    }
}

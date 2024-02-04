using Mindmagma.Curses;

namespace Blackguard.Utilities;

public static class CursesUtils {
    public static void WindowAddLinesWithHighlight(nint window, params (Highlight highlight, int x, int y, string text)[] segments) {
        foreach ((Highlight highlight, int x, int y, string text) in segments) {
            NCurses.MoveWindowAddString(window, y, x, text);
            Mvwchgat(window, x, y, text.Length, highlight.GetAttr(), (short)highlight.GetPair());
        }
    }

    public static void WindowAddLines(nint window, string[] lines) {
        for (int i = 0; i < lines.Length; i++) {
            NCurses.WindowAddString(window, lines[i]);
        }
    }

    public static void Mvwchgat(nint window, int x, int y, int len, uint attr, short pair) {
        NCurses.WindowMove(window, x, y);
        NCurses.WindowChangeAttribute(window,  len, attr, pair, nint.Zero);
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

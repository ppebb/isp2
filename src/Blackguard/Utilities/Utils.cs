using Mindmagma.Curses;

namespace Blackguard.Utilities;

public static class CursesUtils {
    public static void WindowAddLinesWithHighlight(nint window, params (Highlight highlight, int x, int y, string text)[] segments) {
        foreach ((Highlight highlight, int x, int y, string text) in segments) {
            NCurses.MoveWindowAddString(window, y, x, text);
            Mvwchgat(window, x, y, text.Length, highlight.GetAttr(), (short)highlight.GetPair());
        }
    }

    public static void Mvwchgat(nint window, int x, int y, int len, uint attr, short pair) {
        NCurses.WindowMove(window, y, x);
        NCurses.WindowChangeAttribute(window,  len, attr, pair, nint.Zero);
    }
}

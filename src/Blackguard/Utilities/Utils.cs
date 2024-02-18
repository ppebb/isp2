using Mindmagma.Curses;

namespace Blackguard.Utilities;

public static class CursesUtils {
    public static void WindowAddLinesWithHighlight(nint window, params (Highlight highlight, int x, int y, string text)[] segments) {
        foreach ((Highlight highlight, int x, int y, string text) in segments) {
            NCurses.MoveWindowAddString(window, y, x, text);
            mvwchgat(window, x, y, text.Length, highlight.GetAttr(), (short)highlight.GetPair());
        }
    }

#pragma warning disable IDE1006 // Shut up naming violation
    private static void mvwchgat(nint window, int x, int y, int len, uint attr, short pair) {
        NCurses.WindowMove(window, y, x);
        NCurses.WindowChangeAttribute(window, len, attr, pair, nint.Zero);
    }
}
#pragma warning restore IDE1006

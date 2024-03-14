using System;
using System.Collections.Generic;
using System.Text;
using Mindmagma.Curses;

namespace Blackguard.Utilities;

public static class Utils {
    public static void WindowAddLinesWithHighlight(nint window, params (Highlight highlight, int x, int y, string text)[] segments) {
        foreach ((Highlight highlight, int x, int y, string text) in segments) {
            if (x < 0 || y < 0 || y > NCurses.Lines || x + text.Length > NCurses.Columns)
                throw new Exception($"Attempted to draw out of bounds! The window is {NCurses.Columns}x{NCurses.Lines}, but a line was printed at {x}x{y}, ending at {x + text.Length}");

            try {
                NCurses.MoveWindowAddString(window, y, x, text);
            }
            catch { } // If you try to draw to the bottom right corner of the window with scrollok() off, it throws. A check to only catch when this occurs would be better than catching *everything*, but I don't care
            mvwchgat(window, x, y, text.Length, highlight.GetAttr(), highlight.GetPair());
        }
    }

#pragma warning disable IDE1006 // Shut up naming violation
    private static void mvwchgat(nint window, int x, int y, int len, uint attr, short pair) {
        NCurses.WindowMove(window, y, x);
        NCurses.WindowChangeAttribute(window, len, attr, pair, nint.Zero);
    }
#pragma warning restore IDE1006

    private static readonly string[] largeTextGlyphs = [
        " ▄█▄ █▀▀▀▄▄▀▀▀▄█▀▀▄ █▀▀▀▀█▀▀▀▀▄▀▀▀▄█   █▀▀█▀▀  ▀█▀█  ▄▀█    █   ███  █▄▀▀▀▄█▀▀▀▄▄▀▀▀▄█▀▀▄ ▄▀▀▀▄▀▀█▀▀█   ██   ██   ██   ██   █▀▀▀▀█▄▀▀▀▄ ▄█  ▄▀▀▀▄▄▀▀▀▄  ▄█ █▀▀▀▀▄▀▀▀▄▀▀▀▀█▄▀▀▀▄▄▀▀▀▄      ▄▄  ",
        " █ █ █▄▄▄▀█    █   ██▄▄▄ █▄▄▄ █    █▄▄▄█  █     █ █▄▀  █    ██ ███ █ ██   ██▄▄▄▀█   ██▄▄▀ ▀▄▄▄   █  █   █▀▄ ▄▀█ ▄ █ ▀▄▀  ▀▄▀   ▄▀ █▀▄ █  █     ▄▀  ▄▄▀▄▀ █ ▀▄▄▄ █▄▄▄    █ ▀▄▄▄▀▀▄▄▄█      ▀▀  ",
        "█▀▀▀██   ██   ▄█  ▄▀█    █    █  ▀██   █  █  ▄  █ █ ▀▄ █    █ █ ██ ▀▄██   ██    █ ▀▄▀█  █ ▄   █  █  █   █ █ █ █ █ █▄▀ ▀▄  █  ▄▀   █  ▀█  █  ▄▀▀  ▄   █▀▀▀█▀▄   ██   █  █  █   █▄   █      ▄▄  ",
        "▀   ▀▀▀▀▀  ▀▀▀ ▀▀▀  ▀▀▀▀▀▀     ▀▀▀ ▀   ▀▀▀▀▀▀ ▀▀  ▀   ▀▀▀▀▀▀▀ ▀ ▀▀  ▀▀ ▀▀▀ ▀     ▀▀ ▀▀   ▀ ▀▀▀   ▀   ▀▀▀   ▀   ▀ ▀ ▀   ▀  ▀  ▀▀▀▀▀ ▀▀▀ ▀▀▀▀▀▀▀▀▀▀ ▀▀▀    ▀  ▀▀▀  ▀▀▀  ▀    ▀▀▀  ▀▀▀       ▀▀  "
    ];

    // WARN: Terrible hardcode. Works well enough.
    public static string[] ToLargeText(this string str) {
        List<ReadOnlyMemory<char>[]> glyphs = new();

        void AddGlyph(int start) {
            glyphs.Add([
                largeTextGlyphs[0].AsMemory(start, 5),
                largeTextGlyphs[1].AsMemory(start, 5),
                largeTextGlyphs[2].AsMemory(start, 5),
                largeTextGlyphs[3].AsMemory(start, 5)
            ]);
        }

        foreach (char c in str.ToLower()) {
            if (c >= 48 && c <= 57) // Numbers 0-9
                AddGlyph((c - 22) * 5);
            else if (c >= 97 && c <= 127) // a-z
                AddGlyph((c - 97) * 5);
            else if (c == 32) // space
                AddGlyph(36 * 5);
            else if (c == 58) // colon
                AddGlyph(37 * 5);
        }

        // This should work but doesn't. Would be nice to make this method somewhat less ugly.
        /* StringBuilder[] ret = [ */
        /*     new StringBuilder().AppendJoin(' ', glyphs[0]), */
        /*     new StringBuilder().AppendJoin(' ', glyphs[1]), */
        /*     new StringBuilder().AppendJoin(' ', glyphs[2]), */
        /*     new StringBuilder().AppendJoin(' ', glyphs[3]), */
        /* ]; */

        StringBuilder[] ret = [new(), new(), new(), new()];

        string format = "{0}";
        bool set = false;
        foreach (ReadOnlyMemory<char>[] mem in glyphs) {
            ret[0].AppendFormat(format, mem[0]);
            ret[1].AppendFormat(format, mem[1]);
            ret[2].AppendFormat(format, mem[2]);
            ret[3].AppendFormat(format, mem[3]);

            // Kind of hacky. Oh well
            if (!set) {
                format = " {0}";
                set = true;
            }
        }

        return [
            ret[0].ToString(),
            ret[1].ToString(),
            ret[2].ToString(),
            ret[3].ToString()
        ];
    }

    public static string[] Wrap(this string str, int lines) {
        throw new NotImplementedException();
        /* string[] ret = new string[lines]; */

        /* string[] split = str.Split(' '); */
    }
}

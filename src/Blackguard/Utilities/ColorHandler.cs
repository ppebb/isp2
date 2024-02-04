using System.Collections.Generic;
using Mindmagma.Curses;

namespace Blackguard.Utilities;

public enum Color : short {
    TextFg = 8,
    TextBg,
}

public enum ColorPair {
    Text = 1,
}

public enum Highlight {
    Text,
    TextSel,
}

public static class ColorHandler {
    // Color definitions, aligned with the Colors enum, so the 0th element is the RGB set for Text
    public static readonly short[][] ColorDefs = [
        [ 255, 255, 255 ], // TextFg
        [ 0,   0,   0   ], // TextBg
    ];

    public static readonly Color[][] ColorPairDefs = [
        [ Color.TextFg, Color.TextBg ] // It would be nice if it didn't need to specify the Color enum
    ];

    public static readonly Dictionary<Highlight, (ColorPair pair, uint attr)> HighlightDefs = new() {
        { Highlight.Text, (ColorPair.Text, 0) },
        { Highlight.TextSel, (ColorPair.Text, CursesAttribute.UNDERLINE) }
    };

    public static ColorPair GetPair(this Highlight highlight) => HighlightDefs[highlight].pair;

    public static uint GetAttr(this Highlight highlight) => HighlightDefs[highlight].attr;

    public static void Init() {
        for (short i = 0; i < ColorDefs.Length; i++) {
            NCurses.InitColor((short)(i + 8), ColorDefs[i][0], ColorDefs[i][1], ColorDefs[i][2]);
        }

        for (short i = 0; i < ColorPairDefs.Length; i++) {
            NCurses.InitPair((short)(i + 1), (short)ColorPairDefs[i][0], (short)ColorPairDefs[i][1]);
        }
    }
}

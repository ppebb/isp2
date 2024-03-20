using System.Collections.Generic;
using Mindmagma.Curses;

namespace Blackguard.Utilities;

public enum Color : short {
    TextFg = 8,
    TextBg,
    TextBright,
    TextQuiet,
    TextQuieter,
    TextQuietest,

    White,
    Gray90,
    Gray80,
    Gray70,
    Gray60,
    Gray50,
    Gray40,
    Gray30,
    Gray20,
    Gray10,
    Black,

    Red,
    RedOrange,
    Orange,
    Yellow,
    Yuck,
    Green,
    Teal,
    Blue,
    DeepBlue,
    Purple,
    Magenta,
    Pink,

    LightRed,
    LightRedOrange,
    LightOrange,
    LightYellow,
    LightYuck,
    LightGreen,
    LightTeal,
    LightBlue,
    LightDeepBlue,
    LightPurple,
    LightMagenta,
    LightPink,

    DarkRed,
    DarkRorange,
    DarkOrange,
    DarkYellow,
    DarkYuck,
    DarkGreen,
    DarkTeal,
    DarkBlue,
    DarkDeepBlue,
    DarkPurple,
    DarkMagenta,
    DarkPink,
}

public enum ColorPair : uint {
    Text = 1,
    TextSel,
    Warning,
    Error,
    Grass,
    Dirt,
    Stone,
}

public enum Highlight {
    Text,
    TextSel,
    TextSelUnderline,
    TextWarning,
    TextError,
    Grass,
    Dirt,
    Stone,
}

public static class ColorHandler {
    // Color definitions, aligned with the Colors enum, so the 0th element is the RGB set for Text
    public static readonly short[][] ColorDefs = [
        [ 209, 215, 227 ],  // TextNormal
        [ 38,  40,  48  ],  // BackgroundMenu

        [ 242, 247, 255 ],  // TextBright
        [ 149, 154, 164 ],  // TextQuiet
        [ 106, 110, 119 ],  // TextQuieter
        [ 63,  66,  74  ],  // TextQuietest

        [ 255, 255, 255 ],  // White
        [ 230, 230, 230 ],  // Gray90
        [ 204, 204, 204 ],  // Gray80
        [ 179, 179, 179 ],  // Gray70
        [ 153, 153, 153 ],  // Gray60
        [ 128, 128, 128 ],  // Gray50
        [ 102, 102, 102 ],  // Gray40
        [ 77,  77,  77  ],  // Gray30
        [ 51,  51,  51  ],  // Gray20
        [ 26,  26,  26  ],  // Gray10
        [ 0,   0,   0   ],  // Black

        [ 242, 85,  85  ],  // Red
        [ 242, 119, 85  ],  // RedOrange
        [ 242, 143, 85  ],  // Orange
        [ 242, 205, 85  ],  // Yellow
        [ 190, 242, 85  ],  // Yuck
        [ 127, 242, 85  ],  // Green
        [ 85,  242, 199 ],  // Teal
        [ 85,  192, 242 ],  // Blue
        [ 85,  109, 242 ],  // DeepBlue
        [ 167, 85,  242 ],  // Purple
        [ 234, 85,  242 ],  // Magenta
        [ 242, 85,  163 ],  // Pink

        [ 255, 153, 153 ],  // LightRed
        [ 255, 175, 153 ],  // LightRedOrange
        [ 255, 191, 153 ],  // LightOrange
        [ 255, 231, 153 ],  // LightYellow
        [ 221, 255, 153 ],  // LightYuck
        [ 180, 255, 153 ],  // LightGreen
        [ 153, 255, 227 ],  // LightTeal
        [ 153, 222, 255 ],  // LightBlue
        [ 153, 168, 255 ],  // LightDeepBlue
        [ 206, 153, 255 ],  // LightPurple
        [ 250, 153, 255 ],  // LightMagenta
        [ 255, 153, 203 ],  // LightPink

        [ 153, 31,  31  ],  // DarkRed
        [ 153, 57,  31  ],  // DarkRorange
        [ 153, 76,  31  ],  // DarkOrange
        [ 153, 124, 31  ],  // DarkYellow
        [ 113, 153, 31  ],  // DarkYuck
        [ 63,  153, 31  ],  // DarkGreen
        [ 31,  153, 120 ],  // DarkTeal
        [ 31,  114, 153 ],  // DarkBlue
        [ 31,  49,  153 ],  // DarkDeepBlue
        [ 95,  31,  153 ],  // DarkPurple
        [ 147, 31,  153 ],  // DarkMagenta
        [ 153, 31,  91  ],  // DarkPink
    ];

    public static readonly Color[][] ColorPairDefs = [
        [ Color.TextFg,     Color.TextBg     ], // Text
        [ Color.TextBg,     Color.TextFg     ], // TextSel
        [ Color.Yellow,     Color.TextBg     ], // Error
        [ Color.Red,        Color.TextBg     ], // Warning
        [ Color.LightGreen, Color.LightGreen ], // Grass
        [ Color.DarkRed,    Color.DarkRed    ], // Dirt
        [ Color.Gray50,     Color.Gray50     ]  // Stone
    ];

    public static readonly Dictionary<Highlight, (ColorPair pair, uint attr)> HighlightDefs = new() {
        { Highlight.Text,             (ColorPair.Text,    0) },
        { Highlight.TextSel,          (ColorPair.TextSel, 0) },
        { Highlight.TextSelUnderline, (ColorPair.TextSel, CursesAttribute.UNDERLINE) },
        { Highlight.TextWarning,      (ColorPair.Warning, 0)},
        { Highlight.TextError,        (ColorPair.Error,   0)},
        { Highlight.Grass,            (ColorPair.Grass,   0)},
        { Highlight.Dirt,             (ColorPair.Dirt,    0)},
        { Highlight.Stone,            (ColorPair.Stone,   0)}
    };

    // Gets the pair number
    public static short GetPair(this Highlight highlight) => (short)HighlightDefs[highlight].pair;

    // Gets the pair attr
    public static uint GetPairAttr(this Highlight highlight) => NCurses.ColorPair(highlight.GetPair());

    // Gets other attrs (underline, bold, etc)
    public static uint GetAttr(this Highlight highlight) => HighlightDefs[highlight].attr;

    // Combines the color pair attr and the other attrs (underline, bold, etc) into one single uint used by some functions
    public static uint AsMixedAttr(this Highlight highlight) => highlight.GetPairAttr() | highlight.GetAttr();

    public static void Init() {
        for (short i = 0; i < ColorDefs.Length; i++) {
            NCurses.InitColor((short)(i + 8), ColorDefs[i][0], ColorDefs[i][1], ColorDefs[i][2]);
        }

        for (short i = 0; i < ColorPairDefs.Length; i++) {
            NCurses.InitPair((short)(i + 1), (short)ColorPairDefs[i][0], (short)ColorPairDefs[i][1]);
        }
    }
}

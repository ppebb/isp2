using System.Collections.Generic;
using Mindmagma.Curses;

namespace Blackguard.Utilities;

public enum Color : short {
    TextFg = 8,
    TextBg,
}

public enum ColorPair : uint {
    Text = 1,
    TextSel,
}

public enum Highlight {
    Text,
    TextSel,
    TextSelUnderline,
}

public static class ColorHandler {
    // Color definitions, aligned with the Colors enum, so the 0th element is the RGB set for Text
    public static readonly short[][] ColorDefs = [
        [ 209, 215, 227 ],  // textNormal
        [  38,  40,  48 ],  // backgroundMenu

        [ 242, 247, 255 ],  // textBright
        [ 149, 154, 164 ],  // textQuiet
        [ 106, 110, 119 ],  // textQuieter
        [  63,  66,  74 ],  // textQuietest

        [ 255, 255, 255 ],  // white
        [ 230, 230, 230 ],  // gray90
        [ 204, 204, 204 ],  // gray80
        [ 179, 179, 179 ],  // gray70
        [ 153, 153, 153 ],  // gray60
        [ 128, 128, 128 ],  // gray50
        [ 102, 102, 102 ],  // gray40
        [  77,  77,  77 ],  // gray30
        [  51,  51,  51 ],  // gray20
        [  26,  26,  26 ],  // gray10
        [   0,   0,   0 ],  // black

        [ 242, 85,   85 ],  // red
        [ 242, 119,  85 ],  // rorange
        [ 242, 143,  85 ],  // orange
        [ 242, 205,  85 ],  // yellow
        [ 190, 242,  85 ],  // yuck
        [ 127, 242,  85 ],  // green
        [  85, 242, 199 ],  // teal
        [  85, 192, 242 ],  // blue
        [  85, 109, 242 ],  // deepBlue
        [ 167,  85, 242 ],  // purple
        [ 234,  85, 242 ],  // magenta
        [ 242,  85, 163 ],  // pink

        [ 255, 153, 153 ],  // lightRed
        [ 255, 175, 153 ],  // lightRorange
        [ 255, 191, 153 ],  // lightOrange
        [ 255, 231, 153 ],  // lightYellow
        [ 221, 255, 153 ],  // lightYuck
        [ 180, 255, 153 ],  // lightGreen
        [ 153, 255, 227 ],  // lightTeal
        [ 153, 222, 255 ],  // lightBlue
        [ 153, 168, 255 ],  // lightDeepBlue
        [ 206, 153, 255 ],  // lightPurple
        [ 250, 153, 255 ],  // lightMagenta
        [ 255, 153, 203 ],  // lightPink

        [ 153,  31,  31 ],  // darkRed
        [ 153,  57,  31 ],  // darkRorange
        [ 153,  76,  31 ],  // darkOrange
        [ 153, 124,  31 ],  // darkYellow
        [ 113, 153,  31 ],  // darkYuck
        [  63, 153,  31 ],  // darkGreen
        [  31, 153, 120 ],  // darkTeal
        [  31, 114, 153 ],  // darkBlue
        [  31,  49, 153 ],  // darkDeepBlue
        [  95,  31, 153 ],  // darkPurple
        [ 147,  31, 153 ],  // darkMagenta
        [ 153,  31,  91 ],  // darkPink
    ];

    public static readonly Color[][] ColorPairDefs = [
        [ Color.TextFg, Color.TextBg ], // It would be nice if it didn't need to specify the Color enum
        [ Color.TextBg, Color.TextFg ]
    ];

    public static readonly Dictionary<Highlight, (ColorPair pair, uint attr)> HighlightDefs = new() {
        { Highlight.Text,             (ColorPair.Text,    0) },
        { Highlight.TextSel,          (ColorPair.TextSel, 0) },
        { Highlight.TextSelUnderline, (ColorPair.TextSel, CursesAttribute.UNDERLINE) },
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

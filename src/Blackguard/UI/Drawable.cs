using System;
using Blackguard.Utilities;
using static Blackguard.UI.CharacterDefs;
using static Blackguard.Utilities.Utils;
using Mindmagma.Curses;
using System.Runtime.CompilerServices;
using System.Text;

namespace Blackguard.UI;

// Interface wrapping a handle to something NCurses can draw to
public abstract class Drawable : IDisposable, ISizeProvider, IOffsetProvider {
    public virtual nint WHandle { get; protected set; } // Window handle
    public virtual nint Handle { get; protected set; } // Handle to the actual thing (pad, panel, etc)
    public string Name { get; set; } = null!;

    private Highlight _highlight;
    public virtual Highlight Highlight {
        get => _highlight;
        set {
            _highlight = value;
            ChangeHighlight(_highlight);
        }
    }

#pragma warning disable IDE1006 // I want lowercase names. Shut up roslyn
    public int x { get; protected set; } // X-pos
    public int y { get; protected set; } // Y-pos
    public int w { get; protected set; } // Width
    public int h { get; protected set; } // Height
#pragma warning restore IDE1006

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddLineWithHighlight(Highlight highlight, int x, int y, string line) {
        ThrowIfOutOfBounds(x, y, 1, 1);

        try {
            NCurses.MoveWindowAddString(WHandle, y, x, line);
        }
        catch { } // If you try to draw to the bottom right corner of the window with scrollok() off, it throws. A check to only catch when this occurs would be better than catching *everything*, but I don't care
        mvwchgat(WHandle, x, y, line.Length, highlight.GetAttr(), highlight.GetPair());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddCharWithHighlight(Highlight highlight, int x, int y, char glyph) {
        ThrowIfOutOfBounds(x, y, 1, 1);

        try {
            NCurses.MoveWindowAddChar(WHandle, y, x, glyph);
        }
        catch { } // If you try to draw to the bottom right corner of the window with scrollok() off, it throws. A check to only catch when this occurs would be better than catching *everything*, but I don't care
        mvwchgat(WHandle, x, y, 1, highlight.GetAttr(), highlight.GetPair());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddLinesWithHighlight(params (Highlight highlight, int x, int y, string line)[] segments) {
        foreach ((Highlight highlight, int x, int y, string line) in segments)
            AddLineWithHighlight(highlight, x, y, line);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddCharsWithHighlight(params (Highlight highlight, int x, int y, char glyph)[] segments) {
        foreach ((Highlight highlight, int x, int y, char glyph) in segments)
            AddCharWithHighlight(highlight, x, y, glyph);
    }

    protected virtual void ChangeHighlight(Highlight newHighlight) {
        NCurses.WindowBackground(WHandle, newHighlight.AsMixedAttr());
    }

    public void Clear() {
        NCurses.ClearWindow(WHandle);
    }

    public abstract void Dispose();

    public void DrawBorder(Highlight highlight, int x = 0, int y = 0, int w = -1, int h = -1, int skipx = 0, int skipy = 0) {
        // This is good design I swear
        w = w == -1 ? this.w : w;
        h = h == -1 ? this.h : h;

        string ConstructHLine(char glyphL, char glyphH, char glyphR) {
            int cap = w - Math.Abs(skipx);
            StringBuilder b = new(cap);

            int hlen; // Length excluding corners
            if (skipx != 0) // If anything is being skipped, then one corner is not being drawn
                hlen = cap - 1;
            else
                hlen = cap - 2;

            if (skipx < 1)
                b.Append(glyphL);

            b.Append(new string(glyphH, hlen));

            if (skipx > -1)
                b.Append(glyphR);

            return b.ToString();
        }

        int startx = skipx > 0 ? x + skipx : x;
        if (skipy < 1)
            AddLineWithHighlight(highlight, startx, y, ConstructHLine(B_LCT, B_T, B_RCT));

        if (skipy > -1)
            AddLineWithHighlight(highlight, startx, y + h - 1, ConstructHLine(B_LCB, B_B, B_RCB));

        int startj = skipy > 0 ? skipy : 1;
        int endj = skipy < 0 ? h + skipy : h - 1;
        bool drawL = skipx < 1;
        bool drawR = skipx > -1;
        for (int j = startj; j < endj; j++) {
            if (drawL)
                AddLineWithHighlight(highlight, x, y + j, new string(B_L, 1));

            if (drawR)
                AddLineWithHighlight(highlight, x + w - 1, y + j, new string(B_R, 1));
        }

    }

    public (int x, int y) GetOffset() => (x, y);

    public (int w, int h) GetSize() => (w, h);

    public abstract void Move(int newx, int newy);

    public void ReapplyHighlight() {
        ChangeHighlight(Highlight);
    }

    public abstract void Resize(int neww, int newh);
}

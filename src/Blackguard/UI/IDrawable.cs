using System;
using Blackguard.Utilities;
using Mindmagma.Curses;

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

    public void AddLinesWithHighlight(params (Highlight highlight, int x, int y, string text)[] segments) {
        Utils.WindowAddLinesWithHighlight(WHandle, segments);
    }

    protected virtual void ChangeHighlight(Highlight newHighlight) {
        NCurses.WindowBackground(WHandle, newHighlight.AsMixedAttr());
    }

    public void Clear() {
        NCurses.ClearWindow(WHandle);
    }

    public abstract void Dispose();

    public (int x, int y) GetOffset() => (x, y);

    public (int w, int h) GetSize() => (w, h);

    public abstract void HandleTermResize();

    public abstract void Move(int newx, int newy);

    public abstract void Resize(int neww, int newh);
}

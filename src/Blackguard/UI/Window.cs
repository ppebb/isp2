using System;
using Blackguard.Utilities;
using Mindmagma.Curses;

namespace Blackguard.UI;

public class Window : Drawable {
    public override nint WHandle { // Window handle
        get => Handle;
        protected set { Handle = value; }
    }
    public override nint Handle { get; protected set; } // Handle to the actual thing (pad, panel, etc)

    public Window(string name, Highlight highlight, int xi, int yi, int wi, int hi) {
        Handle = NCurses.NewWindow(hi, wi, yi, xi);
        NCurses.NoDelay(Handle, true);
        NCurses.Keypad(Handle, true);
        ChangeHighlight(highlight);
        Name = name;
        x = xi;
        y = yi;
        w = wi;
        h = hi;
    }

    public override void Dispose() {
        NCurses.DeleteWindow(Handle);
        GC.SuppressFinalize(this);
    }

    public override void Move(int newx, int newy) {
        NCurses.MoveWindow(Handle, newy, newx);
        x = newx;
        y = newy;
    }

    public override void Resize(int neww, int newh) {
        NCurses.WindowResize(Handle, newh, neww);
        w = neww;
        h = newh;
    }
}

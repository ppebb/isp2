using Mindmagma.Curses;

namespace Blackguard.UI;

public class Window : ISizeProvider, IOffsetProvider {
    public nint handle;
    public string Name { private set; get; }
#pragma warning disable IDE1006 // I want lowercase names. Shut up roslyn
    public int x { private set; get; } // X-pos
    public int y { private set; get; } // Y-pos
    public int w { private set; get; } // Width
    public int h { private set; get; } // Height
#pragma warning restore IDE1006

    public Window(string name, int xi, int yi, int wi, int hi) {
        handle = NCurses.NewWindow(hi, wi, yi, xi);
        NCurses.NoDelay(handle, true);
        NCurses.Keypad(handle, true);
        Name = name;
        x = xi;
        y = yi;
        w = wi;
        h = hi;
    }

    public static Window NewFullScreenWindow(string name) {
        return new Window(name, 0, 0, NCurses.Columns, NCurses.Lines); ;
    }

    public void ChangeName(string newName) {
        Name = newName;
    }

    public void Move(int newx, int newy) {
        NCurses.MoveWindow(handle, newy, newx);
        x = newx;
        y = newy;
    }

    public void Resize(int neww, int newh) {
        NCurses.WindowResize(handle, newh, neww);
        w = neww;
        h = newh;
    }

    public virtual void HandleTermResize() {
        Resize(NCurses.Columns, NCurses.Lines);
        Clear();
    }

    public void Clear() {
        NCurses.ClearWindow(handle);
    }

    public void Delete() {
        NCurses.DeleteWindow(handle);
    }

    public (int w, int h) GetSize() {
        return (w, h);
    }

    public (int x, int y) GetOffset() {
        return (x, y);
    }
}

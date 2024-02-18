using Mindmagma.Curses;

namespace Blackguard.UI;

public class Panel : ISizeProvider, IOffsetProvider {
    public nint handle;
    private Window window;
    public nint WHandle => window.handle;
    public string Name { private set; get; }
#pragma warning disable IDE1006 // I want lowercase names. Shut up roslyn
    public int x { private set; get; } // X-pos
    public int y { private set; get; } // Y-pos
    public int w { private set; get; } // Width
    public int h { private set; get; } // Height
#pragma warning restore IDE1006

    public Panel(string name, int xi, int yi, int wi, int hi) {
        window = new Window(name, xi, yi, wi, hi);
        handle = NCurses.NewPanel(window.handle);
        Name = name;
        x = xi;
        y = yi;
        w = wi;
        h = hi;
    }

    // This will result in a dicongruency between the window x/y and the panel x/y, but this doesn't matter because nothing external should be accessing the window x/y
    public void Move(int newx, int newy) {
        NCurses.MovePanel(handle, newx, newy);
        x = newx;
        y = newy;
    }

    public void Delete() {
        NCurses.DeletePanel(handle);
        window.Delete();
    }

    public (int w, int h) GetSize() {
        return (w, h);
    }

    public (int x, int y) GetOffset() {
        return (x, y);
    }
}

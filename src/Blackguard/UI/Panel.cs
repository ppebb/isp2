using Blackguard.Utilities;
using Mindmagma.Curses;

namespace Blackguard.UI;

public class Panel : ISizeProvider, IOffsetProvider {
    public nint handle;
    private Window _window;
    public nint WHandle => _window.handle;
    public string Name { private set; get; }
#pragma warning disable IDE1006 // I want lowercase names. Shut up roslyn
    public int x { private set; get; } // X-pos
    public int y { private set; get; } // Y-pos
    public int w { private set; get; } // Width
    public int h { private set; get; } // Height
#pragma warning restore IDE1006

    private Highlight _backingBackground;
    public Highlight Background {
        get {
            return _backingBackground;
        }
        set {
            NCurses.WindowBackground(WHandle, value.GetAttr() | value.GetPairAttr());
            _backingBackground = value;
        }
    }

    public Panel(string name, Highlight background, int xi, int yi, int wi, int hi) {
        _window = new Window(name, xi, yi, wi, hi);
        Background = background;
        handle = NCurses.NewPanel(_window.handle);
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

    public void Clear() {
        _window.Clear();
    }

    public void Delete() {
        NCurses.DeletePanel(handle);
        _window.Delete();
    }

    public (int w, int h) GetSize() {
        return (w, h);
    }

    public (int x, int y) GetOffset() {
        return (x, y);
    }
}

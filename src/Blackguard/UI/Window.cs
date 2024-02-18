using Mindmagma.Curses;

namespace Blackguard.UI;

public class Window : ISizeProvider, IOffsetProvider {
    public nint handle;
    public int x; // X-pos
    public int y; // Y-pos
    public int w; // Width
    public int h; // Height

    public Window(int xi, int yi, int wi, int hi) {
        handle = NCurses.NewWindow(hi, wi, yi, xi);
        NCurses.NoDelay(handle, true);
        NCurses.Keypad(handle, true);
        x = xi;
        y = yi;
        w = wi;
        h = hi;
    }

    public static Window NewFullScreenWindow() {
        return new Window(0, 0, NCurses.Columns, NCurses.Lines); ;
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

    public void HandleTermResize() {
        Resize(NCurses.Columns, NCurses.Lines);
        Clear();
    }

    public void Clear() {
        NCurses.ClearWindow(handle);
    }

    public (int x, int y) GetSize() {
        return (x, y);
    }

    public (int x, int y) GetOffset() {
        return (x, y);
    }
}

using Mindmagma.Curses;

namespace Blackguard.UI;

public struct Window {
    public nint handle;
    public int x; // X-pos
    public int y; // Y-pos
    public int w; // Width
    public int h; // Height

    public Window(int xi, int yi, int wi, int hi) {
        handle = NCurses.NewWindow(hi, wi, yi, xi);
        NCurses.NoDelay(handle, true);
        x = xi;
        y = yi;
        w = wi;
        h = hi;
    }

    public void Move(int newx, int newy) {
        NCurses.MoveWindow(handle, newy, newx);
        x = newx;
        y = newy;
    }

    public void Resize(int neww, int newh) {
        throw new System.NotImplementedException();
    }

    public static Window NewFullScreenWindow() {
        return new Window(0, 0, NCurses.Columns, NCurses.Lines);;
    }
}

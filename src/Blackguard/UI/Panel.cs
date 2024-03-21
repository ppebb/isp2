using System;
using Blackguard.Utilities;
using Mindmagma.Curses;

namespace Blackguard.UI;

public class Panel : Drawable {
    private Window _window;

    public override nint WHandle {
        get => _window.Handle;
    }

    public Panel(string name, Highlight highlight, int xi, int yi, int wi, int hi) {
        _window = new Window(name, highlight, xi, yi, wi, hi);
        Handle = NCurses.NewPanel(_window.Handle);
        Name = name;
        x = xi;
        y = yi;
        w = wi;
        h = hi;
    }

    public static Panel NewFullScreenPanel(string name, Highlight highlight) {
        return new Panel(name, highlight, 0, 0, NCurses.Columns, NCurses.Lines);
    }

    public override void Dispose() {
        NCurses.DeletePanel(Handle);
        _window.Dispose();
        GC.SuppressFinalize(this);
    }

    public override void Move(int newx, int newy) {
        NCurses.MovePanel(Handle, newy, newx);
    }

    public override void Resize(int neww, int newh) {
        Window temp = new(_window.Name, _window.Highlight, x, y, neww, newh);
        NCurses.ReplacePanel(Handle, temp.WHandle);

        _window.Clear();
        _window.Dispose();

        _window = temp;

        w = neww;
        h = newh;
    }
}

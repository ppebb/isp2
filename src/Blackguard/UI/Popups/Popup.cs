using Blackguard.Utilities;
using Mindmagma.Curses;

namespace Blackguard.UI.Popups;

public abstract class Popup : ISizeProvider, IOffsetProvider {
    public Panel Panel { get; protected set; }

    public bool Focused;
    public bool Closed = false;

    public Popup(string name, Highlight background, int x, int y, int w, int h) {
        Panel = new Panel(name, background, x, y, w, h);
    }

    public Popup(string name, Highlight background, int w, int h) {
        Panel = new Panel(name, background, (NCurses.Columns - w) / 2, (NCurses.Lines - h) / 2, w, h);
    }

    public abstract bool RunTick(Game state);

    public abstract void Render(Game state);

    public virtual void Delete() {
        Panel.Dispose();
    }

    public (int w, int h) GetSize() {
        return (Panel.w, Panel.h);
    }

    public (int x, int y) GetOffset() {
        return (Panel.x, Panel.y);
    }
}

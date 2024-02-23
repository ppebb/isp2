using Blackguard.Utilities;
using static Blackguard.UI.CharacterDefs;

namespace Blackguard.UI.Menus;

public abstract class Menu : ISizeProvider, IOffsetProvider {
    public Panel Panel { get; protected set; }

    public Menu(string name, Highlight background, int x, int y, int w, int h) {
        Panel = new Panel(name, background, x, y, w, h);
    }

    public abstract bool RunTick(Game state);

    public abstract void Render(Game state);

    protected void RenderBorder(Highlight highlight) {
        Panel.AddLinesWithHighlight(
            (highlight, 0, 0, B_LCT + new string(B_T, Panel.w - 2) + B_RCT),
            (highlight, 0, Panel.h - 1, B_LCB + new string(B_B, Panel.w - 2) + B_RCB)
        );

        for (int i = 1; i < Panel.h - 1; i++) {
            Panel.AddLinesWithHighlight(
                (highlight, 0, i, new string(B_L, 1)),
                (highlight, Panel.w - 1, i, new string(B_R, 1))
            );
        }
    }

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

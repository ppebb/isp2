using Blackguard.Utilities;
using static Blackguard.UI.CharacterDefs;

namespace Blackguard.UI.Menus;

public abstract class Menu {
    public Panel Panel { get; protected set; }

    public Menu(string name, int x, int y, int w, int h) {
        Panel = new Panel(name, x, y, w, h);
    }

    public abstract bool RunTick();

    public abstract void Render();

    protected void RenderBorder(Highlight highlight) {
        CursesUtils.WindowAddLinesWithHighlight(Panel.WHandle, (Highlight.Text, 5, 5, "guh"));
        /* CursesUtils.WindowAddLinesWithHighlight(Window.handle, */
        /*     (highlight, 0, 0, B_LCT + new string(B_T, Window.w - 2) + B_RCT), */
        /*     (highlight, Window.w - 1, Window.h - 1, B_LCB + new string(B_B, Window.w - 2) + B_RCB) */
        /* ); */

        /* for (int i = 0; i < Window.h; i++) { */
        /*     CursesUtils.WindowAddLinesWithHighlight(Window.handle, */
        /*         (highlight, i, 0, new string(B_L, 1)), */
        /*         (highlight, i, Window.w - 1, new string(B_R, 1)) */
        /*     ); */
        /* } */
    }

    public virtual void Delete() {
        Panel.Delete();
    }
}

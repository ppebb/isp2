using Blackguard.Utilities;

namespace Blackguard.UI.Menus;

public class DebugMenu : Menu {
    public DebugMenu() : base("Debug", 2, 2, 30, 30) { }

    public override bool RunTick() => true;

    public override void Render() {
        RenderBorder(Highlight.Text);
    }
}

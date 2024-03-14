using System.Linq;
using Blackguard.UI.Elements;
using Blackguard.Utilities;
using Mindmagma.Curses;

namespace Blackguard.UI.Popups;

public enum InfoType {
    Info,
    Warning,
    Error
}

public class InfoPopup : Popup {
    private readonly UIContainer container;
    private readonly Highlight highlight;

    public InfoPopup(string name, InfoType type, string[] contents) : base(name, Highlight.Text, contents.Max(s => s.Length) + 2, contents.Length + 4) {
        highlight = type switch {
            InfoType.Info => Highlight.Text,
            InfoType.Warning => Highlight.TextWarning,
            InfoType.Error => Highlight.TextError,
            _ => Highlight.Text,
        };

        container = new(Alignment.Center) {
            Border = true,
            BorderSel = highlight,
            BorderUnsel = highlight,
        };

        container.Add(new UIText(contents) { Highlight = highlight });
        container.Add(new UISpace(0, 1));
        container.Add(new UIButton(["Ok"], (s) => { s.ClosePopup(this); }));

        container.Select();
        container.SelectFirstSelectable();
    }

    public override void Render(Game state) {
        container.Render(Panel, 0, 0, Panel.w, Panel.h);
    }

    public override bool RunTick(Game state) {
        if (state.Input.KeyPressed(CursesKey.DOWN))
            container.Next(true);

        if (state.Input.KeyPressed(CursesKey.UP))
            container.Prev(true);

        if (Focused)
            container.ProcessInput(state);

        return true;
    }
}

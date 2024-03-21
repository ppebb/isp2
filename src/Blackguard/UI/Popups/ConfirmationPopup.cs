using System;
using System.Linq;
using Blackguard.UI.Elements;
using Blackguard.Utilities;
using Mindmagma.Curses;

namespace Blackguard.UI.Popups;

public class ConfirmationPopup : Popup {
    private readonly UIContainer container;

    public Highlight TextHighlight = Highlight.Text;
    public Highlight Border = Highlight.Text;

    public ConfirmationPopup(string name, string[] contents, Action<Game>? cancel, Action<Game>? ok) : base(name, Highlight.Text, contents.Max(s => s.Length) + 2, contents.Length + 5) {
        container = new(Alignment.Center) {
            Border = true,
            BorderSel = Border,
            BorderUnsel = Border,
        };

        container.Add(new UIText(contents) { Highlight = TextHighlight });
        container.Add(new UISpace(0, 1));
        // TODO: Allow buttons to be in a subcontainer and then aligned horizontally
        container.Add(new UIButton(["Cancel"], (s) => {
            cancel?.Invoke(s);
            s.ClosePopup(this);
        }));
        container.Add(new UIButton(["Ok"], (s) => {
            ok?.Invoke(s);
            s.ClosePopup(this);
        }));

        container.Select();
        container.SelectFirstSelectable();
    }

    public override void HandleTermResize() => CenterPopup();

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

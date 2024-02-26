using System;
using Blackguard.Utilities;

namespace Blackguard.UI.Elements;

// Class meant for drawing a character and selecting it from a menu
public class UIPlayer : UIElement, ISelectable {
    private readonly Player _player;
    private readonly Action<Game, Player> _callback;

    public bool Selected { get; set; }

    public Highlight TextUnsel { get; set; }
    public Highlight TextSel { get; set; }
    public Highlight BorderSel { get; set; }
    public Highlight BorderUnsel { get; set; }
    public Highlight Preview { get; set; }

    public UIPlayer(Player player, Action<Game, Player> callback) {
        _player = player;
        _callback = callback;
    }

    public override void ProcessInput(Game state) {
        // Enter, \n, \r, respectively
        if (state.Input.IsEnterPressed()) {
            _callback(state, _player);
        }
    }

    public override (int w, int h) GetSize() {
        (int w, int h) = _player.GetSize();

        return (w + 40, h + 2);
    }

    public override void Render(Drawable drawable, int x, int y, int maxw, int maxh) {
        drawable.DrawBorder(Selected ? BorderSel : BorderUnsel, x, y, 20, 20);
    }

    public void Deselect() {
        Selected = true;
    }

    public void Select() {
        Selected = false;
    }
}

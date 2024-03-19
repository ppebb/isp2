using System;
using Blackguard.Utilities;

namespace Blackguard.UI.Elements;

// Class meant for drawing a character and selecting it from a menu
public class UIPlayer : UIElement, ISelectable, IComparable {
    public readonly Player Player;
    private readonly Action<Game, Player> _callback;

    public bool Selected { get; set; }

    public Highlight TextUnsel = Highlight.Text;
    public Highlight TextSel = Highlight.TextSel;
    public Highlight BorderSel = Highlight.TextSel;
    public Highlight BorderUnsel = Highlight.Text;

    public UIPlayer(Player player, Action<Game, Player> callback) {
        Player = player;
        _callback = callback;
    }

    public override void ProcessInput(Game state) {
        // Enter, \n, \r, respectively
        if (state.Input.IsEnterPressed())
            _callback(state, Player);
    }

    public override (int w, int h) GetSize() {
        return (82, 5);
    }

    public override void Render(Drawable drawable, int x, int y, int maxw, int maxh) {
        drawable.DrawBorder(Selected ? BorderSel : BorderUnsel, x, y, 82, 5);

        Player.Render(drawable, x + 2, y + 2);

        drawable.AddLinesWithHighlight(
            (Selected ? TextSel : TextUnsel, x + 5, y + 1, Player.Name + new string(' ', 76 - Player.Name.Length)),
            (Selected ? TextSel : TextUnsel, x + 5, y + 2, Player.Playtime.ToString())
        );
    }

    public int CompareTo(object? obj) {
        if (obj is not UIPlayer b)
            return 0;
        else
            return Player.Name.CompareTo(b.Player.Name);
    }
}

using System;
using Blackguard.Utilities;

namespace Blackguard.UI.Elements;

public class UIWorld : UIElement, ISelectable, IComparable {
    public readonly World World;
    private readonly Action<Game, World> _callback;

    public bool Selected { get; set; }

    public Highlight TextUnsel = Highlight.Text;
    public Highlight TextSel = Highlight.TextSel;
    public Highlight BorderSel = Highlight.TextSel;
    public Highlight BorderUnsel = Highlight.Text;

    public UIWorld(World world, Action<Game, World> callback) {
        World = world;
        _callback = callback;
    }

    public override void ProcessInput(Game state) {
        // Enter, \n, \r, respectively
        if (state.Input.IsEnterPressed())
            _callback(state, World);
    }

    public override (int w, int h) GetSize() {
        return (82, 5);
    }

    public override void Render(Drawable drawable, int x, int y, int maxw, int maxh) {
        drawable.DrawBorder(Selected ? BorderSel : BorderUnsel, x, y, 82, 5);

        drawable.AddLinesWithHighlight(
            (Selected ? TextSel : TextUnsel, x + 5, y + 1, World.Name),
            (Selected ? TextSel : TextUnsel, x + 5, y + 2, World.Playtime.ToString())
        );
    }

    public int CompareTo(object? obj) {
        if (obj is not UIWorld b)
            return 0;
        else
            return World.Name.CompareTo(b.World.Name);
    }
}

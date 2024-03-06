using System;
using System.Linq;
using Blackguard.Utilities;
using Mindmagma.Curses;

namespace Blackguard.UI.Elements;

public class UIButton : UIElement, ISelectable {
    private string[] _label;
    private readonly Action _onPress;

    public Highlight Norm;
    public Highlight Sel;
    public Highlight SelLastLine;

    private (Highlight, int, int, string)[] _segments;

    public bool Selected { get; set; }

    public UIButton(string[] label, Action onPress) {
        _label = label;
        _onPress = onPress;

        _segments = new (Highlight, int, int, string)[_label.Length];
    }

    public void ChangeLabel(string[] label) {
        _label = label;
        _segments = new (Highlight, int, int, string)[_label.Length];
    }

    public override void ProcessInput(Game state) {
        // Enter, \n, \r, respectively
        if (state.Input.KeyPressed(CursesKey.ENTER) || state.Input.KeyPressed(10) || state.Input.KeyPressed(13)) {
            _onPress();
        }
    }

    public override (int w, int h) GetSize() {
        return (_label.Select(line => line.Length).Max(), _label.Length);
    }

    public override void Render(Drawable drawable, int x, int y, int maxy, int maxh) {
        for (int i = 0; i < _label.Length; i++) {
            Highlight highlight;

            if (!Selected)
                highlight = Norm;
            else if (Selected && i < _label.Length - 1)
                highlight = Sel;
            else
                highlight = SelLastLine;

            _segments[i] = (highlight, x, y + i, _label[i]);
        }

        drawable.AddLinesWithHighlight(_segments);
    }

    public void Select() {
        Selected = true;
    }

    public void Deselect() {
        Selected = false;
    }
}

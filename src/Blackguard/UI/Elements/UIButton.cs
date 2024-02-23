using System;
using System.Linq;
using Blackguard.Utilities;
using Mindmagma.Curses;

namespace Blackguard.UI.Elements;

public class UIButton : UIElement, ISelectable {
    private string[] _label;
    private readonly Action _onPress;

    public bool Selected { get; set; }

    public UIButton(string[] label, Action onPress) {
        _label = label;
        _onPress = onPress;
    }

    public void ChangeLabel(string[] label) {
        _label = label;
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
        drawable.AddLinesWithHighlight(_label.Select((line, i) => (i == _label.Length - 1 && Selected ? Highlight.TextSel : Highlight.Text, x, y + i, _label[i])).ToArray());
    }

    public void Select() {
        Selected = true;
    }

    public void Deselect() {
        Selected = false;
    }
}

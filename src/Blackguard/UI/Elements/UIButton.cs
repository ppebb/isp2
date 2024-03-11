using System;
using System.Linq;
using Blackguard.Utilities;

namespace Blackguard.UI.Elements;

public class UIButton : UIElement, ISelectable {
    private string[] _label;
    private readonly Action<Game> _callback;

    public Highlight Norm = Highlight.Text;
    public Highlight Sel = Highlight.TextSel;
    public Highlight SelLastLine = Highlight.TextSelUnderline;

    private (Highlight, int, int, string)[] _segments;

    public bool Selected { get; set; }

    public UIButton(string[] label, Action<Game> callback) {
        _label = label;
        _callback = callback;
        _segments = new (Highlight, int, int, string)[_label.Length];
    }

    public void ChangeLabel(string[] label) {
        _label = label;
        _segments = new (Highlight, int, int, string)[_label.Length];
    }

    public override void ProcessInput(Game state) {
        // Enter, \n, \r, respectively
        if (state.Input.IsEnterPressed()) {
            _callback(state);
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
}

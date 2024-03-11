using System;
using System.Collections.Generic;
using System.Linq;
using Blackguard.Utilities;
using Mindmagma.Curses;

namespace Blackguard.UI.Elements;

public class UICarouselButton<T> : UIElement, ISelectable {
    private readonly List<(UIText, T)> _options;
    private readonly Action<Game, T>? _enterCallback;
    private readonly Action<Game, T>? _leftRightCallback;
    private int idx = 0;

    public Highlight Norm = Highlight.Text;
    public Highlight Sel = Highlight.TextSel;
    public Highlight SelLastLine = Highlight.TextSelUnderline;

    private (Highlight, int, int, string)[] _segments;
    private bool clear = false;

    public bool Selected { get; set; }

    public UICarouselButton(List<(UIText, T)> options, Action<Game, T>? enterCallback, Action<Game, T>? leftRightCallback) {
        _options = options;
        _enterCallback = enterCallback;
        _leftRightCallback = leftRightCallback;
        _segments = new (Highlight, int, int, string)[_options.FirstOrDefault().Item1.Lines.Length];
    }

    public override (int w, int h) GetSize() {
        return _options[idx].Item1.GetSize();
    }

    public override void ProcessInput(Game state) {
        if (state.Input.IsEnterPressed()) {
            _enterCallback?.Invoke(state, _options[idx].Item2);
        }

        if (state.Input.KeyPressed(CursesKey.LEFT)) {
            if (idx > 0)
                idx--;
            else
                idx = _options.Count - 1;

            _leftRightCallback?.Invoke(state, _options[idx].Item2);
            _segments = new (Highlight, int, int, string)[_options[idx].Item1.Lines.Length];
            clear = true;
        }

        if (state.Input.KeyPressed(CursesKey.RIGHT)) {
            if (idx < _options.Count - 1)
                idx++;
            else
                idx = 0;

            _leftRightCallback?.Invoke(state, _options[idx].Item2);
            _segments = new (Highlight, int, int, string)[_options[idx].Item1.Lines.Length];
            clear = true;
        }
    }

    public override void Render(Drawable drawable, int x, int y, int maxw, int maxh) {
        if (clear) {
            for (int i = 0; i < _options[idx].Item1.Lines.Length; i++) {
                drawable.AddLinesWithHighlight((Norm, 0, y + i, new string(' ', maxw)));
            }

            clear = false;
        }

        RenderUIText(drawable, _options[idx].Item1, x, y, maxw, maxh);
    }

    private void RenderUIText(Drawable drawable, UIText uiText, int x, int y, int _, int _2) {
        for (int i = 0; i < uiText.Lines.Length; i++) {
            Highlight highlight;

            if (!Selected)
                highlight = Norm;
            else if (Selected && i < uiText.Lines.Length - 1)
                highlight = Sel;
            else
                highlight = SelLastLine;

            _segments[i] = (highlight, x, y + i, uiText.Lines[i]);
        }

        drawable.AddLinesWithHighlight(_segments);
    }
}

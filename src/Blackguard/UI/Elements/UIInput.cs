using Blackguard.Utilities;
using Mindmagma.Curses;

namespace Blackguard.UI.Elements;

public class UIInput : UIElement, ISelectable {
    private readonly int _maxRenderWidth; // Does not include the 2 col border
    private readonly int _maxTextWidth;
    private readonly string _label = "";
    private readonly string _previewText = "";
    private string storedText = "";
    private readonly bool _big;

    public bool Selected { get; set; }

    public Highlight TextUnsel = Highlight.Text;
    public Highlight TextSel = Highlight.TextSel;
    public Highlight BorderSel = Highlight.TextSel;
    public Highlight BorderUnsel = Highlight.Text;
    public Highlight Preview = Highlight.Text;

    // WARN: The implementation for big is kind of bad. Oh well
    public UIInput(Alignment alignment, string label, string previewText, int maxTextWidth, bool big = false) {
        _alignment = alignment;
        _label = label;
        _previewText = previewText;
        _maxTextWidth = maxTextWidth;
        _maxRenderWidth = big ? maxTextWidth * 6 : maxTextWidth;
        _big = big;
    }

    public string GetStoredText() => storedText;

    public override (int w, int h) GetSize() => (_maxRenderWidth + 2, _big ? 10 : 4);

    private string Pad(string input) {
        int lpad = 0;
        int rpad = 0;

        if (_alignment.HasFlag(Alignment.Right))
            lpad = _maxRenderWidth - input.Length;
        else if (_alignment.HasFlag(Alignment.Center)) {
            lpad = (_maxRenderWidth - input.Length) / 2;
            rpad = _maxRenderWidth - input.Length - lpad;
        }
        else if (_alignment.HasFlag(Alignment.Left))
            rpad = _maxRenderWidth - input.Length;

        return new string(' ', lpad) + input + new string(' ', rpad);
    }

    public override void Render(Drawable drawable, int x, int y, int maxw, int maxh) {
        int current_y = y;

        void RenderLarge(string input) {
            string[] big = input.ToLargeText();

            for (int i = 0; i < big.Length; i++)
                drawable.AddLinesWithHighlight((Selected ? TextSel : TextUnsel, x + 1, current_y += 1, Pad(big[i])));
        }

        drawable.DrawBorder(Selected ? BorderSel : BorderUnsel, x, y, _maxRenderWidth + 2, _big ? 10 : 4);

        if (_big) {
            RenderLarge(_label);
            RenderLarge(storedText.Length != 0 ? storedText : _previewText);
        }
        else {
            drawable.AddLinesWithHighlight(
                (Selected ? TextSel : TextUnsel, x + 1, y + 1, Pad(_label)),
                (Selected ? TextSel : TextUnsel, x + 1, y + 2, Pad(storedText.Length != 0 ? storedText : _previewText))
            );
        }
    }

    public override void ProcessInput(Game state) {
        if (state.Input.HasInputThisTick()) {
            foreach (int c in state.Input.Keycodes()) {
                if ((c >= 48 && c <= 57 || (c >= 65 && c <= 90) || (c >= 97 && c <= 122) || c == 58) && storedText.Length < _maxTextWidth)
                    storedText += NCurses.Keyname(c);

                if (c == CursesKey.BACKSPACE && storedText.Length > 0)
                    storedText = storedText.Remove(storedText.Length - 1);
            }
        }
    }
}

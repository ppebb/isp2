using Blackguard.Utilities;
using Mindmagma.Curses;

namespace Blackguard.UI.Elements;

public class UIInput : UIElement, ISelectable {
    private readonly int _maxTextWidth;
    private readonly string _label = "";
    private readonly string _previewText = "";
    private string storedText = "";

    public bool Selected { get; set; }

    public Highlight TextUnsel { get; set; }
    public Highlight TextSel { get; set; }
    public Highlight BorderSel { get; set; }
    public Highlight BorderUnsel { get; set; }
    public Highlight Preview { get; set; }

    public UIInput(Alignment alignment, string label, string previewText, int maxTextWidth) {
        _alignment = alignment;
        _label = label;
        _previewText = previewText;
        _maxTextWidth = maxTextWidth;
    }

    public string GetStoredText() => storedText;

    public void Deselect() {
        Selected = false;
    }

    public override (int w, int h) GetSize() => (_maxTextWidth + 2, 4);

    private string Pad(string input) {
        int lpad = 0;
        int rpad = 0;

        if (_alignment.HasFlag(Alignment.Right))
            lpad = _maxTextWidth - input.Length;
        else if (_alignment.HasFlag(Alignment.Center)) {
            lpad = (_maxTextWidth - input.Length) / 2;
            rpad = _maxTextWidth - input.Length - lpad;
        }
        else if (_alignment.HasFlag(Alignment.Left))
            rpad = _maxTextWidth - input.Length;

        return new string(' ', lpad) + input + new string(' ', rpad);
    }

    public override void Render(Drawable drawable, int x, int y, int maxw, int maxh) {
        drawable.DrawBorder(Selected ? BorderSel : BorderUnsel, x, y, _maxTextWidth + 2, 4);

        drawable.AddLinesWithHighlight(
            (Selected ? TextSel : TextUnsel, x + 1, y + 1, Pad(_label)),
            (Selected ? TextSel : TextUnsel, x + 1, y + 2, Pad(storedText.Length != 0 ? storedText : _previewText))
        );
    }

    public void Select() {
        Selected = true;
    }

    public override void ProcessInput(Game state) {
        if (state.Input.HasInputThisTick()) {
            foreach (int c in state.Input.Keycodes()) {
                if ((c >= 48 && c <= 57 || (c >= 65 && c <= 90) || (c >= 97 && c <= 122)) && storedText.Length < _maxTextWidth)
                    storedText += NCurses.Keyname(c);

                if (c == CursesKey.BACKSPACE && storedText.Length > 0)
                    storedText = storedText.Remove(storedText.Length - 1);
            }
        }
    }
}

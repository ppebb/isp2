using System.Linq;
using Blackguard.Utilities;

namespace Blackguard.UI.Elements;

public class UIText : UIElement {
    private string[] _lines;

    public UIText(string[] lines, Alignment alignment = Alignment.Left) {
        _alignment = alignment;
        _lines = lines;
    }

    public void ChangeLines(string[] lines) {
        _lines = lines;
    }

    public override (int w, int h) GetSize() {
        return (_lines.Select(line => line.Length).Max(), _lines.Length);
    }

    public override void Render(nint window, int x, int y, int maxy, int maxh) {
        CursesUtils.WindowAddLinesWithHighlight(window, _lines.Select((l, i) => (Highlight.Text, x, y + i, l)).ToArray());
    }
}

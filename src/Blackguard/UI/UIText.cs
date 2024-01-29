using System.Linq;

namespace Blackguard.UI;

public class UIText : UIElement {
    private string[] _lines;
    public Alignment Alignment { private set; get; }

    public UIText(string[] lines, Alignment alignment = Alignment.Left) {
        Alignment = alignment;
        _lines = lines;
    }

    public void ChangeLines(string[] lines) {
        _lines = lines;
    }

    public void ChangeAlignment(Alignment alignment) {
        Alignment = alignment;

        // Should probably call a rerender
    }

    public override (int x, int y) Size() {
        return (_lines.Select(line => line.Length).Max(), _lines.Length);
    }
}

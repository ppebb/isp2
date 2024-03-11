using System.Linq;
using Blackguard.Utilities;

namespace Blackguard.UI.Elements;

public class UIText : UIElement {
    public string[] Lines;

    public Highlight Highlight;

    public UIText(string[] lines, Alignment alignment = Alignment.Left) {
        _alignment = alignment;
        Lines = lines;
    }

    public override (int w, int h) GetSize() {
        return (Lines.Select(line => line.Length).Max(), Lines.Length);
    }

    public override void Render(Drawable drawable, int x, int y, int maxy, int maxh) {
        // TODO: Draw based on alignment?
        drawable.AddLinesWithHighlight(Lines.Select((l, i) => (Highlight, x, y + i, l)).ToArray());
    }
}

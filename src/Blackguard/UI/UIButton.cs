using System;
using System.Linq;
using Blackguard.Utilities;
using Mindmagma.Curses;

namespace Blackguard.UI;

public class UIButton : UIElement {
    private string[] _label;
    private readonly Action _onPress;

    public UIButton(string[] label, Action onPress) {
        _label = label;
        _onPress = onPress;
    }

    public void ChangeLabel(string[] label) {
        _label = label;
    }

    public override void ProcessInput() {
        if (Game.KeyPressed(CursesKey.ENTER)) {
            _onPress();
        }
    }

    public override (int x, int y) GetSize() {
        return (_label.Select(line => line.Length).Max(), _label.Length);
    }

    public override void Render(nint window, int x, int y, int maxy, int maxh) {
        CursesUtils.WindowAddLinesWithHighlight(window, _label.Select((line, i) => (i == _label.Length - 1 ? Highlight.TextSel : Highlight.Text, x, y + i, _label[i])).ToArray());
    }
}

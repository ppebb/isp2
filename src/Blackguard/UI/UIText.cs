namespace Blackguard.UI;

public class UIText : UIElement {
    private string[] _lines;

    public UIText(string[] lines) {
        _lines = lines;
    }

    public void ChangeLines(string[] lines) {
        _lines = lines;
        Render();
    }
}
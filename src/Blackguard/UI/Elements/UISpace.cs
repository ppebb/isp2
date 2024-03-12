namespace Blackguard.UI.Elements;

public class UISpace : UIElement {
    private int _w;
    private int _h;

    public UISpace(int w, int h) {
        _w = w;
        _h = h;
    }

    public void ChangeSize(int neww, int newh) {
        _w = neww;
        _h = newh;
    }

    public override (int w, int h) GetSize() {
        return (_w, _h);
    }

    public override void Render(Drawable drawable, int x, int y, int maxw, int maxh) {
        // Do nothing. Nothing to render. Blank space
    }
}

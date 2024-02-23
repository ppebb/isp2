namespace Blackguard.UI.Elements;

public abstract class UIElement : ISizeProvider {
    protected Alignment _alignment;

    public void ChangeAlignment(Alignment alignment, bool replace = false) {
        if (replace)
            _alignment = alignment;
        else {
            Alignment temp = _alignment;
            temp.UpdateAlignment(alignment);
            _alignment = temp;
        }
    }

    public abstract (int w, int h) GetSize();

    public virtual void RunTick(Game state) { }

    // Called whenever input is detected for a tick
    public virtual void ProcessInput(Game state) { }

    // Handle drawing the text in addition to resizing
    public abstract void Render(Drawable drawable, int x, int y, int maxw, int maxh);
}

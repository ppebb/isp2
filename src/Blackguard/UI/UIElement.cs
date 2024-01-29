namespace Blackguard.UI;

public abstract class UIElement {
    protected bool selected;

    public void Select() {
        selected = true;
    }

    public void Deselect() {
        selected = false;
    }

    public abstract (int x, int y) Size();

    public virtual void Tick() { }

    // Called whenever input is detected for a tick
    public virtual void ProcessInput() { }

    // Handle drawing the text in addition to resizing
    public virtual void Render(nint window, int x, int y) { }
}

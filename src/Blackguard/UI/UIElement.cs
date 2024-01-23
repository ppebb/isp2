namespace Blackguard.UI;

public abstract class UIElement {
    private bool selected;

    public void Select() {
        selected = true;
    }

    public void Deselect() {
        selected = false;
    }

    public virtual void Tick() { }

    // Called whenever input is detected for a tick
    public virtual void ProcessInput() { }

    // Handle drawing the text in addition to resizing
    public virtual void Render() { }
}
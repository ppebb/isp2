namespace Blackguard.UI;

public interface ISelectable {
    public bool Selected { get; protected set; }

    public void Select() {
        Selected = true;
    }

    public void Deselect() {
        Selected = false;
    }
}

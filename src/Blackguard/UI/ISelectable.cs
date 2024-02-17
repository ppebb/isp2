namespace Blackguard.UI;

public interface ISelectable {
    public bool Selected { get; protected set; }

    public void Select();

    public void Deselect();
}

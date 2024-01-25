using System.Collections.Generic;

namespace Blackguard.UI;

public class UIContainer : UIElement {
    private readonly List<UIElement> _elements;
    private int selected;

    // TODO: Handle sizing dynamically somehow

    public UIContainer(List<UIElement> elements) {
        _elements = elements;
    }

    public UIContainer() {
        _elements = new();
    }

    public void Add(UIElement element) => _elements.Add(element);

    public void Remove(UIElement element) => _elements.Remove(element);

    public bool Empty() => _elements?.Count == 0;

    public void Next() {
        if (Empty())
            return;

        if (_elements[selected] is UIContainer container) {
            if (!container.Bottom()) {
                container.Next();
                return;
            }
        }

        _elements[selected].Deselect();

        if (++selected >= _elements.Count)
            selected = 0;

        _elements[selected].Select();
    }

    public void Prev() {
        if (Empty())
            return;

        if (_elements[selected] is UIContainer container) {
            if (!container.Top()) {
                container.Prev();
                return;
            }
        }

        _elements[selected].Deselect();

        if (--selected < 0)
            selected = _elements.Count - 1;

        _elements[selected].Select();
    }

    public bool Top() {
        bool top = selected == _elements.Count - 1;

        if (_elements[selected] is UIContainer container)
            return top && container.Top();

        return top;
    }

    public bool Bottom() {
        bool bottom = selected == _elements.Count - 1;

        if (_elements[selected] is UIContainer container)
            return bottom && container.Bottom();

        return bottom;
    }

    // Called every tick
    public override void Tick() {
        if (Game.IsInput())
            ProcessInput();
    }

    public override void ProcessInput() {
        if (Empty())
            return;

        // I don't know if changing scenes within ProcessInput will result in a memory leak. Will have to profile later.
        _elements[selected].ProcessInput();
    }
}

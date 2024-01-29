using System.Collections.Generic;

namespace Blackguard.UI;

public class UIContainer : UIElement, IBoundsProvider {
    private readonly List<UIElement> _elements;
    private int selected_element;

    public (int, int) GetBounds() {
        throw new System.NotImplementedException();
    }

    public (int, int) GetOffset() {
        throw new System.NotImplementedException();
    }

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

        if (_elements[selected_element] is UIContainer container) {
            if (!container.Bottom()) {
                container.Next();
                return;
            }
        }

        _elements[selected_element].Deselect();

        if (++selected_element >= _elements.Count)
            selected_element = 0;

        _elements[selected_element].Select();
    }

    public void Prev() {
        if (Empty())
            return;

        if (_elements[selected_element] is UIContainer container) {
            if (!container.Top()) {
                container.Prev();
                return;
            }
        }

        _elements[selected_element].Deselect();

        if (--selected_element < 0)
            selected_element = _elements.Count - 1;

        _elements[selected_element].Select();
    }

    public bool Top() {
        bool top = selected_element == _elements.Count - 1;

        if (_elements[selected_element] is UIContainer container)
            return top && container.Top();

        return top;
    }

    public bool Bottom() {
        bool bottom = selected_element == _elements.Count - 1;

        if (_elements[selected_element] is UIContainer container)
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
        _elements[selected_element].ProcessInput();
    }

    public override (int x, int y) Size() {
        throw new System.NotImplementedException();
    }

    public override void Render(nint window, int x, int y) {
        // Handle sizing soon

        foreach (UIElement child in _elements)
            child.Render(window, 0, 0);
    }
}

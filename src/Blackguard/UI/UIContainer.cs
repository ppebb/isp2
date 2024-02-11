using System.Collections.Generic;
using System.Linq;

namespace Blackguard.UI;

public class UIContainer : UIElement {
    private readonly List<UIElement> _elements;
    private int selected_element;

    public override (int, int) GetSize() {
        return (_elements.Select(e => e.GetSize().x).Max(), _elements.Select(e => e.GetSize().y).Sum());
    }

    public UIContainer(List<UIElement> elements, Alignment alignment) {
        _elements = elements;
        _alignment = alignment;
    }

    public UIContainer(Alignment alignment, params UIElement[] elements) {
        _elements = elements.ToList();
        _alignment = alignment;
    }

    public UIContainer() {
        _elements = new List<UIElement>();
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

        _elements[selected_element].ProcessInput();
    }

    public override void Render(nint window, int x, int y, int maxw, int maxh) {
        int cy = y;

        foreach (UIElement child in _elements) {
            int cx = 0;
            (int cw, int ch) = child.GetSize();

            if (_alignment.HasFlag(Alignment.Center))
                cx += (maxw - cw) / 2;
            else if (_alignment.HasFlag(Alignment.Right))
                cx += maxw - cw;

            child.Render(window, cx, cy, maxw, maxh - cy);
            cy += ch;
        }
    }
}

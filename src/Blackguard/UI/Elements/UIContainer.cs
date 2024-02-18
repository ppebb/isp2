using System.Collections.Generic;
using System.Linq;

namespace Blackguard.UI.Elements;

public class UIContainer : UIElement, ISelectable {
    private readonly List<UIElement> _elements;
    private int selected_element = 0;

    public bool Selected { get; set; }

    public override (int w, int h) GetSize() {
        return (_elements.Select(e => e.GetSize().w).Max(), _elements.Select(e => e.GetSize().h).Sum());
    }

    public UIContainer(List<UIElement> elements, Alignment alignment) {
        _elements = elements;
        _alignment = alignment;
        SelectFirstSelectable();
    }

    public UIContainer(Alignment alignment, params UIElement[] elements) {
        _elements = elements.ToList();
        _alignment = alignment;
        SelectFirstSelectable();
    }

    public UIContainer() {
        _elements = new List<UIElement>();
    }

    public void Add(UIElement element) => _elements.Add(element);

    public void Remove(UIElement element) => _elements.Remove(element);

    public bool Empty() => _elements?.Count == 0;

    private bool SelectFirstSelectable() => SelectNextSelectable(0, true);

    private bool SelectNextSelectable(int start, bool forwards) {
        for (int i = start; i >= 0 && i < _elements.Count; i += forwards ? 1 : -1) {
            if (_elements[i] is ISelectable selectable) {
                if (i == selected_element) // Don't select the same element
                    continue;

                (_elements[selected_element] as ISelectable)?.Deselect();
                selected_element = i;
                selectable.Select();
                return true;
            }
        }

        return false;
    }

    // Returns true if able to successfully move to the next element, false if not
    public bool Next(bool wrap) {
        if (Empty())
            return false;

        if (_elements[selected_element] is UIContainer container)
            if (container.Next(false)) // If we can move to the next element in the child container, then we're done
                return true;

        // Otherwise, deselect the current element and look for the next one. If we can't find one return false and the parent will handle it!
        if (SelectNextSelectable(selected_element + 1, true))
            return true;
        else if (wrap)
            return SelectNextSelectable(0, true);

        return false;
    }

    public bool Prev(bool wrap) {
        if (Empty())
            return false;

        if (_elements[selected_element] is UIContainer container)
            if (container.Prev(false)) // If we can move to the previous element in the child container, then we're done
                return true;

        // Otherwise, deselect the current element and look for the previous one. If we can't find one return false and the parent will handle it!
        if (SelectNextSelectable(selected_element - 1, false))
            return true;
        else if (wrap)
            return SelectNextSelectable(_elements.Count - 1, false);

        return false;
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
    public override void RunTick(Game state) {
        ProcessInput(state);
    }

    public override void ProcessInput(Game state) {
        if (Empty())
            return;

        _elements[selected_element].ProcessInput(state);
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

    public void Select() {
        throw new System.NotImplementedException();
    }

    public void Deselect() {
        throw new System.NotImplementedException();
    }
}

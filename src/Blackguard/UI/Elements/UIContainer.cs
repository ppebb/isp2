using System;
using System.Collections.Generic;
using System.Linq;
using Blackguard.Utilities;

namespace Blackguard.UI.Elements;

public class UIContainer : UIElement, ISelectable {
    private readonly List<UIElement> _elements;
    private int selectedElement = 0;
    public bool Border;
    public int? Height; // Setting this means the container is scrollable. I think this is very good design.
    private int hOffset = 0; // Used for scrolling. The height worth of elements to skip.

    public Highlight BorderUnsel = Highlight.Text;
    public Highlight BorderSel = Highlight.TextSel;
    public Highlight ScrollUnsel = Highlight.Text;
    public Highlight ScrollSel = Highlight.TextSel;

    private IComparer<UIElement>? _backingComparison;
    public IComparer<UIElement>? Comparer {
        get {
            return _backingComparison;
        }
        set {
            if (value != null) {
                _backingComparison = value;
                _elements.Sort(_backingComparison);
            }
        }
    }

    public bool Selected { get; set; }

    public override (int w, int h) GetSize() {
        int w = 0, h = 0;

        foreach (UIElement element in _elements) {
            (int cw, int ch) = element.GetSize();
            w = Math.Max(w, cw);
            h += ch;
        }

        int hAdd = 0;
        if (Border) {
            w += 2;
            hAdd = 2;
        }

        if (Height != null && h > Height)
            return (w + 1, Height.Value + hAdd);

        return (w, h + hAdd);
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

    public void Add(UIElement element) {
        if (Comparer != null) {
            int idx = _elements.BinarySearch(element, Comparer);
            if (idx < 0)
                idx = ~idx;

            _elements.Insert(idx, element);
        }
        else
            _elements.Add(element);
    }

    public void Remove(UIElement element) => _elements.Remove(element);

    public bool Empty() => _elements?.Count == 0;

    public bool SelectFirstSelectable() => SelectNextSelectable(0, true);

    public bool SelectLastSelectable() => SelectNextSelectable(_elements.Count - 1, false);

    private bool SelectNextSelectable(int start, bool forwards) {
        for (int i = start; i >= 0 && i < _elements.Count; i += forwards ? 1 : -1) {
            if (_elements[i] is ISelectable selectable) {
                (_elements[selectedElement] as ISelectable)?.Deselect();
                selectedElement = i;
                selectable.Select();

                if (selectable is UIContainer c) {
                    if (forwards)
                        c.SelectFirstSelectable();
                    else
                        c.SelectLastSelectable();
                }

                if (Height != null) {
                    (int hexc, int hinc) = SumHeights(0, i);

                    if (hinc > Height + hOffset)
                        hOffset = hinc - Height.Value;
                    else if (hexc < hOffset)
                        hOffset = hexc;
                }

                return true;
            }
        }

        return false;
    }

    // Returns true if able to successfully move to the next element, false if not
    public bool Next(bool wrap) {
        if (Empty())
            return false;

        if (_elements[selectedElement] is UIContainer container)
            if (container.Next(false)) // If we can move to the next element in the child container, then we're done
                return true;

        // Otherwise, deselect the current element and look for the next one. If we can't find one return false and the parent will handle it!
        if (SelectNextSelectable(selectedElement + 1, true))
            return true;
        else if (wrap)
            return SelectNextSelectable(0, true);

        return false;
    }

    public bool Prev(bool wrap) {
        if (Empty())
            return false;

        if (_elements[selectedElement] is UIContainer container)
            if (container.Prev(false)) // If we can move to the previous element in the child container, then we're done
                return true;

        // Otherwise, deselect the current element and look for the previous one. If we can't find one return false and the parent will handle it!
        if (SelectNextSelectable(selectedElement - 1, false))
            return true;
        else if (wrap)
            return SelectNextSelectable(_elements.Count - 1, false);

        return false;
    }

    public bool Top() {
        bool top = selectedElement == _elements.Count - 1;

        if (_elements[selectedElement] is UIContainer container)
            return top && container.Top();

        return top;
    }

    public bool Bottom() {
        bool bottom = selectedElement == _elements.Count - 1;

        if (_elements[selectedElement] is UIContainer container)
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

        _elements[selectedElement].ProcessInput(state);
    }

    private (int exc, int inc) SumHeights(int start = 0, int end = -1) {
        int h = 0;

        for (int i = start; i <= (end == -1 ? _elements.Count : end); i++)
            h += _elements[i].GetSize().h;

        return (h - _elements[^1].GetSize().h, h);
    }

    public override void Render(Drawable drawable, int x, int y, int maxw, int maxh) {
        int realMaxh = Height ?? maxh;
        int cy = Border ? y + 1 : y; // child y position

        int tg = 0; // total gaps for fill
        int th = 0;
        int lw = 0;
        for (int i = 0; i < _elements.Count; i++) {
            if (_elements[i] is not UISpace && i != _elements.Count - 1)
                tg++;

            (int cw, int ch) = _elements[i].GetSize();
            th += ch;
            lw = Math.Max(lw, cw);
        }

        bool scroll = Height != null && th > Height;

        if (scroll)
            lw += 1;

        if (Border)
            lw += 2;

        int fillGapSize;
        if (tg > 0) // Avoid dividing by zero
            fillGapSize = (realMaxh - th) / tg;
        else
            fillGapSize = 0;

        int j = 0; // Sum of heights looped through
        for (int i = 0; i < _elements.Count; i++) {
            UIElement child = _elements[i];
            bool nextSpace = i < _elements.Count - 1 && _elements[i + 1] is UISpace;

            int cx = 0; // child x
            (int cw, int ch) = child.GetSize(); // child width, height

            if (scroll)
                cw += 1;

            if (Border)
                cw += 2;

            j += ch;
            if (j <= hOffset)
                continue;

            if (_alignment.HasFlag(Alignment.Center))
                cx += (maxw - cw) / 2;
            else if (_alignment.HasFlag(Alignment.Right))
                cx += maxw - cw;

            if (_alignment.HasFlag(Alignment.Bottom)) {
                cy = realMaxh - th;
                th -= ch;
            }

            if (cy + ch > y + realMaxh + (Border ? 1 : 0))
                break;

            child.Render(drawable, Border ? cx + 1 : cx, cy, maxw, realMaxh - cy);

            cy += ch;

            // This works, but the gap size may not fill *all* the way if the terminal isn't in perfect increments
            if (_alignment.HasFlag(Alignment.Fill) && !(nextSpace || child is UISpace))
                cy += fillGapSize;
        }

        // Render scrollbar
        if (Height != null && scroll) {
            int size = (int)Math.Round(Height.Value / (double)th * Height.Value);

            int starty = (int)Math.Round(hOffset / (double)th * Height.Value);

            for (int i = 0; i < Height + 1; i++) {
                if (i < starty || i > starty + size)
                    drawable.AddLinesWithHighlight((ScrollUnsel, x + lw - (Border ? 2 : 1), i + y, " "));
                else
                    drawable.AddLinesWithHighlight((Selected ? ScrollSel : ScrollUnsel, x + lw - (Border ? 2 : 1), i + y, "b"));
            }
        }

        if (Border) {
            int bh;
            if (Height != null && th > Height.Value)
                bh = Height.Value + 2;
            else
                bh = th + 2;

            drawable.DrawBorder(Selected ? BorderSel : BorderUnsel, x, y, lw, bh);
        }
    }

    public void Select() {
        Selected = true;
    }

    public void Deselect() {
        Selected = false;
        // Just in case!
        _elements.ForEach((e) => (e as ISelectable)?.Deselect());
    }
}

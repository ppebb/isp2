using System.Collections.Generic;
using Mindmagma.Curses;

namespace Blackguard.UI;

public class UIContainer : UIElement {
    private List<UIElement> _elements;
    private int selected;

    // TODO: Handle sizing dynamically somehow

    public UIContainer(List<UIElement> elements) {
        _elements = elements;
    }

    public void Add(UIElement element) => _elements.Add(element);

    public void Remove(UIElement element) => _elements.Remove(element);

    public void Next() {
        _elements[selected].Deselect();
        
        if (++selected >= _elements.Count)
            selected = 0;
        
        _elements[selected].Select();
    }

    // Called every tick
    public override void Tick() {
        if (Game.IsInput())
            ProcessInput();
    }
    
    // This implementation can likely be generalized to other things. Not sure yet
    private int debounceTimer;
    private List<int> inputBuffer = new();

    public override void ProcessInput() {
        _elements[selected].ProcessInput();
    }
}
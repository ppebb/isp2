using System;
using Mindmagma.Curses;

namespace Blackguard.UI;

public class UIButton : UIElement {
    private string _label;
    private readonly Action _onPress;

    public UIButton(string label, Action onPress) {
        _label = label;
        _onPress = onPress;
    }

    public void ChangeLabel(string label) {
        _label = label;
        Render();
    }

    public override void ProcessInput() {
        if (Game.KeyPressed(CursesKey.ENTER)) {
            _onPress();
        }
    }
}
using System.IO;
using Blackguard.UI.Elements;
using Blackguard.UI.Popups;
using Blackguard.Utilities;

namespace Blackguard.UI.Scenes;

public class WorldCreationScene : Scene {
    private readonly UIInput _nameInput;

    public Highlight Norm = Highlight.Text;
    public Highlight Sel = Highlight.TextSel;
    public Highlight SelLastLine = Highlight.TextSelUnderline;

    public WorldCreationScene() {
        UIText header = new("Customize Your World".ToLargeText());

        _nameInput = new(Alignment.Center, "Name:", "Enter a name", 15, true);

        UIButton finish = new("Create World".ToLargeText(), (state) => {
            string storedText = _nameInput.GetStoredText();

            if (storedText.Length == 0) {
                state.OpenPopup(new InfoPopup("NameTooShortWarning", InfoType.Warning, ["A name must be choosen to continue!"]), true);
                return;
            }

            if (File.Exists(Path.Combine(Game.WorldPath, storedText + ".wld"))) {
                state.OpenPopup(new InfoPopup("WorldExistsWarning", InfoType.Warning, [$"A world with the name {storedText} already exists!"]), true);
                return;
            }

            callback?.Invoke(new World(_nameInput.GetStoredText()));

            state.PrevScene();
        });

        UIButton back = new("Back".ToLargeText(), (state) => {
            container.SelectFirstSelectable();
            state.PrevScene();
        });

        container = new UIContainer(Alignment.Center, header, _nameInput, finish, back);
    }

    public override bool RunTick(Game state) {
        ProcessInput(state);
        return true;
    }

    public override void Render(Game state) {
        container.Render(state.CurrentWin, 0, 0, state.CurrentWin.w, state.CurrentWin.h);
    }
}

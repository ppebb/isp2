using System.IO;
using Blackguard.UI.Elements;
using Blackguard.Utilities;

namespace Blackguard.UI.Scenes;

public class PlayerCreationScene : Scene {
    private RaceType _selectedRace;
    private PlayerType _selectedType;
    private readonly UIInput _nameInput;

    public Highlight Norm = Highlight.Text;
    public Highlight Sel = Highlight.TextSel;
    public Highlight SelLastLine = Highlight.TextSelUnderline;

    public PlayerCreationScene() {
        UIText header = new("Customize Your Player".ToLargeText());

        _nameInput = new(Alignment.Center, "Name:", "Enter your name", 15, true);

        UICarouselButton<RaceType> raceSelector = new(
            [
                (new UIText("Human".ToLargeText(), Alignment.Left), RaceType.Human),
                (new UIText("Ork".ToLargeText(), Alignment.Left), RaceType.Ork),
                (new UIText("Elf".ToLargeText(), Alignment.Left), RaceType.Elf),
                (new UIText("Dwarf".ToLargeText(), Alignment.Left), RaceType.Dwarf),
                (new UIText("Demon".ToLargeText(), Alignment.Left), RaceType.Demon),
                (new UIText("Gnome".ToLargeText(), Alignment.Left), RaceType.Gnome)
            ],
            null,
            (_, r) => {
                _selectedRace = r;
            }
        );

        UICarouselButton<PlayerType> typeSelector = new(
            [
                (new UIText("Knight".ToLargeText(), Alignment.Left), PlayerType.Knight),
                (new UIText("Archer".ToLargeText(), Alignment.Left), PlayerType.Archer),
                (new UIText("Mage".ToLargeText(), Alignment.Left), PlayerType.Mage),
                (new UIText("Barbarian".ToLargeText(), Alignment.Left), PlayerType.Barbarian)
            ],
            null,
            (_, t) => {
                _selectedType = t;
            }
        );

        UIButton finish = new("Create Player".ToLargeText(), (state) => {
            string storedText = _nameInput.GetStoredText();

            if (storedText.Length < 0) {
                // TODO: Create some error popup system
                return;
            }

            if (File.Exists(Path.Combine(Game.PlayerPath, storedText + ".plr"))) {
                // TODO: Create some error popup system
                return;
            }

            callback?.Invoke(new Player(storedText, _selectedType, _selectedRace));

            state.PrevScene();
        });

        UIButton back = new("Back".ToLargeText(), (state) => {
            container.SelectFirstSelectable();
            state.PrevScene();
        });

        container = new UIContainer(Alignment.Center, header, _nameInput, raceSelector, typeSelector, finish, back);
    }

    public override bool RunTick(Game state) {
        ProcessInput(state);
        return true;
    }

    public override void Render(Game state) {
        container.Render(state.CurrentWin, 0, 0, state.CurrentWin.w, state.CurrentWin.h);
    }
}

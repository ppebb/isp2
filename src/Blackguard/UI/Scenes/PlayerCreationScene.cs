using System.Collections.Generic;
using Blackguard.UI.Elements;

namespace Blackguard.UI.Scenes;

public class PlayerCreationScene : Scene {
    private static string[] racesHeader = {
        "█▀▀▄   ▄█▄  ▄▀▀▀▄ █▀▀▀▀ ▄▀▀▀▄",
        "█▄▄▀   █ █  █     █▄▄▄  ▀▄▄▄ ",
        "█  █  █▀▀▀█ █   ▄ █     ▄   █",
        "▀   ▀ ▀   ▀  ▀▀▀  ▀▀▀▀▀  ▀▀▀ "
    };
    private static string[] classesHeader = {
        "▄▀▀▀▄ █     ▄█▄  ▄▀▀▀▄ ▄▀▀▀▄ █▀▀▀▀ ▄▀▀▀▄",
        "█     █     █ █  ▀▄▄▄  ▀▄▄▄  █▄▄▄  ▀▄▄▄ ",
        "█   ▄ █    █▀▀▀█ ▄   █ ▄   █ █     ▄   █",
        " ▀▀▀  ▀▀▀▀ ▀   ▀  ▀▀▀   ▀▀▀  ▀▀▀▀▀  ▀▀▀ "
    };
    private static string[] humanRaceText = {
        "█  █ █  █ █▄ ▄█ ▄▀▀▄ █▄ █",
        "█▀▀█ █  █ █▀▄▀█ █▀▀█ █ ██",
        "▀  ▀  ▀▀  ▀ ▀ ▀ ▀  ▀ ▀  ▀"
    };
    private static string[] orkRaceText = {
        "▄▀▀▄ █▀▀▄ █ ▄▀",
        "█  █ █▀█  █▀▄ ",
        " ▀▀  ▀  ▀ ▀  ▀"
    };
    private static string[] elfRaceText = {
        "█▀▀▀ █    █▀▀▀",
        "█▀▀  █    █▀▀ ",
        "▀▀▀▀ ▀▀▀▀ ▀   "
    };
    private static string[] dwarfRaceText = {
        "█▀▀▄ █ █ █ ▄▀▀▄ █▀▀▄ █▀▀▀",
        "█  █ █ █ █ █▀▀█ █▀█  █▀▀ ",
        "▀▀▀   ▀ ▀  ▀  ▀ ▀  ▀ ▀   "
    };
    private static string[] demonRaceText = {
        "█▀▀▄ █▀▀▀ █▄ ▄█ ▄▀▀▄ █▄ █",
        "█  █ █▀▀  █▀▄▀█ █  █ █ ██",
        "▀▀▀  ▀▀▀▀ ▀ ▀ ▀  ▀▀  ▀  ▀"
    };
    private static string[] gnomeRaceText = {
        "▄▀▀  █▄ █ ▄▀▀▄ █▄ ▄█ █▀▀▀",
        "█ ▀█ █ ██ █  █ █▀▄▀█ █▀▀ ",
        " ▀▀  ▀  ▀  ▀▀  ▀ ▀ ▀ ▀▀▀▀"
    };
    private static string[] knightClassText = {
        "█ ▄▀ █▄ █ ▀█▀ ▄▀▀  █  █ ▀█▀",
        "█▀▄  █ ██  █  █ ▀█ █▀▀█  █ ",
        "▀  ▀ ▀  ▀ ▀▀▀  ▀▀  ▀  ▀  ▀ "
    };
    private static string[] archerClassText = {
        "▄▀▀▄ █▀▀▄ ▄▀▀▄ █  █ █▀▀▀ █▀▀▄",
        "█▀▀█ █▀█  █  ▄ █▀▀█ █▀▀  █▀█ ",
        "▀  ▀ ▀  ▀  ▀▀  ▀  ▀ ▀▀▀▀ ▀  ▀"
    };
    private static string[] mageClassText = {
        "█▄ ▄█ ▄▀▀▄ ▄▀▀▀ █▀▀▀",
        "█▀▄▀█ █▀▀█ █ ▀█ █▀▀ ",
        "▀ ▀ ▀ ▀  ▀  ▀▀  ▀▀▀▀"
    };
    private static string[] barbarianClassText = {
        "█▀▀▄ ▄▀▀▄ █▀▀▄ █▀▀▄ ▄▀▀▄ █▀▀▄ ▀█▀ ▄▀▀▄ █▄ █",
        "█▀▀▄ █▀▀█ █▀█  █▀▀▄ █▀▀█ █▀█   █  █▀▀█ █ ██",
        "▀▀▀  ▀  ▀ ▀  ▀ ▀▀▀  ▀  ▀ ▀  ▀ ▀▀▀ ▀  ▀ ▀  ▀"
    };

    private Player? createdPlayer;

    public PlayerCreationScene() {
        UIText racesHeaderText = new(racesHeader);
        UIText classesHeaderText = new(classesHeader);

        UIButton humanButton = new(humanRaceText, (_) => { });
        UIButton orkButton = new(orkRaceText, (_) => { });
        UIButton elfButton = new(elfRaceText, (_) => { });
        UIButton dwarfButton = new(dwarfRaceText, (_) => { });
        UIButton demonButton = new(demonRaceText, (_) => { });
        UIButton gnomeButton = new(gnomeRaceText, (_) => { });

        UIButton knightButton = new(knightClassText, (_) => { });
        UIButton archerButton = new(archerClassText, (_) => { });
        UIButton mageButton = new(mageClassText, (_) => { });
        UIButton barbarianButton = new(barbarianClassText, (_) => { });

        UIButton finish = new(["Create Player"], (_) => callback?.Invoke(createdPlayer));

        List<UIElement> elements = [
            racesHeaderText, classesHeaderText, humanButton, orkButton, elfButton, dwarfButton, demonButton, gnomeButton, knightButton, archerButton, mageButton, barbarianButton, finish
        ];

        container = new UIContainer(elements, Alignment.Center);
    }

    public override bool RunTick(Game state) {
        ProcessInput(state);
        return true;
    }

    public override void Render(Game state) {
        container.Render(state.CurrentWin, 0, 0, state.CurrentWin.w, state.CurrentWin.h);
    }
}

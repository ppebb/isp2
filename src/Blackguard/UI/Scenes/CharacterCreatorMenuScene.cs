using System;
using System.Collections.Generic;
using Blackguard.Utilities;
using Mindmagma.Curses;

namespace Blackguard.UI.Scenes;

public class CharacterCreationScene : Scene {
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

    public CharacterCreationScene() {
        UIText racesHeaderText = new(racesHeader);

        UIButton humanButton = new(humanRaceText, () => { });
        UIButton orkButton = new(orkRaceText, () => { });
        UIButton elfButton = new(elfRaceText, () => { });
        UIButton dwarfButton = new(dwarfRaceText, () => { });
        UIButton demonButton = new(demonRaceText, () => { });
        UIButton gnomeButton = new(gnomeRaceText, () => { });

        UIButton knightButton = new(knightClassText, () => { });
        UIButton archerButton = new(archerClassText, () => { });
        UIButton mageButton = new(mageClassText, () => { });
        UIButton barbarianButton = new(barbarianClassText, () => { });

        List<UIElement> elements = [
            racesHeaderText, racesHeaderText,
            humanButton, orkButton, elfButton, dwarfButton, demonButton, gnomeButton,
            knightButton, archerButton, mageButton, barbarianButton
        ];

        container = new UIContainer(elements, Alignment.Left);
    }

    int tick = 0;
    public override bool RunTick() {
        tick++;
        return true;
    }

    public override void Render() {

    }

    public override void Finish() {
        NCurses.DeleteWindow(CurrentWin.handle);
    }
}

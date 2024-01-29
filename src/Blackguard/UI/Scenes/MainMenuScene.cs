using System;
using System.Collections.Generic;
using System.ComponentModel;
using Blackguard.Utilities;
using Mindmagma.Curses;

namespace Blackguard.UI.Scenes;

public class MainMenuScene : Scene {
    private static string[] Logo = {
        "██████╗░██╗░░░░░░█████╗░░█████╗░██╗░░██╗░██████╗░██╗░░░██╗░█████╗░██████╗░██████╗░",
        "██╔══██╗██║░░░░░██╔══██╗██╔══██╗██║░██╔╝██╔════╝░██║░░░██║██╔══██╗██╔══██╗██╔══██╗",
        "██████╦╝██║░░░░░███████║██║░░╚═╝█████═╝░██║░░██╗░██║░░░██║███████║██████╔╝██║░░██║",
        "██╔══██╗██║░░░░░██╔══██║██║░░██╗██╔═██╗░██║░░╚██╗██║░░░██║██╔══██║██╔══██╗██║░░██║",
        "██████╦╝███████╗██║░░██║╚█████╔╝██║░╚██╗╚██████╔╝╚██████╔╝██║░░██║██║░░██║██████╔╝",
        "╚═════╝░╚══════╝╚═╝░░╚═╝░╚════╝░╚═╝░░╚═╝░╚═════╝░░╚═════╝░╚═╝░░╚═╝╚═╝░░╚═╝╚═════╝░"
    };
    private static string[] Start = {
        "▄▀▀▀▄ ▀▀█▀▀  ▄█▄  █▀▀▄  ▀▀█▀▀",
        "▀▄▄▄    █    █ █  █▄▄▀    █  ",
        "▄   █   █   █▀▀▀█ █  █    █  ",
        " ▀▀▀    ▀   ▀   ▀ ▀   ▀   ▀  "
    };
    private static string[] Settings = {
        "▄▀▀▀▄ █▀▀▀▀ ▀▀█▀▀ ▀▀█▀▀ ▀▀█▀▀ ██  █ ▄▀▀▀▄ ▄▀▀▀▄",
        "▀▄▄▄  █▄▄▄    █     █     █   █ █ █ █     ▀▄▄▄ ",
        "▄   █ █       █     █     █   █ ▀▄█ █  ▀█ ▄   █",
        " ▀▀▀  ▀▀▀▀▀   ▀     ▀   ▀▀▀▀▀ ▀  ▀▀  ▀▀▀   ▀▀▀ "
    };
    private static string[] Credits = {
        "▄▀▀▀▄ █▀▀▄  █▀▀▀▀ █▀▀▄  ▀▀█▀▀ ▀▀█▀▀ ▄▀▀▀▄",
        "█     █▄▄▀  █▄▄▄  █   █   █     █   ▀▄▄▄ ",
        "█   ▄ █  █  █     █  ▄▀   █     █   ▄   █",
        " ▀▀▀  ▀   ▀ ▀▀▀▀▀ ▀▀▀   ▀▀▀▀▀   ▀    ▀▀▀ "
    };
    private static string[] Quit = {
        "▄▀▀▀▄ █   █ ▀▀█▀▀ ▀▀█▀▀",
        "█   █ █   █   █     █  ",
        "█ ▀▄▀ █   █   █     █  ",
        " ▀▀ ▀  ▀▀▀  ▀▀▀▀▀   ▀  "
    };

    public MainMenuScene() {
        UIText logoText = new(Logo);
        UIButton startButton = new(Start, () => {  });
        UIButton settingsButton = new(Settings, () => {  });
        UIButton creditsButton = new(Credits, () => {  });
        UIButton quitButton = new(Quit, () => {  });


        List<UIElement> elements = [logoText, startButton, settingsButton, creditsButton, quitButton];
        // or should it be like the following?
        // List<UIElement> elements = [logoText];
        // List<UIElement> elements = [startButton];
        // List<UIElement> elements = [settingsButton];
        // List<UIElement> elements = [creditsButton];
        // List<UIElement> elements = [quitButton];

        container = new UIContainer(elements);
    }

    int tick = 0;
    public override bool RunTick() {
        tick++;
        return true;
    }

    public override void Render() {
        if (tick % 60 == 0)
            NCurses.MoveWindowAddString(CurrentWin, 0, 0, $"ticks {tick}, seconds {tick / 60}");

        NCurses.WindowRefresh(CurrentWin);
    }

    public override void Finish() {
        NCurses.DeleteWindow(CurrentWin);
    }
}

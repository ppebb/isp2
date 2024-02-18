using Blackguard.UI.Elements;
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

    private static readonly string[] Settings = {
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
        CurrentWin = Window.NewFullScreenWindow("Main Menu");

        UIText logoText = new(Logo);
        UIButton startButton = new(Start, () => { });
        UIButton settingsButton = new(Settings, () => { });
        UIButton creditsButton = new(Credits, () => { });
        UIButton quitButton = new(Quit, () => { });

        container = new UIContainer(Alignment.Center, logoText, startButton, settingsButton, creditsButton, quitButton) { Selected = true };
    }

    int tick = 0;
    public override bool RunTick() {
        ProcessInput();

        tick++;
        return true;
    }

    public override void Render() {
        NCurses.MoveWindowAddString(CurrentWin.handle, 0, 0, $"ticks {tick}, seconds {tick / 60}");

        container.Render(CurrentWin.handle, 0, 0, CurrentWin.w, CurrentWin.h);
    }

    public override void Finish() {
        NCurses.DeleteWindow(CurrentWin.handle);
    }
}

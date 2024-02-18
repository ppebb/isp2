using Blackguard.UI.Elements;
using Mindmagma.Curses;

namespace Blackguard.UI.Scenes;

public class MainMenuScene : Scene {
    private static readonly string[] Logo = {
        "██████╗░██╗░░░░░░█████╗░░█████╗░██╗░░██╗░██████╗░██╗░░░██╗░█████╗░██████╗░██████╗░",
        "██╔══██╗██║░░░░░██╔══██╗██╔══██╗██║░██╔╝██╔════╝░██║░░░██║██╔══██╗██╔══██╗██╔══██╗",
        "██████╦╝██║░░░░░███████║██║░░╚═╝█████═╝░██║░░██╗░██║░░░██║███████║██████╔╝██║░░██║",
        "██╔══██╗██║░░░░░██╔══██║██║░░██╗██╔═██╗░██║░░╚██╗██║░░░██║██╔══██║██╔══██╗██║░░██║",
        "██████╦╝███████╗██║░░██║╚█████╔╝██║░╚██╗╚██████╔╝╚██████╔╝██║░░██║██║░░██║██████╔╝",
        "╚═════╝░╚══════╝╚═╝░░╚═╝░╚════╝░╚═╝░░╚═╝░╚═════╝░░╚═════╝░╚═╝░░╚═╝╚═╝░░╚═╝╚═════╝░"
    };

    private static readonly string[] Start = {
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

    private static readonly string[] Credits = {
        "▄▀▀▀▄ █▀▀▄  █▀▀▀▀ █▀▀▄  ▀▀█▀▀ ▀▀█▀▀ ▄▀▀▀▄",
        "█     █▄▄▀  █▄▄▄  █   █   █     █   ▀▄▄▄ ",
        "█   ▄ █  █  █     █  ▄▀   █     █   ▄   █",
        " ▀▀▀  ▀   ▀ ▀▀▀▀▀ ▀▀▀   ▀▀▀▀▀   ▀    ▀▀▀ "
    };

    private static readonly string[] Quit = {
        "▄▀▀▀▄ █   █ ▀▀█▀▀ ▀▀█▀▀",
        "█   █ █   █   █     █  ",
        "█ ▀▄▀ █   █   █     █  ",
        " ▀▀ ▀  ▀▀▀  ▀▀▀▀▀   ▀  "
    };

    private bool shouldExit = false;

    public MainMenuScene() {
        CurrentWin = Window.NewFullScreenWindow("Main Menu");

        UIText logoText = new(Logo);
        UIButton startButton = new(Start, () => { });
        UIButton settingsButton = new(Settings, () => { });
        UIButton creditsButton = new(Credits, () => { });
        UIButton quitButton = new(Quit, () => { shouldExit = true; });

        container = new UIContainer(Alignment.Center, logoText, startButton, settingsButton, creditsButton, quitButton) { Selected = true };
    }

    public override bool RunTick(Game state) {
        ProcessInput(state);
        return !shouldExit;
    }

    public override void Render(Game state) {
        container.Render(CurrentWin.handle, 0, 0, CurrentWin.w, CurrentWin.h);
    }

    public override void Finish() {
        NCurses.DeleteWindow(CurrentWin.handle);
    }
}

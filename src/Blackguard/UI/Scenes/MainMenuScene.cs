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
    /* private static string[] Start = { */
    /*     "▄▀▀▀▄ ▀▀█▀▀  ▄█▄  █▀▀▄  ▀▀█▀▀", */
    /*     "▀▄▄▄    █    █ █  █▄▄▀    █  ", */
    /*     "▄   █   █   █▀▀▀█ █  █    █  ", */
    /*     " ▀▀▀    ▀   ▀   ▀ ▀   ▀   ▀  " */
    /* }; */
    /* private static readonly string[] Settings = { */
    /*     "▄▀▀▀▄ █▀▀▀▀ ▀▀█▀▀ ▀▀█▀▀ ▀▀█▀▀ ██  █ ▄▀▀▀▄ ▄▀▀▀▄", */
    /*     "▀▄▄▄  █▄▄▄    █     █     █   █ █ █ █     ▀▄▄▄ ", */
    /*     "▄   █ █       █     █     █   █ ▀▄█ █  ▀█ ▄   █", */
    /*     " ▀▀▀  ▀▀▀▀▀   ▀     ▀   ▀▀▀▀▀ ▀  ▀▀  ▀▀▀   ▀▀▀ " */
    /* }; */
    /* private static string[] Credits = { */
    /*     "▄▀▀▀▄ █▀▀▄  █▀▀▀▀ █▀▀▄  ▀▀█▀▀ ▀▀█▀▀ ▄▀▀▀▄", */
    /*     "█     █▄▄▀  █▄▄▄  █   █   █     █   ▀▄▄▄ ", */
    /*     "█   ▄ █  █  █     █  ▄▀   █     █   ▄   █", */
    /*     " ▀▀▀  ▀   ▀ ▀▀▀▀▀ ▀▀▀   ▀▀▀▀▀   ▀    ▀▀▀ " */
    /* }; */
    /* private static string[] Quit = { */
    /*     "▄▀▀▀▄ █   █ ▀▀█▀▀ ▀▀█▀▀", */
    /*     "█   █ █   █   █     █  ", */
    /*     "█ ▀▄▀ █   █   █     █  ", */
    /*     " ▀▀ ▀  ▀▀▀  ▀▀▀▀▀   ▀  " */
    /* }; */

    public MainMenuScene() {
        CurrentWin = Window.NewFullScreenWindow();

        UIText logoText = new(Logo);
        // Support for multiline buttons will come soon!
        UIButton startButton = new("Start", () => {  });
        UIButton settingsButton = new("Settings", () => {  });
        UIButton creditsButton = new("Credits", () => {  });
        UIButton quitButton = new("Quit", () => {  });

        container = new UIContainer(Alignment.Center, logoText, startButton, settingsButton, creditsButton, quitButton);
    }

    int tick = 0;
    public override bool RunTick() {
        tick++;
        return true;
    }

    public override void Render() {
        /* if (tick % 60 == 0) */
        /*     NCurses.MoveWindowAddString(CurrentWin.handle, 0, 0, $"ticks {tick}, seconds {tick / 60}"); */

        container.Render(CurrentWin.handle, 0, 0, CurrentWin.w, CurrentWin.h);
    }

    public override void Finish() {
        NCurses.DeleteWindow(CurrentWin.handle);
    }
}

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

    public MainMenuScene() {
        UIText logoText = new(Logo);
        UIButton startButton = new(Start, () => { });
        UIButton settingsButton = new(Settings, () => { });
        UIButton creditsButton = new(Credits, () => { });
        UIButton quitButton = new(Quit, () => { });


        List<UIElement> elements = [logoText, startButton, settingsButton, creditsButton, quitButton];

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

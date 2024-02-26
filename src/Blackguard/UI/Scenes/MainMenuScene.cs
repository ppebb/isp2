using Blackguard.UI.Elements;
using Blackguard.Utilities;

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

    private bool shouldExit = false;

    public MainMenuScene() {
        CurrentWin = Window.NewFullScreenWindow("Main Menu", Highlight.Text);

        UISpace topSpace = new(0, 10);
        UIText logoText = new(Logo);
        UIButton startButton = new("Start".ToLargeText(), () => { });
        UIButton settingsButton = new("Settings".ToLargeText(), () => { });
        UIButton creditsButton = new("Credits".ToLargeText(), () => { });
        UIButton quitButton = new("Quit".ToLargeText(), () => { shouldExit = true; });
        UISpace bottomSpace = new(0, 10);

        container = new UIContainer(Alignment.Center | Alignment.Fill, topSpace, logoText, startButton, settingsButton, creditsButton, quitButton, bottomSpace) { Selected = true };
    }

    public override bool RunTick(Game state) {
        ProcessInput(state);
        return !shouldExit;
    }

    public override void Render(Game state) {
        container.Render(CurrentWin, 0, 0, CurrentWin.w, CurrentWin.h);
    }

    public override void Finish() {
        CurrentWin.Dispose();
    }
}

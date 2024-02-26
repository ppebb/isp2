using System;
using System.IO;
using Blackguard.UI.Elements;

namespace Blackguard.UI.Scenes;

public class PlayerSelectionScene : Scene {
    private readonly Action<Game, Player> selectCallback = (s, p) => {
        s.Player = p;
        s.ForwardScene<WorldSelectionScene>();
    };

    public PlayerSelectionScene() {
        container = new UIContainer(Alignment.Center);

        container.Add(new UISpace(0, 10));
        container.Add(new UIText(["Select a Character"]));

        foreach (string playerFile in Directory.GetFiles(Game.PlayerPath)) {
            Player? player = Player.Deserialize(playerFile);
            if (player == null)
                continue;

            // I kind of hate just setting the player in a callback, but oh well
            container.Add(new UIPlayer(player, selectCallback));
        }

        container.Add(new UIButton(["Create New Character"], (s) => s.ForwardScene<PlayerCreationScene>((data) => {
            if (data != null)
                container.Add(new UIPlayer((Player)data, selectCallback));
        })));

        container.Add(new UIButton(["Back"], (s) => {
            s.Player = null!; // Unset the player since it was unselected
            s.PrevScene();
        }));

        container.SelectFirstSelectable();
    }

    public override bool RunTick(Game state) {
        ProcessInput(state);
        return true;
    }

    public override void Render(Game state) {
        container.Render(state.CurrentWin, 0, 0, state.CurrentWin.w, state.CurrentWin.h);
    }
}

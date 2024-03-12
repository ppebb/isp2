using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blackguard.UI.Elements;
using Blackguard.Utilities;

namespace Blackguard.UI.Scenes;

public class PlayerSelectionScene : Scene {
    private readonly UIText noneFound = new("No Players Found".ToLargeText(), Alignment.Center);

    private readonly UIContainer playerList;

    private readonly Action<Game, Player> selectCallback = (s, p) => {
        s.Player = p;
        s.ForwardScene<WorldSelectionScene>();
    };

    public PlayerSelectionScene() {
        container = new UIContainer(Alignment.Center);
        playerList = new UIContainer(Alignment.Center) {
            Comparer = Comparer<UIElement>.Default,
            Height = 30,
            Border = true
        };

        container.Add(new UISpace(0, 10));
        container.Add(new UIText("Select a Player".ToLargeText()));
        container.Add(playerList);

        IEnumerable<string> files = Directory.GetFiles(Game.PlayerPath).Where((f) => Path.GetExtension(f) == ".plr");

        if (files.Any()) {
            foreach (string file in files) {
                Player? player = Player.Deserialize(file);
                if (player == null)
                    continue;

                // I kind of hate just setting the player in a callback, but oh well
                playerList.Add(new UIPlayer(player, selectCallback));
            }
        }
        else
            container.Add(noneFound);

        container.Add(new UIButton("Create New Player".ToLargeText(), (s) => s.ForwardScene<PlayerCreationScene>((data) => {
            if (data != null) {
                Player created = (Player)data;
                created.Serialize();
                playerList.Add(new UIPlayer(created, selectCallback));
                container.Remove(noneFound);
            }
        })));

        container.Add(new UIButton("Back".ToLargeText(), (s) => {
            s.Player = null!; // Unset the player since it was unselected
            container.SelectFirstSelectable();
            s.PrevScene();
        }));

        container.Select();
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

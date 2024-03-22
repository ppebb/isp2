using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blackguard.UI.Elements;
using Blackguard.UI.Popups;
using Blackguard.Utilities;
using Mindmagma.Curses;

namespace Blackguard.UI.Scenes;

public class WorldSelectionScene : Scene {
    private readonly UIText noneFound = new("No Worlds Found".ToLargeText(), Alignment.Center);

    private readonly UIContainer worldList;

    private readonly Action<Game, World> selectCallback = (s, w) => {
        s.World = w;
        s.World.Initialize(s);
        s.inGame = true;
        s.ForwardScene<GameScene>();
    };

    public WorldSelectionScene() {
        container = new UIContainer(Alignment.Center);
        worldList = new UIContainer(Alignment.Center) {
            Comparer = Comparer<UIElement>.Default,
            Height = 30,
            Border = true
        };

        container.Add(new UISpace(0, 10));
        container.Add(new UIText("Select a World".ToLargeText()));
        container.Add(worldList);

        IEnumerable<string> directories = Directory.GetDirectories(Game.WorldsPath);

        if (directories.Any()) {
            foreach (string directory in directories) {
                World? world = World.Deserialize(Path.Combine(directory, "meta"));
                if (world == null)
                    continue;

                // I kind of hate just setting the world in a callback, but oh well
                worldList.Add(new UIWorld(world, selectCallback));
            }
        }
        else
            container.Add(noneFound);

        container.Add(new UIButton("Create New World".ToLargeText(), (s) => s.ForwardScene<WorldCreationScene>((data) => {
            if (data != null) {
                World created = (World)data;
                worldList.Add(new UIWorld(created, selectCallback));
                container.Remove(noneFound);
            }
        })));

        container.Add(new UIButton("Back".ToLargeText(), (s) => {
            s.World = null!; // Unset the world since it was unselected
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

    public override void ProcessInput(Game state) {
        base.ProcessInput(state);

        if (state.Input.KeyPressed(CursesKey.DELETEKEY) && container.GetSelectedElement() is UIContainer) {
            UIWorld w = (UIWorld)worldList.GetSelectedElement();
            state.OpenPopup(
                new ConfirmationPopup(
                    "DeletePlayerConfirmation",
                    [$"Are you sure you want to delete the player {w.World.Name}"],
                    null,
                    (_) => {
                        w.World.Delete();
                        worldList.Remove(w);

                        if (!worldList.SelectFirstSelectable())
                            container.SelectFirstSelectable();

                    }
            ),
                true);
        }
    }

    public override void Render(Game state) {
        container.Render(state.CurrentPanel, 0, 0, state.CurrentPanel.w, state.CurrentPanel.h);
    }
}

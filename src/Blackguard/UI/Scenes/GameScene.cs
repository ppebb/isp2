using System.Numerics;
using Blackguard.Utilities;

namespace Blackguard.UI.Scenes;

public class GameScene : Scene {
    public override bool RunTick(Game state) {
        if (!Focused)
            return true;

        state.Player.RunTick(state);
        state.World.RunTick(state);

        return true;
    }

    public override void Render(Game state) {
        Player player = state.Player;
        Vector2 screenPos = Utils.ToScreenPos(state.ViewOrigin, player.Position);

        state.World.Render(state.CurrentPanel, state, state.CurrentPanel.w, state.CurrentPanel.h);

        state.Player.Render(state.CurrentPanel, (int)screenPos.X, (int)screenPos.Y);
    }
}

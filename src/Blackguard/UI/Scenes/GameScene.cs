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

        state.World.Render(state.CurrentWin, state, 0, 0, state.CurrentWin.w, state.CurrentWin.h);

        state.Player.Render(state.CurrentWin, (int)screenPos.X, (int)screenPos.Y);
    }
}

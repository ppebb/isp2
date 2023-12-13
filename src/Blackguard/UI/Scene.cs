namespace Blackguard.UI;

public abstract class Scene {
    public abstract nint CurrentWin { get; protected set; }

    // Returns false to exit the game
    public abstract bool RunTick();

    public abstract void Render();

    public abstract void Finish();
}

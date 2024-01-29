namespace Blackguard.UI.Scenes;

public abstract class Scene {
    public nint CurrentWin { get; protected set; }

    // Should be defined in the constructor inheriting Scene. Default container for the Scene where elements should be stored
    protected UIContainer container = null!;

    // Returns false to exit the game
    public abstract bool RunTick();

    public abstract void Render();

    public abstract void Finish();
}

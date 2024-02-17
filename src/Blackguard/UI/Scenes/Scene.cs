using Mindmagma.Curses;

namespace Blackguard.UI.Scenes;

public abstract class Scene : ISizeProvider {
    public Window CurrentWin { protected set; get; }

    // Should be defined in the constructor inheriting Scene. Default container for the Scene where elements should be stored
    protected UIContainer container;

    public (int, int) GetSize() {
        return (CurrentWin.w, CurrentWin.h);
    }

    public (int, int) GetOffset() {
        return (CurrentWin.x, CurrentWin.y);
    }

    // Returns false to exit the game
    public abstract bool RunTick();

    public abstract void Render();

    public abstract void Finish();

    // Default impl handles navigating various UI Elements. For the game's main view it should not need to use this.
    private const int DEBOUNCETICKS = 30;
    private int debounceTimer = DEBOUNCETICKS + 1; // Just needs a default value above DEBOUNCETICKS
    private int lastKey;

    public virtual void ProcessInput() {
        container.ProcessInput();

        if (InputHandler.KeyPressed(CursesKey.DOWN) && debounceTimer > DEBOUNCETICKS) {
            container.Next(true);
            lastKey = CursesKey.DOWN;
            debounceTimer = 0;
        }
        else if (!InputHandler.KeyPressed(CursesKey.DOWN) && lastKey == CursesKey.DOWN)
            debounceTimer = DEBOUNCETICKS + 1; // Set it to something above DEBOUNCETICKS so that the key can be pressed again if someone is rapidly pressing

        if (InputHandler.KeyPressed(CursesKey.UP) && debounceTimer > DEBOUNCETICKS) {
            container.Prev(true);
            lastKey = CursesKey.UP;
            debounceTimer = DEBOUNCETICKS + 1;
        }
        else if (!InputHandler.KeyPressed(CursesKey.UP) && lastKey == CursesKey.UP)
            debounceTimer = 100;

        // Can add left and right eventually if planning to support more than just vertical menus

        debounceTimer++;
    }
}

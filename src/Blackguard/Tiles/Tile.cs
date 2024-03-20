namespace Blackguard.Tiles;

public struct Tile {
    public TileDefinition Type { get; private set; }

    public bool Foreground;

    public Tile(TileDefinition type, bool foreground = true) {
        Type = type;
        Foreground = foreground;
    }
}

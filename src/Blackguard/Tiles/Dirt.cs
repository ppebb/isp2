using Blackguard.Utilities;

namespace Blackguard.Tiles;

public class Dirt : TileDefinition {
    public Dirt() {
        Id = "Dirt";
        Glyph = '#';
        Highlight = Highlight.Dirt;
    }
}

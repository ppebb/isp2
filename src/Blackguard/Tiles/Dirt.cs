using Blackguard.Utilities;

namespace Blackguard.Tiles;

public class Dirt : TileDefinition {
    public Dirt() {
        Name = "Dirt";
        Glyph = '#';
        Highlight = Highlight.Dirt;
    }
}

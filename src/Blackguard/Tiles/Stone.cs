using Blackguard.Utilities;

namespace Blackguard.Tiles;

public class Stone : TileDefinition {
    public Stone() {
        Name = "Stone";
        Glyph = '#';
        Highlight = Highlight.Stone;
    }
}

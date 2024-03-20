using Blackguard.Utilities;

namespace Blackguard.Tiles;

public class Stone : TileDefinition {
    public Stone() {
        Id = "Stone";
        Glyph = '#';
        Highlight = Highlight.Stone;
    }
}

using Blackguard.Utilities;

namespace Blackguard.Tiles;

public class Grass : TileDefinition {
    public Grass() {
        Id = "Grass";
        Glyph = '#';
        Highlight = Highlight.Grass;
    }
}

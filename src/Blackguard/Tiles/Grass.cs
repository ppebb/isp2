using Blackguard.Utilities;

namespace Blackguard.Tiles;

public class Grass : TileDefinition {
    public Grass() {
        Name = "Grass";
        Glyph = '#';
        Highlight = Highlight.Grass;
    }
}

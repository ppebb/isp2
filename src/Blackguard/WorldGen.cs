using System.Numerics;
using Blackguard.Tiles;

namespace Blackguard;

public static class WorldGen {
    public static Chunk GenChunk(string basePath, Vector2 position) {
        Chunk ret = new(position);

        // TODO: Actual noise-based worldgen
        for (int i = 0; i < 20; i++) {
            for (int j = 0; j < 20; j++) {
                if ((position.X + position.Y) % 2 == 0)
                    ret.Tiles[i, j] = new Tile(TileDefinition.GetTileDefinition<Grass>(), false);
                else
                    ret.Tiles[i, j] = new Tile(TileDefinition.GetTileDefinition<Dirt>(), false);
            }
        }

        ret.Serialize(basePath);
        return ret;
    }
}

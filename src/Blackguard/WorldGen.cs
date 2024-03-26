using System;
using System.Numerics;
using Blackguard.Tiles;

namespace Blackguard;

public class WorldGen {
    private readonly Random rand;

    public WorldGen(int seed) {
        rand = new(seed);
    }

    public Chunk GenChunk(string basePath, Vector2 position) {
        Chunk ret = new(position);

        // TODO: Actual noise-based worldgen
        for (int i = 0; i < Chunk.CHUNKSIZE; i++) {
            for (int j = 0; j < Chunk.CHUNKSIZE; j++) {
                if ((position.X + position.Y) % 2 == 0)
                    ret.Tiles[i, j] = new Tile(Registry.GetDefinition<Grass>(), false);
                else
                    ret.Tiles[i, j] = new Tile(Registry.GetDefinition<Dirt>(), false);
            }
        }

        while (rand.NextSingle() > 0.5) {
            int cx = (int)(rand.NextSingle() * Chunk.CHUNKSIZE);
            int cy = (int)(rand.NextSingle() * Chunk.CHUNKSIZE);

            ret.Tiles[cx, cy] = new Tile(Registry.GetDefinition<Stone>(), true);
        }

        ret.Serialize(basePath);
        return ret;
    }
}

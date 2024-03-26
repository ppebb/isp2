using System.IO;
using System.IO.Compression;
using System.Numerics;
using Blackguard.Tiles;
using Blackguard.UI;
using Blackguard.Utilities;

namespace Blackguard;

public class Chunk {
    public const int CHUNKSIZE = 20;
    public readonly Vector2 Position;
    public Vector2 WorldPosition => Position * CHUNKSIZE;

    public Tile[,] Tiles = new Tile[CHUNKSIZE, CHUNKSIZE];

    public Chunk(Vector2 position) {
        Position = position;
    }

    public void Render(Drawable drawable, int x, int y, int skipx = 0, int skipy = 0, bool border = false) {
        int endi = skipx < 0 ? CHUNKSIZE + skipx : CHUNKSIZE;
        int endj = skipy < 0 ? CHUNKSIZE + skipy : CHUNKSIZE;

        for (int i = skipx > 0 ? skipx : 0; i < endi; i++) {
            for (int j = skipy > 0 ? skipy : 0; j < endj; j++) {
                Tile t = Tiles[i, j]; // This is flipped for some reason?
                drawable.AddCharWithHighlight(t.Type.Highlight, x + i, y + j, t.Type.Glyph);
            }
        }

        if (border)
            drawable.DrawBorder(Highlight.TextError, x, y, CHUNKSIZE, CHUNKSIZE, skipx, skipy);
    }

    public void Serialize(string basePath) {
        using FileStream uncompressed = new(Path.Combine(basePath, $"{Position.X}:{Position.Y}.chunk"), FileMode.OpenOrCreate);
        using DeflateStream compressor = new(uncompressed, CompressionLevel.Optimal);
        using BinaryWriter w = new(compressor);

        foreach (Tile t in Tiles) {
            w.Write(t.Foreground);
            w.Write(t.Type.Id);
        }
    }

    public static Chunk? Deserialize(string basePath, Vector2 position) {
        string path = Path.Combine(basePath, $"{position.X}:{position.Y}.chunk");
        if (!File.Exists(path))
            return null;

        using FileStream compressed = new(path, FileMode.Open);
        using DeflateStream decompressor = new(compressed, CompressionMode.Decompress);
        using BinaryReader r = new(decompressor);

        Chunk chunk = new(position);

        for (int i = 0; i < CHUNKSIZE; i++) {
            for (int j = 0; j < CHUNKSIZE; j++) {
                bool fg = r.ReadBoolean();
                int id = r.ReadInt32();

                chunk.Tiles[i, j] = new Tile(Registry.GetDefinition<TileDefinition>(id), fg);
            }
        }

        return chunk;
    }
}

using System.IO;
using System.Numerics;
using Blackguard.Tiles;
using Blackguard.UI;
using Blackguard.Utilities;

namespace Blackguard;

public class Chunk {
    public readonly Vector2 Position;
    private static Vector2 Twenty = new(20, 20);
    public Vector2 WorldPosition => Position * Twenty;

    public Tile[,] Tiles = new Tile[20, 20];

    public Chunk(Vector2 position) {
        Position = position;
    }

    public void Render(Drawable drawable, int x, int y, int skipx = 0, int skipy = 0, bool border = false) {
        int endi = skipx < 0 ? 20 + skipx : 20;
        int endj = skipy < 0 ? 20 + skipy : 20;

        for (int i = skipx > 0 ? skipx : 0; i < endi; i++) {
            for (int j = skipy > 0 ? skipy : 0; j < endj; j++) {
                Tile t = Tiles[i, j];
                drawable.AddCharWithHighlight(t.Type.Highlight, x + i, y + j, t.Type.Glyph);
            }
        }

        if (border) {
            try {
                drawable.DrawBorder(Highlight.TextError, x, y, 20, 20);
            }
            catch { }
        }
    }

    public void Serialize(string basePath) {
        using FileStream fs = new(Path.Combine(basePath, $"{Position.X}:{Position.Y}.chunk"), FileMode.OpenOrCreate);
        using BinaryWriter w = new(fs);

        foreach (Tile t in Tiles) {
            w.Write(t.Foreground);
            w.Write(t.Type.Id);
        }
    }

    public static Chunk? Deserialize(string basePath, Vector2 position) {
        string path = Path.Combine(basePath, $"{position.X}:{position.Y}.chunk");
        if (!File.Exists(path))
            return null;

        using FileStream fs = new(path, FileMode.Open);
        using BinaryReader r = new(fs);

        Chunk chunk = new(position);

        for (int i = 0; i < 20; i++) {
            for (int j = 0; j < 20; j++) {
                bool fg = r.ReadBoolean();
                string id = r.ReadString();

                chunk.Tiles[i, j] = new Tile(TileDefinition.GetTileDefinition(id), fg);
            }
        }

        return chunk;
    }
}

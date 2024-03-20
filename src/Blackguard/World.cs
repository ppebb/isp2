using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Blackguard.UI;
using Blackguard.Utilities;
using Newtonsoft.Json;

namespace Blackguard;

[JsonObject(MemberSerialization.OptIn)]
public class World {
    [JsonProperty]
    public string Name { get; private set; }

    [JsonProperty]
    public DateTime CreationDate { get; private set; }

    [JsonProperty]
    public TimeSpan Playtime { get; private set; }

    public string BaseSavePath => Path.Combine(Game.WorldsPath, Name);
    public string MetaSavePath => Path.Combine(BaseSavePath, "meta");
    public string ChunksPath => Path.Combine(BaseSavePath, "Chunks");

    public Dictionary<Vector2, Chunk> ChunksByPosition;

    public World(string name) {
        Name = name;
        CreationDate = DateTime.Now;
        Playtime = TimeSpan.Zero;
        ChunksByPosition = new();
    }

    public static World CreateNew(string name) {
        World world = new(name);

        world.Serialize();
        world.LoadChunks(Vector2.Zero);

        return world;
    }

    private void LoadChunks(Vector2 center) {
        // TODO: Determine simulation distance based on screen size

        // Load 5 chunks on all sides of the player
        for (int i = -5; i <= 5; i++) {
            for (int j = -5; j <= 5; j++) {
                Vector2 position = new(center.X + i, center.Y + j);

                if (!ChunksByPosition.ContainsKey(position))
                    ChunksByPosition.Add(position, Chunk.Deserialize(ChunksPath, position) ?? WorldGen.GenChunk(ChunksPath, position));
            }
        }
    }

    public void Initialize(Game state) {
        LoadChunks(state.Player.ChunkPosition);
    }

    public void RunTick(Game state) {
        // Remove faraway chunks
        foreach ((Vector2 position, Chunk chunk) in ChunksByPosition) {
            if (Math.Abs(position.X - state.Player.ChunkPosition.X) > 5 || Math.Abs(position.Y - state.Player.ChunkPosition.Y) > 5) {
                ChunksByPosition.Remove(key: position);
                chunk.Serialize(ChunksPath);
            }
        }

        LoadChunks(state.Player.ChunkPosition);
    }

    public void Render(Drawable drawable, Game state, int x, int y, int maxw, int maxh) {
        foreach ((_, Chunk chunk) in ChunksByPosition) {
            Vector2 sp = Utils.ToScreenPos(state.ViewOrigin, chunk.WorldPosition);
            int rx = (int)sp.X;
            int ry = (int)sp.Y;
            if (!Utils.CheckOutOfBounds(rx, ry, 20, 20, maxw, maxh, out int byX, out int byY))
                chunk.Render(drawable, rx, ry, 0, 0, state.drawChunkOutline);
            else if (Math.Abs(byX) < 20 && Math.Abs(byY) < 20)
                chunk.Render(drawable, rx, ry, byX, byY, state.drawChunkOutline);
        }
    }

    public void Serialize() {
        Directory.CreateDirectory(BaseSavePath);
        Directory.CreateDirectory(ChunksPath);

        string json = JsonConvert.SerializeObject(this);

        File.WriteAllText(MetaSavePath, json);
    }

    public static World? Deserialize(string path) {
        string json = File.ReadAllText(path);

        return JsonConvert.DeserializeObject<World>(json);
    }

    public void Delete() {
        Directory.Delete(BaseSavePath, true);
    }
}

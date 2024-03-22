using System;
using System.IO;
using System.Numerics;
using Blackguard.Tiles;
using Blackguard.UI;
using Blackguard.Utilities;
using Mindmagma.Curses;
using Newtonsoft.Json;
using static Blackguard.Game;

namespace Blackguard;

[JsonObject(MemberSerialization.OptIn)]
public class Player : ISizeProvider {
    // Serialized state
    [JsonProperty]
    public string Name { get; private set; }

    [JsonProperty]
    public DateTime CreationDate { get; private set; }

    [JsonProperty]
    public TimeSpan Playtime { get; private set; }

    [JsonProperty]
    public PlayerType PlayerType;

    [JsonProperty]
    public RaceType Race;

    [JsonProperty]
    public Vector2 Position;

    // Other stuff
    public string SavePath => Path.Combine(PlayersPath, Name + ".plr");
    public string Glyph { get; private set; }
    public Highlight Highlight { get; private set; }
    public Vector2 ChunkPosition => new((float)Math.Floor(Position.X / Chunk.CHUNKSIZE), (float)Math.Floor(Position.Y / Chunk.CHUNKSIZE));

    // Stats
    public int MaxMana;
    public int MaxHealth;
    public int MaxSpeed;
    public double BluntEffect;
    public double SlashEffect;
    public double PierceEffect;
    public double MagicEffect;
    public double BaseEffect;
    public double FireEffect;
    public double ElectricityEffect;
    public double IceEffect;
    public double WaterEffect;
    public double EarthEffect;
    public double MindEffect;
    public int Health;
    public int Mana;
    public int Speed;

    public Player(string name, PlayerType type, RaceType race) {
        Name = name;
        CreationDate = DateTime.Now;
        Playtime = TimeSpan.Zero;
        PlayerType = type;
        Race = race;
        Glyph = "#";
    }

    public static Player CreateNew(string name, PlayerType type, RaceType race) {
        Player player = new(name, type, race);

        player.Serialize();

        return player;
    }

    public void Initialize(Game state) {
        HandleTermResize(state);
    }

    public void RunTick(Game state) {
        ProcessInput(state);
    }

    private void ProcessInput(Game state) {
        InputHandler input = state.Input;

        int changeX = 0;
        int changeY = 0;

        if (input.KeyPressed('w'))
            changeY--;

        if (input.KeyPressed('a'))
            changeX--;

        if (input.KeyPressed('s'))
            changeY++;

        if (input.KeyPressed('d'))
            changeX++;

        if (changeX != 0 || changeY != 0) {
            int nPosX = (int)Position.X + changeX;
            int nPosY = (int)Position.Y + changeY;

            Tile? next = state.World.GetTile(new Vector2(nPosX, nPosY));
            if (next is not null && !next.Value.Foreground) {
                Position.X = nPosX;
                Position.Y = nPosY;
                state.ViewOrigin.X += changeX;
                state.ViewOrigin.Y += changeY;
            }
        }
    }

    public void Render(Drawable drawable, int x, int y) {
        drawable.AddLinesWithHighlight((Highlight, x, y, Glyph));
    }

    public void Serialize() {
        string json = JsonConvert.SerializeObject(this);

        File.WriteAllText(SavePath, json);
    }

    public static Player? Deserialize(string path) {
        string json = File.ReadAllText(path);

        return JsonConvert.DeserializeObject<Player>(json);
    }

    public void Delete() {
        File.Delete(SavePath);
    }

    public (int w, int h) GetSize() {
        return (1, 1); // May be expanded eventually depending on how the player is rendered
    }

    public void HandleTermResize(Game state) {
        state.ViewOrigin = new Vector2(
            (int)(Position.X - NCurses.Columns / 2),
            (int)(Position.Y - NCurses.Lines / 2)
        );
    }

}

public enum PlayerType {
    Knight,
    Archer,
    Mage,
    Barbarian,
}

public enum RaceType {
    Human,
    Ork,
    Elf,
    Dwarf,
    Demon,
    Gnome,
}

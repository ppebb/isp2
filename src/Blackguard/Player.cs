using System;
using System.IO;
using System.Numerics;
using Blackguard.UI;
using Blackguard.Utilities;
using Newtonsoft.Json;

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

    // Other stuff
    public string SavePath => Path.Combine(Game.PlayerPath, Name + ".plr");
    public string Glyph { get; private set; }
    public Vector2 Position { get; private set; }
    public Highlight Highlight { get; private set; }

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

    public (int w, int h) GetSize() {
        return (1, 1); // May be expanded eventually depending on how the player is rendered
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

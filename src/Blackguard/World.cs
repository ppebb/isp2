using System;
using System.IO;
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

    public string SavePath => Path.Combine(Game.WorldPath, Name + ".wld");

    public World(string name) {
        Name = name;
        CreationDate = DateTime.Now;
        Playtime = TimeSpan.Zero;
    }

    public void Serialize() {
        string json = JsonConvert.SerializeObject(this);

        File.WriteAllText(SavePath, json);
    }

    public static World? Deserialize(string path) {
        string json = File.ReadAllText(path);

        return JsonConvert.DeserializeObject<World>(json);
    }
}

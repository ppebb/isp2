using System.IO;
using Newtonsoft.Json;

namespace Blackguard;

[JsonObject(MemberSerialization.OptIn)]
public class Config {
    public static string ConfigPath => Path.Combine(Program.Platform.DataPath(), "config");

    [JsonProperty]
    public bool SetFontAuto;

    public void Serialize() {
        string json = JsonConvert.SerializeObject(this);

        File.WriteAllText(ConfigPath, json);
    }

    public static Config? Deserialize() {
        if (!File.Exists(ConfigPath))
            return null;

        string json = File.ReadAllText(ConfigPath);

        return JsonConvert.DeserializeObject<Config>(json);
    }
}

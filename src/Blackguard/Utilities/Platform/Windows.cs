using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Hashing;
using System.Linq;
using System.Reflection;

namespace Blackguard.Utilities.Platform;

public class Windows : Platform {
    public override string CachePath() {
        return Path.Combine(DataPath(), "cache");
    }

    public override void Configure() {
        return; // Nothing to configure
    }

    public override string DataPath() {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "blackguard");
    }

    private List<string>? cached = null;
    // TODO: Generalize extraction code
    public override List<string> ExtractEmbeddedResources() {
        if (cached != null)
            return cached;

        List<string> ret = new();

        Assembly assembly = Assembly.GetAssembly(typeof(Platform))!;
        string[] resourceNames = assembly.GetManifestResourceNames();

        XxHash64 hasher = new();

        // Get the Hashes file into a string
        using Stream? hashesStream = assembly.GetManifestResourceStream(resourceNames.First(n => n.Contains("Hashes"))) ?? throw new IOException("Unable to initialize stream for Hashes resource");
        using StreamReader reader = new(hashesStream);
        string res = reader.ReadToEnd();

        Dictionary<string, ulong> hashes = new();

        // This can be hardcoded because the hashes file should always generate like this
        foreach (string line in res.TrimEnd().Split('\n')) {
            string[] split = line.Split(' ');
            hashes.Add(string.Join(' ', split[..^1]), ulong.Parse(split[^1]));
        }

        foreach (string resource in resourceNames) {
            if (resource == "Blackguard.Resources.Windows.Hashes")
                continue;

            // Hardcoded for windows for now, can be generalized if natives are needed fro any other platform
            string path = Path.Combine(CachePath(), resource.Replace("Blackguard.Resources.Windows.", string.Empty));
            if (File.Exists(path)) {
                byte[] buffer = new byte[8192];
                using FileStream fs = File.OpenRead(path);

                while (fs.Read(buffer, 0, buffer.Length) > 0)
                    hasher.Append(buffer);

                fs.Close();

                if (hasher.GetCurrentHashAsUInt64() == hashes[resource]) {
                    ret.Add(path);
                    continue;
                }

                // If the hashes don't match, delete the file
                File.Delete(path);
            }

            using Stream? resourceStream = assembly.GetManifestResourceStream(resource);
            if (resourceStream == null)
                continue;

            using FileStream fileStream = File.OpenWrite(path);
            resourceStream.CopyTo(fileStream);

            ret.Add(path);
        }

        cached = ret;

        return ret;
    }
}

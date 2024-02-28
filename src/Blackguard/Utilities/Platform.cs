using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Hashing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Blackguard.Utilities;

public abstract class Platform {
    public Platform() {
        Directory.CreateDirectory(DataPath());
        Directory.CreateDirectory(CachePath());
    }

    public static Platform GetPlatform() {
        if (OperatingSystem.IsWindows())
            return new Windows();
        else if (OperatingSystem.IsMacOS())
            throw new PlatformNotSupportedException("Unknown or unsupported platform!");
        else if (OperatingSystem.IsLinux())
            return new Linux();

        throw new PlatformNotSupportedException("Unknown or unsupported platform!");
    }

    public abstract string CachePath();

    public abstract string DataPath();

    public abstract void Configure();

    public abstract List<string> ExtractNativeDependencies();

    public class Linux : Platform {
        [DllImport("libc", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern void setlocale(int category, string locale);

        private const int LC_ALL = 6;

        public override string CachePath() {
            string? xdg = Environment.GetEnvironmentVariable("XDG_CACHE_HOME");

            if (!string.IsNullOrEmpty(xdg))
                return Path.Combine(xdg, "blackguard");
            else
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".cache", "blackguard");
        }

        public override void Configure() {
            setlocale(LC_ALL, "");
        }

        public override string DataPath() {
            string? xdg = Environment.GetEnvironmentVariable("XDG_DATA_HOME");

            if (!string.IsNullOrEmpty(xdg))
                return Path.Combine(xdg, "blackguard");
            else
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".local", "share", "blackguard");
        }

        public override List<string> ExtractNativeDependencies() {
            return new List<string>();
        }
    }

    public class Windows : Platform {
        public override string CachePath() {
            return Path.Combine(DataPath(), "cache");
        }

        public override void Configure() {
            return; // Nothing specific to configure on windows yet.
        }

        public override string DataPath() {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "blackguard");
        }

        public override List<string> ExtractNativeDependencies() {
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
            foreach (string line in res.Split('\n')) {
                string[] split = line.Split(' ');
                hashes.Add(split[0], ulong.Parse(split[1]));
            }

            foreach (string resource in resourceNames) {
                if (!resource.EndsWith(".dll"))
                    continue;

                string path = Path.Combine(CachePath(), resource.Replace("Blackguard.Resources.", string.Empty));
                if (File.Exists(path)) {
                    byte[] buffer = new byte[8192];
                    using FileStream fs = File.OpenRead(path);

                    while (fs.Read(buffer, 0, buffer.Length) > 0) {
                        hasher.Append(buffer);
                    }

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

            return ret;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

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

    public abstract List<string> ExtractNativeDependencies();

    public class Linux : Platform {
        public override string CachePath() {
            var xdg = Environment.GetEnvironmentVariable("XDG_CACHE_HOME");

            if (!string.IsNullOrEmpty(xdg))
                return Path.Combine(xdg, "blackguard");
            else
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".cache", "blackguard");
        }

        public override string DataPath() {
            var xdg = Environment.GetEnvironmentVariable("XDG_DATA_HOME");

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

        public override string DataPath() {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "blackguard");
        }

        public override List<string> ExtractNativeDependencies() {
            List<string> ret = new();

            Assembly assembly = Assembly.GetAssembly(typeof(Platform))!;
            foreach (string resource in assembly.GetManifestResourceNames()) {
                if (!resource.EndsWith(".dll"))
                    continue;

                string path = Path.Combine(CachePath(), resource.Replace("Blackguard.Resources.", string.Empty));
                if (File.Exists(path)) {
                    ret.Add(path);
                    continue;
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

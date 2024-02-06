using System;
using System.Collections.Generic;
using System.IO;
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

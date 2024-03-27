using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Blackguard.Utilities.Platform;

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

    public override List<string> ExtractEmbeddedResources() {
        return new List<string>();
    }
}

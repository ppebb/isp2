using System;
using System.Collections.Generic;
using System.IO;

namespace Blackguard.Utilities.Platform;

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

    public abstract List<string> ExtractEmbeddedResources();
}

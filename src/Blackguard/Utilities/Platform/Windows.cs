using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Hashing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Blackguard.Utilities.Platform;

public class Windows : Platform {
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    static extern bool GetCurrentConsoleFontEx(
        IntPtr consoleOutput,
        bool maximumWindow,
        ref CONSOLE_FONT_INFO_EX lpConsoleCurrentFontEx
    );

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool SetCurrentConsoleFontEx(
        IntPtr consoleOutput,
        bool maximumWindow,
        CONSOLE_FONT_INFO_EX consoleCurrentFontEx
    );

    private const int STD_OUTPUT_HANDLE = -11;
    private const int TMPF_TRUETYPE = 4;
    private const int LF_FACESIZE = 32;
    private static readonly IntPtr INVALID_HANDLE_VALUE = new(-1);

    public override string CachePath() {
        return Path.Combine(DataPath(), "cache");
    }

    private static bool PromptYN(string prompt) {
        Console.Write(prompt);

        bool ret = char.ToLowerInvariant(Console.ReadKey().KeyChar) == 'y';

        Console.WriteLine();

        return ret;
    }

    public override unsafe void Configure() {
        // https://github.com/dotnet/docs/blob/f2eadd634efab2b303185730c640f95b4c557088/docs/fundamentals/runtime-libraries/snippets/System/Console/Overview/csharp/setfont1.cs#L17
        string fontName = "MesloLGS NF";

        nint hnd = GetStdHandle(STD_OUTPUT_HANDLE);

        if (hnd == INVALID_HANDLE_VALUE)
            throw new Exception("Unable to get Console handle!");

        CONSOLE_FONT_INFO_EX info = new();
        info.cbSize = (uint)Marshal.SizeOf(info);

        if (!GetCurrentConsoleFontEx(hnd, false, ref info))
            throw new Exception("Unable to get current ConsoleFontEx!");

        string faceName = new(info.FaceName, 0, LF_FACESIZE);

        if (faceName == "Meslo LGS NF")
            return;

        if (!PromptYN($"Current console font is {faceName}, would you like to switch to the font bundled with the game? [y/n] "))
            return;

        if (!IsElevated() && !PromptYN("An elevated executable must be spawned to install the font, continue? [y/n] "))
            return;

        Console.WriteLine("Copying Melso LGS NF fonts using FontReg.exe");

        Process proc = new() {
            StartInfo = new ProcessStartInfo {
                Verb = "runas",
                FileName = Path.Combine(CachePath(), "FontReg.exe"),
                Arguments = "/copy",
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Hidden,
            }
        };

        proc.Start();
        proc.WaitForExit();

        CONSOLE_FONT_INFO_EX mesloInfo = new();
        mesloInfo.cbSize = (uint)Marshal.SizeOf(mesloInfo);
        mesloInfo.FontFamily = TMPF_TRUETYPE;
        nint ptr = new(mesloInfo.FaceName);
        Marshal.Copy(fontName.ToCharArray(), 0, ptr, fontName.Length);
        mesloInfo.dwFontSize = new COORD(info.dwFontSize.X, info.dwFontSize.Y);
        mesloInfo.FontWeight = info.FontWeight;

        SetCurrentConsoleFontEx(hnd, false, mesloInfo);

        Console.WriteLine("Set console font to Meslo LGS NF");
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

#pragma warning disable CA1416 // It's lying. This isn't reachable on all platforms
    public override bool IsElevated() {
        // https://github.com/dotnet/sdk/blob/v6.0.100/src/Cli/dotnet/Installer/Windows/WindowsUtils.cs#L38
        using WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }
#pragma warning restore CA1416

    [StructLayout(LayoutKind.Sequential)]
    internal struct COORD {
        internal short X;
        internal short Y;

        internal COORD(short x, short y) {
            X = x;
            Y = y;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CONSOLE_FONT_INFO_EX {
        internal uint cbSize;
        internal uint nFont;
        internal COORD dwFontSize;
        internal int FontFamily;
        internal int FontWeight;
        internal fixed char FaceName[LF_FACESIZE];
    }
}

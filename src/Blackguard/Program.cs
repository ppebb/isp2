using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using Blackguard.Utils;
using Mindmagma.Curses;

namespace Blackguard;

public class LibraryNames : CursesLibraryNames {
    public override bool ReplaceLinuxDefaults => false;

    public override List<string> NamesLinux => new List<string> { "libncursesw.so" };

    public override List<string> NamesWindows => Program.Platform.ExtractNativeDependencies();
}

public static class Program {
    public static Platform Platform { get; private set; }

    static Program() {
        Platform = Platform.GetPlatform();
    }

    public static void Main(string[] args) {
        // Any arg parsing we eventually implement should be here, before any initialization

        // Initialize console window
        nint screen = NCurses.InitScreen();
        NCurses.NoDelay(screen, true);
        NCurses.NoEcho();

        // Finish the program
        NCurses.EndWin();
    }
}

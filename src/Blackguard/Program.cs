using System;
using System.Collections.Generic;
using Blackguard.Utilities;
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

        Console.CancelKeyPress += Handler; // Register this so that NCurses can uninitialize if ctrl-c is pressed

        NCurses.InitScreen();
        /* NCurses.Refresh(); */

        // Control is passed off to the game
        new Game().Run();

        NCurses.EndWin();
    }

    private static void Handler(object? sender, ConsoleCancelEventArgs e) {
        NCurses.EndWin();
    }
}

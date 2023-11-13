using System.Collections.Generic;
using Mindmagma.Curses;

namespace Blackguard;

public class LibaryNames : CursesLibraryNames {
    public override bool ReplaceLinuxDefaults => false;
    public override List<string> NamesLinux => new List<string> { "libncursesw.so" };
}

public static class Program {
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

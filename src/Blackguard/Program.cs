using System;
using System.Collections.Generic;
using Blackguard.Utilities;
using Mindmagma.Curses;

namespace Blackguard;

public class LibraryNames : CursesLibraryNames {
    public override bool ReplaceLinuxDefaults => false;

    public override List<string> NamesLinux => new() { "libncursesw.so" };

    public override List<string> NamesWindows => Program.Platform.ExtractNativeDependencies();
}

public static class Program {
    public static Platform Platform { get; private set; }

    // Not useful for now, but may be helpful for later
    public static nint StdScreen { get; private set; }

    static Program() {
        Platform = Platform.GetPlatform();
        Platform.Configure();
    }

    public static void Main(string[] args) {
        // Any arg parsing we eventually implement should be here, before any initialization

        StdScreen = NCurses.InitScreen();

        if (!NCurses.HasColors() /*|| !NCurses.CanChangeColor()*/) { // Can change color is seemingly returning false on windows. We can decide if it's neccessary later
            NCurses.EndWin();

            Console.WriteLine("Terminal does not support colors. Please use a terminal that supports colors.");
            Console.ReadKey();
            Environment.Exit(1);
        }

        NCurses.SetCursor(0); // Hide the color
        NCurses.NoEcho();
        NCurses.StartColor(); // Starts the color functionality
        ColorHandler.Init(); // Initialize all of our color pairs and highlights

        Console.CancelKeyPress += Handler; // Register this so that NCurses can uninitialize if ctrl-c is pressed

        Exception? exception = null;
        // Control is passed off to the game
        try {
            new Game().Run();
        }
        catch (Exception e) {
            exception = e;
        }
        finally {
            NCurses.EndWin();

            if (exception != null)
                Console.WriteLine(exception);
        }

    }

    private static void Handler(object? sender, ConsoleCancelEventArgs e) {
        NCurses.EndWin();
    }
}

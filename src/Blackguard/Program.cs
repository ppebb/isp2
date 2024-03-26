using System;
using System.Collections.Generic;
using Blackguard.Tiles;
using Blackguard.Utilities;
using Mindmagma.Curses;

namespace Blackguard;

public class LibraryNames : CursesLibraryNames {
    public override bool ReplaceLinuxDefaults => false;

    public override List<string> NamesLinux => new() { "libncursesw.so" };

    public override List<string> NamesWindows => Program.Platform.ExtractNativeDependencies();
}

public class LibraryNames2 : PanelLibraryNames {
    public override bool ReplaceLinuxDefaults => false;

    public override List<string> NamesLinux => new() { "libpanelw.so" };

    // TODO: On windows with PDCurses there is no longer a separate panel library
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

        Console.Title = "Blackguard";

        NCurses.SetCursor(0); // Hide the cursor
        NCurses.CBreak(); // Makes input immediately available to the terminal instead of performing line buffering
        NCurses.NoEcho(); // Stops input from being printed to the screen automatically
        NCurses.StartColor(); // Starts the color functionality
        ColorHandler.Init(); // Initialize all of our color pairs and highlights
        Registry.InitializeDefinitionType<TileDefinition>(); // Initialize Tile definitions

        Console.CancelKeyPress += SIGINT; // Register this so that NCurses can uninitialize if ctrl-c is pressed

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

    private static void SIGINT(object? sender, ConsoleCancelEventArgs e) {
        NCurses.EndWin();
    }
}

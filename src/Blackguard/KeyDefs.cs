using System.Collections.Generic;
using Mindmagma.Curses;

namespace Blackguard;

public static class InputHandler {
    private static readonly List<int> received_codes = new(); // Raw bytes from ncurses
    public static readonly List<int> pressed_keys = new(); // Processed input for special keys and escape sequences

    private static readonly Dictionary<(int, int, int), int> SpecialKeyDefs = new() {
        { ( 27, 79, 65 ), CursesKey.UP },
        { ( 27, 79, 66 ), CursesKey.DOWN },
        { ( 27, 79, 67 ), CursesKey.RIGHT },
        { ( 27, 79, 68 ), CursesKey.LEFT },
    };

    public static void PollInput(nint windowHandle) {
        received_codes.Clear();
        pressed_keys.Clear();

        int c;
        try {
            while ((c = NCurses.WindowGetChar(windowHandle)) != -1)
                received_codes.Add(c);
        }
        catch { } // Empty catch block because WindowGetChar throws if there is not a currently pressed key

        // Check if any triplets match the special key defs
        for (int i = 0; i < received_codes.Count; i++) {
            if (received_codes.Count - i > 2 && SpecialKeyDefs.TryGetValue((received_codes[i], received_codes[i + 1], received_codes[i + 2]), out int key)) {
                pressed_keys.Add(key);
                i += 3;
            }
            else
                pressed_keys.Add(received_codes[i]);
        }
    }

    public static bool HasInputThisTick() => pressed_keys?.Count > 0;

    public static bool KeyPressed(int keyCode) => pressed_keys?.Contains(keyCode) ?? false;


}

﻿using System.Collections.Generic;
using Mindmagma.Curses;

namespace Blackguard;

public static class InputHandler {
    private static readonly List<int> keys = new();

    public static void PollInput(nint windowHandle) {
        keys.Clear();

        int c;
        try {
            while ((c = NCurses.WindowGetChar(windowHandle)) != -1)
                keys.Add(c);
        }
        catch { } // Empty catch block because WindowGetChar throws if there is not a currently pressed key
    }

    public static bool HasInputThisTick() => keys?.Count > 0;

    public static bool KeyPressed(int keyCode) => keys?.Contains(keyCode) ?? false;
}
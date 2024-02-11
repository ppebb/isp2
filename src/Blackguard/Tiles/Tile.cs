using System;
using System.Drawing;

namespace Blackguard.Tiles;

public abstract class Tile {

    public abstract class Foreground {
        public struct Color {
            public byte R;
            public byte G;
            public byte B;
        }

    }

    public abstract class Background {
            public struct Color {
                public byte R;
                public byte G;
                public byte B;
        }

    }
}
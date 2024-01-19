using System.Numerics;

namespace Blackguard.Entities;

public abstract class Entity {
    public struct DefaultsStruct {
        public int MaxMana;
        public int MaxHealth;
        public int MaxSpeed;
        public double BluntEffect;
        public double SlashEffect;
        public double PierceEffect;
        public double MagicEffect;
        public double BaseEffect;
        public double FireEffect;
        public double ElecEffect;
        public double IceEffect;
        public double WaterEffect;
        public double EarthEffect;
        public double MindEffect;

    }

    public static DefaultsStruct Defaults;

    public Vector2 Position { get; set; }
    public abstract int Health { get;  set; }
    public abstract int Mana { get; set; }
    public abstract int Speed { get; set; }
}
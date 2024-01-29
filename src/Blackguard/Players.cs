using System;
using System.Numerics;

namespace Blackguard;
public class Player {
    public int MaxMana;

    public int MaxHealth;

    public int MaxSpeed;

    public double BluntEffect;

    public double SlashEffect;

    public double PierceEffect;

    public double MagicEffect;

    public double BaseEffect;

    public double FireEffect;

    public double ElectricityEffect;
    
    public double IceEffect;

    public double WaterEffect;

    public double EarthEffect;

    public double MindEffect;

    public Vector2 Position {get; set; }

    public int Health {get; set; }

    public int Mana {get; set; }

    public int Speed {get; set; }
}
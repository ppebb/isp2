// This is an example on how to implement an entities stats in Blackgaurd.

namespace Blackguard.Entities;
public class Slime : Entity {
    public static new DefaultsStruct Defaults = new() {
        MaxHealth = 75,
        MaxMana = 50,        
        MaxSpeed = 80,        
        BluntEffect = .75,        
        SlashEffect = 1.5,        
        PierceEffect = 1.0,        
        MagicEffect = 1.0,        
        BaseEffect = 1.0,        
        FireEffect = .5,        
        ElectricityEffect = 1.25,        
        IceEffect = 1.0,        
        WaterEffect = .5,        
        EarthEffect = 1.0,        
        MindEffect = 1.5,
    };

    
    public override int Health { get; set; }
    
    public override int Mana { get; set; }
    
    public override int Speed { get; set; }

    public override void Movement() {
        throw new System.NotImplementedException();
    }
}
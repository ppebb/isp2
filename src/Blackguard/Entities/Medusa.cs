namespace Blackguard.Entities;
public class Medusa : Entity {
    public static new DefaultsStruct Defaults = new() {
        MaxHealth = 190,
        MaxMana = 50,        
        MaxSpeed = 100,        
        BluntEffect = .85,        
        SlashEffect = .5,        
        PierceEffect = 1.0,        
        MagicEffect = 0,        
        BaseEffect = 1.0,        
        FireEffect = .5,        
        ElectricityEffect = 1.25,        
        IceEffect = 2.0,        
        WaterEffect = 1.5,        
        EarthEffect = 1.0,        
        MindEffect = .5,
    };

    
    public override int Health { get; set; }
    
    public override int Mana { get; set; }
    
    public override int Speed { get; set; }

    public override void Movement() {
        throw new System.NotImplementedException();
    }
}
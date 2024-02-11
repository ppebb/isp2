namespace Blackguard.Entities;
public class ArmyofSkeletons : Entity {
    public static new DefaultsStruct Defaults = new() {
        MaxHealth = 75,
        MaxMana = 50,        
        MaxSpeed = 140,        
        BluntEffect = 2.0,        
        SlashEffect = .9,        
        PierceEffect = .9,        
        MagicEffect = 2.0,        
        BaseEffect = 1.0,        
        FireEffect = .5,        
        ElectricityEffect = .5,        
        IceEffect = 1.0,        
        WaterEffect = .5,        
        EarthEffect = 1.5,        
        MindEffect = 1.0,
    };

    
    public override int Health { get; set; }
    
    public override int Mana { get; set; }
    
    public override int Speed { get; set; }

    public override void Movement() {
        throw new System.NotImplementedException();
    }
}
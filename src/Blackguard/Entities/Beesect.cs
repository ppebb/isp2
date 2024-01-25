namespace Blackguard.Entities;

public class Beesect : Entity {
    public static new DefaultsStruct Defaults = new() {
        MaxHealth = 50,

        MaxMana = 60,
        
        MaxSpeed = 120,
        
        BluntEffect = 1.5,
        
        SlashEffect = 1.5,
        
        PierceEffect = .5,
        
        MagicEffect = .5,
        
        BaseEffect = .9,
        
        FireEffect = 3,
        
        ElecEffect = 1,
        
        IceEffect = 2,
        
        WaterEffect = 1,
        
        EarthEffect = 1,
        
        MindEffect = 1,
    };

    
    public override int Health { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    
    public override int Mana { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    
    public override int Speed { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public override void Movement() {
        throw new System.NotImplementedException();
    }
}
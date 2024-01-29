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
        ElectricityEffect = 1,
        IceEffect = 2,
        WaterEffect = 1,
        EarthEffect = 1,
        MindEffect = 1,
    };

    
    public override int Health { get; set; }
    
    public override int Mana { get; set; }
    
    public override int Speed { get; set; }

    public override void Movement() {
        throw new System.NotImplementedException();
    }
}
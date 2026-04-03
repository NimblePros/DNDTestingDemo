namespace TestDoublesDemo.Domain;

/// <summary>
/// Represents a weapon in D&D
/// </summary>
public class Weapon
{
    public required string Name { get; set; }
    public int MinDamage { get; set; }
    public int MaxDamage { get; set; }
    public WeaponType Type { get; set; }

    public int RollDamage(Random random)
    {
        return random.Next(MinDamage, MaxDamage + 1);
    }
}

public enum WeaponType
{
    Sword,
    Dagger,
    Bow,
    Staff,
    Mace
}

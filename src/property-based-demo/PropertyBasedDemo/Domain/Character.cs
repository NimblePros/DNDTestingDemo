namespace PropertyBasedDemo.Domain;

/// <summary>
/// Represents a D&D character in property-based testing examples
/// </summary>
public class Character
{
    public required string Name { get; set; }
    public int Level { get; set; }
    public int MaxHealthPoints { get; set; }
    public int CurrentHealthPoints { get; set; }
    public int Armor { get; set; }
    public CharacterClass Class { get; set; }
    public Race Race { get; set; }

    public bool IsAlive => CurrentHealthPoints > 0;
    public int HealthPercentage => (CurrentHealthPoints * 100) / MaxHealthPoints;

    public void TakeDamage(int damage)
    {
        int mitigatedDamage = Math.Max(1, damage - Armor / 2);
        CurrentHealthPoints = Math.Max(0, CurrentHealthPoints - mitigatedDamage);
    }

    public void Heal(int amount)
    {
        CurrentHealthPoints = Math.Min(MaxHealthPoints, CurrentHealthPoints + amount);
    }

    public override string ToString() => $"{Name} (L{Level} {Class} {Race})";
}

public enum CharacterClass
{
    Barbarian,
    Bard,
    Cleric,
    Druid,
    Fighter,
    Monk,
    Paladin,
    Ranger,
    Rogue,
    Sorcerer,
    Warlock,
    Wizard
}

public enum Race
{
    Dwarf,
    Elf,
    Halfling,
    Human,
    Dragonborn,
    Gnome,
    HalfElf,
    HalfOrc,
    Tiefling
}

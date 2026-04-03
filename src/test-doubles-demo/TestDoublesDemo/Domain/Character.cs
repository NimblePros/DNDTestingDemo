namespace TestDoublesDemo.Domain;

/// <summary>
/// Represents a D&D character
/// </summary>
public class Character
{
    public required string Name { get; set; }
    public int HealthPoints { get; set; }
    public int ExperiencePoints { get; set; }
    public List<Weapon> Inventory { get; set; } = new();
    public int Level { get; set; } = 1;

    public bool IsAlive => HealthPoints > 0;

    public void TakeDamage(int damage)
    {
        HealthPoints = Math.Max(0, HealthPoints - damage);
    }

    public void GainExperience(int xp)
    {
        ExperiencePoints += xp;
    }

    public void Equip(Weapon weapon)
    {
        Inventory.Add(weapon);
    }
}

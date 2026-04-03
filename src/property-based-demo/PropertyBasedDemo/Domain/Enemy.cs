namespace PropertyBasedDemo.Domain;

/// <summary>
/// Represents an enemy in property-based testing examples
/// </summary>
public class Enemy
{
    public required string Name { get; set; }
    public int ChallengeRating { get; set; }
    public int MaxHealthPoints { get; set; }
    public int CurrentHealthPoints { get; set; }
    public int BaseDamage { get; set; }
    public EnemyType Type { get; set; }

    public bool IsDefeated => CurrentHealthPoints <= 0;

    public void TakeDamage(int damage)
    {
        CurrentHealthPoints = Math.Max(0, CurrentHealthPoints - damage);
    }

    public int AttackDamage(Random random)
    {
        // Variance in damage: base ± 20%
        int variance = (int)(BaseDamage * 0.2);
        return random.Next(BaseDamage - variance, BaseDamage + variance + 1);
    }

    public override string ToString() => $"{Name} (CR{ChallengeRating}, HP: {CurrentHealthPoints})";
}

public enum EnemyType
{
    Goblin,
    Orc,
    Dragon,
    Lich,
    Troll,
    Giant,
    Demon,
    Undead,
    Beast
}

namespace PropertyBasedDemo.Domain;

/// <summary>
/// Represents a party of adventurers
/// </summary>
public class Party
{
    public required string Name { get; set; }
    public List<Character> Members { get; set; } = new();
    public int TotalLevel => Members.Sum(c => c.Level);
    public int AverageLevel => Members.Count > 0 ? TotalLevel / Members.Count : 0;
    public int AliveCount => Members.Count(c => c.IsAlive);

    public override string ToString() => $"Party '{Name}' ({Members.Count} members, Total Level: {TotalLevel})";
}

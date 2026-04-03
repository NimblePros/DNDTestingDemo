namespace PropertyBasedDemo.Domain;

/// <summary>
/// Represents treasure/loot
/// </summary>
public class Loot
{
    public int Gold { get; set; }
    public int Experience { get; set; }
    public List<string> Items { get; set; } = new();

    public override string ToString() => 
        $"Loot: {Gold}gp, {Experience}xp, {Items.Count} items";
}

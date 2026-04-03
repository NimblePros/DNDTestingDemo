namespace TestDoublesDemo.Domain;

/// <summary>
/// Represents a Non-Player Character (NPC)
/// </summary>
public class Npc
{
    public required string Name { get; set; }
    public required string Role { get; set; }
    public int TrustworthyRating { get; set; }
}

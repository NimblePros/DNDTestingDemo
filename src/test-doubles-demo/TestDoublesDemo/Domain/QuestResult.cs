namespace TestDoublesDemo.Domain;

/// <summary>
/// Represents the result of a quest
/// </summary>
public class QuestResult
{
    public required string QuestName { get; set; }
    public required string CharacterName { get; set; }
    public bool Succeeded { get; set; }
    public int ExperienceEarned { get; set; }
    public int TreasureAmount { get; set; }
    public DateTime CompletedAt { get; set; }
    public required string Summary { get; set; }
}

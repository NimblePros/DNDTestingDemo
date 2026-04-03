namespace PropertyBasedDemo.Domain;

/// <summary>
/// Represents the result of combat
/// </summary>
public class CombatResult
{
    public required Encounter EncounterState { get; set; }
    public bool PartyWon { get; set; }
    public int RoundsToComplete { get; set; }
    public List<string> EventLog { get; set; } = new();
    public int TotalDamageToParty { get; set; }
    public int TotalDamageToBoss { get; set; }

    public override string ToString() => 
        $"Combat { (PartyWon ? "WON" : "LOST") } in {RoundsToComplete} rounds";
}

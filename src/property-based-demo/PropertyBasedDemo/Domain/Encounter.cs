namespace PropertyBasedDemo.Domain;

/// <summary>
/// Represents an encounter (party vs enemy/boss)
/// </summary>
public class Encounter
{
    public required Party Party { get; set; }
    public required Enemy Boss { get; set; }
    public int Difficulty { get; set; } // 1-5: Easy, Normal, Hard, Deadly, Impossible
    public int RoundsElapsed { get; set; }

    public int PartyTotalHealth => Party.Members.Sum(m => m.CurrentHealthPoints);
    public bool IsPartyDefeated => Party.Members.All(m => !m.IsAlive);
    public bool IsBossDefeated => Boss.IsDefeated;
    public bool IsEncounterComplete => IsPartyDefeated || IsBossDefeated;

    public override string ToString() 
        => $"Encounter: {Party} vs {Boss} (Difficulty: {Difficulty}, Round: {RoundsElapsed})";
}

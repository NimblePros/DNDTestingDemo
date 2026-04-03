namespace TestDoublesDemo.Services;

using TestDoublesDemo.Domain;

/// <summary>
/// Interface for NPC interactions
/// </summary>
public interface INpcInteraction
{
    string GetNpcGreeting(Npc npc);
    bool CanTrustNpc(Npc npc);
    void ReportNpcBehavior(string npcName, string behavior);
}

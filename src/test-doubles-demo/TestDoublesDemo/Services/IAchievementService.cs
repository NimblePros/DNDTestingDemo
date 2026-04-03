namespace TestDoublesDemo.Services;

using TestDoublesDemo.Domain;

/// <summary>
/// Interface for achievement tracking
/// </summary>
public interface IAchievementService
{
    void UnlockAchievement(string characterName, string achievementName);
    IEnumerable<string> GetAchievements(string characterName);
    bool HasAchievement(string characterName, string achievementName);
}

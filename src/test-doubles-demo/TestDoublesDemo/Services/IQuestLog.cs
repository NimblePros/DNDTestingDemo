namespace TestDoublesDemo.Services;

using TestDoublesDemo.Domain;

/// <summary>
/// Interface for quest logging
/// </summary>
public interface IQuestLog
{
    void LogQuestCompletion(QuestResult result);
    IEnumerable<QuestResult> GetQuestHistory(string characterName);
}

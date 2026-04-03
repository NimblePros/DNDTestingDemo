namespace TestDoublesDemo.Services;

using TestDoublesDemo.Domain;

/// <summary>
/// Service for handling quests with D&D characters
/// </summary>
public class QuestService
{
    private readonly IQuestLog _questLog;
    private readonly IAchievementService _achievementService;
    private readonly IRandomNumberGenerator _randomNumberGenerator;

    public QuestService(
        IQuestLog questLog,
        IAchievementService achievementService,
        IRandomNumberGenerator randomNumberGenerator)
    {
        _questLog = questLog;
        _achievementService = achievementService;
        _randomNumberGenerator = randomNumberGenerator;
    }

    public QuestResult CompleteQuest(Character character, string questName, int difficulty)
    {
        // Roll a d20 to determine success
        int roll = _randomNumberGenerator.RollDice(20);
        bool succeeded = roll + character.Level > difficulty;

        int xpEarned = succeeded ? 100 * difficulty : 10;
        int treasureAmount = succeeded ? 50 * difficulty : 0;

        var result = new QuestResult
        {
            QuestName = questName,
            CharacterName = character.Name,
            Succeeded = succeeded,
            ExperienceEarned = xpEarned,
            TreasureAmount = treasureAmount,
            CompletedAt = DateTime.UtcNow,
            Summary = succeeded 
                ? $"{character.Name} successfully completed {questName}!" 
                : $"{character.Name} failed to complete {questName}."
        };

        character.GainExperience(xpEarned);

        if (succeeded)
        {
            _achievementService.UnlockAchievement(character.Name, questName);
        }

        _questLog.LogQuestCompletion(result);

        return result;
    }

    public int GetCharacterQuestCount(string characterName)
    {
        return _questLog.GetQuestHistory(characterName).Count();
    }
}

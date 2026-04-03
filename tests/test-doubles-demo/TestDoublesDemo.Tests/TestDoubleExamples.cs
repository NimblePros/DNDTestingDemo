namespace TestDoublesDemo.Tests;

using NSubstitute;
using TestDoublesDemo.Domain;
using TestDoublesDemo.Services;
using Xunit;

/// <summary>
/// Demonstrates the STUB test double: An object that returns predetermined responses.
/// Stubs configure specific return values for test scenarios.
/// </summary>
public class StubTestDoubleExamples
{
    [Fact]
    public void AttackWithWeapon_WhenWeaponExists_ReturnsExpectedDamage()
    {
        // Arrange
        var character = new Character { Name = "Legolas", HealthPoints = 80 };

        // Create a STUB - returns predetermined responses
        var weaponRepositoryStub = Substitute.For<IWeaponRepository>();
        var longbow = new Weapon 
        { 
            Name = "Elven Longbow",
            MinDamage = 8,
            MaxDamage = 12,
            Type = WeaponType.Bow
        };
        
        // STUB: Configure the repository to always return our specific weapon
        weaponRepositoryStub.GetWeaponById("longbow-1").Returns(longbow);

        // STUB: Configure random generator to return predictable values
        var randomStub = Substitute.For<IRandomNumberGenerator>();
        randomStub.RollDamage(8, 12).Returns(10);

        var combatService = new CombatService(weaponRepositoryStub, randomStub);

        // Act
        int damage = combatService.AttackWithWeapon(character, "longbow-1");

        // Assert
        Assert.Equal(10, damage);
    }

    [Fact]
    public void AttackWithWeapon_WhenWeaponNotFound_ThrowsException()
    {
        // Arrange
        var character = new Character { Name = "Gandalf" };

        // STUB: Configure to return null for a non-existent weapon
        var weaponRepositoryStub = Substitute.For<IWeaponRepository>();
        weaponRepositoryStub.GetWeaponById(Arg.Any<string>()).Returns((Weapon?)null);

        var randomStub = Substitute.For<IRandomNumberGenerator>();
        var combatService = new CombatService(weaponRepositoryStub, randomStub);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => combatService.AttackWithWeapon(character, "non-existent"));
        
        Assert.Contains("not found", exception.Message);
    }
}

/// <summary>
/// Demonstrates the FAKE test double: A working implementation with simplified behavior.
/// Fakes have real logic but are easier to use than real implementations (e.g., in-memory storage).
/// </summary>
public class FakeTestDoubleExamples
{
    // FAKE implementation - a real working implementation with simplified behavior
    private class FakeQuestLog : IQuestLog
    {
        private readonly List<QuestResult> _questHistory = new();

        public void LogQuestCompletion(QuestResult result)
        {
            _questHistory.Add(result);
        }

        public IEnumerable<QuestResult> GetQuestHistory(string characterName)
        {
            return _questHistory.Where(q => q.CharacterName == characterName).ToList();
        }
    }

    private class FakeRandomNumberGenerator : IRandomNumberGenerator
    {
        private int _nextRollValue = 15;

        public void SetNextRoll(int value) => _nextRollValue = value;

        public int RollDice(int sides)
        {
            return _nextRollValue;
        }

        public int RollDamage(int minDamage, int maxDamage)
        {
            return Math.Min(_nextRollValue, maxDamage);
        }
    }

    [Fact]
    public void CompleteQuest_WithFakeImplementations_LogsQuestAndTracksHistory()
    {
        // Arrange
        var character = new Character { Name = "Frodo", HealthPoints = 50, Level = 1 };
        
        // Use FAKE implementations that behave like real ones but are simpler
        var fakeQuestLog = new FakeQuestLog();
        var fakeAchievements = Substitute.For<IAchievementService>();
        var fakeRandom = new FakeRandomNumberGenerator();
        fakeRandom.SetNextRoll(12);

        var questService = new QuestService(fakeQuestLog, fakeAchievements, fakeRandom);

        // Act
        var result = questService.CompleteQuest(character, "Destroy the Ring", 10);

        // Assert - The FAKE actually stores and retrieves data like the real thing
        Assert.True(result.Succeeded);
        Assert.Equal(1000, result.ExperienceEarned); // 100 * difficulty (10)
        
        var history = fakeQuestLog.GetQuestHistory("Frodo");
        Assert.Single(history);
        Assert.Equal("Destroy the Ring", history.First().QuestName);
    }

    [Fact]
    public void EquipCharacter_WithFakeWeaponRepository_EquipsFromInMemoryList()
    {
        // Arrange
        var character = new Character { Name = "Gimli" };

        // FAKE weapon repository with in-memory data
        var fakeWeaponRepository = Substitute.For<IWeaponRepository>();
        var weapons = new[]
        {
            new Weapon { Name = "Axe of Durin", MinDamage = 10, MaxDamage = 14, Type = WeaponType.Mace },
            new Weapon { Name = "Shield", MinDamage = 1, MaxDamage = 2, Type = WeaponType.Mace }
        };
        fakeWeaponRepository.GetAllWeapons().Returns(weapons);

        var randomStub = Substitute.For<IRandomNumberGenerator>();
        var combatService = new CombatService(fakeWeaponRepository, randomStub);

        // Act
        combatService.EquipCharacter(character);

        // Assert
        Assert.Equal(2, character.Inventory.Count);
        Assert.Contains("Axe of Durin", character.Inventory.Select(w => w.Name));
    }
}

/// <summary>
/// Demonstrates the SPY test double: An object that records how it was called
/// while also delegating to a real implementation (or partial mock).
/// Spies verify interactions AND maintain some real behavior.
/// </summary>
public class SpyTestDoubleExamples
{
    [Fact]
    public void CompleteQuest_WithSpy_VerifiesMethodsCalledAndRecordsInteractions()
    {
        // Arrange
        var character = new Character { Name = "Bilbo", HealthPoints = 60, Level = 2 };

        // Create SPIES - these verify what was called while also doing real work
        var questLogSpy = Substitute.For<IQuestLog>();
        var achievementSpy = Substitute.For<IAchievementService>();
        var randomSpy = Substitute.For<IRandomNumberGenerator>();
        randomSpy.RollDice(20).Returns(16); // Spy returns a value AND records the call

        var questService = new QuestService(questLogSpy, achievementSpy, randomSpy);

        // Act
        var result = questService.CompleteQuest(character, "Retrieve the Arkenstone", 8);

        // Assert - SPY: Verify the exact calls that were made
        Assert.True(result.Succeeded);
        
        // Spy verifies this was called exactly once with the result
        questLogSpy.Received(1).LogQuestCompletion(Arg.Is<QuestResult>(
            q => q.QuestName == "Retrieve the Arkenstone" && q.CharacterName == "Bilbo"));

        // Spy verifies achievement was unlocked for successful quest
        achievementSpy.Received(1).UnlockAchievement("Bilbo", "Retrieve the Arkenstone");

        // Spy verifies random dice roll was called once with 20 sides
        randomSpy.Received(1).RollDice(20);
    }

    [Fact]
    public void CompleteQuest_OnFailure_VerifiesDifferentBehavior()
    {
        // Arrange
        var character = new Character { Name = "Smaug", HealthPoints = 200, Level = 1 };

        // SPY implementations
        var questLogSpy = Substitute.For<IQuestLog>();
        var achievementSpy = Substitute.For<IAchievementService>();
        var randomSpy = Substitute.For<IRandomNumberGenerator>();
        randomSpy.RollDice(20).Returns(3); // Low roll = failure

        var questService = new QuestService(questLogSpy, achievementSpy, randomSpy);

        // Act
        var result = questService.CompleteQuest(character, "Slay the Dragon", 15);

        // Assert - SPY: Verify different behavior on failure
        Assert.False(result.Succeeded);
        Assert.Equal(10, result.ExperienceEarned); // Only 10 XP for failure

        // SPY: Verify achievement was NOT unlocked on failure
        achievementSpy.DidNotReceive().UnlockAchievement("Smaug", "Slay the Dragon");

        // SPY: Verify quest log was still called (different from not being called at all)
        questLogSpy.Received(1).LogQuestCompletion(Arg.Is<QuestResult>(q => !q.Succeeded));
    }
}

/// <summary>
/// Demonstrates the MOCK test double: An object that verifies expected interactions.
/// Mocks establish expectations before executing and assert they were met.
/// </summary>
public class MockTestDoubleExamples
{
    [Fact]
    public void QuestService_ExpectsSpyToRecordMultipleQuests()
    {
        // Arrange
        var character = new Character { Name = "Thorin", HealthPoints = 100, Level = 3 };

        // MOCK: Set up strict expectations for calls
        var questLogMock = Substitute.For<IQuestLog>();
        var achievementMock = Substitute.For<IAchievementService>();
        var randomMock = Substitute.For<IRandomNumberGenerator>();
        randomMock.RollDice(20).Returns(18, 5, 12);

        var questService = new QuestService(questLogMock, achievementMock, randomMock);

        // Act
        var result1 = questService.CompleteQuest(character, "Quest 1", 5);
        var result2 = questService.CompleteQuest(character, "Quest 2", 8);
        var result3 = questService.CompleteQuest(character, "Quest 3", 10);

        // Assert - MOCK: Verify exactly what was called, in order, with correct arguments
        Received.InOrder(() =>
        {
            questLogMock.LogQuestCompletion(Arg.Any<QuestResult>());
            questLogMock.LogQuestCompletion(Arg.Any<QuestResult>());
            questLogMock.LogQuestCompletion(Arg.Any<QuestResult>());
        });

        // MOCK: Verify 3 achievement unlocks for 3 successful quests
        achievementMock.Received(2).UnlockAchievement(Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public void CombatService_VerifiesWeaponRepositoryCalledCorrectly()
    {
        // Arrange
        var character = new Character { Name = "Boromir", HealthPoints = 95 };

        // MOCK: Strict expectations - we care about exact sequence and arguments
        var weaponRepositoryMock = Substitute.For<IWeaponRepository>();
        var sword = new Weapon { Name = "Anduril", MinDamage = 12, MaxDamage = 16, Type = WeaponType.Sword };
        weaponRepositoryMock.GetWeaponById("sword-1").Returns(sword);

        var randomMock = Substitute.For<IRandomNumberGenerator>();
        randomMock.RollDamage(12, 16).Returns(14);

        var combatService = new CombatService(weaponRepositoryMock, randomMock);

        // Act
        var damage = combatService.AttackWithWeapon(character, "sword-1");

        // Assert - MOCK: Very specific expectations
        Assert.Equal(14, damage);
        
        // MOCK: Verify GetWeaponById was called EXACTLY ONCE with exactly "sword-1"
        weaponRepositoryMock.Received(1).GetWeaponById("sword-1");
        
        // MOCK: Verify it wasn't called with any other IDs
        weaponRepositoryMock.DidNotReceive().GetWeaponById(Arg.Is<string>(s => s != "sword-1"));
        
        // MOCK: Verify the damage roll was called
        randomMock.Received(1).RollDamage(12, 16);
    }

    [Fact]
    public void EquipCharacter_VerifiesGetAllWeaponsCallCount()
    {
        // Arrange
        var character = new Character { Name = "Legolas" };

        var weaponRepositoryMock = Substitute.For<IWeaponRepository>();
        var weapons = new List<Weapon>
        {
            new Weapon { Name = "Bow", MinDamage = 6, MaxDamage = 10, Type = WeaponType.Bow },
            new Weapon { Name = "Sword", MinDamage = 8, MaxDamage = 12, Type = WeaponType.Sword },
            new Weapon { Name = "Dagger", MinDamage = 3, MaxDamage = 6, Type = WeaponType.Dagger }
        };
        weaponRepositoryMock.GetAllWeapons().Returns(weapons);

        var randomMock = Substitute.For<IRandomNumberGenerator>();
        var combatService = new CombatService(weaponRepositoryMock, randomMock);

        // Act
        combatService.EquipCharacter(character);

        // Assert - MOCK: Verify exact interaction pattern
        // GetAllWeapons should be called exactly once
        weaponRepositoryMock.Received(1).GetAllWeapons();
        
        // Character should have exactly 2 weapons (Take(2))
        Assert.Equal(2, character.Inventory.Count);
    }
}

/// <summary>
/// Summary: Test Double Types
/// 
/// STUB: An object that returns predetermined responses.
///       Use when you need specific return values for your test scenario.
/// 
/// FAKE: A working implementation with simplified behavior (usually in-memory).
///       Use when you want real behavior but simpler than production code.
/// 
/// SPY: An object that records how it was called while also doing real work.
///      Use when you want to verify interactions but also maintain behavior.
/// 
/// MOCK: An object that establishes expectations about how it will be called.
///       Use when you need strict verification of interactions.
/// </summary>
public class TestDoubleComparisonGuide
{
    // This class is just documentation - see the guide above!
}

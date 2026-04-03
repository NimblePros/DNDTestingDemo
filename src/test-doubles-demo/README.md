# Test Doubles Demo - D&D Adventures Edition

This demo illustrates the five types of **test doubles** in C#, using a D&D (Dungeons & Dragons) theme for practical, engaging examples.

## What is a Test Double?

A **test double** is an object that replaces a real object in tests. Like stunt doubles in movies, they stand in for the real thing when we don't need or can't use the actual implementation.

## The Five Test Double Types

### 1. **DUMMY** 👻
A placeholder object passed to fulfill a parameter but **never used**.

**When to use:**
- When you need to satisfy a method signature but don't care about the behavior
- When the dependency isn't relevant to what you're testing

**Example:**
```csharp
// We're testing Character.GainExperience(), so we pass dummy dependencies
var questLogDummy = Substitute.For<IQuestLog>();
var achievementDummy = Substitute.For<IAchievementService>();
var randomDummy = Substitute.For<IRandomNumberGenerator>();

var questService = new QuestService(questLogDummy, achievementDummy, randomDummy);
character.GainExperience(50); // Dummies are never called
```

### 2. **STUB** 🔌
Returns **predetermined responses** designed for your test scenario.

**When to use:**
- When you need specific return values to test different paths
- When you want to isolate a specific behavior
- When you need to control outcomes (like dice rolls)

**Example:**
```csharp
// STUB: Configure predictable weapon damage
var weaponRepositoryStub = Substitute.For<IWeaponRepository>();
weaponRepositoryStub.GetWeaponById("longbow-1").Returns(longbow);

var randomStub = Substitute.For<IRandomNumberGenerator>();
randomStub.RollDamage(8, 12).Returns(10); // Always roll 10

int damage = combatService.AttackWithWeapon(character, "longbow-1");
// Guaranteed to get 10 damage
```

### 3. **FAKE** 📚
A **working implementation** with simplified behavior (like an in-memory database instead of a real database).

**When to use:**
- When you want real behavior but simpler than production code
- For integration-style tests that test multiple components together
- When full behavior verification matters, not just interaction

**Example:**
```csharp
// FAKE: Real working implementation, just in-memory storage
private class FakeQuestLog : IQuestLog
{
    private readonly List<QuestResult> _questHistory = new();
    
    public void LogQuestCompletion(QuestResult result) => _questHistory.Add(result);
    
    public IEnumerable<QuestResult> GetQuestHistory(string characterName)
        => _questHistory.Where(q => q.CharacterName == characterName);
}

var fakeQuestLog = new FakeQuestLog();
questService.CompleteQuest(character, "Destroy the Ring", 10);
var history = fakeQuestLog.GetQuestHistory("Frodo");
Assert.Single(history); // FAKE actually stores and retrieves data
```

### 4. **SPY** 🕵️
Records **how it was called** while **also doing real work** (delegating to actual behavior).

**When to use:**
- When you want to verify interactions AND maintain real behavior
- When you need to assert both "what happened" and "what was returned"
- For behavior-verification tests that don't isolate

**Example:**
```csharp
// SPY: Records calls while also performing real behavior
var questLogSpy = Substitute.For<IQuestLog>();
var randomSpy = Substitute.For<IRandomNumberGenerator>();
randomSpy.RollDice(20).Returns(16); // Spy returns a value

questService.CompleteQuest(character, "Retrieve the Arkenstone", 8);

// SPY: Verify exactly what was called
questLogSpy.Received(1).LogQuestCompletion(
    Arg.Is<QuestResult>(q => q.QuestName == "Retrieve the Arkenstone"));

randomSpy.Received(1).RollDice(20); // Verify the call happened
```

### 5. **MOCK** 📋
**Establishes expectations** about how it will be called before executing, then **verifies** those expectations.

**When to use:**
- When you need strict verification of interactions
- When you want to test sequence and order of calls
- When exact call counts and arguments matter greatly

**Example:**
```csharp
// MOCK: Set up strict expectations
var questLogMock = Substitute.For<IQuestLog>();
var achievementMock = Substitute.For<IAchievementService>();

questService.CompleteQuest(character, "Quest 1", 5);
questService.CompleteQuest(character, "Quest 2", 8);

// MOCK: Verify exact sequence and call counts
Received.InOrder(() =>
{
    questLogMock.LogQuestCompletion(Arg.Any<QuestResult>());
    questLogMock.LogQuestCompletion(Arg.Any<QuestResult>());
});

achievementMock.Received(2).UnlockAchievement(
    Arg.Any<string>(), 
    Arg.Any<string>()
);
```

## Quick Comparison Table

| Type | Used? | Verifies Behavior? | Returns Values? | Tracks Calls? |
|------|-------|-------------------|-----------------|---------------|
| **Dummy** | ❌ | - | ❌ | ❌ |
| **Stub** | ✅ | ❌ | ✅ | ❌ |
| **Fake** | ✅ | ✅ | ✅ | ❌ |
| **Spy** | ✅ | ✅ | ✅ | ✅ |
| **Mock** | ✅ | ✅ | ✅ | ✅ |

## Project Structure

```
TestDoublesDemo/
├── Domain/
│   ├── Character.cs          // D&D character with HP, XP, inventory
│   ├── Weapon.cs             // Weapons with damage ranges
│   ├── QuestResult.cs        // Quest completion results
│   └── Npc.cs                // Non-player characters
├── Services/
│   ├── IWeaponRepository.cs  // Interface for weapon storage
│   ├── IQuestLog.cs          // Interface for quest logging
│   ├── IRandomNumberGenerator.cs  // Interface for dice rolls
│   ├── INpcInteraction.cs    // Interface for NPC interactions
│   ├── IAchievementService.cs    // Interface for achievements
│   ├── QuestService.cs       // Quest completion logic
│   └── CombatService.cs      // Combat mechanics
└── Tests/
    └── TestDoubleExamples.cs // All 5 test double types demonstrated
```

## D&D Theme Examples

The tests use D&D characters and scenarios:
- **Dummies:** Testing Character.GainExperience(), passing unused quest services
- **Stubs:** Testing combat with controlled weapon damage and dice rolls
- **Fakes:** Testing quest logging with in-memory quest history
- **Spies:** Verifying quest completion triggers achievement unlocks
- **Mocks:** Verifying weapons are equipped in specific sequence

## Running the Tests

```bash
# Run all tests
dotnet test tests/test-doubles-demo/TestDoublesDemo.Tests

# Run with verbose output
dotnet test tests/test-doubles-demo/TestDoublesDemo.Tests -v normal

# Run specific test class
dotnet test tests/test-doubles-demo/TestDoublesDemo.Tests --filter StubTestDoubleExamples
```

## Technologies Used

- **C# 12+** - Modern C# features
- **.NET 10** - Latest .NET runtime
- **xUnit** - Testing framework
- **NSubstitute** - Mocking library for creating test doubles
- **Bogus** - Fake data generation (used in future examples)

## Key Takeaways

1. **Dummies** are the lightest - just satisfy signatures
2. **Stubs** give you control through predetermined returns
3. **Fakes** are mini-implementations for integrated scenarios
4. **Spies** combine real behavior with call verification
5. **Mocks** verify strict behavioral contracts

Choose the right test double based on what you're testing and what you need to verify!

## Next Steps

Once you're comfortable with test doubles, explore:
- **Property-Based Testing** - Automated test generation
- **Integration Tests** - Testing real databases and APIs
- **Mutation Testing** - Verify your tests are effective

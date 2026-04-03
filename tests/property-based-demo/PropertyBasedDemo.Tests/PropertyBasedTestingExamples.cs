namespace PropertyBasedDemo.Tests;

using FsCheck;
using FsCheck.Xunit;
using FsCheck.Fluent;
using PropertyBasedDemo.Domain;
using Xunit;

public class CustomBasicGeneratorExamples
{
    [Property(Arbitrary = new[] { typeof(DndArbitraries) })]
    public void CharacterLevelAlwaysInValidRange(int level)
    {
        Assert.InRange(level, 1, 20);
    }

    [Property(Arbitrary = new[] { typeof(DndArbitraries) })]
    public void ArmorClassAlwaysValid(int armor)
    {
        Assert.InRange(armor, 10, 20);
    }

    [Property(Arbitrary = new[] { typeof(DndArbitraries) })]
    public void ChallengeRatingAlwaysValid(int cr)
    {
        Assert.InRange(cr, 1, 10);
    }

    [Property(Arbitrary = new[] { typeof(DndArbitraries) })]
    public void CharacterNamesAreValid(string name)
    {
        var validNames = new[] { "Aragorn", "Legolas", "Gimli", "Boromir", "Gandalf" };
        Assert.Contains(name, validNames);
    }

    [Property(Arbitrary = new[] { typeof(DndArbitraries) })]
    public void CharacterClassesAlwaysDefined(CharacterClass charClass)
    {
        Assert.True(Enum.IsDefined(typeof(CharacterClass), charClass));
    }
}

public class CustomComplexGeneratorExamples
{
    [Property(Arbitrary = new[] { typeof(DndArbitraries) })]
    public void GeneratedCharactersAreValid(Character character)
    {
        Assert.NotNull(character.Name);
        Assert.InRange(character.Level, 1, 20);
        Assert.InRange(character.MaxHealthPoints, 1, 300);
        Assert.True(character.IsAlive);
    }

    [Property(Arbitrary = new[] { typeof(DndArbitraries) })]
    public void GeneratedEnemiesAreValid(Enemy enemy)
    {
        Assert.NotNull(enemy.Name);
        Assert.InRange(enemy.ChallengeRating, 1, 10);
        Assert.InRange(enemy.MaxHealthPoints, 1, 300);
        Assert.False(enemy.IsDefeated);
    }
}

public class CustomCombinedGeneratorExamples
{
    [Property(Arbitrary = new[] { typeof(DndArbitraries) })]
    public void GeneratedEncountersAreValid(Encounter encounter)
    {
        Assert.NotNull(encounter.Party);
        Assert.NotNull(encounter.Boss);
        Assert.InRange(encounter.Difficulty, 1, 5);
        Assert.Equal(0, encounter.RoundsElapsed);
    }
}

public class CustomShrinkerExamples
{
    private IEnumerable<Character> ShrinkCharacter(Character character)
    {
        if (character.Level > 1)
            yield return new Character
            {
                Name = character.Name,
                Level = character.Level - 1,
                MaxHealthPoints = character.MaxHealthPoints,
                CurrentHealthPoints = character.CurrentHealthPoints,
                Armor = character.Armor,
                Class = character.Class,
                Race = character.Race
            };

        if (character.MaxHealthPoints > 1)
            yield return new Character
            {
                Name = character.Name,
                Level = character.Level,
                MaxHealthPoints = character.MaxHealthPoints - 1,
                CurrentHealthPoints = Math.Min(character.CurrentHealthPoints, character.MaxHealthPoints - 1),
                Armor = character.Armor,
                Class = character.Class,
                Race = character.Race
            };
    }

    [Fact]
    public void CustomCharacterShrinkerReducesLevelFirst()
    {
        var character = new Character
        {
            Name = "Aragorn",
            Level = 15,
            MaxHealthPoints = 200,
            CurrentHealthPoints = 200,
            Armor = 18,
            Class = CharacterClass.Fighter,
            Race = Race.Human
        };
        var shrunken = ShrinkCharacter(character).ToList();
        Assert.NotEmpty(shrunken);
        Assert.Contains(shrunken, c => c.Level < character.Level);
    }
}

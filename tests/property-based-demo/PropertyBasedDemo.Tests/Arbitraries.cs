namespace PropertyBasedDemo.Tests;

using FsCheck;
using FsCheck.Fluent;
using PropertyBasedDemo;
using PropertyBasedDemo.Domain;
using PropertyBasedDemo.Generators;

/// <summary>
/// Registers custom FsCheck Arbitraries for D&D domain objects.
/// Use with [Property(Arbitrary = new [] { typeof(DndArbitraries) })] to inject into tests.
/// </summary>
public static class DndArbitraries
{
    public static Arbitrary<int> Level() => Arb.From(DndGenerators.GenLevel());
    
    public static Arbitrary<int> ArmorClass() => Arb.From(DndGenerators.GenArmor());
    
    public static Arbitrary<int> HitPoints() => Arb.From(DndGenerators.GenHP());
    
    public static Arbitrary<int> ChallengeRating() => Arb.From(DndGenerators.GenCR());
    
    public static Arbitrary<string> CharacterName() => Arb.From(DndGenerators.GenCharName());
    
    public static Arbitrary<string> EnemyName() => Arb.From(DndGenerators.GenEnemyName());
    
    public static Arbitrary<Character> Character() => Arb.From(DndGenerators.GenCharacter());
    
    public static Arbitrary<Enemy> Enemy() => Arb.From(DndGenerators.GenEnemy());

    public static Arbitrary<Party> Party() => Arb.From(DndGenerators.GenParty());

    public static Arbitrary<Encounter> Encounter() => Arb.From(DndGenerators.GenEncounter());
}

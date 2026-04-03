namespace PropertyBasedDemo.Generators;

using System.Linq;
using FsCheck;
using FsCheck.Fluent;
using PropertyBasedDemo.Domain;

public static class DndGenerators
{
    public static Gen<int> GenLevel() => Gen.Choose(1, 20);
    public static Gen<int> GenArmor() => Gen.Choose(10, 20);
    public static Gen<int> GenHP() => Gen.Choose(1, 300);
    public static Gen<int> GenCR() => Gen.Choose(1, 10);

    public static Gen<string> GenCharName() => Gen.Elements(
        "Aragorn", "Legolas", "Gimli", "Boromir", "Gandalf"
    );

    public static Gen<string> GenEnemyName() => Gen.Elements(
        "Smaug", "Lich King", "Balrog", "Tiamat", "Acererak"
    );

    public static Gen<CharacterClass> GenCharacterClass() =>
        Gen.Elements(Enum.GetValues(typeof(CharacterClass)).Cast<CharacterClass>());

    public static Gen<Race> GenRace() =>
        Gen.Elements(Enum.GetValues(typeof(Race)).Cast<Race>());

    public static Gen<EnemyType> GenEnemyType() =>
        Gen.Elements(Enum.GetValues(typeof(EnemyType)).Cast<EnemyType>());

    public static Gen<Character> GenCharacter() =>
        from name in GenCharName()
        from level in GenLevel()
        from hp in GenHP()
        from armor in GenArmor()
        from @class in GenCharacterClass()
        from race in GenRace()
        select new Character
        {
            Name = name,
            Level = level,
            MaxHealthPoints = hp,
            CurrentHealthPoints = hp,
            Armor = armor,
            Class = @class,
            Race = race
        };

    public static Gen<Enemy> GenEnemy() =>
        from name in GenEnemyName()
        from cr in GenCR()
        from hp in GenHP()
        from damage in Gen.Choose(2, 20)
        from type in GenEnemyType()
        select new Enemy
        {
            Name = name,
            ChallengeRating = cr,
            MaxHealthPoints = hp,
            CurrentHealthPoints = hp,
            BaseDamage = damage,
            Type = type
        };

    public static Gen<Party> GenParty() =>
        from size in Gen.Choose(1, 4)
        from members in Gen.ListOf<Character>(GenCharacter(), size)
        select new Party { Name = "Adventurers", Members = members.ToList() };

    public static Gen<Encounter> GenEncounter() =>
        from party in GenParty()
        from boss in GenEnemy()
        from diff in Gen.Choose(1, 5)
        select new Encounter { Party = party, Boss = boss, Difficulty = diff, RoundsElapsed = 0 };
}

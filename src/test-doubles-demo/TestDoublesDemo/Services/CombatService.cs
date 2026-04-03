namespace TestDoublesDemo.Services;

using TestDoublesDemo.Domain;

/// <summary>
/// Service for handling combat encounters
/// </summary>
public class CombatService
{
    private readonly IWeaponRepository _weaponRepository;
    private readonly IRandomNumberGenerator _randomNumberGenerator;

    public CombatService(
        IWeaponRepository weaponRepository,
        IRandomNumberGenerator randomNumberGenerator)
    {
        _weaponRepository = weaponRepository;
        _randomNumberGenerator = randomNumberGenerator;
    }

    public int AttackWithWeapon(Character attacker, string weaponId)
    {
        var weapon = _weaponRepository.GetWeaponById(weaponId);
        
        if (weapon == null)
            throw new InvalidOperationException($"Weapon {weaponId} not found");

        int damage = _randomNumberGenerator.RollDamage(weapon.MinDamage, weapon.MaxDamage);
        return damage;
    }

    public void EquipCharacter(Character character)
    {
        var allWeapons = _weaponRepository.GetAllWeapons();
        foreach (var weapon in allWeapons.Take(2))
        {
            character.Equip(weapon);
        }
    }
}

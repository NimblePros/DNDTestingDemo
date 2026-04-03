namespace TestDoublesDemo.Services;

using TestDoublesDemo.Domain;

/// <summary>
/// Interface for weapon repository
/// </summary>
public interface IWeaponRepository
{
    Weapon? GetWeaponById(string weaponId);
    IEnumerable<Weapon> GetAllWeapons();
    void SaveWeapon(Weapon weapon);
}

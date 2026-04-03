namespace TestDoublesDemo.Services;

/// <summary>
/// Interface for random number generation (dice rolls)
/// </summary>
public interface IRandomNumberGenerator
{
    int RollDice(int sides);
    int RollDamage(int minDamage, int maxDamage);
}

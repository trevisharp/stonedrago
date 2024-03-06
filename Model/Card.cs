namespace Stonedrago.Model;

/// <summary>
/// Represetns a valid card of the game.
/// </summary>
public enum Card
{
    None     = 0b000000000,
    Joker    = 0b000000001,
    Servant  = 0b000000010,
    Warrior  = 0b000000100,
    knight   = 0b000001000,
    Priest   = 0b000010000,
    Princess = 0b000100000,
    Queen    = 0b001000000,
    King     = 0b010000000,
    Schield  = 0b100000000,
    Crown    = King | Queen
}
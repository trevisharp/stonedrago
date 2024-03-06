using System;

namespace Stonedrago.Model;

public record Game
{
    /// <summary>
    /// Constant representing a full hand.
    /// 0-7 bit representing the numbers.
    /// 8 bit representing the x card. 
    /// </summary>
    const int fullHand = 0b111_111_111;
    
    private int greenHand = fullHand;
    private int redHand = fullHand;
    private byte greenPoints = 0;
    private byte redPoints = 0;
    private byte singleDragonRemaining = 3;
    private byte doubleDragonReamining = 2;
    private byte currentDragon = 0;
    private byte cardPosition = 0;
    
    /// <summary>
    /// Get the current dragon in the table.
    /// </summary>
    public Dragon Current => (Dragon)currentDragon;

    /// <summary>
    /// Place a dragon card in center of table.
    /// </summary>
    public void PlaceDragon(Dragon dragon)
        => currentDragon = (byte)dragon;
    
    /// <summary>
    /// Add points of the current dragon card to green player.
    /// </summary>
    public void GreenWin()
        => greenPoints += currentDragon;
    
    /// <summary>
    /// Add points of the current dragon card to red player.
    /// </summary>
    public void RedWin()
        => redPoints += currentDragon;

    /// <summary>
    /// Remove dragon card from the game.
    /// </summary>
    public void RemoveDragon()
    {
        if (currentDragon == 1)
            singleDragonRemaining--;
        else if (currentDragon == 2)
            doubleDragonReamining--;
        currentDragon = 0;
    }

    /// <summary>
    /// Place a random valid dragon card in center of table.
    /// </summary>
    public void PlanceRandomDragon()
    {
        int totalOptions = 
            singleDragonRemaining + doubleDragonReamining;
        
        int selectedOption =
            Random.Shared.Next(totalOptions);
        
        currentDragon = 
            selectedOption < singleDragonRemaining ? 
            (byte)1 : (byte)2;
    }
}
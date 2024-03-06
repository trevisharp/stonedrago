using System;

namespace Stonedrago.Model;

public class Game
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
    private byte dragonReamining = 5;
    private byte currentDragon = 0;
    private int cardPosition = 0;
    
    /// <summary>
    /// Get the current dragon in the table.
    /// </summary>
    public Dragon Current => (Dragon)currentDragon;

    public bool Finished => dragonReamining == 0; 

    public Player Winner =>
        (greenPoints, redPoints) switch
        {
            (>3, >3) => Player.None,
            (>3, _) => Player.Green,
            (_, >3) => Player.Red,
            _ => Player.None
        };

    /// <summary>
    /// Get the green player points.
    /// </summary>
    public int GreenPoints => greenPoints;

    /// <summary>
    /// Get the red player points.
    /// </summary>
    public int RedPoints => redPoints;

    /// <summary>
    /// Get the position of Dragon.
    /// 1 = Green player is winning the dragon card.
    /// 0 = The battle for the draw is a tie.
    /// -1 = Red player is winning the dragom card.
    /// </summary>
    public int DragonPoints => cardPosition;

    /// <summary>
    /// Returns true if green player has a specific card.
    /// </summary>
    public bool GreenHas(Card card)
        => (greenHand & (int)card) > 0;

    /// <summary>
    /// Returns true if red player has a specific card.
    /// </summary>
    public bool RedHas(Card card)
        => (redHand & (int)card) > 0;

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
        dragonReamining--;
        currentDragon = 0;
    }

    /// <summary>
    /// Place a random valid dragon card in center of table.
    /// If do not exist avaliable cards, do nothing.
    /// </summary>
    public void PlaceRandomDragon()
    {
        if (dragonReamining == 0)
            return;
        
        int selectedOption =
            Random.Shared.Next(dragonReamining);
        
        currentDragon = 
            selectedOption < singleDragonRemaining ? 
            (byte)1 : (byte)2;
    }

    /// <summary>
    /// Reset the position of card on center of table.
    /// </summary>
    public void ResetCardPosition()
        => cardPosition = 0;

    /// <summary>
    /// Reset green and red hands.
    /// </summary>
    public void ResetHands()
    {
        greenHand = fullHand;
        redHand = fullHand;
    }

    /// <summary>
    /// Make a round removing cards from players.
    /// Give points to winner and reset hands if needed.
    /// </summary>
    public void MakeRound(Card greenCard, Card redCard)
    {
        var result = GetRoundResult(greenCard, redCard);

        greenHand ^= (int)greenCard;
        redHand ^= (int)redCard;

        cardPosition += result;
        if (cardPosition == 2)
        {
            GreenWin();
            ResetCardPosition();
            RemoveDragon();
            PlaceRandomDragon();
        }

        if (cardPosition == -2)
        {
            RedWin();
            ResetCardPosition();
            RemoveDragon();
            PlaceRandomDragon();
        }

        if (greenCard == 0 || redCard == 0)
            ResetHands();
    }

    /// <summary>
    /// Get the result of a hipotetic round.
    /// Returns 1 if green wins, -1 if red wins, otherwise 0.
    /// </summary>
    public int GetRoundResult(Card greenCard, Card redCard)
    {
        if (greenCard == redCard)
            return 0;
        var playBytes = greenCard | redCard;
        
        if (playBytes == Card.Schield)
            return 0;
        
        if (playBytes == Card.Crown)
            return 0;
        
        return (greenCard, redCard) switch
        {
            (Card.Joker, Card.King) or (Card.Joker, Card.Queen) => true,
            (Card.King, Card.Joker) or (Card.Queen, Card.Joker) => false,
            (Card.Priest, _) or (_, Card.Priest) => greenCard < redCard,
            _ => greenCard > redCard
        } ? 1 : -1;
    }
}
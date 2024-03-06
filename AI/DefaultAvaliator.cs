using System;

namespace Stonedrago.AI;

using Model;

public class DefaultAvaliator : IAvaliator
{
    public float Avaliate(Game game)
    {
        if (game.Finished)
            return game.Winner switch
            {
                Player.Green => float.PositiveInfinity,
                Player.Red => float.NegativeInfinity,
                _ => 0
            };

        int[] cards = [
            (int)Card.Joker,
            (int)Card.King,
            (int)Card.knight,
            (int)Card.Priest,
            (int)Card.Princess,
            (int)Card.Queen,
            (int)Card.Schield,
            (int)Card.Servant,
            (int)Card.Warrior,
        ];
        byte[] randBuffer = new byte[16];
        Random.Shared.NextBytes(randBuffer);

        var gh = game.GreenHand;
        var rh = game.RedHand;
        var gexp = (float)game.GreenPoints;
        var rexp = (float)game.RedPoints;
        var crrDragon = (int)game.Current;

        var handGap = 0;
        for (int i = 0; i < 8; i++)
        {
            var gc = gh & cards[randBuffer[2 * i] % 9];
            var rc = rh & cards[randBuffer[2 * i + 1] % 9];
            if (gc == 0 || rc == 0 || gc == rc)
                continue;
            
            handGap += game.GetRoundResult((Card)gc, (Card)rc);
        }

        if (handGap > 4)
            gexp += crrDragon;
        else if (handGap < -4)
            rexp += crrDragon;
        else
        {
            gexp += crrDragon * (handGap + 4) / 4f;
            rexp += crrDragon * (4 - handGap) / 4f;
        }

        if (gexp > 4f)
            return 12f - rexp;
        else if (rexp > 4f)
            return -12f + gexp;
        
        return 1f / (4f - gexp) - 1f / (4f - rexp);
    }
}
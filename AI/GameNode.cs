using System.Collections.Generic;

namespace Stonedrago.AI;

using Model;

public class GameNode(Game game)
{
    public Game Game { get; private set; } = game;
    public Card NextGreenCard { get; set; } = Card.None;
    public Card NextRedCard { get; set; } = Card.None;
    public float Avaliation { get; set; } = 0;
    public bool Expanded { get; set; } = false;
    public List<GameNode> Children { get; set; } = new ();
}
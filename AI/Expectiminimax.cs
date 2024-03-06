using System;
using System.Collections.Generic;

namespace Stonedrago.AI;

using System.Linq;
using System.Runtime.InteropServices;
using Model;

public class Expectiminimax : IMiniMax
{
    public GameNode Root { get; set; }
    public IAvaliator Avaliator { get; set; }

    public float RootAvaliation 
        => Root.Avaliation;

    public IEnumerable<Game> GetPlays
        => throw new System.NotImplementedException();

    public void Expand(int depth)
    {
        expectiminimax(Root, depth, 
            float.NegativeInfinity, float.PositiveInfinity
        );
    }

    private float expectiminimax(GameNode node, int depth, float alfa, float beta)
    {
        if (node.Game.Finished || depth == 0)
        {
            node.Avaliation = Avaliator.Avaliate(node.Game);
            return node.Avaliation;
        }

        if (node.NextGreenCard == Card.None)
        {
            expandIfNeeded(addGreen);
            float avaliation = float.NegativeInfinity;
            foreach (var child in node.Children)
            {
                var value = expectiminimax(node, depth - 1, alfa, beta);
                if (value > avaliation)
                    avaliation = value;
                
                if (avaliation > beta)
                    break;
                
                if (avaliation > alfa)
                    alfa = avaliation;
            }
            node.Avaliation = avaliation;
            return avaliation;
        }
        else if (node.NextRedCard == Card.None)
        {
            expandIfNeeded(addRed);
            float avaliation = float.PositiveInfinity;
            foreach (var child in node.Children)
            {
                var value = expectiminimax(node, depth - 1, alfa, beta);
                if (value < avaliation)
                    avaliation = value;
                
                if (avaliation < alfa)
                    break;
                
                if (avaliation < beta)
                    beta = avaliation;
            }
            node.Avaliation = avaliation;
            return avaliation;
        }
        else
        {
            expandRoundIfNeeded();
            float avaliation = 0;
            foreach (var child in node.Children)
            {
                float drags = 
                    child.Game.SingleDragonRemaining +
                    child.Game.DoubleDragonReamining;
                var prob = 
                    child.Game.Current == Dragon.Single ?
                    child.Game.SingleDragonRemaining / drags :
                    child.Game.DoubleDragonReamining / drags;
                avaliation += prob * expectiminimax(node, depth - 1, alfa, beta);
            }
            node.Avaliation = avaliation;
            return avaliation;
        }

        void expandRoundIfNeeded()
        {
            if (node.Expanded)
                return;
            node.Expanded = true;
            
            var clone = node.Game.Clone();
            clone.MakeRound(
                node.NextGreenCard,
                node.NextRedCard
            );
            if (clone.Current != Dragon.Empty)
            {
                node.Children.Add(new GameNode(clone));
                return;
            }
            
            if (clone.SingleDragonRemaining > 0)
            {
                var dragonClone = clone.Clone();
                dragonClone.PlaceDragon(Dragon.Single);
                node.Children.Add(new GameNode(dragonClone));
            }

            if (clone.DoubleDragonReamining > 0)
            {
                var dragonClone = clone.Clone();
                dragonClone.PlaceDragon(Dragon.Double);
                node.Children.Add(new GameNode(dragonClone));
            }
        }

        void expandIfNeeded(Action<Card> adder)
        {
            if (node.Expanded)
                return;

            adder(Card.Joker);
            adder(Card.Servant);
            adder(Card.Warrior);
            adder(Card.knight);
            adder(Card.Priest);
            adder(Card.Princess);
            adder(Card.King);
            adder(Card.Queen);
            adder(Card.Schield);
            node.Expanded = true;
        }
        
        void addGreen(Card card)
        {
            if (node.Game.GreenHas(card))
            {
                var clone = node.Game.Clone();
                var newNode = new GameNode(clone) {
                    NextGreenCard = card
                };
                node.Children.Add(newNode);
            }
        }

        void addRed(Card card)
        {
            if (node.Game.RedHas(card))
            {
                var clone = node.Game.Clone();
                var newNode = new GameNode(clone) {
                    NextRedCard = card
                };
                node.Children.Add(newNode);
            }
        }
    }

    public void PlayBest()
    {
        if (Root.Children.Count == 0)
            return;

        if (Root.NextGreenCard == Card.None)
            Root = Root.Children.MaxBy(n => n.Avaliation);
        else if (Root.NextRedCard == Card.None)
            Root = Root.Children.MinBy(n => n.Avaliation);
        else
        {
            Root = Root.Children[
                Random.Shared.Next(
                    Root.Children.Count
                )
            ];
        }
    }

    public void Load(Game game, IAvaliator avaliator)
    {
        this.Avaliator = avaliator;
        this.Root = new GameNode(game);
    }

    public void Load(string file)
    {
        throw new System.NotImplementedException();
    }

    public void Play(Game game)
    {
        throw new System.NotImplementedException();
    }

    public void Save(string file)
    {
        throw new System.NotImplementedException();
    }
}
using System.Collections.Generic;

namespace Stonedrago.AI;

using Model;

/// <summary>
/// Represents a AI algorithm that uses MiniMax approuch.
/// </summary>
public interface IMiniMax
{
    void Load(Game game, IAvaliator avaliator);
    void Load(string file);
    void Save(string file);

    GameNode Root { get; }
    float RootAvaliation { get; }
    IEnumerable<Game> GetPlays { get; }

    void Expand(int depth);

    void PlayBest();
    void Play(Game game);
}
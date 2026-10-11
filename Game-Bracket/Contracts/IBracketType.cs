using System.Collections.Generic;
using Game_Bracket.Domain;

namespace Game_Bracket.Contracts;

public interface IBracketType
{
    string Name { get; }
    IReadOnlyList<Match> GenerateMatches(IReadOnlyCollection<Team> teams);
}
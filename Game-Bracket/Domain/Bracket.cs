using System;
using System.Collections.Generic;
using System.Linq;
using Game_Bracket.Contracts;

namespace Game_Bracket.Domain;

public class Bracket
{
    private readonly List<Match> _matches = new List<Match>();

    public IBracketType Type { get; }

    public IReadOnlyCollection<Match> Matches => _matches.AsReadOnly();

    public Bracket(IBracketType type, IEnumerable<Match> matches)
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type), "Bracket type cannot be null.");
        }

        if (matches == null)
        {
            throw new ArgumentNullException(nameof(matches), "Matches cannot be null.");
        }

        List<Match> matchList = matches.ToList();

        if (matchList.Count == 0)
        {
            throw new ArgumentException("No matches found.", nameof(matches));
        }

        Type = type;
        _matches.AddRange(matchList);
    }
}
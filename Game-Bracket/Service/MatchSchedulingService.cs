using System;
using System.Collections.Generic;
using Game_Bracket.Contracts;
using Game_Bracket.Domain;

namespace Game_Bracket.Service;

public class MatchSchedulingService : IMatchSchedulingService
{
    private readonly List<Bracket> _brackets = new();
    
    public IReadOnlyCollection<Bracket> Brackets => _brackets.AsReadOnly();
    
    public Bracket CreateBracket(IBracketType bracketType, IEnumerable<Team> teams)
    {
        if (bracketType == null)
        {
            throw new ArgumentNullException(nameof(bracketType));
        }
        
        if (teams == null)
        {
            throw new ArgumentNullException(nameof(teams));
        }

        // Convert the input team enumerable into a list for processing
        var teamList = new List<Team>(teams);

        // Generate the opening round matches using the provided bracket type strategy
        var initialMatches = bracketType.GenerateMatches(teamList);
        
        var bracket = new Bracket(bracketType, initialMatches);
        _brackets.Add(bracket);

        return bracket;
    }
    
    public void RescheduleMatch(Match match, DateTime newTime)
    {
        if (match == null)
        {
            throw new ArgumentNullException(nameof(match));
        }

        // Delegate the time update to the match domain model
        match.UpdateSchedule(newTime);
    }
}
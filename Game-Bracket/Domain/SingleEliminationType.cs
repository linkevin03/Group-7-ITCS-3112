using System;
using System.Collections.Generic;
using Game_Bracket.Contracts;
using Game_Bracket.Domain;

namespace Game_Bracket.Domain;

public class SingleEliminationType : IBracketType
{
    public string Name { get; }

    public SingleEliminationType(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Bracket type name cannot be null or whitespace.", nameof(name));
        }

        Name = name;
    }

    // responsible for the initial set-up not the elimination process and progression
    public IReadOnlyList<Match> GenerateMatches(IReadOnlyCollection<Team> teams)
    {
        if (teams == null)
        {
            throw new ArgumentNullException(nameof(teams));
        }

        var teamList = new List<Team>(teams);
        if (teamList.Count < 2)
        {
            throw new InvalidOperationException("At least two teams are required to generate a bracket.");
        }

        var matches = new List<Match>();
        for (int i = 0; i < teamList.Count - 1; i += 2)
        {
            var match = new Match(teamList[i], teamList[i + 1], DateTime.Now, 1);
            matches.Add(match);
        }

        return matches.AsReadOnly();
    }

    public IReadOnlyList<Match> GenerateNextRound(IReadOnlyCollection<Match> completedMatches, int nextRoundNumber)
    {
        if (completedMatches == null)
        {
            throw new ArgumentNullException(nameof(completedMatches));
        }

        var advancingTeams = new List<Team>();

        foreach (var match in completedMatches)
        {
            if (match.Winner == null)
            {
                throw new InvalidOperationException("Cannot generate next round while uncompleted matches exist.");
            }

            Team loser = (match.Winner == match.TeamA) ? match.TeamB : match.TeamA;
            if (!loser.IsEliminated)
            {
                loser.Eliminate();
            }

            advancingTeams.Add(match.Winner);
        }

        if (advancingTeams.Count < 2)
        {
            return new List<Match>().AsReadOnly();
        }

        var nextRoundMatches = new List<Match>();
        for (int i = 0; i < advancingTeams.Count - 1; i += 2)
        {
            nextRoundMatches.Add(new Match(advancingTeams[i], advancingTeams[i + 1], DateTime.Now, nextRoundNumber));
        }

        return nextRoundMatches.AsReadOnly();
    }
}
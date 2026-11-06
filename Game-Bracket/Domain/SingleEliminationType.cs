using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Game_Bracket.Contracts;
using Game_Bracket.Domain;

namespace Game_Bracket.Domain;

public class SingleEliminationType : IBracketType
{
    public string Name => "Single Elimination";

    // Responsible for initial set-up: validates team count, randomizes seedings, builds round 1 pairings, and assigns schedule timestamps
    public IReadOnlyList<Match> GenerateMatches(IReadOnlyCollection<Team> teams)
    {
        if (teams == null)
        {
            throw new ArgumentNullException(nameof(teams));
        }

        var teamList = new List<Team>(teams);

        // 1. Validate team count (minimum of 2 teams required)
        if (teamList.Count < 2)
        {
            throw new ArgumentException("A tournament requires at least 2 teams to generate a bracket.");
        }

        // 2. Randomize team seedings for fair initial matchups
        var rng = new Random();
        var seededTeams = teamList.OrderBy(_ => rng.Next()).ToList();

        var matches = new List<Match>();
        int roundNumber = 1;
        DateTime baseStartTime = DateTime.UtcNow.AddHours(1); // Default start time

        // 3. Build match pairings for the first round with assigned schedule timestamps
        for (int i = 0; i < seededTeams.Count; i += 2)
        {
            if (i + 1 < seededTeams.Count)
            {
                var match = new Match(
                    teamA: seededTeams[i],
                    teamB: seededTeams[i + 1],
                    startTime: baseStartTime.AddMinutes(30 * (i / 2)),
                    roundNumber: roundNumber
                );
                matches.Add(match);
            }
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
        DateTime nextRoundStartTime = DateTime.UtcNow.AddDays(1);

        for (int i = 0; i < advancingTeams.Count; i += 2)
        {
            if (i + 1 < advancingTeams.Count)
            {
                nextRoundMatches.Add(new Match(advancingTeams[i], advancingTeams[i + 1], nextRoundStartTime.AddMinutes(30 * (i / 2)), nextRoundNumber));
            }
        }

        return nextRoundMatches.AsReadOnly();
    }
}
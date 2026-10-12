using System;
using System.Collections.Generic;
using System.Linq;
using Game_Bracket.Contracts;
using Game_Bracket.Domain;

namespace Game_Bracket.Service;

public class LeaderboardService : ILeaderboardService
{
    public IReadOnlyList<Team> GetTeamStandings(IEnumerable<Team> teams)
    {
        if (teams == null)
        {
            throw new ArgumentNullException(nameof(teams));
        }

        // Sort teams by wins descending, then active status
        return teams
            .OrderByDescending(t => t.Wins)
            .ThenBy(t => t.IsEliminated)
            .ToList()
            .AsReadOnly();
    }

    public IReadOnlyList<Player> GetPlayerRankings(IEnumerable<Player> players, string statMetric)
    {
        if (players == null)
        {
            throw new ArgumentNullException(nameof(players));
        }

        if (statMetric == null)
        {
            throw new ArgumentNullException(nameof(statMetric));
        }

        // Dynamically query the requested stat metric polymorphically via PlayerStats
        return players
            .OrderByDescending(p => p.Stats?.GetStatValue(statMetric) ?? 0)
            .ToList()
            .AsReadOnly();
    }
}
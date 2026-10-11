using System;
using Game_Bracket.Domain;

namespace Game_Bracket.Domain;

public class Match
{
    public Team TeamA { get; }

    public Team TeamB { get; }

    public DateTime StartTime { get; private set; }

    public int RoundNumber { get; }

    public Team Winner { get; private set; }

    public Match(Team teamA, Team teamB, DateTime startTime, int roundNumber)
    {
        if (teamA == null)
        {
            throw new ArgumentNullException(nameof(teamA));
        }

        if (teamB == null)
        {
            throw new ArgumentNullException(nameof(teamB));
        }

        if (ReferenceEquals(teamA, teamB))
        {
            throw new ArgumentException("Must be different teams.");
        }

        if (teamA.IsEliminated || teamB.IsEliminated)
        {
            throw new InvalidOperationException("Cannot schedule a match involving an eliminated team.");
        }

        TeamA = teamA;
        TeamB = teamB;
        StartTime = startTime;
        RoundNumber = roundNumber;
        Winner = null;
    }

    public void UpdateSchedule(DateTime newTime)
    {
        StartTime = newTime;
    }

    public void SetWinner(Team winner)
    {
        if (winner == null)
        {
            throw new ArgumentNullException(nameof(winner));
        }

        if (!ReferenceEquals(winner, TeamA) && !ReferenceEquals(winner, TeamB))
        {
            throw new ArgumentException("Winner must be one of the competing teams.");
        }

        Winner = winner;
    }
}
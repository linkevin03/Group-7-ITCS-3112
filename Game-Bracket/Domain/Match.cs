using System;
using Game_Bracket.Domain;

namespace Game_Bracket.Domain;

public class Match
{
    public Team Team1 { get; }

    public Team Team2 { get; }

    public DateTime StartTime { get; private set; }

    public int RoundNumber { get; }

    public Team Winner { get; private set; }

    public Match(Team team1, Team team2, DateTime startTime, int roundNumber)
    {
        if (team1 == null)
        {
            throw new ArgumentNullException(nameof(team1));
        }

        if (team2 == null)
        {
            throw new ArgumentNullException(nameof(team2));
        }

        if (ReferenceEquals(team1, team2))
        {
            throw new ArgumentException("Must be different teams.");
        }

        if (team1.IsEliminated || team2.IsEliminated)
        {
            throw new InvalidOperationException("Cannot schedule a match involving an eliminated team.");
        }

        Team1 = team1;
        Team2 = team2;
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

        if (!ReferenceEquals(winner, Team1) && !ReferenceEquals(winner, Team2))
        {
            throw new ArgumentException("Winner must be one of the competing teams.");
        }

        Winner = winner;
    }
}
using System;
using System.Collections.Generic;
using Game_Bracket.Contracts;
using Game_Bracket.Domain;

namespace Game_Bracket.Service;

public class TournamentProgressionService : ITournamentProgressionService
{
    public void RecordMatchResult(Match match, Team winner, Team loser)
    {
        if (match == null)
        {
            throw new ArgumentNullException(nameof(match));
        }

        if (winner == null)
        {
            throw new ArgumentNullException(nameof(winner));
        }

        if (loser == null)
        {
            throw new ArgumentNullException(nameof(loser));
        }

        // Match.SetWinner validates that the winner is part of the match
        match.SetWinner(winner);

        // Validate that the loser is the other competing team
        if (!ReferenceEquals(loser, match.Team1) && !ReferenceEquals(loser, match.Team2))
        {
            throw new ArgumentException("Loser must be one of the competing teams.");
        }

        // Trigger team elimination for the loser
        if (!loser.IsEliminated)
        {
            loser.Eliminate();
        }
    }

    public void AdvanceBracket(Bracket bracket, Match completedMatch)
    {
        if (bracket == null)
        {
            throw new ArgumentNullException(nameof(bracket));
        }

        if (completedMatch == null)
        {
            throw new ArgumentNullException(nameof(completedMatch));
        }

        if (completedMatch.Winner == null)
        {
            throw new InvalidOperationException("Cannot advance bracket using an uncompleted match.");
        }

        // Generate the next round matches using the bracket type strategy
        var nextRoundNumber = completedMatch.RoundNumber + 1;
        var nextRoundMatches = bracket.Type.GenerateNextRound(bracket.Matches, nextRoundNumber);
    }
}
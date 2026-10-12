using System;
using Game_Bracket.Domain;

namespace Game_Bracket.Contracts;

/// <summary>
/// Defines service operations for recording match results and advancing brackets.
/// </summary>
public interface ITournamentProgressionService
{
    /// <summary>
    /// Records the outcome of a match with the specified winner and loser.
    /// </summary>
    /// <param name="match">
    /// The match being recorded. Must not be null.
    /// </param>
    /// <param name="winner">
    /// The winning team. Must not be null.
    /// </param>
    /// <param name="loser">
    /// The losing team. Must not be null.
    /// </param>
    /// <remarks>
    /// Preconditions:
    /// - match, winner, and loser must not be null.
    /// - winner and loser must be the competing teams assigned to the match.
    /// 
    /// Postconditions:
    /// - The match records the specified winner.
    /// - The losing team's elimination state is triggered.
    /// 
    /// Expected failures:
    /// - ArgumentNullException when match, winner, or loser is null.
    /// - InvalidOperationException when the teams did not participate in the match.
    /// 
    /// Side Effects:
    /// - Updates the match winner state and invokes elimination on the losing team entity.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Thrown when match, winner, or loser is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the teams did not participate in the match.
    /// </exception>
    void RecordMatchResult(Match match, Team winner, Team loser);

    /// <summary>
    /// Advances the tournament bracket based on a completed match.
    /// </summary>
    /// <param name="bracket">
    /// The bracket to advance. Must not be null.
    /// </param>
    /// <param name="completedMatch">
    /// The match that just completed. Must not be null and must have a winner.
    /// </param>
    /// <remarks>
    /// Preconditions:
    /// - bracket and completedMatch must not be null.
    /// - completedMatch must already have a recorded winner.
    /// 
    /// Postconditions:
    /// - Subsequent round matches are generated using the bracket strategy and appended to the bracket's match collection.
    /// 
    /// Expected failures:
    /// - ArgumentNullException when bracket or completedMatch is null.
    /// - InvalidOperationException when the match is uncompleted or missing a winner.
    /// 
    /// Side Effects:
    /// - Appends newly created match instances to the Bracket's match collection.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Thrown when bracket or completedMatch is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the match is uncompleted or missing a winner.
    /// </exception>
    void AdvanceBracket(Bracket bracket, Match completedMatch);
}
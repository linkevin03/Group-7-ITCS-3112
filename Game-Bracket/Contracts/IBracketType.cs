using System;
using System.Collections.Generic;
using Game_Bracket.Domain;

namespace Game_Bracket.Contracts;

/// <summary>
/// Defines a strategy for generating tournament brackets and match pairings.
/// </summary>
public interface IBracketType
{
    /// <summary>
    /// Gets the descriptive name of the bracket format.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Generates an initial sequence of matches based on the provided team collection.
    /// </summary>
    /// <param name="teams">The collection of competing teams.</param>
    /// <returns>A read-only list of generated <see cref="Match"/> objects.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="teams"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when there are insufficient teams to generate matches.</exception>
    IReadOnlyList<Match> GenerateMatches(IReadOnlyCollection<Team> teams);
    
    /// <summary>
    /// Generates the next round of matches based on the outcomes of completed matches.
    /// </summary>
    /// <param name="completedMatches">The collection of completed matches from the current round.</param>
    /// <param name="nextRoundNumber">The round number to assign to the newly generated matches.</param>
    /// <returns>A read-only list of generated <see cref="Match"/> objects for the next round.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="completedMatches"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if any match in the collection is uncompleted.</exception>
    IReadOnlyList<Match> GenerateNextRound(IReadOnlyCollection<Match> completedMatches, int nextRoundNumber);
}
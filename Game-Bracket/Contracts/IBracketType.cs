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
}
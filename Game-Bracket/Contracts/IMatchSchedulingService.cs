using System;
using System.Collections.Generic;
using Game_Bracket.Domain;

namespace Game_Bracket.Contracts;

/// <summary>
/// Defines service operations for creating brackets and managing match schedules.
/// </summary>
public interface IMatchSchedulingService
{
    /// <summary>
    /// Creates a new tournament bracket using the specified bracket type and teams.
    /// </summary>
    /// <param name="bracketType">
    /// The strategy defining the tournament format. Must not be null.
    /// </param>
    /// <param name="teams">
    /// The collection of participating teams. Must not be null and must contain a valid number of teams.
    /// </param>
    /// <returns>
    /// The newly created <see cref="Bracket"/> instance containing the initial schedule.
    /// </returns>
    /// <remarks>
    /// Preconditions:
    /// - bracketType must not be null.
    /// - teams must not be null and must contain at least two teams.
    /// 
    /// Postconditions:
    /// - A new Bracket is initialized, linked to the specified bracket type, and populated with initial opening round matches.
    /// 
    /// Expected failures:
    /// - ArgumentNullException when bracketType or teams is null.
    /// - InvalidOperationException when there are insufficient teams to generate matches.
    /// 
    /// Side Effects:
    /// - Instantiates a new bracket object and generates initial match entities in memory.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Thrown when bracketType or teams is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when there are insufficient teams to generate matches.
    /// </exception>
    Bracket CreateBracket(IBracketType bracketType, IEnumerable<Team> teams);

    /// <summary>
    /// Reschedules an existing match to a new time.
    /// </summary>
    /// <param name="match">
    /// The match to update. Must not be null.
    /// </param>
    /// <param name="newTime">
    /// The new start time to assign.
    /// </param>
    /// <remarks>
    /// Preconditions:
    /// - match must not be null.
    /// 
    /// Postconditions:
    /// - The target match's start time property is updated to the new time.
    /// 
    /// Expected failures:
    /// - ArgumentNullException when match is null.
    /// 
    /// Side Effects:
    /// - Mutates the scheduled start time state of the provided Match instance.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Thrown when match is null.
    /// </exception>
    void RescheduleMatch(Match match, DateTime newTime);
}
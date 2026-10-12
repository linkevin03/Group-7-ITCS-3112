using System.Collections.Generic;
using Game_Bracket.Domain;

namespace Game_Bracket.Contracts;

/// <summary>
/// Defines service operations for generating team standings and player rankings.
/// </summary>
public interface ILeaderboardService
{
    /// <summary>
    /// Retrieves the standings for the specified teams.
    /// </summary>
    /// <param name="teams">
    /// The collection of teams. Must not be null.
    /// </param>
    /// <returns>
    /// A read-only list of ranked teams sorted by performance.
    /// </returns>
    /// <remarks>
    /// Preconditions:
    /// - teams must not be null.
    /// 
    /// Postconditions:
    /// - Returns a sorted, read-only collection of teams ordered by performance metrics.
    /// - This operation does not modify team records.
    /// 
    /// Expected failures:
    /// - System.ArgumentNullException when teams is null.
    /// 
    /// Side Effects:
    /// - None (pure query operation).
    /// </remarks>
    /// <exception cref="System.ArgumentNullException">
    /// Thrown when teams is null.
    /// </exception>
    IReadOnlyList<Team> GetTeamStandings(IEnumerable<Team> teams);

    /// <summary>
    /// Retrieves the rankings for the specified players based on a given metric.
    /// </summary>
    /// <param name="players">
    /// The collection of players. Must not be null.
    /// </param>
    /// <param name="statMetric">
    /// The statistical metric used to sort and rank the players (e.g., "Kills"). Must not be null or empty.
    /// </param>
    /// <returns>
    /// A read-only list of ranked players sorted by the specified statistic.
    /// </returns>
    /// <remarks>
    /// Preconditions:
    /// - players must not be null.
    /// - statMetric must not be null or empty.
    /// 
    /// Postconditions:
    /// - Returns a sorted, read-only collection of players ordered by the requested statistic.
    /// - This operation does not modify player records.
    /// 
    /// Expected failures:
    /// - System.ArgumentNullException when players or statMetric is null.
    /// 
    /// Side Effects:
    /// - None (pure query operation).
    /// </remarks>
    /// <exception cref="System.ArgumentNullException">
    /// Thrown when players or statMetric is null.
    /// </exception>
    IReadOnlyList<Player> GetPlayerRankings(IEnumerable<Player> players, string statMetric);
}
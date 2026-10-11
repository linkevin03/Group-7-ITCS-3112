using System;
using System.Collections.Generic;
using Game_Bracket.Domain;

namespace Game_Bracket.Contracts;

/// <summary>
/// Defines service operations for managing player entities within the tournament system.
/// </summary>
public interface IPlayerService
{
    /// <summary>
    /// Creates and registers a new player in the system.
    /// </summary>
    /// <param name="player">The player instance to register.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="player"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when a player with the same email already exists.</exception>
    void CreatePlayer(Player player);

    /// <summary>
    /// Retrieves a registered player by their unique email address.
    /// </summary>
    /// <param name="email">The email address of the player to retrieve.</param>
    /// <returns>The matching <see cref="Player"/> instance.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="email"/> is null, empty, or whitespace.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when no player with the specified email is found.</exception>
    Player GetPlayerByEmail(string email);

    /// <summary>
    /// Retrieves a read-only collection of all registered players in the system.
    /// </summary>
    /// <returns>An <see cref="IReadOnlyCollection{Player}"/> containing all registered players.</returns>
    IReadOnlyCollection<Player> GetAllPlayers();

    /// <summary>
    /// Deletes a registered player from the system by their email address.
    /// </summary>
    /// <param name="email">The email address of the player to delete.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="email"/> is null, empty, or whitespace.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when no player with the specified email is found.</exception>
    void DeletePlayer(string email);
}
using System.Collections.Generic;
using Game_Bracket.Domain;

namespace Game_Bracket.Contracts;

/// <summary>
/// Defines the contract for managing team records, persistence, and state transitions within the tournament system.
/// </summary>
public interface ITeamService
{
    /// <summary>
    /// Registers a new team.
    /// </summary>
    /// <param name="team">The team instance to add.</param>
    void CreateTeam(Team team);
    
    /// <summary>
    /// Retrieves a specific team by its unique name (ignoring case differences).
    /// </summary>
    /// <param name="name">The name of the team to search for in the collection.</param>
    /// <returns>The matching 'Team' instance.</returns>
    Team GetTeamByName(string name);
    
    /// <summary>
    /// Retrieves a read-only list of all registered teams in the system.
    /// </summary>
    /// <returns>An immutable collection of all teams.</returns>
    IReadOnlyCollection<Team> GetAllTeams();
    
    /// <summary>
    /// Updates the name of an existing team.
    /// </summary>
    /// <param name="currentName">The current identifier name of the team</param>
    /// <param name="newName">The replacement identifier name for the team.</param>
    void UpdateTeamName(string currentName, string newName);
    
    /// <summary>
    /// Removes a team from the collection by its name.
    /// </summary>
    /// <param name="name">The name of the team to delete.</param>
    void DeleteTeam(string name);
}
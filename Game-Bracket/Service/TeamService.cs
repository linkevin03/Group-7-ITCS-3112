using System.Collections.Generic;
using Game_Bracket.Contracts;
using Game_Bracket.Domain;

namespace Game_Bracket.Service;

/// <summary>
/// Provides procedural logic for managing tournament teams.
/// </summary>
public class TeamService : ITeamService
{
    private readonly List<Team> _teams;

    /// <summary>
    /// Initializes a new instance of the 'TeamService' class with an empty internal team collection.
    /// </summary>
    public TeamService()
    {
        _teams = new List<Team>();
    }

    /// <summary>
    /// Registers a new team into the collection after verifying uniqueness.
    /// </summary>
    /// <param name="team">The team instance to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when 'team' is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when a team with the same name already exists.</exception>
    public void CreateTeam(Team team)
    {
        if (team == null)
        {
            throw new ArgumentNullException(nameof(team), "Team cannot be null.");
        }

        foreach (var existingTeam in _teams)
        {
            if (existingTeam.Name.Equals(team.Name, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"A team with the name '{team.Name}' already exists.");
            }
        }

        _teams.Add(team);
    }

    /// <summary>
    /// Searches for and retrieves a team matching the specified name.
    /// </summary>
    /// <param name="name">The name of the team to find.</param>
    /// <returns>The matching 'Team' instance.</returns>
    /// <exception cref="ArgumentException">Thrown when 'name' is null or whitespace.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when no team matching the name exists.</exception>
    public Team GetTeamByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Team name cannot be null or whitespace.", nameof(name));
        }

        // Searches for team in collection
        Team foundTeam = null;
        foreach (var existingTeam in _teams)
        {
            if (existingTeam.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                foundTeam = existingTeam;
                break;
            }
        }

        if (foundTeam == null)
        {
            throw new KeyNotFoundException($"Team with name '{name}' was not found.");
        }

        return foundTeam;
    }

    /// <summary>
    /// Returns all registered teams as a read-only collection.
    /// </summary>
    /// <returns>An enumerable collection containing all registered teams.</returns>
    public IEnumerable<Team> GetAllTeams()
    {
        return _teams.AsReadOnly();
    }

    /// <summary>
    /// Updates an existing team's name while verifying target name uniqueness.
    /// </summary>
    /// <param name="currentName">The current name of the target team.</param>
    /// <param name="newName">The new name to assign to the team.</param>
    /// <exception cref="ArgumentException">Thrown when 'newName' is null or whitespace.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the team with the 'currentName' does not exist.</exception>
    /// <exception cref="InvalidOperationException">Thrown when another team already uses 'newName'.</exception>
    public void UpdateTeam(string currentName, string newName)
    {
        var team = GetTeamByName(currentName);

        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("New team name cannot be null or whitespace.", nameof(newName));
        }

        // Searches if newName is already used
        if (!currentName.Equals(newName, StringComparison.OrdinalIgnoreCase))
        {
            foreach (var existingTeam in _teams)
            {
                if (existingTeam.Name.Equals(newName, StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException($"A team with the name '{newName}' already exists.");
                }
            }
        }

        team.UpdateName(newName);
    }

    /// <summary>
    /// Deletes a team from the collection by its name.
    /// </summary>
    /// <param name="name">The name of the team to remove.</param>
    /// <exception cref="KeyNotFoundException">Thrown when the specified team cannot be found.</exception>
    public void DeleteTeam(string name)
    {
        var team = GetTeamByName(name);
        _teams.Remove(team);
    }
}
using System.Collections.Generic;
using Game_Bracket.Contracts;
using Game_Bracket.Domain;

namespace Game_Bracket.Service;

public class TeamService : ITeamService
{
    private readonly List<Team> _teams;

    public TeamService()
    {
        _teams = new List<Team>();
    }

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

    public IEnumerable<Team> GetAllTeams()
    {
        return _teams.AsReadOnly();
    }

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

    public void DeleteTeam(string name)
    {
        var team = GetTeamByName(name);
        _teams.Remove(team);
    }
}
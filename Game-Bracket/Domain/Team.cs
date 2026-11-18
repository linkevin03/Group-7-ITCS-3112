using System;
using System.Collections.Generic;

namespace Game_Bracket.Domain;

public class Team
{
    private readonly List<Player> _roster = new();

    public string Name { get; private set; }
    public string Logo { get; private set; }
    public IReadOnlyCollection<Player> Roster => _roster.AsReadOnly();
    public bool IsEliminated { get; private set; }
    public int Wins { get; private set; }
    public int Losses { get; private set; }

    public Team(string name, string logo)
    {
        UpdateName(name);
        UpdateLogo(logo);
    }

    public void AddPlayer(Player player)
    {
        if (player == null)
            throw new ArgumentNullException(nameof(player), "Player cannot be null.");

        if (!_roster.Contains(player))
        {
            _roster.Add(player);
        }
    }

    public void RemovePlayer(Player player)
    {
        if (player == null)
            throw new ArgumentNullException(nameof(player), "Player cannot be null.");

        _roster.Remove(player);
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Must have a team name.", nameof(newName));

        Name = newName;
    }

    public void UpdateLogo(string newLogo)
    {
        if (string.IsNullOrWhiteSpace(newLogo))
            throw new ArgumentException("Must have a team logo.", nameof(newLogo));

        Logo = newLogo;
    }

    public void RecordWin()
    {
        if (IsEliminated)
            throw new InvalidOperationException("Team is eliminated.");

        Wins++;
    }

    public void RecordLoss()
    {
        if (IsEliminated)
            throw new InvalidOperationException("Team is already eliminated - cannot record loss.");

        Losses++;
    }

    public void Eliminate()
    {
        if (IsEliminated)
            throw new InvalidOperationException("Team is already eliminated.");

        IsEliminated = true;
    }
}
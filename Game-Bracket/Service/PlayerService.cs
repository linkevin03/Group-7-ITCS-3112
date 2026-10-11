using System;
using System.Collections.Generic;
using Game_Bracket.Contracts;
using Game_Bracket.Domain;

namespace Game_Bracket.Service;

public class PlayerService : IPlayerService
{
    private readonly List<Player> _players;

    public PlayerService()
    {
        _players = new List<Player>();
    }

    public void CreatePlayer(Player player)
    {
        if (player == null)
        {
            throw new ArgumentNullException(nameof(player), "Player cannot be null.");
        }

        foreach (var existingPlayer in _players)
        {
            if (existingPlayer.Email.Equals(player.Email, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"A player with the email '{player.Email}' already exists.");
            }
        }

        _players.Add(player);
    }

    public Player GetPlayerByEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Player email cannot be null or whitespace.", nameof(email));
        }

        Player foundPlayer = null;
        foreach (var existingPlayer in _players)
        {
            if (existingPlayer.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
            {
                foundPlayer = existingPlayer;
                break;
            }
        }

        if (foundPlayer == null)
        {
            throw new KeyNotFoundException($"Player with email '{email}' was not found.");
        }

        return foundPlayer;
    }

    public IReadOnlyCollection<Player> GetAllPlayers()
    {
        return _players.AsReadOnly();
    }

    public void DeletePlayer(string email)
    {
        var player = GetPlayerByEmail(email);
        _players.Remove(player);
    }
}
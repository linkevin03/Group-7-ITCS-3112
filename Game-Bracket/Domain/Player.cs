using System;
using Game_Bracket.Domain;

namespace Game_Bracket.Domain;

public class Player
{
    public string Name { get; private set; }

    public string Email { get; private set; }

    public PlayerStats Stats { get; private set; }

    public Player(string name, string email, PlayerStats stats)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Player name cannot be null or whitespace.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Player email cannot be null or whitespace.", nameof(email));
        }

        if (stats == null)
        {
            throw new ArgumentNullException(nameof(stats), "Player stats cannot be null.");
        }

        Name = name;
        Email = email;
        Stats = stats;
    }

    public string GetName()
    {
        return Name;
    }

    public string GetEmail()
    {
        return Email;
    }
}
using System;

namespace Game_Bracket.Domain;

public class FPSStats : PlayerStats
{
    public int Kills { get; private set; }
    public int Deaths { get; private set; }
    public int Assists { get; private set; }

    public FPSStats(int kills, int deaths, int assists)
    {
        if (kills < 0 || deaths < 0 || assists < 0)
        {
            throw new ArgumentException("Stats cannot be negative.");
        }

        Kills = kills;
        Deaths = deaths;
        Assists = assists;
    }

    public void AddKill()
    {
        Kills++;
    }

    public void AddDeath()
    {
        Deaths++;
    }

    public void AddAssist()
    {
        Assists++;
    }
    
    // Dynamic lookup for any stat name
    public override int GetStatValue(string statName)
    {
        return statName?.ToLowerInvariant() switch
        {
            "kills" => Kills,
            "deaths" => Deaths,
            "assists" => Assists,
            _ => 0 // Fallback if an unknown stat is requested
        };
    }

    public override string GetSummary()
    {
        return $"Kills: {Kills}, Deaths: {Deaths}, Assists: {Assists}";
    }
}
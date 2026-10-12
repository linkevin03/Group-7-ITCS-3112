namespace Game_Bracket.Domain;

public abstract class PlayerStats
{
    public abstract int GetStatValue(string statName); 
    
    public abstract string GetSummary();
}
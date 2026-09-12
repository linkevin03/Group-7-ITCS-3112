namespace Game_Bracket.Domain;

public class Team
{
    public string Name { get; private set; }
    public string Logo { get; private set; }
    public bool IsEliminated { get; private set; }
    public int Wins { get; private set; }
    public int Losses { get; private set; }

    public Team(string name, string logo)
    {
        if (string.IsNullOrEmpty(Name))
            throw new ArgumentException("Must have a team name.",
                nameof(name));
        if (string.IsNullOrEmpty(Logo))
            throw new ArgumentException("Must have a team logo.",
                nameof(logo));
        
        Name = name;
        Logo = logo;
        IsEliminated = false;
        Wins = 0;
        Losses = 0;
    }

    public void UpdateName(string newName)
    {
        Name = newName;
    }
    public void RecordWin() //Adds to win counter
    {
        if (IsEliminated){throw new InvalidOperationException(
            "Team is eliminated.");}
        Wins++;
    }

    public void RecordLoss() //Updates loss counter
    {
        if (IsEliminated){throw new InvalidOperationException(
            "Team is already eliminated - cannot record loss.");}
        Losses++;
    }

    public void Eliminate() //Updates team to eliminated if they lose
    {
        if (IsEliminated)
        {
            throw new InvalidOperationException(
                "Team is already eliminated.");
        }

        IsEliminated = true;
    }
}
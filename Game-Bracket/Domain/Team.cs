namespace Game_Bracket.Domain;

/// <summary>
/// Represents a competitive team in the tournament, tracking identity, state, and match statistics.
/// </summary>
public class Team
{
    /// <summary>
    /// Gets the unique name of the team.
    /// </summary>
    public string Name { get; private set; }
    
    /// <summary>
    /// Gets the file path/URl representing the team's logo
    /// </summary>
    public string Logo { get; private set; }
    
    /// <summary>
    /// Gets a value indicating whether a team is eliminated from the tournament.
    /// </summary>
    public bool IsEliminated { get; private set; }
    
    /// <summary>
    /// Gets the total number of match wins by the team.
    /// </summary>
    public int Wins { get; private set; }
    
    /// <summary>
    /// Gets the total number of match loss by the team.
    /// </summary>
    public int Losses { get; private set; }

    /// <summary>
    /// Initializes a new instance of the Team class with a valid name and logo.
    /// </summary>
    /// <param name="name">The display name assigned to the team.</param>
    /// <param name="logo">The visual identifier for the team logo.</param>
    /// <exception cref="ArgumentException">Thrown when 'name' or 'logo' is null or empty.</exception>
    public Team(string name, string logo)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Must have a team name.",
                nameof(name));
        if (string.IsNullOrEmpty(logo))
            throw new ArgumentException("Must have a team logo.",
                nameof(logo));

        Name = name;
        Logo = logo;
        IsEliminated = false;
        Wins = 0;
        Losses = 0;
    }

    /// <summary>
    /// Modifies the display name of the team.
    /// </summary>
    /// <param name="newName">The replaced name for the team.</param>
    /// <exception cref="ArgumentException">Thrown when 'newName' is null, empty, or whitespace.</exception>
    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("Must have a team name.",
                nameof(newName));
        }

        Name = newName;
    }

    /// <summary>
    /// Increments the team's win counter by one.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the team is already eliminated.</exception>
    public void RecordWin()
    {
        if (IsEliminated)
        {
            throw new InvalidOperationException(
                "Team is eliminated.");
        }

        Wins++;
    }

    /// <summary>
    /// Increments the team's loss counter by one.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the team is already eliminated.</exception>
    public void RecordLoss()
    {
        if (IsEliminated)
        {
            throw new InvalidOperationException(
                "Team is already eliminated - cannot record loss.");
        }

        Losses++;
    }

    /// <summary>
    /// Updates whether the team is eliminated from the tournament.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the team is already marked as eliminated.</exception>
    public void Eliminate()
    {
        if (IsEliminated)
        {
            throw new InvalidOperationException(
                "Team is already eliminated.");
        }

        IsEliminated = true;
    }
}
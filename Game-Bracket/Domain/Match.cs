namespace Game_Bracket.Domain;

/// <summary>
/// Represents a scheduled battle between two distinct active teams.
/// </summary>
public class Match
{
    /// <summary>
    /// Gets the first competing team in the match.
    /// </summary>
    public Team TeamA { get; }
    
    /// <summary>
    /// Gets the second competing team in the match.
    /// </summary>
    public Team TeamB { get; }
    
    /// <summary>
    /// Gets the scheduled data and time when the match is set to start.
    /// </summary>
    public DateTime StartTime { get; private set; }

    /// <summary>
    /// Initializes a new instance of the 'Match' class with two competing teams and a start time.
    /// </summary>
    /// <param name="teamA">The first team partaking in the match.</param>
    /// <param name="teamB">The second team partaking in the match.</param>
    /// <param name="startTime">The scheduled match time.</param>
    /// <exception cref="ArgumentNullException">Thrown when either teamA or teamB is null.</exception>
    /// <exception cref="ArgumentException">Thrown when both teamA and teamB are the same team.</exception>
    /// <exception cref="InvalidOperationException">Thrown when either team is already eliminated from the tournament.</exception>
    public Match(Team teamA, Team teamB, DateTime startTime)
    {
        if (teamA == null) //Multiplicities
        {
            throw new ArgumentNullException(nameof(teamA));
        }
        if (teamB == null)
        {
            throw new ArgumentNullException(nameof(teamB));
        }

        if (ReferenceEquals(teamA, teamB))
        {
            throw new ArgumentException("Must be different teams.");
        }

        if (teamA.IsEliminated || teamB.IsEliminated)
        {
            throw new InvalidOperationException("Cannot schedule a match involving an eliminated team.");
        }
        
        TeamA = teamA;
        TeamB = teamB;
        StartTime = startTime;
    }

    /// <summary>
    /// Reschedules the match start time to a new time.
    /// </summary>
    /// <param name="newTime">The updated time for the match.</param>
    public void UpdateSchedule(DateTime newTime)
    {
        StartTime = newTime;
    }
    
}
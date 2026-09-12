namespace Game_Bracket.Domain;

public class Match
{
    public Team TeamA { get; }
    public Team TeamB { get; }
    public DateTime StartTime { get; private set; }

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

        if (ReferenceEquals(teamA, teamB)) // Teams cannot face themselves
        {
            throw new ArgumentNullException("Must be different teams.");
        }
        TeamA = teamA;
        TeamB = teamB;
        StartTime = startTime;
    }
    
}
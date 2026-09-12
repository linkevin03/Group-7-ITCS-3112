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

    public void UpdateSchedule(DateTime newTime)
    {
        StartTime = newTime;
    }
    
}
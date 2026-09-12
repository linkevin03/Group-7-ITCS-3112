namespace Game_Bracket.Domain;

public class Bracket
{
    private readonly List<Match> _matches = new List<Match>();
    
    public string BracketType { get; }
    
    public IReadOnlyCollection<Match> Matches => _matches.AsReadOnly();

    public Bracket(string type, IEnumerable<Match> matches)
    {
        if (string.IsNullOrWhiteSpace(type))
        {
            throw new ArgumentException("Bracket type cannot be null or whitespace.", nameof(type));
        }

        if (matches == null)
        {
            throw new ArgumentNullException(nameof(matches), "Matches cannot be null");
        }
        
        List<Match> matchList = matches.ToList();

        if (matchList.Count == 0)
        {
            throw new ArgumentException("No matches found.", nameof(matches));
        }
        
        BracketType =  type;
        _matches.AddRange(matchList);
    }
}
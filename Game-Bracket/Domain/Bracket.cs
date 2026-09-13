namespace Game_Bracket.Domain;

/// <summary>
/// Represents a structural grouping of tournament matches associated with a specific format.
/// </summary>
public class Bracket
{
    private readonly List<Match> _matches = new List<Match>();
    
    /// <summary>
    /// Gets the format of the bracket (e.g, Single or double elimination)
    /// </summary>
    public string BracketType { get; }
    
    /// <summary>
    /// Gets a read-only collection of all matches assigned to this bracket.
    /// </summary>
    public IReadOnlyCollection<Match> Matches => _matches.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of the 'Bracket' class with a specified format and collection of matches.
    /// </summary>
    /// <param name="type">The structural format of the bracket.</param>
    /// <param name="matches">The collection of matches belonging to this bracket.</param>
    /// <exception cref="ArgumentException">Thrown when 'type' is null or whitespace or when matches is empty.</exception>
    /// <exception cref="ArgumentNullException">Thrown when the 'matches' collection is null.</exception>
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
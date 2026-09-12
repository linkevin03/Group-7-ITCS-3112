namespace Game_Bracket.Domain;

public class Bracket
{
    private IReadOnlyList<Match> Matches { get; }
    public string bracketFill { get; }

    public Bracket(string bracketFill, IEnumerable<Match> matches = null)
    {
        if (string.IsNullOrEmpty(bracketFill))
            throw new ArgumentException("Bracket cannot be empty.",
                nameof(bracketFill));
        if (matches == null)
            throw new ArgumentNullException(nameof(matches));
        List<Match> matchList = matches.ToList();

        if (matchList.Any(match => match == null))
            throw new ArgumentException("Bracket cannot hold a null match.",
                nameof(matches));
        this.bracketFill = bracketFill;
    }
}
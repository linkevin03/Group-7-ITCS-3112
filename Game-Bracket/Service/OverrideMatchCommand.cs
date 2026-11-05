public class OverrideMatchCommand : ICommand
{
    private readonly BracketStateService _bracketService;
    private readonly int _matchId;
    private readonly string _newWinnerId;
    private readonly string _previousWinnerId; // Saved for rollback capability

    public OverrideMatchCommand(BracketStateService bracketService, int matchId, string newWinnerId, string previousWinnerId)
    {
        _bracketService = bracketService;
        _matchId = matchId;
        _newWinnerId = newWinnerId;
        _previousWinnerId = previousWinnerId;
    }

    public void Execute()
    {
        _bracketService.UpdateMatchWinner(_matchId, _newWinnerId);
    }

    public void Unexecute()
    {
        _bracketService.RestorePreviousState(_matchId, _previousWinnerId);
    }
}

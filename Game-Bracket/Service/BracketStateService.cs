public class BracketStateService
{
    public void UpdateMatchWinner(int matchId, string correctedWinnerTeamId)
    {
        System.Console.WriteLine($"[BracketService] Match {matchId} updated. Winner set to {correctedWinnerTeamId}. Recalculating downstream rounds...");
    }

    public void RestorePreviousState(int matchId, string originalWinnerTeamId)
    {
        System.Console.WriteLine($"[BracketService] Rolling back Match {matchId}. Restoring original winner: {originalWinnerTeamId}.");
    }
}

using System.Collections.Generic;
using Game_Bracket.Domain;

namespace Game_Bracket.Contracts;

/// <summary>
/// Defines operations for managing player agent selections and lock-ins during a match.
/// </summary>
public interface IAgentSelectionService
{
    /// <summary>
    /// Selects and locks in an agent for a player participating in a match, enforcing team composition rules.
    /// </summary>
    /// <param name="match">The match in which the selection is taking place.</param>
    /// <param name="team">The team the player belongs to.</param>
    /// <param name="player">The player choosing the agent.</param>
    /// <param name="agent">The agent being selected and locked in.</param>
    void SelectAndLockAgent(Match match, Team team, Player player, Agent agent);

    /// <summary>
    /// Retrieves all agent selections recorded for a specific match.
    /// </summary>
    /// <param name="match">The match to query selections for.</param>
    /// <returns>A read-only collection of agent selections for the match.</returns>
    IReadOnlyCollection<AgentSelection> GetSelectionsForMatch(Match match);
}
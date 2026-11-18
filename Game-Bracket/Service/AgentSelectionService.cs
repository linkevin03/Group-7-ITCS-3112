using System;
using System.Collections.Generic;
using System.Linq;
using Game_Bracket.Contracts;
using Game_Bracket.Domain;

namespace Game_Bracket.Service;

public class AgentSelectionService : IAgentSelectionService
{
    private readonly Dictionary<Match, List<AgentSelection>> _matchSelections = new();

    public void SelectAndLockAgent(Match match, Team team, Player player, Agent agent)
    {
        if (match == null) throw new ArgumentNullException(nameof(match));
        if (team == null) throw new ArgumentNullException(nameof(team));
        if (player == null) throw new ArgumentNullException(nameof(player));
        if (agent == null) throw new ArgumentNullException(nameof(agent));

        // Verify team belongs to this match
        if (team != match.Team1 && team != match.Team2)
            throw new InvalidOperationException("Team is not participating in this match.");

        // Verify player belongs to this team
        if (!team.Roster.Contains(player))
            throw new InvalidOperationException($"{player.Name} is not on {team.Name}'s roster.");

        if (!_matchSelections.ContainsKey(match))
        {
            _matchSelections[match] = new List<AgentSelection>();
        }

        var selections = _matchSelections[match];

        // Enforce no duplicate agents on the same team
        bool isAgentTakenOnTeam = selections.Any(s =>
            team.Roster.Contains(s.Player) &&
            s.SelectedAgent.Name.Equals(agent.Name, StringComparison.OrdinalIgnoreCase) &&
            s.Player != player);

        if (isAgentTakenOnTeam)
            throw new InvalidOperationException($"{agent.Name} is already selected by another player on {team.Name}.");

        //  Create or update selection and lock in
        var existing = selections.FirstOrDefault(s => s.Player == player);
        if (existing != null)
        {
            existing.ChangeAgent(agent);
            existing.LockIn();
        }
        else
        {
            var newSelection = new AgentSelection(player, agent);
            newSelection.LockIn();
            selections.Add(newSelection);
        }
    }

    public IReadOnlyCollection<AgentSelection> GetSelectionsForMatch(Match match)
    {
        return _matchSelections.TryGetValue(match, out var selections)
            ? selections.AsReadOnly()
            : Array.Empty<AgentSelection>();
    }
}
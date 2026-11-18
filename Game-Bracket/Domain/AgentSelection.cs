using System;

namespace Game_Bracket.Domain;

public class AgentSelection
{
    public Player Player { get; }
    public Agent SelectedAgent { get; private set; }
    public bool IsLockedIn { get; private set; }

    public AgentSelection(Player player, Agent initialAgent)
    {
        Player = player ?? throw new ArgumentNullException(nameof(player));
        SelectedAgent = initialAgent ?? throw new ArgumentNullException(nameof(initialAgent));
        IsLockedIn = false;
    }

    public void ChangeAgent(Agent newAgent)
    {
        if (IsLockedIn)
            throw new InvalidOperationException($"{Player.Name} has already locked in {SelectedAgent.Name}.");

        SelectedAgent = newAgent ?? throw new ArgumentNullException(nameof(newAgent));
    }

    public void LockIn()
    {
        IsLockedIn = true;
    }
}
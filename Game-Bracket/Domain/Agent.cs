using System;

namespace Game_Bracket.Domain;

public enum AgentRole
{
    Duelist,
    Controller,
    Initiator,
    Sentinel
}

public class Agent
{
    public string Name { get; }
    public AgentRole Role { get; }
    public string ImagePath { get; }

    public Agent(string name, AgentRole role, string imagePath)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Agent name cannot be empty.", nameof(name));
        if (string.IsNullOrWhiteSpace(imagePath))
            throw new ArgumentException("Agent image path cannot be empty.", nameof(imagePath));

        Name = name;
        Role = role;
        ImagePath = imagePath;
    }
}
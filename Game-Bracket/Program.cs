using System;
using System.Collections.Generic;
using Game_Bracket.Contracts;
using Game_Bracket.Domain;
using Game_Bracket.Service;

namespace Game_Bracket;

class Program
{
    static void Main(string[] args)
    {
        // Create at least two valid domain objects and add them to the service.
        ITeamService teamService = new TeamService();
        IPlayerService playerService = new PlayerService();

        Console.WriteLine("TOURNAMENT MANAGEMENT SYSTEM\n");

        Console.WriteLine("[CREATE]");
        Team team1 = new Team("Red Birds", "red_birds.png");
        Team team2 = new Team("Blue Birds", "blue_birds.png");

        teamService.CreateTeam(team1);
        teamService.CreateTeam(team2);
        Console.Write($"Created team {team1.Name} and {team2.Name}.\n");

        // Add players and stats to teams using PlayerService
        Console.WriteLine("[ROSTER & STATS SETUP]");
        PlayerStats stats1 = new FPSStats(15, 5, 10);
        Player player1 = new Player("Alice", "alice@redbirds.com", stats1);

        playerService.CreatePlayer(player1);
        team1.AddPlayer(player1);

        Player retrievedPlayer = playerService.GetPlayerByEmail("alice@redbirds.com");
        Console.WriteLine($"Created and registered player {retrievedPlayer.GetName()} ({retrievedPlayer.GetEmail()})");
        Console.WriteLine($"Added player to {team1.Name} with stats: {retrievedPlayer.Stats.GetSummary()}\n");

        // Read object by identifier
        Console.WriteLine("[READ BY IDENTIFIER]");
        Team retrievedTeam = teamService.GetTeamByName("Red Birds");
        Console.WriteLine(
            $"Team {retrievedTeam.Name} has {retrievedTeam.Wins} wins and {retrievedTeam.Roster.Count} player(s).\n");

        // Read and display all managed objects
        Console.WriteLine("[READ ALL]");
        Console.WriteLine("All registered teams in the system:");
        foreach (Team team in teamService.GetAllTeams())
        {
            Console.WriteLine(team.Name);
            Console.WriteLine($"team logo: {team.Logo}");
            Console.WriteLine($"is eliminated?: {team.IsEliminated}\n");
        }

        // Update one object through the service and display the changed state.
        Console.WriteLine("[UPDATE]");
        Console.WriteLine($"Before Update: Team name is {team1.Name}");
        teamService.UpdateTeamName("Red Birds", "Angry Birds");
        Team updatedTeam = teamService.GetTeamByName("Angry Birds");
        Console.WriteLine($"After Update: Team name is {updatedTeam.Name}\n");

        // Demonstrate bracket generation and match workflow
        Console.WriteLine("[BRACKET & MATCH GENERATION]");
        Team team3 = new Team("Green Pigs", "green_pigs.png");
        Team team4 = new Team("King Pig", "king_pig.png");
        teamService.CreateTeam(team3);
        teamService.CreateTeam(team4);

        IBracketType bracketType = new SingleEliminationType("Single Elimination");
        var matches = bracketType.GenerateMatches(teamService.GetAllTeams());
        Bracket tournamentBracket = new Bracket(bracketType, matches);

        Console.WriteLine(
            $"Generated {tournamentBracket.Matches.Count} match(es) for bracket type: {tournamentBracket.Type.Name}");
        foreach (var match in tournamentBracket.Matches)
        {
            Console.WriteLine(
                $"Match: {match.TeamA.Name} vs {match.TeamB.Name} (Round {match.RoundNumber}) at {match.StartTime}");

            // Simulate setting a winner
            match.SetWinner(match.TeamA);
            Console.WriteLine($"Winner decided: {match.Winner.Name}\n");
        }

        // Delete one object and display the remaining collection.
        Console.WriteLine("[DELETE]");
        Console.WriteLine("Removing team 'Blue Birds'");
        teamService.DeleteTeam("Blue Birds");
        Console.WriteLine("Remaining teams in the system:");
        foreach (Team team in teamService.GetAllTeams())
        {
            Console.WriteLine(team.Name);
        }

        Console.WriteLine();

        // Demonstrate rule enforcement
        Console.WriteLine("[RULE ENFORCEMENT]");

        // Domain-Object Operation
        Console.WriteLine("[REJECTED DOMAIN-OBJECT OPERATION] - Scheduling a match with eliminated team");
        Console.WriteLine("Scheduling a match with an eliminated team...");
        Team team5 = new Team("Sad Pigs", "sad_pigs.png");
        teamService.CreateTeam(team5);

        // Eliminate sad pigs
        team5.Eliminate();
        Console.WriteLine($"Team {team5.Name} has been eliminated.");

        try
        {
            Match invalidMatch = new Match(updatedTeam, team5, DateTime.Now, 1);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Expected Domain Exception Caught: {ex.Message}");

            int currentCount = 0;
            foreach (var t in teamService.GetAllTeams())
            {
                currentCount++;
            }

            Console.WriteLine($"State Verification: '{team5.Name}' remains eliminated = {team5.IsEliminated}, " +
                              $"and active collection count is intact at {currentCount} teams.\n");
        }

        // Service Operation
        Console.WriteLine("[REJECTED SERVICE OPERATION] - Duplicate Team Name");
        Team duplicateTeam = new Team("Angry Birds", "duplicate.png");

        int countBeforeRejection = 0;
        foreach (Team team in teamService.GetAllTeams())
        {
            countBeforeRejection++;
        }

        Console.WriteLine($"Team count before rejection attempt: {countBeforeRejection}");

        try
        {
            teamService.CreateTeam(duplicateTeam);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Expected Service Exception Caught: {ex.Message}");

            int countAfterRejection = 0;
            foreach (var t in teamService.GetAllTeams())
            {
                countAfterRejection++;
            }

            Console.WriteLine(
                $"State Verification: Collection count remains valid ({countAfterRejection} teams). Duplicate was successfully rejected.");
        }
    }
}
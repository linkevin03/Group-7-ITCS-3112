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

        // --- MEMO 2 SERVICES ---
        IMatchSchedulingService matchSchedulingService = new MatchSchedulingService();
        ITournamentProgressionService progressionService = new TournamentProgressionService();
        ILeaderboardService leaderboardService = new LeaderboardService();

        Console.WriteLine("TOURNAMENT MANAGEMENT SYSTEM - MEMO 1 & 2 DEMO\n");

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

        PlayerStats stats2 = new FPSStats(22, 3, 8);
        Player player2 = new Player("Bob", "bob@bluebirds.com", stats2);

        playerService.CreatePlayer(player1);
        playerService.CreatePlayer(player2);
        team1.AddPlayer(player1);
        team2.AddPlayer(player2);

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

        // --- MEMO 2: MATCH SCHEDULING SERVICE DEMO ---
        Console.WriteLine("[MEMO 2 - MATCH SCHEDULING SERVICE]");
        Team team3 = new Team("Green Pigs", "green_pigs.png");
        Team team4 = new Team("King Pig", "king_pig.png");
        teamService.CreateTeam(team3);
        teamService.CreateTeam(team4);

        IBracketType bracketType = new SingleEliminationType("Single Elimination");

        // Using MatchSchedulingService instead of direct instantiation
        Bracket tournamentBracket = matchSchedulingService.CreateBracket(bracketType, teamService.GetAllTeams());
        Console.WriteLine(
            $"MatchSchedulingService created a bracket with {tournamentBracket.Matches.Count} match(es).");

        // Demonstrate rescheduling a match via MatchSchedulingService
        var sampleMatch = tournamentBracket.Matches.First();
        var rescheduledTime = DateTime.Now.AddDays(1);
        matchSchedulingService.RescheduleMatch(sampleMatch, rescheduledTime);
        Console.WriteLine($"Rescheduled match start time to: {sampleMatch.StartTime}\n");

        // --- MEMO 2: TOURNAMENT PROGRESSION SERVICE DEMO ---
        Console.WriteLine("[MEMO 2 - TOURNAMENT PROGRESSION SERVICE]");
        Console.WriteLine($"Simulating match result between {sampleMatch.TeamA.Name} and {sampleMatch.TeamB.Name}...");

        // Record result using TournamentProgressionService
        progressionService.RecordMatchResult(sampleMatch, sampleMatch.TeamA, sampleMatch.TeamB);
        Console.WriteLine($"Winner recorded: {sampleMatch.Winner.Name}");

        // Check loser elimination status
        Team loser = ReferenceEquals(sampleMatch.TeamA, sampleMatch.Winner) ? sampleMatch.TeamB : sampleMatch.TeamA;
        Console.WriteLine($"Loser ({loser.Name}) elimination status: {loser.IsEliminated}\n");

        // --- MEMO 2: LEADERBOARD SERVICE DEMO ---
        Console.WriteLine("[MEMO 2 - LEADERBOARD SERVICE]");

        var teamStandings = leaderboardService.GetTeamStandings(teamService.GetAllTeams());
        Console.WriteLine("Team Standings:");
        foreach (var t in teamStandings)
        {
            Console.WriteLine($" - {t.Name} | Wins: {t.Wins} | Eliminated: {t.IsEliminated}");
        }

        var playerRankings = leaderboardService.GetPlayerRankings(new[] { player1, player2 }, "Kills");
        Console.WriteLine("\nPlayer Rankings (Sorted by Kills):");
        foreach (var p in playerRankings)
        {
            Console.WriteLine($" - {p.GetName()} | Kills: {p.Stats.GetStatValue("Kills")}");
        }

        Console.WriteLine();

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
        Team team5 = new Team("Sad Pigs", "sad_pigs.png");
        teamService.CreateTeam(team5);

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
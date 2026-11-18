using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game_Bracket.Contracts;
using Game_Bracket.Domain;
using Game_Bracket.Service;

namespace Game_Bracket;

class Program
{
    static void Main(string[] args)
    {
        // --- CORE SERVICES ---
        ITeamService teamService = new TeamService();
        IPlayerService playerService = new PlayerService();
        IMatchSchedulingService matchSchedulingService = new MatchSchedulingService();
        ITournamentProgressionService progressionService = new TournamentProgressionService();
        ILeaderboardService leaderboardService = new LeaderboardService();
        IAgentSelectionService agentSelectionService = new AgentSelectionService();
        BracketStateService bracketStateService = new BracketStateService();

        Console.WriteLine("TOURNAMENT MANAGEMENT SYSTEM DEMO\n");

        // 1. CREATE TEAMS
        Console.WriteLine("[CREATE]");
        Team team1 = new Team("Red Birds", "red_birds.png");
        Team team2 = new Team("Blue Birds", "blue_birds.png");
        Team team3 = new Team("Green Pigs", "green_pigs.png");
        Team team4 = new Team("King Pig", "king_pig.png");

        teamService.CreateTeam(team1);
        teamService.CreateTeam(team2);
        teamService.CreateTeam(team3);
        teamService.CreateTeam(team4);
        Console.WriteLine($"Created teams: {team1.Name}, {team2.Name}, {team3.Name}, {team4.Name}.\n");

        // 2. READ BY IDENTIFIER & READ ALL
        Console.WriteLine("[READ BY IDENTIFIER]");
        Team retrievedTeam = teamService.GetTeamByName("Red Birds");
        Console.WriteLine(
            $"Team {retrievedTeam.Name} has {retrievedTeam.Wins} wins and {retrievedTeam.Roster.Count} player(s).\n");

        Console.WriteLine("[READ ALL]");
        Console.WriteLine("All registered teams in the system:");
        foreach (Team team in teamService.GetAllTeams())
        {
            Console.WriteLine(team.Name);
            Console.WriteLine($"team logo: {team.Logo}");
            Console.WriteLine($"is eliminated?: {team.IsEliminated}\n");
        }

        // 3. UPDATE TEAM
        Console.WriteLine("[UPDATE]");
        Console.WriteLine($"Before Update: Team name is {team1.Name}");
        teamService.UpdateTeamName("Red Birds", "Angry Birds");
        Team updatedTeam = teamService.GetTeamByName("Angry Birds");
        Console.WriteLine($"After Update: Team name is {updatedTeam.Name}\n");

        // 4. MATCH SCHEDULING SERVICE DEMO
        Console.WriteLine("[MATCH SCHEDULING SERVICE]");
        IBracketType bracketType = new SingleEliminationType();
        Bracket tournamentBracket = matchSchedulingService.CreateBracket(bracketType, teamService.GetAllTeams());
        Console.WriteLine(
            $"MatchSchedulingService created a bracket with {tournamentBracket.Matches.Count} match(es).");

        var sampleMatch = tournamentBracket.Matches.First();
        var rescheduledTime = DateTime.Now.AddDays(1);
        matchSchedulingService.RescheduleMatch(sampleMatch, rescheduledTime);
        Console.WriteLine($"Rescheduled match start time to: {sampleMatch.StartTime}\n");

        // 5. MAP POOL VETO DEMO (FACTORY PATTERN)
        Console.WriteLine("[MAP VETO SERVICE (FACTORY PATTERN)]");
        Map[] mapPool = new Map[]
        {
            new Map("Ascent", "ascent.png"),
            new Map("Bind", "bind.png"),
            new Map("Haven", "haven.png")
        };
        IMapVetoService mapVetoService = new MapVetoService(mapPool, "BestOfOne", new MapVetoFactory());
        mapVetoService.RunMapVeto();
        Console.WriteLine($"Map Veto Complete. Selected Map: {mapVetoService.GetSelectedMap().Name}\n");

        // 6. ROSTER & STATS SETUP (Assigned to the actual teams playing in sampleMatch)
        Console.WriteLine("[ROSTER & STATS SETUP]");
        PlayerStats stats1 = new FPSStats(15, 5, 10);
        Player player1 = new Player("Alice", "alice@redbirds.com", stats1);

        PlayerStats stats2 = new FPSStats(22, 3, 8);
        Player player2 = new Player("Bob", "bob@bluebirds.com", stats2);

        PlayerStats stats3 = new FPSStats(12, 7, 14);
        Player player3 = new Player("Charlie", "charlie@redbirds.com", stats3);

        playerService.CreatePlayer(player1);
        playerService.CreatePlayer(player2);
        playerService.CreatePlayer(player3);

        sampleMatch.Team1.AddPlayer(player1);
        sampleMatch.Team1.AddPlayer(player3);
        sampleMatch.Team2.AddPlayer(player2);

        Player retrievedPlayer = playerService.GetPlayerByEmail("alice@redbirds.com");
        Console.WriteLine($"Created and registered player {retrievedPlayer.GetName()} ({retrievedPlayer.GetEmail()})");
        Console.WriteLine($"Added players to match teams ({sampleMatch.Team1.Name} vs {sampleMatch.Team2.Name})\n");

        // 7. VALORANT AGENT SELECTION DEMO
        Console.WriteLine("[AGENT SELECTION SERVICE]");
        Agent jett = new Agent("Jett", AgentRole.Duelist, "jett.png");
        Agent omen = new Agent("Omen", AgentRole.Controller, "omen.png");

        agentSelectionService.SelectAndLockAgent(sampleMatch, sampleMatch.Team1, player1, jett);
        agentSelectionService.SelectAndLockAgent(sampleMatch, sampleMatch.Team2, player2, jett);

        Console.WriteLine($"Agent selections locked for {sampleMatch.Team1.Name} vs {sampleMatch.Team2.Name}:");
        foreach (var selection in agentSelectionService.GetSelectionsForMatch(sampleMatch))
        {
            Console.WriteLine(
                $" - {selection.Player.GetName()} locked in {selection.SelectedAgent.Name} ({selection.SelectedAgent.Role})");
        }

        try
        {
            Console.WriteLine($"Attempting duplicate pick: {player3.GetName()} picking {jett.Name} on {sampleMatch.Team1.Name}...");
            agentSelectionService.SelectAndLockAgent(sampleMatch, sampleMatch.Team1, player3, jett);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Expected Agent Rule Caught: {ex.Message}");
            agentSelectionService.SelectAndLockAgent(sampleMatch, sampleMatch.Team1, player3, omen);
            Console.WriteLine($"Resolved: {player3.GetName()} locked in {omen.Name} ({omen.Role}).\n");
        }

        // 8. TOURNAMENT PROGRESSION SERVICE DEMO
        Console.WriteLine("[TOURNAMENT PROGRESSION SERVICE]");
        Console.WriteLine($"Simulating match result between {sampleMatch.Team1.Name} and {sampleMatch.Team2.Name}...");

        progressionService.RecordMatchResult(sampleMatch, sampleMatch.Team1, sampleMatch.Team2);
        Console.WriteLine($"Winner recorded: {sampleMatch.Winner.Name}");

        Team loser = ReferenceEquals(sampleMatch.Team1, sampleMatch.Winner) ? sampleMatch.Team2 : sampleMatch.Team1;
        Console.WriteLine($"Loser ({loser.Name}) elimination status: {loser.IsEliminated}\n");

        // 9. MATCH DISPUTE OVERRIDE & ROLLBACK DEMO (COMMAND PATTERN)
        Console.WriteLine("[MATCH DISPUTE OVERRIDE (COMMAND PATTERN)]");
        int matchId = sampleMatch.RoundNumber; 
        string previousWinnerId = sampleMatch.Winner.Name;
        string newWinnerId = loser.Name;

        Console.WriteLine($"Dispute filed on Match {matchId}. Overriding winner to {newWinnerId}...");
        ICommand overrideCommand = new OverrideMatchCommand(bracketStateService, matchId, newWinnerId, previousWinnerId);
        overrideCommand.Execute();

        Console.WriteLine("Rolling back override via Unexecute()...");
        overrideCommand.Unexecute();
        Console.WriteLine("Rollback completed successfully.\n");

        // 10. LEADERBOARD SERVICE DEMO
        Console.WriteLine("[LEADERBOARD SERVICE]");
        var teamStandings = leaderboardService.GetTeamStandings(teamService.GetAllTeams());
        Console.WriteLine("Team Standings:");
        foreach (var t in teamStandings)
        {
            Console.WriteLine($" - {t.Name} | Wins: {t.Wins} | Eliminated: {t.IsEliminated}");
        }

        var playerRankings = leaderboardService.GetPlayerRankings(new[] { player1, player2, player3 }, "Kills");
        Console.WriteLine("\nPlayer Rankings (Sorted by Kills):");
        foreach (var p in playerRankings)
        {
            Console.WriteLine($" - {p.GetName()} | Kills: {p.Stats.GetStatValue("Kills")}");
        }

        Console.WriteLine();

        // 11. DELETE & RULE ENFORCEMENT DEMOS
        Console.WriteLine("[DELETE]");
        Console.WriteLine("Removing team 'Blue Birds'");
        teamService.DeleteTeam("Blue Birds");
        Console.WriteLine("Remaining teams in the system:");
        foreach (Team team in teamService.GetAllTeams())
        {
            Console.WriteLine(team.Name);
        }

        Console.WriteLine();
        Console.WriteLine("[RULE ENFORCEMENT]");

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
            int currentCount = teamService.GetAllTeams().Count;
            Console.WriteLine($"State Verification: '{team5.Name}' remains eliminated = {team5.IsEliminated}, " +
                              $"and active collection count is intact at {currentCount} teams.\n");
        }

        Console.WriteLine("[REJECTED SERVICE OPERATION] - Duplicate Team Name");
        Team duplicateTeam = new Team("Angry Birds", "duplicate.png");
        int countBeforeRejection = teamService.GetAllTeams().Count;
        Console.WriteLine($"Team count before rejection attempt: {countBeforeRejection}");

        try
        {
            teamService.CreateTeam(duplicateTeam);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Expected Service Exception Caught: {ex.Message}");
            int countAfterRejection = teamService.GetAllTeams().Count;
            Console.WriteLine(
                $"State Verification: Collection count remains valid ({countAfterRejection} teams). Duplicate was successfully rejected.");
        }
    }
}
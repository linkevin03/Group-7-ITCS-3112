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

        Console.WriteLine("TOURNAMENT MANAGEMENT SYSTEM\n");

        Console.WriteLine("[CREATE]");
        Team team1 = new Team("Red Birds", "red_birds.png");
        Team team2 = new Team("Blue Birds", "blue_birds.png");

        teamService.CreateTeam(team1);
        teamService.CreateTeam(team2);
        Console.Write($"Created team {team1.Name} and {team2.Name}.\n");

        // Read object by identifier
        Console.WriteLine("[READ BY IDENTIFIER]");
        Team retrievedTeam = teamService.GetTeamByName("Red Birds");
        Console.WriteLine($"Team {retrievedTeam.Name} has {retrievedTeam.Wins} wins.\n");

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
        teamService.UpdateTeam("Red Birds", "Angry Birds");
        Team updatedTeam = teamService.GetTeamByName("Angry Birds");
        Console.WriteLine($"After Update: Team name is {updatedTeam.Name}\n");

        // Delete one object and display the remaining collection.
        Console.WriteLine("[DELETE]");
        Console.WriteLine("Removing team 'Blue Birds");
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
        Team team3 = new Team("Sad Pigs", "sad_pigs.png");
        teamService.CreateTeam(team3);

        // Eliminate sad pigs
        team3.Eliminate();
        Console.WriteLine($"Team {team3.Name} has been eliminated.");

        try
        {
            Match invalidMatch = new Match(updatedTeam, team3, DateTime.Now);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Expected Domain Exception Caught: {ex.Message}");
            
            int currentCount = 0;
            foreach (var t in teamService.GetAllTeams())
            {
                currentCount++;
            }

            Console.WriteLine($"State Verification: '{team3.Name}' remains eliminated = {team3.IsEliminated}, " +
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
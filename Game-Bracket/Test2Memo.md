# Test 2 Memo
## System Actors and Selected Use Cases

### Tournament Organizer (external)
### Use case: Initialize and Schedule Tournament Bracket. 
The organizer registers teams, selects a bracket strategy, and 
triggers the MatchSchedulingService to generate the complete tournament schedule.

### Team Manager (external)
### Use case: Register Team and Manage Roster.
The manager registers new players into the system via PlayerService and assigns them to a team roster 
via TeamService while adhering to unique name constraints.

### Spectator (external)
### Use case: View Live Standings and Leaderboard.
The spectator requests current team standings and player rankings from the LeaderboardService to track ongoing 
tournament progress.

## Use Case Specifications

### Use Case 1 - Initialize and Schedule Tournament Bracket
* **Actor**: Tournament Organizer
* **Goal**: Generate and schedule the tournament bracket for all registered teams.
* **Trigger**: The organizer selects the bracket generation option from `Program.cs`.
* **Preconditions**: At least two valid teams are registered in `TeamService`.
* **Primary Flow**:
1. The organizer picks the tournament type.
2. `Program.cs` calls `MatchSchedulingService.CreateBracket()`, passing in the teams.
3. The service uses the selected tournament type to build the bracket structure and pairings.
4. All generated matches are saved into the master match collection (`_matches`).
5. The system returns the generated bracket so it can be displayed.
* **Failure Flow**: If there are fewer than two teams or the format is invalid, an exception is thrown and the bracket isn't created.
* **Postconditions**: A new bracket is generated and all matches are tracked in the scheduling service's collection.

### Use Case 2 - Register Team and Manage Roster
* **Actor**: Team Manager
* **Goal**: Create a new team with a unique name and assign players to its roster.
* **Trigger**: The manager submits a new team registration via `Program.cs`.
* **Preconditions**: Players must already be registered in the system's player service.
* **Primary Flow**:
1. The manager provides a unique team name.
2. `Program.cs` calls `TeamService.CreateTeam()`.
3. The service checks the private collection to ensure the name is unique.
4. The team is added to the `TeamService` master collection.
5. The manager adds players from the global pool to the team roster.
* **Failure Flow**: If the team name already exists, `TeamService` throws an exception preventing duplicates.
* **Postconditions**: A new team is added to the master list with a unique name and active roster.

### Use Case 3 - View Live Standings and Leaderboard
* **Actor**: Spectator
* **Goal**: Check current team standings and player rankings.
* **Trigger**: The spectator requests to view the leaderboard from the console menu.
* **Preconditions**: Teams, players, and match data must exist in the system.
* **Primary Flow**:
1. The spectator selects the leaderboard option.
2. `Program.cs` calls `LeaderboardService.GetTeamStandings()` and `LeaderboardService.GetPlayerRankings()`.
3. The service pulls the current team and player data.
4. Win/loss records and stats are calculated.
5. The standings are printed to the console.
* **Failure Flow**: If no matches or teams exist yet, the service returns empty lists without crashing.
* **Postconditions**: Up-to-date standings and rankings are displayed to the spectator.

## Responsibility and Collaboration Traces

### Trace 1: Tournament Organizer - Tournament Management

#### 1a. Creating Bracket and Scheduling Matches
Tournament Organizer -> Program.cs -> ITeamService
-> TeamService
-> IMatchSchedulingService
-> MatchSchedulingService
-> IBracketType -> SingleEliminationType
-> Bracket -> Match -> Team (Domain Objects)

#### 1b. Recording Match Results and Advancing Bracket
Tournament Organizer -> Program.cs -> ITournamentProgressionService
-> TournamentProgressionService
-> IMatchSchedulingService
-> MatchSchedulingService
-> Match -> Team -> Bracket (Domain Objects)

### Trace 2: Team Manager Registers Team and Adds Players
Team Manager -> Program.cs -> ITeamService
-> TeamService
-> IPlayerService
-> PlayerService
-> Team -> Player (Domain Objects)

### Trace 3: Spectator Views Leaderboards and Standings
Spectator -> Program.cs -> ILeaderboardService
-> LeaderboardService
-> ITeamService -> TeamService
-> IPlayerService -> PlayerService
-> Team -> Player -> FPSStats (Domain Objects)

## SOLID Design Reflection
### S - Single Responsibility
Every class owns a single, cohesive responsibility tied to a specific  actor/stakeholder.
For example, `LeaderboardService` exclusively manages sorting and querying standings and rankings.
The actor responsible who can request this class to change is the spectator.

### O - Open/Closed
Our system allows extension without requiring modification through
polymorphism. For example, `LeaderboardService.GetPlayerRankings` evaluates performance polymorphically via abstract 
`PlayerStats.GetStatValue`, allowing new game stats, such as Soccer, to be added without altering service logic.
Similarly, new tournament formats are introduced by implementing `IBracketType`.

### L - Liskov Substitution
`FPSStats` implements `PlayerStats` while honoring expected return types and non-negative numeric invariants.

Scenario: A subclass that bypassed validation to permit negative kills or threw unexpected exceptions instead of safe 
fallbacks during `GetStatValue` would break substitutability and expected failure behavior.

### I - Interface Segregation
The service layer is segregated into `IMatchSchedulingService`, `ITournamentProgressionService`, 
`ILeaderboardService`, `ITeamService`, and `IPlayerService`.

Clients depend only on the exact service methods required for their specific workflow.
Like spectators would depend on ILeaderBoardService, tournament organizers would use IMatchSchedulingService/ITournamentProgressionService
while team managers would use ITeamService/IPlayerService.

### D - Dependency Inversion
After reviewing high-level modules such as Program.cs and our other services.
A place where high-level policy could otherwise depend
directly on a low-level concrete implementation would be in Program.cs
with direct instantiation, but instead we depend strictly on abstractions like `IMatchSchedulingService` and `ILeaderboardService`.
This ensures system wiring remains flexible and protected from internal changes.

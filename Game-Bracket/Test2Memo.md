# Test 2 Memo
## System Actors and Selected Use Cases

### 1. Team Manager
* **Actor Type:** External Actor
* **Planned Service:** RegistrationService
* **Use Case:** Register new teams, retrieve team profiles by name, and manage 
team records (updates and deletions) prior to tournament generation.

### 2. Tournament Organizer
* **Actor Type:** External Actor
* **Planned Service:** RandomizerService
* **Use Case:** Generate initial tournament match pairings and schedules randomly using 
the verified pool of registered teams.

### 3. Tournament Organizer
* **Actor Type:** External Actor
* **Planned Service:** LowerBracketService
* **Use Case:** Process match results to track elimination rules, determine progression, and route losing
teams into the loser's bracket.

## Use Case Specifications
### Use Case 1 - Team Registration
* **Actor:** Team Manager
* **Goal:** Successfully register new competing teams into the tournament system with unique names and logos.
* **Trigger:** The Team Manager requests team registration via the application entry point (`Program.cs`).
* **Preconditions:** The application is running, and an instance of `RegistrationService` (backed by `ITeamService`) is initialized.
* **Primary Flow:**
1. The manager selects the team registration option.
2. The system prompts for a team name and logo URL/path.
3. The manager provides valid inputs.
4. `RegistrationService` validates that the name and logo are not null or whitespace.
5. The service checks the existing team collection to ensure name uniqueness (case-insensitive).
6. A new `Team` instance is created and successfully added to the system repository.
* **Failure Flow:**
    * *Condition:* Input fields are blank/null, or a team with the exact same name already exists.
    * *Failure Behavior:* An `ArgumentException` or `InvalidOperationException` is thrown, catching the error and displaying a descriptive message to the console without modifying the registry.
* **Postconditions:** The team is recorded in the system inventory with zero wins, zero losses, and `IsEliminated` set to false.

### Use Case 2 - Random Bracket Matchup Generation
* **Actor:** Tournament Organizer
* **Goal:** Randomly pair registered teams and generate a structured tournament match schedule.
* **Trigger:** The Tournament Organizer triggers bracket generation from `Program.cs` once team registration closes.
* **Preconditions:** At least two active, non-eliminated teams are registered in the system.
* **Primary Flow:**
  1. The organizer requests initial bracket generation.
  2. `RandomizerService` retrieves all registered teams from the team repository.
  3. The service validates that there is a sufficient team count.
  4. The teams are randomly shuffled and paired into individual `Match` objects with assigned start times.
  5. A new `Bracket` object containing these matches is instantiated and presented to the organizer.
* **Failure Flow:**
  * *Condition:* Fewer than two teams are registered in the system.
  * *Failure Behavior:* An `InvalidOperationException` is thrown, informing the user that minimum team requirements are not met.
* **Postconditions:** A new tournament bracket is structured with valid, scheduled matches between distinct active teams.

### Use Case 3 - Loser's Bracket Elimination and Progression
* **Actor:** Tournament Organizer
* **Goal:** Process completed match outcomes, record wins/losses, and route defeated teams into the loser's bracket.
* **Trigger:** A scheduled match concludes, and the organizer inputs the match results into the system.
* **Preconditions:** The target match exists, is currently active, and involves two non-eliminated teams.
* **Primary Flow:**
  1. The organizer submits the match outcome specifying the winner.
  2. `LowerBracketService` locates the match and identifies the winning and losing teams.
  3. The winner's win counter is incremented; the loser's loss counter is incremented.
  4. The losing team's state is updated via `Eliminate()`.
  5. The defeated team is automatically queued and routed into the loser's bracket structure for subsequent scheduling.
* **Failure Flow:**
  * *Condition:* The match ID cannot be found, or a team involved is already marked as eliminated.
  * *Failure Behavior:* A `KeyNotFoundException` or `InvalidOperationException` is thrown and handled, preventing corrupted state transitions.
* **Postconditions:** The match result is logged, the loser is officially marked as eliminated from the primary bracket, and their transition to the loser's bracket is secured.
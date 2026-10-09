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
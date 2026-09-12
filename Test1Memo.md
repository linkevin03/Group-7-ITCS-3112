# Test 1 Memo
## Complete Noun and Noun-Phrase Inventory
Team, win-loss records, Organizers, matches, elimination, loser's bracket, bracket, match schedules, team matchups, prize, match telemetry data


## Keep as a Class
Bracket: The overall structure of matches, supporting both winner and loser brackets
Team: Contain their own names, logos, elimination state, and win-loss records
Match: Represents the scheduled match between two distinct teams


## Reclassify as an Attribute
Elimination - Describes a state or action of a team (IsEliminated)
Match schedules - Clarifying data on when the matches take place (StartTime)
Win-Loss Records - Tracking team performance (Wins, Losses)
Loser's bracket - Categorized via bracket type attributes (BracketType)


## Remove with Reasoning
Organizers: external actor
Prize: outside our boundary
Match Telemetry data: outside our boundary
Team matchups: duplicate: match
Matches: duplicate: match


## Selected Domain Objects and CRC Cards
Bracket, Team, Match


## Class Invariants and Enforcement
Bracket: A Bracket must maintain a collection containing at least one match. 
Enforced by the Bracket constructor, which validates the size of the passed match collection upon creation.

Team: Team instance must possess a valid, non-empty name and logo, along with non-negative win and loss counts and a valid initial elimination state.
Enforced by the Team constructor, which rejects invalid or blank strings by throwing an exception.

Match: Match must reference two distinct, non-null teams, and neither participating team can be marked as eliminated when the match is instantiated.
Enforced by the Match constructor, which evaluates team references and elimination properties before assignment.

## CRUD Service and Service Invariants
Domain object: Team
Service Interface: ITeamService
Service Implementation: TeamService


Create: Add one valid, non-null Team object to the managed collection.
Read: Retrieve a Team by its unique name and retrieve a read-only collection of all teams.
Update: Locate an existing Team and change its state (e.g., name, win-loss records).
Delete: Remove an existing Team from the collection.


Service Invariants:
The service ensures that no managed teams can share the same unique name, rejecting duplicates.
The service prevents null additions and only exposes necessary data via read-only collections, ensuring that external callers cannot alter the collection directly.
The service must prevent any CRUD operations on non-existent teams by throwing an exception.

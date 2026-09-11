# Test 1 Memo  
## Complete Noun and Noun-Phrase Inventory
Team, win-loss records, Organizers, matches, elimination, loser's bracket, bracket, match schedules, team matchups, prize, match telemetry data


## Keep as a Class
Bracket: The overall structure of matches
Team: Contain their own names, logos, etc.
Matches: Represents the scheduled matches between teams


## Reclassify as an Attribute
Elimination - Describes a state or action of a team
Match schedules - Clarifying data on when the matches take place
Win-Loss Records - Tracking the performance


## Remove with Reasoning
Organizers: external actor
Loser’s bracket: duplicate: bracket
Prize: outside our boundary
Match Telemetry data: outside our boundary
Team matchups: duplicate: matches


## Selected Domain Objects and CRC Cards
Bracket, Team, Matches


## Class Invariants and Enforcement
Bracket: A Bracket must maintain a collection containing strictly more than one match. A Bracket must maintain a collection containing strictly more than one match.

Team: A Team instance must possess a valid, non-empty name, logo, and region throughout its lifecycle. A Team instance must possess a valid, non-empty name, logo, and region throughout its lifecycle.

Matches: Matches must reference two distinct, non-null teams, and neither participating team can be marked as eliminated when the match is instantiated. Enforced by the Matches constructor, which evaluates team references and elimination properties before assignment.

# Game Bracket
## Team Members
- Kevin Lin - linkevin03
- John Kim - JK-0096
- Greg Lowther - MightbetheG
- …
## Problem Statement
In competitive gaming tournaments, multiple teams compete to be the proclaimed best, however it is hard to 
keep track of which team is eliminated, who advances, who’s in the loser bracket, and which teams goes against each other. 
Organizers and teams spend a lot of time meeting, resolving scheduling conflicts, and drawing lots to create a structured competitive tournament. 
The domain requires managing match progression, win-loss records, and elimination rules across brackets. 
The proposed system must provide team matchups, time scheduling, tracking which team loses and can compete in the loser’s bracket. 
The system boundary includes generated team matchups, match schedules, and brackets for the winning/losing teams. 
Outside of the system boundary we exclude match telemetry data (e.g., goals or KDA) and prize management.

# Test 3 Memo
## Design Pattern Integration Reflection

### John Kim
#### Design-Pattern Selection and Justification
* **Selected Pattern:** Command Pattern
* **What it does:** It turns a user action like changing a match winner or fixing a mistake in the score into its own task. This keeps the button or menu click completely separate from the actual code that updates the tournament bracket.
* **Why we used it:** Makes changing match winners a cleaner process, and gives an easier way to undo mistakes/rollback the bracket if a match result is disputed.

### Kevin Lin
#### Design-Pattern Selection and Justification
- **Selected Pattern:** Strategy Pattern
- **Supported End-to-End Use Case:** Case 10 - Initialize and Schedule Tournament Bracket
- **Problem Motivated By:** Hardcoding tournament bracket algorithms (such as single elimination and future double-elimination formats) directly into the scheduling service using conditional `if/else` or `switch` statements, which leads to tight coupling and rigid code.
- **Why We Used It:** It encapsulates bracket generation algorithms into interchangeable strategy classes (`IBracketType`), adhering to the Open/Closed Principle. This allows new tournament formats to be added seamlessly without modifying core service logic (`MatchSchedulingService`).
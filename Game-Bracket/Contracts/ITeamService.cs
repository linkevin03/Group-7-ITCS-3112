using System.Collections.Generic;
using Game_Bracket.Domain;

namespace Game_Bracket.Contracts;

public interface ITeamService
{
    void CreateTeam(Team team);
    Team GetTeamByName(string name);
    IEnumerable<Team> GetAllTeams();
    void UpdateTeam(string currentName, string newName);
    void DeleteTeam(string name);
}
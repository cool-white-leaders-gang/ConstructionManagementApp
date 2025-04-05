using System.Collections.Generic;
using System.Linq;
using ConstructionManagementApp.App.Database;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Repositories
{
    internal class TeamRepository
    {
        private readonly AppDbContext _context;

        public TeamRepository(AppDbContext context)
        {
            _context = context;
        }

        public void CreateTeam(Team team)
        {
            _context.Teams.Add(team);
            _context.SaveChanges();
        }

        public void UpdateTeam(Team team)
        {
            var existingTeam = GetTeamById(team.Id);
            if (existingTeam == null)
                throw new KeyNotFoundException("Nie znaleziono zespołu o podanym Id.");

            _context.Teams.Update(team);
            _context.SaveChanges();
        }

        public void DeleteTeam(int teamId)
        {
            var team = GetTeamById(teamId);
            if (team == null)
                throw new KeyNotFoundException("Nie znaleziono zespołu o podanym Id.");

            _context.Teams.Remove(team);
            _context.SaveChanges();
        }

        public Team GetTeamById(int teamId)
        {
            return _context.Teams.FirstOrDefault(t => t.Id == teamId);
        }

        public List<Team> GetAllTeams()
        {
            return _context.Teams.ToList();
        }
    }
}

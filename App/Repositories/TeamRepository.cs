using System;
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
            if (team == null)
                throw new ArgumentNullException(nameof(team), "Zespół nie może być null.");

            _context.Teams.Add(team);
            _context.SaveChanges();
        }

        public void UpdateTeam(Team team)
        {
            var existingTeam = GetTeamById(team.Id);
            if (existingTeam == null)
                throw new KeyNotFoundException("Nie znaleziono zespołu o podanym Id.");

            existingTeam.Name = team.Name;
            existingTeam.ManagerId = team.ManagerId;

            _context.Teams.Update(existingTeam);
            _context.SaveChanges();
        }

        public void DeleteTeamById(int teamId)
        {
            var team = GetTeamById(teamId);
            if (team == null)
                throw new KeyNotFoundException("Nie znaleziono zespołu o podanym Id.");

            _context.Teams.Remove(team);
            _context.SaveChanges();
        }

        public Team GetTeamById(int id)
        {
            return _context.Teams.FirstOrDefault(t => t.Id == id);
        }

        public Team GetTeamByName(string name)
        {
            return _context.Teams.FirstOrDefault(t => t.Name == name);
        }

        public List<Team> GetAllTeams()
        {
            return _context.Teams.ToList();
        }
    }
}

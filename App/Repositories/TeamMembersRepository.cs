using System;
using System.Collections.Generic;
using System.Linq;
using ConstructionManagementApp.App.Database;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Repositories
{
    internal class TeamMembersRepository
    {
        private readonly AppDbContext _context;

        public TeamMembersRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddMemberToTeam(int teamId, int userId)
        {
            var team = _context.Teams.FirstOrDefault(t => t.Id == teamId);
            if (team == null)
                throw new KeyNotFoundException($"Nie znaleziono zespołu o ID {teamId}.");

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                throw new KeyNotFoundException($"Nie znaleziono użytkownika o ID {userId}.");

            // Sprawdzenie, czy użytkownik już jest członkiem zespołu
            if (_context.TeamMembers.Any(tm => tm.TeamId == teamId && tm.UserId == userId))
                throw new InvalidOperationException($"Użytkownik o ID {userId} jest już członkiem zespołu o ID {teamId}.");

            var teamMember = new TeamMembers(teamId, userId);
            _context.TeamMembers.Add(teamMember);
            _context.SaveChanges();
        }

        public void RemoveMemberFromTeam(int teamId, int userId)
        {
            var teamMember = _context.TeamMembers.FirstOrDefault(tm => tm.TeamId == teamId && tm.UserId == userId);
            if (teamMember == null)
                throw new KeyNotFoundException($"Nie znaleziono użytkownika o ID {userId} w zespole o ID {teamId}.");

            _context.TeamMembers.Remove(teamMember);
            _context.SaveChanges();
        }

        public List<User> GetMembersOfTeam(int teamId)
        {
            return _context.TeamMembers
                .Where(tm => tm.TeamId == teamId)
                .Select(tm => tm.User)
                .ToList();
        }
    }
}
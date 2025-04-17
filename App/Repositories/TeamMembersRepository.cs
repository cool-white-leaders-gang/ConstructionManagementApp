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
        private readonly UserRepository _userRepository;

        public TeamMembersRepository(AppDbContext context, UserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public void AddMemberToTeam(int teamId, string userName)
        {
            var team = _context.Teams.FirstOrDefault(t => t.Id == teamId);
            if (team == null)
                throw new KeyNotFoundException($"Nie znaleziono zespołu o ID {teamId}.");

            var user = _userRepository.GetUserByUsername(userName);
            if (user == null)
                throw new KeyNotFoundException($"Nie znaleziono użytkownika o nazwie {userName}.");

            // Sprawdzenie, czy użytkownik już jest członkiem zespołu
            if (_context.TeamMembers.Any(tm => tm.TeamId == teamId && tm.UserId == user.Id))
                throw new InvalidOperationException($"Użytkownik {userName} jest już członkiem zespołu o ID {teamId}.");

            var teamMember = new TeamMembers(teamId, user.Id);
            _context.TeamMembers.Add(teamMember);
            _context.SaveChanges();
        }

        public void RemoveMemberFromTeam(int teamId, string userName)
        {
            var user = _userRepository.GetUserByUsername(userName);
            if (user == null)
                throw new KeyNotFoundException($"Nie znaleziono użytkownika o nazwie {userName}."); 
            var teamMember = _context.TeamMembers.FirstOrDefault(tm => tm.TeamId == teamId && tm.UserId == user.Id);
            if (teamMember == null)
                throw new KeyNotFoundException($"Nie znaleziono użytkownika {userName} w zespole o ID {teamId}.");

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
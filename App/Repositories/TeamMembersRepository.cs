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

        // Konstruktor repozytorium - przyjmuje kontekst bazy danych i repozytorium użytkowników
        public TeamMembersRepository(AppDbContext context, UserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        // Dodanie członka do zespołu
        public void AddMemberToTeam(int teamId, string userName)
        {
            // Sprawdzenie, czy zespół o danym ID istnieje
            var team = _context.Teams.FirstOrDefault(t => t.Id == teamId);
            if (team == null)
                throw new KeyNotFoundException($"Nie znaleziono zespołu o ID {teamId}.");

            // Pobranie użytkownika na podstawie nazwy użytkownika
            var user = _userRepository.GetUserByUsername(userName);
            if (user == null)
                throw new KeyNotFoundException($"Nie znaleziono użytkownika o nazwie {userName}.");

            // Sprawdzenie, czy użytkownik już jest członkiem zespołu
            if (_context.TeamMembers.Any(tm => tm.TeamId == teamId && tm.UserId == user.Id))
                throw new InvalidOperationException($"Użytkownik {userName} jest już członkiem zespołu o ID {teamId}.");

            // Dodanie nowego członka do zespołu
            var teamMember = new TeamMembers(teamId, user.Id);
            _context.TeamMembers.Add(teamMember);
            _context.SaveChanges();
        }

        // Usunięcie członka z zespołu
        public void RemoveMemberFromTeam(int teamId, string userName)
        {
            // Pobranie użytkownika na podstawie nazwy użytkownika
            var user = _userRepository.GetUserByUsername(userName);
            if (user == null)
                throw new KeyNotFoundException($"Nie znaleziono użytkownika o nazwie {userName}.");

            // Sprawdzenie, czy użytkownik jest członkiem zespołu
            var teamMember = _context.TeamMembers.FirstOrDefault(tm => tm.TeamId == teamId && tm.UserId == user.Id);
            if (teamMember == null)
                throw new KeyNotFoundException($"Nie znaleziono użytkownika {userName} w zespole o ID {teamId}.");

            // Usunięcie członka z zespołu
            _context.TeamMembers.Remove(teamMember);
            _context.SaveChanges();
        }

        // Pobranie wszystkich członków zespołu na podstawie ID zespołu
        public List<User> GetMembersOfTeam(int teamId)
        {
            return _context.TeamMembers
                .Where(tm => tm.TeamId == teamId)
                .Select(tm => tm.User)
                .ToList();
        }
    }
}

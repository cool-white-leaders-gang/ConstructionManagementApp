using System;
using System.Collections.Generic;
using System.Linq;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Database;

namespace ConstructionManagementApp.App.Repositories
{
    internal class TeamMembersRepository
    {
        private readonly AppDbContext _context;

        public TeamMembersRepository(AppDbContext context)
        {
            _context = context;
        }

        // Dodaj użytkownika do zespołu
        public void AddUserToTeam(int teamId, int userId)
        {
            try
            {
                // sprawdzam czy istnieje team
                var team = _context.Teams.FirstOrDefault(t => t.Id == teamId);
                if (team == null)
                    throw new ArgumentException($"Zespół o Id {teamId} nie istnieje.");

                // sprawdzam czy istnieje user
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                    throw new ArgumentException($"Użytkownik o Id {userId} nie istnieje.");

                // sprawdzam czy user ma role Worker
                if (user.Role != Role.Worker)
                    throw new InvalidOperationException($"Użytkownik o Id {userId} nie ma roli Worker i nie może być dodany do zespołu.");

                // sprawdzam czy jest juz nie ma pracownika w teamie
                var existingAssignment = _context.TeamMembers
                    .FirstOrDefault(tm => tm.TeamId == teamId && tm.UserId == userId);
                if (existingAssignment != null)
                    throw new InvalidOperationException($"Użytkownik o Id {userId} jest już członkiem zespołu o Id {teamId}.");

                // jeśli nie wywaliło błędu to przypisuje usera to teamu
                var teamMember = new TeamMember(teamId, userId);
                _context.TeamMembers.Add(teamMember);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas dodawania użytkownika do zespołu: {ex.Message}");
                throw;
            }
        }

        // Pobiera listę użytkowników w zespole
        public List<User> GetUsersInTeam(int teamId)
        {
            try
            {
                // sprawdza czy zespol w ogole istnieje
                var team = _context.Teams.FirstOrDefault(t => t.Id == teamId);
                if (team == null)
                    throw new ArgumentException($"Zespół o Id {teamId} nie istnieje.");

                // jesli istnieje to pobieram pracownikow z danego teamu
                return _context.TeamMembers
                    .Where(tm => tm.TeamId == teamId)
                    .Select(tm => tm.User)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas pobierania użytkowników w zespole: {ex.Message}");
                throw;
            }
        }

        // Usuń użytkownika z zespołu
        public void RemoveUserFromTeam(int teamId, int userId)
        {
            try
            {
                // Znajduje przypisanie uzytkownika
                var teamMember = _context.TeamMembers
                    .FirstOrDefault(tm => tm.TeamId == teamId && tm.UserId == userId);

                if (teamMember == null)
                    throw new ArgumentException($"Przypisanie użytkownika o Id {userId} do zespołu o Id {teamId} nie istnieje.");

                // usuwa przypisanie
                _context.TeamMembers.Remove(teamMember);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas usuwania użytkownika z zespołu: {ex.Message}");
                throw;
            }
        }
    }
}
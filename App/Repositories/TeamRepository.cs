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

        // Konstruktor repozytorium - przyjmuje kontekst bazy danych
        public TeamRepository(AppDbContext context)
        {
            _context = context;
        }

        // Dodanie nowego zespołu do bazy danych
        public void CreateTeam(Team team)
        {
            // Sprawdzenie, czy zespół nie jest null
            if (team == null)
                throw new ArgumentNullException(nameof(team), "Zespół nie może być null.");

            // Dodanie zespołu do bazy danych i zapisanie zmian
            _context.Teams.Add(team);
            _context.SaveChanges();
        }

        // Aktualizacja istniejącego zespołu
        public void UpdateTeam(Team team)
        {
            // Pobranie istniejącego zespołu na podstawie ID
            var existingTeam = GetTeamById(team.Id);
            if (existingTeam == null)
                throw new KeyNotFoundException("Nie znaleziono zespołu o podanym Id.");

            // Zaktualizowanie właściwości zespołu
            existingTeam.Name = team.Name;
            existingTeam.ManagerId = team.ManagerId;

            // Aktualizacja zespołu w bazie danych i zapisanie zmian
            _context.Teams.Update(existingTeam);
            _context.SaveChanges();
        }

        // Usunięcie zespołu z bazy danych
        public void DeleteTeamById(int teamId)
        {
            // Pobranie zespołu na podstawie ID
            var team = GetTeamById(teamId);
            if (team == null)
                throw new KeyNotFoundException("Nie znaleziono zespołu o podanym Id.");

            // Usunięcie zespołu z bazy danych i zapisanie zmian
            _context.Teams.Remove(team);
            _context.SaveChanges();
        }

        // Pobranie zespołu na podstawie ID
        public Team GetTeamById(int id)
        {
            return _context.Teams.FirstOrDefault(t => t.Id == id);
        }

        // Pobranie zespołu na podstawie nazwy
        public Team GetTeamByName(string name)
        {
            return _context.Teams.FirstOrDefault(t => t.Name == name);
        }

        // Pobranie wszystkich zespołów
        public List<Team> GetAllTeams()
        {
            return _context.Teams.ToList();
        }
    }
}

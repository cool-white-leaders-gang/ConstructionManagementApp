using System;
using System.Collections.Generic;
using System.Linq;
using ConstructionManagementApp.App.Database;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Repositories
{
    internal class IssueRepository
    {
        private readonly AppDbContext _context;

        // Konstruktor repozytorium, inicjalizuje kontekst bazy danych
        public IssueRepository(AppDbContext context)
        {
            _context = context;
        }

        // Dodanie nowego zgłoszenia do bazy danych
        public void AddIssue(Issue issue)
        {
            if (issue == null)
                throw new ArgumentNullException(nameof(issue), "Zgłoszenie nie może być null.");

            // Dodanie zgłoszenia i zapisanie zmian w bazie
            _context.Issues.Add(issue);
            _context.SaveChanges();
        }

        // Aktualizacja istniejącego zgłoszenia
        public void UpdateIssue(Issue issue)
        {
            // Sprawdzenie, czy zgłoszenie o podanym Id istnieje
            var existingIssue = GetIssueById(issue.Id);
            if (existingIssue == null)
                throw new KeyNotFoundException("Nie znaleziono zgłoszenia o podanym Id.");

            // Aktualizacja danych zgłoszenia
            existingIssue.Priority = issue.Priority;
            existingIssue.Status = issue.Status;
            existingIssue.ResolvedAt = issue.ResolvedAt;

            // Zaktualizowanie zgłoszenia w bazie danych
            _context.Issues.Update(existingIssue);
            _context.SaveChanges();
        }

        // Usunięcie zgłoszenia na podstawie jego ID
        public void DeleteIssueById(int issueId)
        {
            // Pobranie zgłoszenia na podstawie ID
            var issue = GetIssueById(issueId);
            if (issue == null)
                throw new KeyNotFoundException("Nie znaleziono zgłoszenia o podanym Id.");

            // Usunięcie zgłoszenia z bazy
            _context.Issues.Remove(issue);
            _context.SaveChanges();
        }

        // Pobranie zgłoszenia na podstawie ID
        public Issue GetIssueById(int id)
        {
            // Zwrócenie zgłoszenia lub null, jeśli nie znaleziono
            return _context.Issues.FirstOrDefault(i => i.Id == id);
        }

        // Pobranie wszystkich zgłoszeń z bazy danych
        public List<Issue> GetAllIssues()
        {
            // Zwrócenie listy wszystkich zgłoszeń
            return _context.Issues.ToList();
        }
    }
}

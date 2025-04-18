using System;
using System.Collections.Generic;
using System.Linq;
using ConstructionManagementApp.App.Database;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Repositories
{
    internal class LogRepository
    {
        private readonly AppDbContext _context;

        // Konstruktor repozytorium, inicjalizuje kontekst bazy danych
        public LogRepository(AppDbContext context)
        {
            _context = context;
        }

        // Dodanie nowego logu do bazy danych
        public void AddLog(Log log)
        {
            if (log == null)
                throw new ArgumentNullException(nameof(log), "Log nie może być null.");

            // Dodanie logu i zapisanie zmian w bazie
            _context.Logs.Add(log);
            _context.SaveChanges();
        }

        // Pobranie wszystkich logów, posortowanych według czasu malejąco
        public List<Log> GetAllLogs()
        {
            return _context.Logs
                .OrderByDescending(l => l.Timestamp)  // Sortowanie logów według daty i godziny w porządku malejącym
                .ToList();  // Zwrócenie listy logów
        }

        // Pobranie logów zawierających określoną wiadomość
        public List<Log> GetLogsByMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Wiadomość logu nie może być pusta.");

            return _context.Logs
                .Where(l => l.Message.Contains(message, StringComparison.OrdinalIgnoreCase))  // Filtrowanie logów na podstawie wiadomości
                .OrderByDescending(l => l.Timestamp)  // Sortowanie logów według daty i godziny w porządku malejącym
                .ToList();  // Zwrócenie listy logów
        }

        // Pobranie logów z danego dnia
        public List<Log> GetLogsByDate(DateTime date)
        {
            return _context.Logs
                .Where(l => l.Timestamp.Date == date.Date)  // Filtrowanie logów na podstawie daty
                .OrderByDescending(l => l.Timestamp)  // Sortowanie logów według daty i godziny w porządku malejącym
                .ToList();  // Zwrócenie listy logów
        }
    }
}

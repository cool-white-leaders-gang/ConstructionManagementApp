using System;
using System.Collections.Generic;
using System.Linq;
using ConstructionManagementApp.App.Database;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Repositories
{
    internal class ProgressReportRepository
    {
        private readonly AppDbContext _context;

        // Konstruktor repozytorium, inicjalizuje kontekst bazy danych
        public ProgressReportRepository(AppDbContext context)
        {
            _context = context;
        }

        // Dodanie nowego raportu postępu
        public void AddProgressReport(ProgressReport report)
        {
            if (report == null)
                throw new ArgumentNullException(nameof(report), "Raport postępu nie może być null.");

            // Dodanie raportu do bazy danych i zapisanie zmian
            _context.ProgressReports.Add(report);
            _context.SaveChanges();
        }

        // Aktualizacja istniejącego raportu postępu
        public void UpdateProgressReport(ProgressReport report)
        {
            // Wyszukiwanie istniejącego raportu na podstawie Id
            var existingReport = GetProgressReportById(report.Id);
            if (existingReport == null)
                throw new KeyNotFoundException("Nie znaleziono raportu postępu o podanym Id.");

            // Aktualizacja danych w raporcie
            existingReport.Title = report.Title;
            existingReport.Content = report.Content;
            existingReport.CompletionPercentage = report.CompletionPercentage;

            // Zapisanie zaktualizowanego raportu w bazie danych
            _context.ProgressReports.Update(existingReport);
            _context.SaveChanges();
        }

        // Usunięcie raportu postępu na podstawie Id
        public void DeleteProgressReportById(int reportId)
        {
            var report = GetProgressReportById(reportId);
            if (report == null)
                throw new KeyNotFoundException("Nie znaleziono raportu postępu o podanym Id.");

            // Usunięcie raportu z bazy danych
            _context.ProgressReports.Remove(report);
            _context.SaveChanges();
        }

        // Pobranie raportu postępu na podstawie Id
        public ProgressReport GetProgressReportById(int id)
        {
            return _context.ProgressReports.FirstOrDefault(r => r.Id == id);
        }

        // Pobranie wszystkich raportów postępu
        public List<ProgressReport> GetAllProgressReports()
        {
            return _context.ProgressReports.ToList();
        }
    }
}

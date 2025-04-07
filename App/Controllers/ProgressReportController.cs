using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Controllers
{
    internal class ProgressReportController
    {
        private readonly ProgressReportRepository _progressReportRepository;

        public ProgressReportController(ProgressReportRepository progressReportRepository)
        {
            _progressReportRepository = progressReportRepository;
        }

        public void AddProgressReport(string title, string content, int userId, int projectId, int completionPercentage)
        {
            try
            {
                var report = new ProgressReport(title, content, userId, projectId, completionPercentage);
                _progressReportRepository.AddProgressReport(report);
                Console.WriteLine("Raport postępu został pomyślnie dodany.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nieoczekiwany błąd: {ex.Message}");
            }
        }

        public void UpdateProgressReport(int reportId, string title, string description, int completionPercentage)
        {
            try
            {
                var report = _progressReportRepository.GetProgressReportById(reportId);
                if (report == null)
                    throw new KeyNotFoundException($"Nie znaleziono raportu postępu o ID {reportId}.");

                report.Title = title;
                report.Content = description;
                report.CompletionPercentage = completionPercentage;

                _progressReportRepository.UpdateProgressReport(report);
                Console.WriteLine("Raport postępu został pomyślnie zaktualizowany.");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nieoczekiwany błąd: {ex.Message}");
            }
        }

        public void DeleteProgressReport(int reportId)
        {
            try
            {
                _progressReportRepository.DeleteProgressReportById(reportId);
                Console.WriteLine("Raport postępu został pomyślnie usunięty.");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nieoczekiwany błąd: {ex.Message}");
            }
        }

        public void DisplayAllProgressReports()
        {
            try
            {
                var reports = _progressReportRepository.GetAllProgressReports();
                if (reports.Count == 0)
                {
                    Console.WriteLine("Brak raportów postępu w systemie.");
                    return;
                }

                Console.WriteLine("--- Lista raportów postępu ---");
                foreach (var report in reports)
                {
                    Console.WriteLine($"ID: {report.Id}, Tytuł: {report.Title}, Data: {report.CreatedAt}, Opis: {report.Content}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }
    }
}
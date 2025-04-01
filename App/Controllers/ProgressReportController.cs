using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Repositories;

namespace ConstructionManagementApp.App.Controllers
{
    internal class ProgressReportController
    {
        private readonly ProgressReportRepository _progressReportRepository;

        // Konstruktor kontrolera
        public ProgressReportController(ProgressReportRepository progressReportRepository)
        {
            _progressReportRepository = progressReportRepository;
        }

        // Dodaj nowy raport postępu
        public void CreateProgressReport(string title, string content, int createdByUserId, int projectId, int completionPercentage)
        {
            try
            {
                var progressReport = new ProgressReport(title, content, createdByUserId, projectId, completionPercentage);
                _progressReportRepository.CreateProgressReport(progressReport);
                Console.WriteLine("Raport postępu został pomyślnie dodany.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas dodawania raportu postępu: {ex.Message}");
            }
        }

        // Zaktualizuj raport postępu
        public void UpdateProgressReport(int reportId, string title, string content, int completionPercentage)
        {
            try
            {
                var progressReport = _progressReportRepository.GetReportById(reportId);
                if (progressReport == null)
                {
                    Console.WriteLine($"Raport postępu o Id {reportId} nie został znaleziony.");
                    return;
                }

                progressReport.Title = title;
                progressReport.Content = content;
                progressReport.CompletionPercentage = completionPercentage;

                _progressReportRepository.UpdateProgressReport(progressReport);
                Console.WriteLine("Raport postępu został pomyślnie zaktualizowany.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas aktualizacji raportu postępu: {ex.Message}");
            }
        }

        // Usuń raport postępu
        public void DeleteProgressReport(int reportId)
        {
            try
            {
                _progressReportRepository.DeleteProgressReport(reportId);
                Console.WriteLine("Raport postępu został pomyślnie usunięty.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas usuwania raportu postępu: {ex.Message}");
            }
        }

        // Wyświetl wszystkie raporty postępu
        public void DisplayAllProgressReports()
        {
            try
            {
                var progressReports = _progressReportRepository.GetAllReports();
                if (progressReports.Count == 0)
                {
                    Console.WriteLine("Brak raportów postępu do wyświetlenia.");
                    return;
                }

                foreach (var report in progressReports)
                {
                    Console.WriteLine(report.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas pobierania raportów postępu: {ex.Message}");
            }
        }

        // Wyświetl raport postępu po Id
        public void DisplayProgressReportById(int reportId)
        {
            try
            {
                var progressReport = _progressReportRepository.GetReportById(reportId);
                if (progressReport == null)
                {
                    Console.WriteLine($"Raport postępu o Id {reportId} nie został znaleziony.");
                    return;
                }

                Console.WriteLine(progressReport.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas pobierania raportu postępu: {ex.Message}");
            }
        }
    }
}
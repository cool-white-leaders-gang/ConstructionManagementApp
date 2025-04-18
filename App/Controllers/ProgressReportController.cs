using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Services;
using ConstructionManagementApp.Events;
using ConstructionManagementApp.App.Delegates;
using ConstructionManagementApp.App.Enums;

namespace ConstructionManagementApp.App.Controllers
{
    internal class ProgressReportController
    {
        private readonly ProgressReportRepository _progressReportRepository;
        private readonly AuthenticationService _authenticationService;
        private readonly ProjectRepository _projectRepository;
        private readonly RBACService _rbacService;

        // Eventy logujące akcje na raportach postępu
        public event LogEventHandler ProgressReportAdded;
        public event LogEventHandler ProgressReportDeleted;
        public event LogEventHandler ProgressReportUpdated;

        // Konstruktor controller-a inicjalizujący zależności
        public ProgressReportController(ProgressReportRepository progressReportRepository, AuthenticationService authenticationService, ProjectRepository projectRepository, RBACService rBACService)
        {
            _progressReportRepository = progressReportRepository;
            _authenticationService = authenticationService;
            _projectRepository = projectRepository;
            _rbacService = rBACService;
        }

        // Dodawanie raportu postępu do projektu
        public void AddProgressReport(string title, string content, string projectName, int completionPercentage)
        {
            try
            {
                // Pobieranie projektu po nazwie
                var project = _projectRepository.GetProjectByName(projectName);
                if (project == null)
                    throw new KeyNotFoundException($"Nie udało się znaleźć projektu o nazwie {projectName}");

                int projectId = project.Id;

                // Tworzenie nowego obiektu raportu
                ProgressReport report = new ProgressReport(title, content, _authenticationService.CurrentSession.User.Id, projectId, completionPercentage);

                // Dodanie raportu do repozytorium
                _progressReportRepository.AddProgressReport(report);
                Console.WriteLine("Raport postępu został pomyślnie dodany.");

                // Wywołanie eventu logowania
                ProgressReportAdded?.Invoke(this, new LogEventArgs(
                    _authenticationService.CurrentSession.User.Username,
                    $"Dodano nowy raport postępu o tytule {title}"));
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

        // Aktualizacja istniejącego raportu postępu
        public void UpdateProgressReport(int reportId, string title, string description, int completionPercentage)
        {
            try
            {
                // Wyszukiwanie raportu po ID
                var report = _progressReportRepository.GetProgressReportById(reportId);
                if (report == null)
                    throw new KeyNotFoundException($"Nie znaleziono raportu postępu o ID {reportId}.");

                // Aktualizacja właściwości raportu
                report.Title = title;
                report.Content = description;
                report.CompletionPercentage = completionPercentage;

                _progressReportRepository.UpdateProgressReport(report);
                Console.WriteLine("Raport postępu został pomyślnie zaktualizowany.");

                // Wywołanie eventu logowania
                ProgressReportUpdated?.Invoke(this, new LogEventArgs(
                    _authenticationService.CurrentSession.User.Username,
                    $"Zaktualizowano raport postępu o ID {reportId} i tytule {title}"));
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

        // Usuwanie raportu postępu po ID
        public void DeleteProgressReport(int reportId)
        {
            try
            {
                _progressReportRepository.DeleteProgressReportById(reportId);
                Console.WriteLine("Raport postępu został pomyślnie usunięty.");

                // Logowanie usunięcia
                ProgressReportDeleted?.Invoke(this, new LogEventArgs(
                    _authenticationService.CurrentSession.User.Username,
                    $"Usunięto raport postępu o ID {reportId}"));
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

        // Wyświetlanie raportów postępu — zależnie od roli użytkownika
        public void DisplayReportsForUser()
        {
            try
            {
                var currentUser = _authenticationService.CurrentSession.User;
                var userRole = currentUser.Role;
                var userId = currentUser.Id;
                List<ProgressReport> reports;

                if (userRole == Role.Admin)
                {
                    // Admin widzi wszystkie raporty
                    reports = _progressReportRepository.GetAllProgressReports();
                }
                else if (userRole == Role.Manager)
                {
                    // Manager widzi tylko raporty z projektów, którymi zarządza
                    var managerProjects = _rbacService.GetProjectsManagedBy(userId);
                    reports = _progressReportRepository.GetAllProgressReports()
                        .Where(report => managerProjects.Contains(report.ProjectId))
                        .ToList();
                }
                else if (userRole == Role.Client)
                {
                    // Klient widzi raporty dla projektów, w których jest klientem
                    reports = _progressReportRepository.GetAllProgressReports()
                        .Where(report => _projectRepository.GetProjectById(report.ProjectId)?.ClientId == userId)
                        .ToList();
                }
                else
                {
                    Console.WriteLine("Nie masz uprawnień do przeglądania raportów postępu.");
                    return;
                }

                if (!reports.Any())
                {
                    Console.WriteLine("Brak raportów postępu do wyświetlenia.");
                    return;
                }

                foreach (var report in reports)
                {
                    Console.WriteLine($"ID: {report.Id}, Tytuł: {report.Title}, Data: {report.CreatedAt}, Opis: {report.Content}, Procent ukończenia: {report.CompletionPercentage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }
    }
}

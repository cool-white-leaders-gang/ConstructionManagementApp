using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Enums;
using System.Net.Security;
using ConstructionManagementApp.App.Services;
using ConstructionManagementApp.Events;
using ConstructionManagementApp.App.Delegates;

namespace ConstructionManagementApp.App.Controllers
{
    // Kontroler odpowiedzialny za zarządzanie zgłoszeniami w systemie.
    internal class IssueController
    {
        private readonly IssueRepository _issueRepository;
        private readonly AuthenticationService _authenticationService;
        private readonly ProjectRepository _projectRepository;
        private readonly RBACService _rbacService;

        // Zdarzenia logujące operacje na zgłoszeniach
        public event LogEventHandler IssueAdded;
        public event LogEventHandler IssueDeleted;
        public event LogEventHandler IssueUpdated;

        // Konstruktor przypisujący zależności
        public IssueController(IssueRepository issueRepository, AuthenticationService authenticationService, ProjectRepository projectRepository, RBACService rbacService)
        {
            _issueRepository = issueRepository;
            _authenticationService = authenticationService;
            _projectRepository = projectRepository;
            _rbacService = rbacService;
        }

        // Dodaje nowe zgłoszenie do projektu
        public void AddIssue(string title, string content, string projectName, TaskPriority priority)
        {
            try
            {
                var currentUser = _authenticationService.CurrentSession.User;

                // Pobranie projektu na podstawie nazwy
                var project = _projectRepository.GetProjectByName(projectName);
                if (project == null)
                    throw new KeyNotFoundException($"Nie udało się znaleźć projektu o nazwie {projectName}");

                int projectId = project.Id;

                // Sprawdzenie uprawnień: pracownik zespołu lub manager
                var isWorkerInProject = _rbacService.IsWorkerInProjectTeam(currentUser, projectId, _projectRepository);
                var isManagerOfProject = _rbacService.IsProjectManager(currentUser, projectId, _projectRepository);

                if (!isWorkerInProject || !isManagerOfProject)
                    throw new UnauthorizedAccessException("Nie masz uprawnień do dodawania zgłoszeń do tego projektu.");

                // Tworzenie zgłoszenia i zapis do repozytorium
                var issue = new Issue(title, content, currentUser.Id, projectId, priority);
                _issueRepository.AddIssue(issue);

                Console.WriteLine("Zgłoszenie zostało pomyślnie dodane.");

                // Logowanie operacji
                IssueAdded?.Invoke(this, new LogEventArgs(currentUser.Username, $"Dodano nowe zgłoszenie o tytule {title}"));
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

        // Aktualizuje istniejące zgłoszenie (priorytet i status)
        public void UpdateIssue(int issueId, TaskPriority priority, IssueStatus status)
        {
            try
            {
                var currentUser = _authenticationService.CurrentSession.User;

                // Pobranie zgłoszenia po ID
                var issue = _issueRepository.GetIssueById(issueId);
                if (issue == null)
                    throw new KeyNotFoundException($"Nie znaleziono zgłoszenia o ID {issueId}.");

                // Sprawdzenie czy użytkownik jest managerem projektu zgłoszenia
                if (!_rbacService.IsProjectManager(currentUser, issue.ProjectId, _projectRepository))
                    throw new UnauthorizedAccessException("Nie masz uprawnień do aktualizowania zgłoszeń w tym projekcie");

                // Aktualizacja wartości
                issue.Priority = priority;
                issue.Status = status;

                // Jeśli zgłoszenie zostało rozwiązane, zapisz datę rozwiązania
                if (status == IssueStatus.Resolved)
                    issue.ResolvedAt = DateTime.Now;

                _issueRepository.UpdateIssue(issue);

                Console.WriteLine("Zgłoszenie zostało pomyślnie zaktualizowane.");

                // Logowanie operacji
                IssueUpdated?.Invoke(this, new LogEventArgs(currentUser.Username, $"Zaktualizowano zgłoszenie o ID {issueId} i nazwie {issue.Title}"));
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

        // Usuwa zgłoszenie z repozytorium
        public void DeleteIssue(int issueId)
        {
            try
            {
                var currentUser = _authenticationService.CurrentSession.User;

                // Pobranie zgłoszenia do usunięcia
                var issue = _issueRepository.GetIssueById(issueId);
                if (issue == null)
                    throw new KeyNotFoundException($"Nie znaleziono zgłoszenia o ID {issueId}.");

                // Sprawdzenie czy użytkownik ma uprawnienia managerskie
                if (!_rbacService.IsProjectManager(currentUser, issue.ProjectId, _projectRepository))
                    throw new UnauthorizedAccessException("Nie masz uprawnień do usuwania zgłoszeń w tym projekcie");

                // Usunięcie zgłoszenia
                _issueRepository.DeleteIssueById(issueId);
                Console.WriteLine("Zgłoszenie zostało pomyślnie usunięte.");

                // Logowanie operacji
                IssueDeleted?.Invoke(this, new LogEventArgs(currentUser.Username, $"Usunięto zgłoszenie o ID {issueId}"));
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

        // Wyświetla listę zgłoszeń odpowiednich dla aktualnie zalogowanego użytkownika
        public void DisplayIssuesForUser()
        {
            try
            {
                var currentUser = _authenticationService.CurrentSession.User;
                var userRole = currentUser.Role;
                var userId = currentUser.Id;
                List<Issue> issues;

                // Rola decyduje o zakresie widoczności zgłoszeń
                if (userRole == Role.Admin)
                {
                    issues = _issueRepository.GetAllIssues();  // Admin widzi wszystkie zgłoszenia
                }
                else if (userRole == Role.Manager)
                {
                    var managerProjects = _rbacService.GetProjectsManagedBy(userId);
                    issues = _issueRepository.GetAllIssues()
                        .Where(issue => managerProjects.Contains(issue.ProjectId)).ToList();  // Manager widzi swoje projekty
                }
                else if (userRole == Role.Worker)
                {
                    var workerProjects = _rbacService.GetProjectsForUserOrManagedBy(currentUser, _projectRepository);
                    issues = _issueRepository.GetAllIssues()
                        .Where(issue => workerProjects.Contains(issue.ProjectId)).ToList();  // Pracownik widzi swoje zgłoszenia
                }
                else
                {
                    Console.WriteLine("Nie masz uprawnień do przeglądania zgłoszeń.");
                    return;
                }

                // Jeśli brak zgłoszeń
                if (!issues.Any())
                {
                    Console.WriteLine("Brak zgłoszeń do wyświetlenia.");
                    return;
                }

                // Wyświetlanie zgłoszeń
                foreach (var issue in issues)
                {
                    Console.WriteLine($"ID: {issue.Id}, Tytuł: {issue.Title}, Priorytet: {issue.Priority}, Status: {issue.Status}, Data utworzenia: {issue.CreatedAt}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }
    }
}

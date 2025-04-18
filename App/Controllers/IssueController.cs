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
    internal class IssueController
    {
        private readonly IssueRepository _issueRepository;
        private readonly AuthenticationService _authenticationService;
        private readonly ProjectRepository _projectRepository;
        private readonly RBACService _rbacService;
        public event LogEventHandler IssueAdded;
        public event LogEventHandler IssueDeleted;
        public event LogEventHandler IssueUpdated;

        public IssueController(IssueRepository issueRepository, AuthenticationService authenticationService, ProjectRepository projectRepository, RBACService rbacService)
        {
            _issueRepository = issueRepository;
            _authenticationService = authenticationService;
            _projectRepository = projectRepository;
            _rbacService = rbacService;
        }

        public void AddIssue(string title, string content, string projectName, TaskPriority priority)
        {
            try
            {
                var currentUser = _authenticationService.CurrentSession.User;

                var project = _projectRepository.GetProjectByName(projectName);
                if (project == null)
                    throw new KeyNotFoundException($"Nie udało się znaleźć projektu o nazwie {projectName}");
                int projectId = project.Id;

                var isWorkerInProject = _rbacService.IsWorkerInProjectTeam(currentUser, projectId, _projectRepository);
                var isManagerOfProject = _rbacService.IsProjectManager(currentUser, projectId, _projectRepository);

                if (!isWorkerInProject || !isManagerOfProject)
                    throw new UnauthorizedAccessException("Nie masz uprawnień do dodawania zgłoszeń do tego projektu.");

                var issue = new Issue(title, content, _authenticationService.CurrentSession.User.Id, projectId, priority);
                _issueRepository.AddIssue(issue);
                Console.WriteLine("Zgłoszenie zostało pomyślnie dodane.");
                IssueAdded?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Dodano nowe zgłoszenie o tytule {title}"));
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

        public void UpdateIssue(int issueId, TaskPriority priority, IssueStatus status)
        {
            try
            {
                var currentUser = _authenticationService.CurrentSession.User;
                var issue = _issueRepository.GetIssueById(issueId);
                if (issue == null)
                    throw new KeyNotFoundException($"Nie znaleziono zgłoszenia o ID {issueId}.");

                if(!_rbacService.IsProjectManager(currentUser, issue.ProjectId, _projectRepository))
                    throw new UnauthorizedAccessException("Nie masz uprawnień do aktualizowania zgłoszeń w tym projekcie");

                issue.Priority = priority;
                issue.Status = status;
                if (status == IssueStatus.Resolved)
                    issue.ResolvedAt = DateTime.Now;

                _issueRepository.UpdateIssue(issue);
                Console.WriteLine("Zgłoszenie zostało pomyślnie zaktualizowane.");
                IssueUpdated?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Zaktualizowano zgłoszenie o ID {issueId} i nazwie {issue.Title}"));
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

        public void DeleteIssue(int issueId)
        {
            try
            {
                var currentUser = _authenticationService.CurrentSession.User;
                var issue = _issueRepository.GetIssueById(issueId);
                if (issue == null)
                    throw new KeyNotFoundException($"Nie znaleziono zgłoszenia o ID {issueId}.");

                if (!_rbacService.IsProjectManager(currentUser, issue.ProjectId, _projectRepository))
                    throw new UnauthorizedAccessException("Nie masz uprawnień do usuwania zgłoszeń w tym projekcie");

                _issueRepository.DeleteIssueById(issueId);
                Console.WriteLine("Zgłoszenie zostało pomyślnie usunięte.");
                IssueDeleted?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Usunięto zgłoszenie o ID {issueId}"));
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

        public void DisplayIssuesForUser()
        {
            try
            {
                var currentUser = _authenticationService.CurrentSession.User;
                var userRole = currentUser.Role;
                var userId = currentUser.Id;
                List<Issue> issues;

                if (userRole == Role.Admin)
                {
                    // Admin sees all issues
                    issues = _issueRepository.GetAllIssues();
                }
                else if (userRole == Role.Manager)
                {
                    // Manager sees issues related to projects they manage
                    var managerProjects = _rbacService.GetProjectsManagedBy(userId);
                    issues = _issueRepository.GetAllIssues()
                        .Where(issue => managerProjects.Contains(issue.ProjectId)).ToList();
                }
                else if (userRole == Role.Worker)
                {
                    // Worker sees issues in projects they are part of
                    var workerProjects = _rbacService.GetProjectsForUserOrManagedBy(currentUser, _projectRepository);
                    issues = _issueRepository.GetAllIssues()
                        .Where(issue => workerProjects.Contains(issue.ProjectId)).ToList();
                }
                else
                {
                    Console.WriteLine("Nie masz uprawnień do przeglądania zgłoszeń.");
                    return;
                }

                if (!issues.Any())
                {
                    Console.WriteLine("Brak zgłoszeń do wyświetlenia.");
                    return;
                }

                Console.WriteLine("--- Lista zgłoszeń ---");
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
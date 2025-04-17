using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Enums;
using System.Net.Security;
using ConstructionManagementApp.App.Services;
using ConstructionManagementApp.Events;

namespace ConstructionManagementApp.App.Controllers
{
    internal class IssueController
    {
        private readonly IssueRepository _issueRepository;
        private readonly AuthenticationService _authenticationService;
        private readonly ProjectRepository _projectRepository;
        public event LogEventHandler IssueAdded;
        public event LogEventHandler IssueDeleted;
        public event LogEventHandler IssueUpdated;

        public IssueController(IssueRepository issueRepository, AuthenticationService authenticationService, ProjectRepository projectRepository)
        {
            _issueRepository = issueRepository;
            _authenticationService = authenticationService;
            _projectRepository = projectRepository;
        }

        public void AddIssue(string title, string content, string projectName, TaskPriority priority)
        {
            try
            {
                var project = _projectRepository.GetProjectByName(projectName);
                if (project == null)
                    throw new KeyNotFoundException($"Nie udało się znaleźć projektu o nazwie {projectName}");
                int projectId = project.Id;
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

        public void UpdateIssue(int issueId, TaskPriority priority, IssueStatus status, DateTime? resolvedAt)
        {
            try
            {
                var issue = _issueRepository.GetIssueById(issueId);
                if (issue == null)
                    throw new KeyNotFoundException($"Nie znaleziono zgłoszenia o ID {issueId}.");

                issue.Priority = priority;
                issue.Status = status;
                issue.ResolvedAt = resolvedAt;

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

        public void DisplayAllIssues()
        {
            try
            {
                var issues = _issueRepository.GetAllIssues();
                if (issues.Count == 0)
                {
                    Console.WriteLine("Brak zgłoszeń w systemie.");
                    return;
                }

                Console.WriteLine("--- Lista zgłoszeń ---");
                foreach (var issue in issues)
                {
                    Console.WriteLine($"ID: {issue.Id}, Tytuł: {issue.Title}, Priorytet: {issue.Priority}, Status: {issue.Status}, Rozwiązano: {issue.ResolvedAt}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }
    }
}
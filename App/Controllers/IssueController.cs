using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Enums;

namespace ConstructionManagementApp.App.Controllers
{
    internal class IssueController
    {
        private readonly IssueRepository _issueRepository;

        public IssueController(IssueRepository issueRepository)
        {
            _issueRepository = issueRepository;
        }

        public void AddIssue(string title, string content, int userId, int projectId, TaskPriority priority)
        {
            try
            {
                var issue = new Issue(title, content, userId, projectId, priority);
                _issueRepository.AddIssue(issue);
                Console.WriteLine("Zgłoszenie zostało pomyślnie dodane.");
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
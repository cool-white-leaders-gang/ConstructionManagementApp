using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Controllers
{
    internal class IssueController
    {
        private readonly IssueRepository _issueRepository;

        public IssueController(IssueRepository issueRepository)
        {
            _issueRepository = issueRepository;
        }

        public void AddIssue(string title, string description, IssueStatus status)
        {
            try
            {
                var issue = new Issue(title, description, status);
                _issueRepository.CreateIssue(issue);
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

        public void UpdateIssue(int issueId, string title, string description, IssueStatus status)
        {
            try
            {
                var issue = _issueRepository.GetIssueById(issueId);
                if (issue == null)
                    throw new KeyNotFoundException($"Nie znaleziono zgłoszenia o ID {issueId}.");

                issue.Title = title;
                issue.Description = description;
                issue.Status = status;

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
                    Console.WriteLine($"ID: {issue.Id}, Tytuł: {issue.Title}, Status: {issue.Status}, Opis: {issue.Description}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Enums;

namespace ConstructionManagementApp.App.Controllers
{
    internal class IssueController
    {
        private readonly IssueRepository _issueRepository;

        // Konstruktor
        public IssueController(IssueRepository issueRepository)
        {
            _issueRepository = issueRepository;
        }

        // Utwórz nowy problem
        public void CreateIssue(string title, string content, int userId, int projectId, TaskPriority priority)
        {
            try
            {
                var issue = new Issue(title, content, userId, projectId, priority)
                {
                    Status = IssueStatus.Open // Domyślny status to "Open"
                };
                _issueRepository.CreateIssue(issue);
                Console.WriteLine("Problem został pomyślnie utworzony.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas tworzenia problemu: {ex.Message}");
            }
        }

        // Zaktualizuj istniejący problem
        public void UpdateIssue(int issueId, string title, string content, TaskPriority priority)
        {
            try
            {
                var issue = _issueRepository.GetIssueById(issueId);
                if (issue == null)
                {
                    Console.WriteLine($"Problem o Id {issueId} nie został znaleziony.");
                    return;
                }

                issue.Title = title;
                issue.Content = content;
                issue.Priority = priority;

                _issueRepository.UpdateIssue(issue);
                Console.WriteLine("Problem został pomyślnie zaktualizowany.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas aktualizacji problemu: {ex.Message}");
            }
        }

        // Zmień status problemu na "InProgress"
        public void StartProgress(int issueId)
        {
            try
            {
                var issue = _issueRepository.GetIssueById(issueId);
                if (issue == null)
                {
                    Console.WriteLine($"Problem o Id {issueId} nie został znaleziony.");
                    return;
                }

                if (issue.Status == IssueStatus.Resolved)
                {
                    Console.WriteLine("Nie można zmienić statusu na 'InProgress', ponieważ problem został już rozwiązany.");
                    return;
                }

                issue.Status = IssueStatus.InProgress;
                _issueRepository.UpdateIssue(issue);
                Console.WriteLine("Status problemu został zmieniony na 'InProgress'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas zmiany statusu problemu: {ex.Message}");
            }
        }

        // Rozwiąż problem
        public void ResolveIssue(int issueId)
        {
            try
            {
                var issue = _issueRepository.GetIssueById(issueId);
                if (issue == null)
                {
                    Console.WriteLine($"Problem o Id {issueId} nie został znaleziony.");
                    return;
                }

                if (issue.Status == IssueStatus.Resolved)
                {
                    Console.WriteLine("Problem został już rozwiązany.");
                    return;
                }

                issue.Status = IssueStatus.Resolved;
                issue.ResolvedAt = DateTime.Now;

                _issueRepository.UpdateIssue(issue);
                Console.WriteLine("Problem został pomyślnie rozwiązany.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas rozwiązywania problemu: {ex.Message}");
            }
        }

        // Usuń problem
        public void DeleteIssue(int issueId)
        {
            try
            {
                _issueRepository.DeleteIssue(issueId);
                Console.WriteLine("Problem został pomyślnie usunięty.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas usuwania problemu: {ex.Message}");
            }
        }

        // Wyświetl wszystkie problemy
        public void DisplayAllIssues()
        {
            try
            {
                var issues = _issueRepository.GetAllIssues();
                if (issues.Count == 0)
                {
                    Console.WriteLine("Brak problemów do wyświetlenia.");
                    return;
                }

                foreach (var issue in issues)
                {
                    Console.WriteLine(issue.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas pobierania problemów: {ex.Message}");
            }
        }

        // Wyświetl problem po Id
        public void DisplayIssueById(int issueId)
        {
            try
            {
                var issue = _issueRepository.GetIssueById(issueId);
                if (issue == null)
                {
                    Console.WriteLine($"Problem o Id {issueId} nie został znaleziony.");
                    return;
                }

                Console.WriteLine(issue.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas pobierania problemu: {ex.Message}");
            }
        }
    }
}
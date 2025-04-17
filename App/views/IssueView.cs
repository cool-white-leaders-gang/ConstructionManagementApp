using System;
using ConstructionManagementApp.App.Controllers;
using ConstructionManagementApp.App.Services;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Enums;

namespace ConstructionManagementApp.App.Views
{
    internal class IssueView
    {
        private readonly IssueController _issueController;
        private readonly RBACService _rbacService;
        private readonly User _currentUser;
        private readonly LogController _logController;

        public IssueView(IssueController issueController, RBACService rbacService, User currentUser, LogController logController)
        {
            _issueController = issueController;
            _rbacService = rbacService;
            _currentUser = currentUser;
            _logController = logController;
        }

        public void ShowView()
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("--- Zarządzanie zgłoszeniami ---");
                Console.WriteLine("1. Wyświetl wszystkie zgłoszenia");
                Console.WriteLine("2. Dodaj nowe zgłoszenie");
                Console.WriteLine("3. Zaktualizuj zgłoszenie");
                Console.WriteLine("4. Usuń zgłoszenie");
                Console.WriteLine("5. Powrót do menu głównego");

                Console.Write("\nTwój wybór: ");
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Niepoprawny wybór. Naciśnij dowolny klawisz, aby spróbować ponownie.");
                    Console.ReadKey();
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        if (HasPermission(Permission.ViewIssues)) DisplayAllIssues();
                        break;
                    case 2:
                        if (HasPermission(Permission.CreateIssue)) AddIssue();
                        break;
                    case 3:
                        if (HasPermission(Permission.UpdateIssue)) UpdateIssue();
                        break;
                    case 4:
                        if (HasPermission(Permission.DeleteIssue)) DeleteIssue();
                        break;
                    case 5:
                        isRunning = false; // Powrót do menu głównego
                        break;
                    default:
                        Console.WriteLine("Niepoprawny wybór. Naciśnij dowolny klawisz, aby spróbować ponownie.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private bool HasPermission(Permission permission)
        {
            if (!_rbacService.HasPermission(_currentUser, permission))
            {
                Console.WriteLine("Brak uprawnień do wykonania tej operacji.");
                Console.ReadKey();
                return false;
            }
            return true;
        }

        private void DisplayAllIssues()
        {
            Console.Clear();
            _issueController.DisplayAllIssues();
            ReturnToMenu();
        }

        private void AddIssue()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Dodaj nowe zgłoszenie ---");

                Console.Write("Podaj tytuł zgłoszenia: ");
                var title = Console.ReadLine();

                Console.Write("Podaj treść zgłoszenia: ");
                var content = Console.ReadLine();

                Console.Write("Podaj nazwę projektu: ");
                string projectName = Console.ReadLine();

                Console.WriteLine("Wybierz priorytet zgłoszenia: 1. Niski, 2. Średni, 3. Wysoki");
                var priorityChoice = Console.ReadLine();
                TaskPriority priority = priorityChoice switch
                {
                    "1" => TaskPriority.Low,
                    "2" => TaskPriority.Medium,
                    "3" => TaskPriority.High,
                    _ => throw new ArgumentException("Nieprawidłowy priorytet zgłoszenia.")
                };

                _issueController.AddIssue(title, content, projectName, priority);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            finally
            {
                ReturnToMenu();
            }
        }

        private void UpdateIssue()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Zaktualizuj zgłoszenie ---");

                Console.Write("Podaj ID zgłoszenia: ");
                if (!int.TryParse(Console.ReadLine(), out var issueId))
                {
                    Console.WriteLine("Niepoprawne ID.");
                    return;
                }

                Console.WriteLine("Wybierz nowy priorytet zgłoszenia: 1. Niski, 2. Średni, 3. Wysoki");
                var priorityChoice = Console.ReadLine();
                TaskPriority priority = priorityChoice switch
                {
                    "1" => TaskPriority.Low,
                    "2" => TaskPriority.Medium,
                    "3" => TaskPriority.High,
                    _ => throw new ArgumentException("Nieprawidłowy priorytet zgłoszenia.")
                };

                Console.WriteLine("Wybierz nowy status zgłoszenia: 1. Otwarte, 2. W trakcie, 3. Zamknięte");
                var statusChoice = Console.ReadLine();
                IssueStatus status = statusChoice switch
                {
                    "1" => IssueStatus.Open,
                    "2" => IssueStatus.InProgress,
                    "3" => IssueStatus.Resolved,
                    _ => throw new ArgumentException("Nieprawidłowy status zgłoszenia.")
                };

                Console.Write("Podaj datę rozwiązania zgłoszenia (yyyy-MM-dd) lub pozostaw puste: ");
                var resolvedAtInput = Console.ReadLine();
                DateTime? resolvedAt = string.IsNullOrWhiteSpace(resolvedAtInput)
                    ? null
                    : DateTime.Parse(resolvedAtInput);

                _issueController.UpdateIssue(issueId, priority, status, resolvedAt);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            finally
            {
                ReturnToMenu();
            }
        }

        private void DeleteIssue()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Usuń zgłoszenie ---");

                Console.Write("Podaj ID zgłoszenia: ");
                if (!int.TryParse(Console.ReadLine(), out var issueId))
                {
                    Console.WriteLine("Niepoprawne ID.");
                    return;
                }

                _issueController.DeleteIssue(issueId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            finally
            {
                ReturnToMenu();
            }
        }

        private void ReturnToMenu()
        {
            Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu zgłoszeń.");
            Console.ReadKey();
        }
    }
}
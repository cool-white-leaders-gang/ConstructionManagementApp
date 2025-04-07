using System;
using ConstructionManagementApp.App.Controllers;
using ConstructionManagementApp.App.Enums;

namespace ConstructionManagementApp.App.Views
{
    internal class IssueView
    {
        private readonly IssueController _issueController;

        public IssueView(IssueController issueController)
        {
            _issueController = issueController;
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
                        DisplayAllIssues();
                        break;
                    case 2:
                        AddIssue();
                        break;
                    case 3:
                        UpdateIssue();
                        break;
                    case 4:
                        DeleteIssue();
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

                Console.Write("Podaj opis zgłoszenia: ");
                var description = Console.ReadLine();

                Console.WriteLine("Wybierz status zgłoszenia: 1. Open, 2. In Progress, 3. Resolved");
                var statusChoice = Console.ReadLine();
                IssueStatus status = statusChoice switch
                {
                    "1" => IssueStatus.Open,
                    "2" => IssueStatus.InProgress,
                    "3" => IssueStatus.Resolved,
                    _ => throw new ArgumentException("Wybrano nieprawidłowy status.")
                };

                _issueController.AddIssue(title, description, status);
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

                Console.Write("Podaj nowy tytuł zgłoszenia: ");
                var title = Console.ReadLine();

                Console.Write("Podaj nowy opis zgłoszenia: ");
                var description = Console.ReadLine();

                Console.WriteLine("Wybierz nowy status zgłoszenia: 1. Open, 2. In Progress, 3. Resolved");
                var statusChoice = Console.ReadLine();
                IssueStatus status = statusChoice switch
                {
                    "1" => IssueStatus.Open,
                    "2" => IssueStatus.InProgress,
                    "3" => IssueStatus.Resolved,
                    _ => throw new ArgumentException("Wybrano nieprawidłowy status.")
                };

                _issueController.UpdateIssue(issueId, title, description, status);
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
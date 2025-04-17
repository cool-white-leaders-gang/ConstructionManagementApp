using System;
using ConstructionManagementApp.App.Controllers;
using ConstructionManagementApp.App.Enums;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Services;

namespace ConstructionManagementApp.App.Views
{
    internal class ProgressReportView
    {
        private readonly ProgressReportController _progressReportController;
        private readonly RBACService _rbacService;
        private readonly User _currentUser;

        public ProgressReportView(ProgressReportController progressReportController, RBACService rbacService, User currentUser)
        {
            _progressReportController = progressReportController;
            _rbacService = rbacService;
            _currentUser = currentUser;
        }

        public void ShowView()
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("--- Zarządzanie raportami postępu ---");
                Console.WriteLine("1. Wyświetl wszystkie raporty postępu");
                Console.WriteLine("2. Dodaj nowy raport postępu");
                Console.WriteLine("3. Zaktualizuj raport postępu");
                Console.WriteLine("4. Usuń raport postępu");
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
                        if (HasPermission(Permission.ViewProgressReports)) DisplayAllProgressReports();
                        break;
                    case 2:
                        if (HasPermission(Permission.CreateProgressReport)) AddProgressReport();
                        break;
                    case 3:
                        if (HasPermission(Permission.UpdateProgressReport)) UpdateProgressReport();
                        break;
                    case 4:
                        if (HasPermission(Permission.DeleteProgressReport)) DeleteProgressReport();
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

        private void DisplayAllProgressReports()
        {
            Console.Clear();
            _progressReportController.DisplayAllProgressReports();
            ReturnToMenu();
        }

        private void AddProgressReport()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Dodaj nowy raport postępu ---");

                Console.Write("Podaj tytuł raportu: ");
                var title = Console.ReadLine();

                Console.Write("Podaj opis raportu: ");
                var content = Console.ReadLine();

                Console.WriteLine("Podaj nazwę projektu: ");
                var projectName = Console.ReadLine();
                Console.WriteLine("Podaj procent ukończenia: ");
                if (!int.TryParse(Console.ReadLine(), out var completionPercentage))
                {
                    Console.WriteLine("Niepoprawny procent ukończenia.");
                    return;
                }
                if (completionPercentage < 0 || completionPercentage > 100)
                {
                    Console.WriteLine("Procent ukończenia musi być w zakresie 0-100.");
                    return;
                }

                _progressReportController.AddProgressReport(title, content, projectName, completionPercentage );
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

        private void UpdateProgressReport()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Zaktualizuj raport postępu ---");

                Console.Write("Podaj ID raportu: ");
                if (!int.TryParse(Console.ReadLine(), out var reportId))
                {
                    Console.WriteLine("Niepoprawne ID.");
                    return;
                }

                Console.Write("Podaj nowy tytuł raportu: ");
                var title = Console.ReadLine();

                Console.Write("Podaj nowy opis raportu: ");
                var content = Console.ReadLine();

                Console.WriteLine("Podaj procent ukończenia: ");
                if (!int.TryParse(Console.ReadLine(), out var completionPercentage))
                {
                    Console.WriteLine("Niepoprawny procent ukończenia.");
                    return;
                }

                _progressReportController.UpdateProgressReport(reportId, title, content, completionPercentage);
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

        private void DeleteProgressReport()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Usuń raport postępu ---");

                Console.Write("Podaj ID raportu: ");
                if (!int.TryParse(Console.ReadLine(), out var reportId))
                {
                    Console.WriteLine("Niepoprawne ID.");
                    return;
                }

                _progressReportController.DeleteProgressReport(reportId);
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
            Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu raportów postępu.");
            Console.ReadKey();
        }
    }
}
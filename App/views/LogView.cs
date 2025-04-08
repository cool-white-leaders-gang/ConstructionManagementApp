using System;
using ConstructionManagementApp.App.Controllers;
using ConstructionManagementApp.App.Enums;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Services;

namespace ConstructionManagementApp.App.Views
{
    internal class LogView
    {
        private readonly LogController _logController;
        private readonly RBACService _rbacService;
        private readonly User _currentUser;

        public LogView(LogController logController, RBACService rBACService, User currentUser)
        {
            _logController = logController;
            _rbacService = rBACService;
            _currentUser = currentUser;
        }

        public void ShowView()
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("--- Zarządzanie logami ---");
                Console.WriteLine("1. Wyświetl wszystkie logi");
                Console.WriteLine("2. Wyszukaj logi po wiadomości");
                Console.WriteLine("3. Wyszukaj logi po dacie");
                Console.WriteLine("4. Dodaj nowy log");
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
                        if (HasPermission(Permission.ViewLogs)) DisplayAllLogs();
                        break;
                    case 2:
                        if (HasPermission(Permission.ViewLogs)) SearchLogsByMessage();
                        break;
                    case 3:
                        if (HasPermission(Permission.ViewLogs)) SearchLogsByDate();
                        break;
                    case 4:
                        if (HasPermission(Permission.CreateLog)) AddLog();
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

        private void DisplayAllLogs()
        {
            Console.Clear();
            _logController.DisplayAllLogs();
            ReturnToMenu();
        }

        private void SearchLogsByMessage()
        {
            try
            {
                Console.Clear();
                Console.Write("Podaj wiadomość lub jej fragment: ");
                var message = Console.ReadLine();
                _logController.DisplayLogsByMessage(message);
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

        private void SearchLogsByDate()
        {
            try
            {
                Console.Clear();
                Console.Write("Podaj datę (yyyy-MM-dd): ");
                if (!DateTime.TryParse(Console.ReadLine(), out var date))
                {
                    Console.WriteLine("Niepoprawny format daty.");
                    return;
                }

                _logController.DisplayLogsByDate(date);
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

        private void AddLog()
        {
            try
            {
                Console.Clear();
                Console.Write("Podaj wiadomość logu: ");
                var message = Console.ReadLine();
                _logController.AddLog(message);
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
            Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu logów.");
            Console.ReadKey();
        }
    }
}
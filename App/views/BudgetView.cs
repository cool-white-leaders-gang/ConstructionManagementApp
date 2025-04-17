using System;
using ConstructionManagementApp.App.Controllers;
using ConstructionManagementApp.App.Services;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Enums;

namespace ConstructionManagementApp.App.Views
{
    internal class BudgetView
    {
        private readonly BudgetController _budgetController;
        private readonly RBACService _rbacService;
        private readonly User _currentUser;
        private readonly LogController _logController;

        public BudgetView(BudgetController budgetController, RBACService rbacService, User currentUser, LogController logController)
        {
            _budgetController = budgetController;
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
                Console.WriteLine("--- Zarządzanie budżetami ---");
                Console.WriteLine("1. Wyświetl wszystkie budżety");
                Console.WriteLine("2. Dodaj nowy budżet");
                Console.WriteLine("3. Zaktualizuj budżet");
                Console.WriteLine("4. Usuń budżet");
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
                        if (HasPermission(Permission.ViewBudget)) DisplayAllBudgets();
                        break;
                    case 2:
                        if (HasPermission(Permission.CreateBudget)) AddBudget();
                        break;
                    case 3:
                        if (HasPermission(Permission.UpdateBudget)) UpdateBudget();
                        break;
                    case 4:
                        if (HasPermission(Permission.DeleteBudget)) DeleteBudget();
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

        private void DisplayAllBudgets()
        {
            Console.Clear();
            _budgetController.DisplayAllBudgets();
            ReturnToMenu();
        }

        private void AddBudget()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Dodaj nowy budżet ---");

                Console.Write("Podaj całkowitą kwotę budżetu: ");
                if (!decimal.TryParse(Console.ReadLine(), out var totalAmount))
                {
                    Console.WriteLine("Niepoprawna kwota.");
                    return;
                }

                _budgetController.AddBudget(totalAmount);
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

        private void UpdateBudget()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Zaktualizuj budżet ---");

                Console.Write("Podaj ID budżetu: ");
                if (!int.TryParse(Console.ReadLine(), out var budgetId))
                {
                    Console.WriteLine("Niepoprawne ID.");
                    return;
                }

                Console.Write("Podaj nową całkowitą kwotę budżetu: ");
                if (!decimal.TryParse(Console.ReadLine(), out var totalAmount))
                {
                    Console.WriteLine("Niepoprawna kwota.");
                    return;
                }

                Console.Write("Podaj nową wydaną kwotę budżetu: ");
                if (!decimal.TryParse(Console.ReadLine(), out var spentAmount))
                {
                    Console.WriteLine("Niepoprawna kwota.");
                    return;
                }

                _budgetController.UpdateBudget(budgetId, totalAmount, spentAmount);
                _logController.AddLog($"Zaktualizowano budżet o Id: {budgetId}", _currentUser.Email);
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

        private void DeleteBudget()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Usuń budżet ---");

                Console.Write("Podaj ID budżetu: ");
                if (!int.TryParse(Console.ReadLine(), out var budgetId))
                {
                    Console.WriteLine("Niepoprawne ID.");
                    return;
                }

                _budgetController.DeleteBudget(budgetId);
                _logController.AddLog($"Usunięto budżet o Id: {budgetId}", _currentUser.Email);
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
            Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu budżetów.");
            Console.ReadKey();
        }
    }
}
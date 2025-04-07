using System;
using ConstructionManagementApp.App.Controllers;

namespace ConstructionManagementApp.App.Views
{
    internal class BudgetView
    {
        private readonly BudgetController _budgetController;

        public BudgetView(BudgetController budgetController)
        {
            _budgetController = budgetController;
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
                Console.WriteLine("5. Wydaj budżet");
                Console.WriteLine("6. Powrót do menu głównego");

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
                        DisplayAllBudgets();
                        break;
                    case 2:
                        AddBudget();
                        break;
                    case 3:
                        UpdateBudget();
                        break;
                    case 4:
                        DeleteBudget();
                        break;
                    case 5:
                        SpendBudget();
                        break;
                    case 6:
                        isRunning = false; // Powrót do menu głównego
                        break;
                    default:
                        Console.WriteLine("Niepoprawny wybór. Naciśnij dowolny klawisz, aby spróbować ponownie.");
                        Console.ReadKey();
                        break;
                }
            }
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

                Console.Write("Podaj nazwę budżetu: ");
                var name = Console.ReadLine();

                Console.Write("Podaj kwotę budżetu: ");
                if (!decimal.TryParse(Console.ReadLine(), out var amount))
                {
                    Console.WriteLine("Niepoprawna kwota.");
                    return;
                }

                _budgetController.AddBudget(name, amount);
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

                Console.Write("Podaj nową nazwę budżetu: ");
                var name = Console.ReadLine();

                Console.Write("Podaj nową kwotę budżetu: ");
                if (!decimal.TryParse(Console.ReadLine(), out var amount))
                {
                    Console.WriteLine("Niepoprawna kwota.");
                    return;
                }

                _budgetController.UpdateBudget(budgetId, name, amount);
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

        private void SpendBudget()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Wydaj budżet ---");

                Console.Write("Podaj ID budżetu: ");
                if (!int.TryParse(Console.ReadLine(), out var budgetId))
                {
                    Console.WriteLine("Niepoprawne ID.");
                    return;
                }

                Console.Write("Podaj kwotę do wydania: ");
                if (!decimal.TryParse(Console.ReadLine(), out var amount))
                {
                    Console.WriteLine("Niepoprawna kwota.");
                    return;
                }

                _budgetController.SpendBudget(budgetId, amount);
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
using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Services;
using ConstructionManagementApp.Events;

namespace ConstructionManagementApp.App.Controllers
{
    internal class BudgetController
    {
        private readonly BudgetRepository _budgetRepository;
        private readonly AuthenticationService _authenticationService;
        public event LogEventHandler BudgetAdded;
        public event LogEventHandler BudgetDeleted;
        public event LogEventHandler BudgetUpdated;

        public BudgetController(BudgetRepository budgetRepository, AuthenticationService authenticationService)
        {
            _budgetRepository = budgetRepository;
            _authenticationService = authenticationService;
        }

        public Budget GetBudgetById(int budgetId)
        {
            try
            {
                var budget = _budgetRepository.GetBudgetById(budgetId);
                if (budget == null)
                {
                    Console.WriteLine($"Budżet o Id {budgetId} nie został znaleziony.");
                    return null;
                }

                return budget;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
                return null;
            }
        }

        public void SpendBudget(int budgetId, decimal amount)
        {
            try
            {
                var budget = _budgetRepository.GetBudgetById(budgetId);
                if (budget == null)
                    throw new KeyNotFoundException($"Nie znaleziono budżetu o ID {budgetId}.");

                if (amount <= 0)
                    throw new ArgumentException("Kwota do wydania musi być większa od zera.");

                if (budget.TotalAmount - budget.SpentAmount < amount)
                    throw new InvalidOperationException("Nie można wydać więcej niż dostępna kwota w budżecie.");

                budget.SpentAmount += amount;
                _budgetRepository.UpdateBudget(budget);

                Console.WriteLine($"Kwota {amount:C} została wydana z budżetu. Pozostała kwota: {budget.TotalAmount - budget.SpentAmount:C}");
                BudgetUpdated?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Kwota {amount:C} została wydana z budżetu o id {budgetId}."));
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Błąd podczas wyszukiwanie budżetu: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Błąd podczas ustawiania kwoty: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Błąd podczs ustawiania wydatków: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nieoczekiwany błąd: {ex.Message}");
            }
        }

        public void AddBudget(decimal totalAmount)
        {
            try
            {
                var budget = new Budget(totalAmount);
                _budgetRepository.AddBudget(budget);
                Console.WriteLine("Budżet został pomyślnie dodany.");
                BudgetAdded?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Dodano nowy budżet"));
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

        public void UpdateBudget(int budgetId, decimal totalAmount, decimal spentAmount)
        {
            try
            {
                var budget = _budgetRepository.GetBudgetById(budgetId);
                if (budget == null)
                    throw new KeyNotFoundException($"Nie znaleziono budżetu o ID {budgetId}.");

                budget.TotalAmount = totalAmount;
                budget.SpentAmount = spentAmount;

                _budgetRepository.UpdateBudget(budget);
                Console.WriteLine("Budżet został pomyślnie zaktualizowany.");
                BudgetUpdated?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Budżet o ID {budgetId} został zaktualizowany."));
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Błąd podczas wyszukiwania: {ex.Message}");
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

        public void DeleteBudget(int budgetId)
        {
            try
            {
                _budgetRepository.DeleteBudgetById(budgetId);
                Console.WriteLine("Budżet został pomyślnie usunięty.");
                BudgetDeleted?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Budżet o ID {budgetId} został usunięty."));
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

        public void DisplayAllBudgets()
        {
            try
            {
                var budgets = _budgetRepository.GetAllBudgets();
                if (budgets.Count == 0)
                {
                    Console.WriteLine("Brak budżetów w systemie.");
                    return;
                }

                Console.WriteLine("--- Lista budżetów ---");
                foreach (var budget in budgets)
                {
                    Console.WriteLine($"ID: {budget.Id}, Całkowita kwota: {budget.TotalAmount:C}, Wydano: {budget.SpentAmount:C}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }
    }
}
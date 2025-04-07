using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Controllers
{
    internal class BudgetController
    {
        private readonly BudgetRepository _budgetRepository;

        public BudgetController(BudgetRepository budgetRepository)
        {
            _budgetRepository = budgetRepository;
        }

        public Budget GetBudgetById(int budgetId)
        {
            var budget = _budgetRepository.GetBudgetById(budgetId);
            if (budget == null)
            {
                Console.WriteLine($"Budżet o Id {budgetId} nie został znaleziony.");
                return null;
            }

            return budget;
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

                if (budget.Amount < amount)
                    throw new InvalidOperationException("Nie można wydać więcej niż dostępna kwota w budżecie.");

                budget.Amount -= amount;
                _budgetRepository.UpdateBudget(budget);

                Console.WriteLine($"Kwota {amount:C} została wydana z budżetu '{budget.Name}'. Pozostała kwota: {budget.Amount:C}");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nieoczekiwany błąd: {ex.Message}");
            }
        }

        public void AddBudget(string name, decimal amount)
        {
            try
            {
                var budget = new Budget(name, amount);
                _budgetRepository.CreateBudget(budget);
                Console.WriteLine("Budżet został pomyślnie dodany.");
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

        public void UpdateBudget(int budgetId, string name, decimal amount)
        {
            try
            {
                var budget = _budgetRepository.GetBudgetById(budgetId);
                if (budget == null)
                    throw new KeyNotFoundException($"Nie znaleziono budżetu o ID {budgetId}.");

                budget.Name = name;
                budget.Amount = amount;

                _budgetRepository.UpdateBudget(budget);
                Console.WriteLine("Budżet został pomyślnie zaktualizowany.");
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

        public void DeleteBudget(int budgetId)
        {
            try
            {
                _budgetRepository.DeleteBudgetById(budgetId);
                Console.WriteLine("Budżet został pomyślnie usunięty.");
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
                    Console.WriteLine($"ID: {budget.Id}, Nazwa: {budget.Name}, Kwota: {budget.Amount:C}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }
    }
}
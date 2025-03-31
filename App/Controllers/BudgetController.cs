using System;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Repositories;

namespace ConstructionManagementApp.App.Controllers
{
    internal class BudgetController
    {
        private readonly BudgetRepository _budgetRepository;

        // Konstruktor kontrolera
        public BudgetController(BudgetRepository budgetRepository)
        {
            _budgetRepository = budgetRepository;
        }

        // Pobierz szczegóły budżetu
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

        // Wydaj środki z budżetu
        public void SpendBudget(int budgetId, decimal amount)
        {
            var budget = _budgetRepository.GetBudgetById(budgetId);
            if (budget == null)
            {
                Console.WriteLine($"Budżet o Id {budgetId} nie został znaleziony.");
                return;
            }

            try
            {
                if (amount <= 0)
                    throw new ArgumentException("Kwota musi być większa od zera.");
                if (budget.SpentAmount + amount > budget.TotalAmount)
                    throw new InvalidOperationException("Nie można przekroczyć całkowitego budżetu.");

                budget.SpentAmount += amount;
                _budgetRepository.UpdateBudget(budget);
                Console.WriteLine($"Zaktualizowano budżet. Wydano: {amount}, Pozostało: {budget.TotalAmount - budget.SpentAmount}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }

        // Zaktualizuj budżet
        public void UpdateBudget(Budget updatedBudget)
        {
            var budget = _budgetRepository.GetBudgetById(updatedBudget.Id);
            if (budget == null)
            {
                Console.WriteLine($"Budżet o Id {updatedBudget.Id} nie został znaleziony.");
                return;
            }

            budget.TotalAmount = updatedBudget.TotalAmount;
            budget.SpentAmount = updatedBudget.SpentAmount;

            _budgetRepository.UpdateBudget(budget);
            Console.WriteLine($"Budżet o Id {budget.Id} został zaktualizowany.");
        }

        // Usuń budżet
        public void DeleteBudget(int budgetId)
        {
            var budget = _budgetRepository.GetBudgetById(budgetId);
            if (budget == null)
            {
                Console.WriteLine($"Budżet o Id {budgetId} nie został znaleziony.");
                return;
            }

            _budgetRepository.DeleteBudget(budgetId);
            Console.WriteLine($"Budżet o Id {budgetId} został usunięty.");
        }
    }
}
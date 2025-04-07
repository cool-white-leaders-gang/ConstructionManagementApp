using System;
using System.Collections.Generic;
using System.Linq;
using ConstructionManagementApp.App.Database;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Repositories
{
    internal class BudgetRepository
    {
        private readonly AppDbContext _context;

        public BudgetRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddBudget(Budget budget)
        {
            if (budget == null)
                throw new ArgumentNullException(nameof(budget), "Budżet nie może być null.");

            _context.Budgets.Add(budget);
            _context.SaveChanges();
        }

        public void UpdateBudget(Budget budget)
        {
            var existingBudget = GetBudgetById(budget.Id);
            if (existingBudget == null)
                throw new KeyNotFoundException("Nie znaleziono budżetu o podanym Id.");

            existingBudget.TotalAmount = budget.TotalAmount;
            existingBudget.SpentAmount = budget.SpentAmount;

            _context.Budgets.Update(existingBudget);
            _context.SaveChanges();
        }

        public void DeleteBudgetById(int budgetId)
        {
            var budget = GetBudgetById(budgetId);
            if (budget == null)
                throw new KeyNotFoundException("Nie znaleziono budżetu o podanym Id.");

            _context.Budgets.Remove(budget);
            _context.SaveChanges();
        }

        public Budget GetBudgetById(int id)
        {
            return _context.Budgets.FirstOrDefault(b => b.Id == id);
        }

        public List<Budget> GetAllBudgets()
        {
            return _context.Budgets.ToList();
        }
    }
}

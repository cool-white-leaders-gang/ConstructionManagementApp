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

        public void CreateBudget(Budget budget)
        {
            _context.Budgets.Add(budget);
            _context.SaveChanges();
        }

        public void UpdateBudget(Budget budget)
        {
            var existingBudget = GetBudgetById(budget.Id);
            if (existingBudget == null)
                throw new KeyNotFoundException("Nie znaleziono budżetu o podanym Id.");

            _context.Budgets.Update(budget);
            _context.SaveChanges();
        }

        public void DeleteBudget(int budgetId)
        {
            var budget = GetBudgetById(budgetId);
            if (budget == null)
                throw new KeyNotFoundException("Nie znaleziono budżetu o podanym Id.");

            _context.Budgets.Remove(budget);
            _context.SaveChanges();
        }

        public Budget GetBudgetById(int budgetId)
        {
            return _context.Budgets.FirstOrDefault(b => b.Id == budgetId);
        }

        public List<Budget> GetAllBudgets()
        {
            return _context.Budgets.ToList();
        }
    }
}

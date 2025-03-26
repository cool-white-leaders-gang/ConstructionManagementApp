using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConstructionManagementApp.App.Database;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Repositories
{
    internal class BudgetReposiotry
    {
        private readonly AppDbContext _context;
        public BudgetReposiotry(AppDbContext context)
        {
            _context = context;
        }

        public void CreateBudget(Budget budget)
        {
            _context.Budgets.Add(budget);
            _context.SaveChanges();
            Console.WriteLine("Dodano nowy budżet");
        }

        public void UpdateBudget(Budget budget)
        {
            try
            {
                _context.Budgets.Update(budget);
                _context.SaveChanges();
                Console.WriteLine("Budżet zaktualizowany");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Próba aktualizacji nieistniejącego budżetu: " + ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void DeleteBudget(int budgetId)
        {
            var budgetToDelete = _context.Budgets.FirstOrDefault(e => budgetId == e.Id);
            if (budgetToDelete == null)
            {
                Console.WriteLine("Nie ma takiego budżetu");
                return;
            }
            _context.Budgets.Remove(budgetToDelete);
            budgetToDelete = null;
            GC.Collect();
            _context.SaveChanges();
            Console.WriteLine("Budżet usunięty");
        }

        public List<Budget> GetAllBudgets()
        {
            return _context.Budgets.ToList();
        }

        public Budget GetBudgetById(int budgetId)
        {
            return _context.Budgets.FirstOrDefault(e => budgetId == e.Id);
        }
    }
}

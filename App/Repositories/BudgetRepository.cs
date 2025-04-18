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

        // Konstruktor repozytorium, inicjalizuje kontekst bazy danych
        public BudgetRepository(AppDbContext context)
        {
            _context = context;
        }

        // Dodanie nowego budżetu do bazy danych
        public void AddBudget(Budget budget)
        {
            if (budget == null)
                throw new ArgumentNullException(nameof(budget), "Budżet nie może być null.");

            // Dodanie budżetu i zapisanie zmian w bazie
            _context.Budgets.Add(budget);
            _context.SaveChanges();
        }

        // Aktualizacja istniejącego budżetu
        public void UpdateBudget(Budget budget)
        {
            // Sprawdzenie, czy budżet o podanym Id istnieje
            var existingBudget = GetBudgetById(budget.Id);
            if (existingBudget == null)
                throw new KeyNotFoundException("Nie znaleziono budżetu o podanym Id.");

            // Aktualizacja wartości budżetu
            existingBudget.TotalAmount = budget.TotalAmount;
            existingBudget.SpentAmount = budget.SpentAmount;

            // Zaktualizowanie budżetu w bazie danych
            _context.Budgets.Update(existingBudget);
            _context.SaveChanges();
        }

        // Usunięcie budżetu na podstawie jego ID
        public void DeleteBudgetById(int budgetId)
        {
            // Pobranie budżetu na podstawie ID
            var budget = GetBudgetById(budgetId);
            if (budget == null)
                throw new KeyNotFoundException("Nie znaleziono budżetu o podanym Id.");

            // Usunięcie budżetu z bazy
            _context.Budgets.Remove(budget);
            _context.SaveChanges();
        }

        // Pobranie budżetu na podstawie ID
        public Budget GetBudgetById(int id)
        {
            // Zwrócenie budżetu lub null, jeśli nie znaleziono
            return _context.Budgets.FirstOrDefault(b => b.Id == id);
        }

        // Pobranie wszystkich budżetów z bazy danych
        public List<Budget> GetAllBudgets()
        {
            // Zwrócenie listy wszystkich budżetów
            return _context.Budgets.ToList();
        }
    }
}

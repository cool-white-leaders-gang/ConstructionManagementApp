using System;
using System.Collections.Generic;
using System.Linq;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Database;
using ConstructionManagementApp.App.Enums;

namespace ConstructionManagementApp.App.Repositories
{
    internal class TaskAssignmentRepository
    {
        private readonly AppDbContext _context;

        public TaskAssignmentRepository(AppDbContext context)
        {
            _context = context;
        }

        // przypisanie pracownika do zadania
        public void AssignWorkerToTask(int taskId, int userId)
        {
            try
            {
                // sprawdzam czy istnieje task o danym id
                var task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
                if (task == null)
                    throw new ArgumentException($"Zadanie o Id {taskId} nie istnieje.");

                // tu sprawdza czy istnieje user o podanym id
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                    throw new ArgumentException($"Użytkownik o Id {userId} nie istnieje.");

                // sprawdza czy użytkownik ma role Worker
                if (user.Role != Role.Worker)
                    throw new InvalidOperationException($"Użytkownik o Id {userId} nie ma roli Worker i nie może być przypisany do zadania.");

                // sprawdza czy nie ma juz powiązania usera z taskiem
                var existingAssignment = _context.TaskAssignments
                    .FirstOrDefault(ta => ta.TaskId == taskId && ta.UserId == userId);
                if (existingAssignment != null)
                    throw new InvalidOperationException($"Użytkownik o Id {userId} jest już przypisany do zadania o Id {taskId}.");

                // uworzenie nowego przypisania
                var taskAssignment = new TaskAssignment(taskId, userId);
                _context.TaskAssignments.Add(taskAssignment); // dodanie przypisania
                _context.SaveChanges(); // zapisanie zmian
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas przypisywania użytkownika do zadania: {ex.Message}");
                throw;
            }
        }

        // lista pracownikow z danego zadania
        public List<User> GetWorkersAssignedToTask(int taskId)
        {
            try
            {
                // sprawdza czy istnieje w ogole takie zadanie
                var task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
                if (task == null)
                    throw new ArgumentException($"Zadanie o Id {taskId} nie istnieje.");

                // zwraca użytkownikow przypisanych do zadania o danym id
                return _context.TaskAssignments
                    .Where(ta => ta.TaskId == taskId) // filtruje po taskid
                    .Select(ta => ta.user) // pobiera przypisanych uzytkownikow
                    .ToList(); // zamienia na liste
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas pobierania pracowników przypisanych do zadania: {ex.Message}");
                throw;
            }
        }

        // usunięcie przypisania
        public void RemoveWorkerFromTask(int taskId, int userId)
        {
            try
            {
                // tu znajduje przypisanie
                var taskAssignment = _context.TaskAssignments
                    .FirstOrDefault(ta => ta.TaskId == taskId && ta.UserId == userId);

                if (taskAssignment == null)
                    throw new ArgumentException($"Przypisanie użytkownika o Id {userId} do zadania o Id {taskId} nie istnieje.");

                _context.TaskAssignments.Remove(taskAssignment); // usunięcie przypisania
                _context.SaveChanges(); // zapisanie zmian
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas usuwania przypisania użytkownika do zadania: {ex.Message}");
                throw;
            }
        }
    }
}
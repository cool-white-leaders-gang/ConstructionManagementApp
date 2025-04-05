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

        public void AssignWorkerToTask(int taskId, int userId)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new KeyNotFoundException($"Zadanie o Id {taskId} nie istnieje.");

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                throw new KeyNotFoundException($"Użytkownik o Id {userId} nie istnieje.");

            // Sprawdzenie, czy użytkownik ma odpowiednią rolę
            if (user.Role != Role.Worker)
                throw new InvalidOperationException($"Użytkownik o Id {userId} nie ma roli Worker i nie może być przypisany do zadania.");

            // Sprawdzenie, czy użytkownik już jest przypisany do zadania
            if (_context.TaskAssignments.Any(ta => ta.TaskId == taskId && ta.UserId == userId))
                throw new InvalidOperationException($"Użytkownik o Id {userId} jest już przypisany do zadania o Id {taskId}.");

            var assignment = new TaskAssignment(taskId, userId);
            _context.TaskAssignments.Add(assignment);
            _context.SaveChanges();
        }

        public void RemoveWorkerFromTask(int taskId, int userId)
        {
            var assignment = _context.TaskAssignments.FirstOrDefault(ta => ta.TaskId == taskId && ta.UserId == userId);
            if (assignment == null)
                throw new KeyNotFoundException($"Nie znaleziono przypisania użytkownika o Id {userId} do zadania o Id {taskId}.");

            _context.TaskAssignments.Remove(assignment);
            _context.SaveChanges();
        }

        public List<User> GetWorkersAssignedToTask(int taskId)
        {
            return _context.TaskAssignments
                .Where(ta => ta.TaskId == taskId)
                .Select(ta => ta.User)
                .ToList();
        }
    }
}
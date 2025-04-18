using System;
using System.Collections.Generic;
using System.Linq;
using ConstructionManagementApp.App.Database;
using Task = ConstructionManagementApp.App.Models.Task;

namespace ConstructionManagementApp.App.Repositories
{
    internal class TaskRepository
    {
        private readonly AppDbContext _context;

        // Konstruktor repozytorium, inicjalizuje kontekst bazy danych
        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        // Dodanie nowego zadania do bazy danych
        public void AddTask(Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task), "Zadanie nie może być null.");

            _context.Tasks.Add(task);
            _context.SaveChanges();
        }

        // Aktualizacja istniejącego zadania
        public void UpdateTask(Task task)
        {
            var existingTask = GetTaskById(task.Id);

            // Aktualizowanie właściwości zadania
            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.Priority = task.Priority;
            existingTask.Progress = task.Progress;

            _context.Tasks.Update(existingTask);
            _context.SaveChanges();
        }

        // Usuwanie zadania po Id
        public void DeleteTaskById(int id)
        {
            var task = GetTaskById(id);
            if (task == null)
                throw new KeyNotFoundException("Nie znaleziono zadania o podanym Id.");

            _context.Tasks.Remove(task);
            _context.SaveChanges();
        }

        // Pobranie zadania po Id
        public Task GetTaskById(int id)
        {
            return _context.Tasks.FirstOrDefault(t => t.Id == id);
        }

        // Pobranie zadania po tytule
        public Task GetTaskByTitle(string title)
        {
            return _context.Tasks.FirstOrDefault(t => t.Title == title);
        }

        // Pobranie zadań po Id projektów
        public List<Task> GetTasksByProjectIds(List<int> projectIds)
        {
            if (projectIds == null || !projectIds.Any())
                return new List<Task>();

            return _context.Tasks
                .Where(task => projectIds.Contains(task.ProjectId))
                .ToList();
        }
    }
}

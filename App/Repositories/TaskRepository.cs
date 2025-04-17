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

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddTask(Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task), "Zadanie nie może być null.");

            _context.Tasks.Add(task);
            _context.SaveChanges();
        }

        public void UpdateTask(Task task)
        {
            var existingTask = GetTaskById(task.Id);

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.Priority = task.Priority;
            existingTask.Progress = task.Progress;

            _context.Tasks.Update(existingTask);
            _context.SaveChanges();
        }

        public void DeleteTaskById(int id)
        {
            var task = GetTaskById(id);
            if (task == null)
                throw new KeyNotFoundException("Nie znaleziono zadania o podanym Id.");

            _context.Tasks.Remove(task);
            _context.SaveChanges();
        }

        public Task GetTaskById(int id)
        {
            return _context.Tasks.FirstOrDefault(t => t.Id == id);
        }

        public Task GetTaskByTitle(string title)
        {
            return _context.Tasks.FirstOrDefault(t => t.Title == title);
        }

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
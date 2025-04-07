using System;
using System.Collections.Generic;
using System.Linq;
using ConstructionManagementApp.App.Database;
using ConstructionManagementApp.App.Models;

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
            if (existingTask == null)
                throw new KeyNotFoundException("Nie znaleziono zadania o podanym Id.");

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.Priority = task.Priority;
            existingTask.Progress = task.Progress;

            _context.Tasks.Update(existingTask);
            _context.SaveChanges();
        }

        public void DeleteTaskById(int taskId)
        {
            var task = GetTaskById(taskId);
            if (task == null)
                throw new KeyNotFoundException("Nie znaleziono zadania o podanym Id.");

            _context.Tasks.Remove(task);
            _context.SaveChanges();
        }

        public Task GetTaskById(int id)
        {
            return _context.Tasks.FirstOrDefault(t => t.Id == id);
        }

        public List<Task> GetAllTasks()
        {
            return _context.Tasks.ToList();
        }
    }
}
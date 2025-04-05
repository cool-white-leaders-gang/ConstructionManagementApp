using System.Collections.Generic;
using System.Linq;
using ConstructionManagementApp.App.Database;
using ConstructionManagementApp.App.Models;
using Microsoft.EntityFrameworkCore;
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

        public void CreateTask(Task task)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
        }

        public void UpdateTask(Task task)
        {
            var existingTask = GetTaskById(task.Id);
            if (existingTask == null)
                throw new KeyNotFoundException("Nie znaleziono zadania o podanym Id.");

            _context.Tasks.Update(task);
            _context.SaveChanges();
        }

        public void DeleteTask(int taskId)
        {
            var task = GetTaskById(taskId);
            if (task == null)
                throw new KeyNotFoundException("Nie znaleziono zadania o podanym Id.");

            _context.Tasks.Remove(task);
            _context.SaveChanges();
        }

        public Task GetTaskById(int id)
        {
            var task =  _context.Tasks
                .Include(t => t.TaskAssignments)
                .ThenInclude(ta => ta.User)
                .FirstOrDefault(t => t.Id == id);
            if (task == null)
                throw new KeyNotFoundException("Nie znaleziono zadania o podanym Id.");
            return task;
        }

        public List<Task> GetAllTasks()
        {
            return _context.Tasks
                .Include(t => t.TaskAssignments)
                .ThenInclude(ta => ta.User)
                .ToList();
        }
    }
}
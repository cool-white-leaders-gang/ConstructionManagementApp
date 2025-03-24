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
            Console.WriteLine("Zadanie dodane do bazy danych");
        }
        public void UpdateTask(Task task)
        {
            try
            {
                _context.Tasks.Update(task);
                _context.SaveChanges();
                Console.WriteLine("Zadanie zaktualizowane");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine("Pr�ba aktualizacji nieistniej�cego zadania " + ex.Message);
            }
        }

        public void DeleteTask(int taskId)
        {
            var task = _context.Tasks.FirstOrDefault(t => taskId == t.Id);
            if (task == null)
            {
                Console.WriteLine("Nie ma takiego zadania");
                return;
            }
            _context.Tasks.Remove(task);
            task = null;
            GC.Collect();
            _context.SaveChanges();
            Console.WriteLine("Zadanie usuni�te");
        }
    }
}
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
                Console.WriteLine("Próba aktualizacji nieistniejącego zadania " + ex.Message);
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
            Console.WriteLine("Zadanie usunięte");
        }

        // Zwraca wszystkie zadania wraz z przypisanymi użytkownikami
        public List<Task> GetAllTasks()
        {
            return _context.Tasks
                .Include(t => t.TaskAssignments)
                .ThenInclude(ta => ta.User) // Zawiera użytkowników w relacji
                .ToList();
        }

        // Zwraca zadanie po id wraz z przypisanymi użytkownikami
        public Task GetTaskById(int id)
        {
            return _context.Tasks
                .Include(t => t.TaskAssignments)
                .ThenInclude(ta => ta.User) // Zawiera użytkowników w relacji
                .FirstOrDefault(t => t.Id == id);
        }
    }
}
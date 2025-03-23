using ConstructionManagementApp.App.Database;
using ConstructionManagementApp.App.Models;
namespace ConstructionManagementApp.App.Repositories
{
    internal class TaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(){
            _context = new AppDbContext();
        }

        public void CreateTask(){
            
        }
    }
}
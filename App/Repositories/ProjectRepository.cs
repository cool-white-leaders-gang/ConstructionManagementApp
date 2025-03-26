using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConstructionManagementApp.App.Database;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Repositories
{
    internal class ProjectRepository
    {
        private readonly AppDbContext _context;
        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }
        public void CreateProject(Project project)
        {
            _context.Projects.Add(project);
            _context.SaveChanges();
            Console.WriteLine("Dodano nowy projekt");
        }
        public void UpdateProject(Project project)
        {
            try
            {
                _context.Projects.Update(project);
                _context.SaveChanges();
                Console.WriteLine("Projekt zaktualizowany");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Próba aktualizacji nieistniejącego projektu: " + ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        public void DeleteProject(int projectId)
        {
            var project = _context.Projects.FirstOrDefault(e => projectId == e.Id);
            if (project == null)
            {
                Console.WriteLine("Nie ma takiego projektu");
                return;
            }
            _context.Projects.Remove(project);
            project = null;
            GC.Collect();
            _context.SaveChanges();
            Console.WriteLine("Projekt usunięty");
        }
        public List<Project> GetAllProjects()
        {
            return _context.Projects.ToList();
        }
        public Project GetProjectById(int projectId)
        {
            return _context.Projects.FirstOrDefault(e => projectId == e.Id);
        }
    }
}

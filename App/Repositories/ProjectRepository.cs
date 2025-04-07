using System;
using System.Collections.Generic;
using System.Linq;
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
            if (project == null)
                throw new ArgumentNullException(nameof(project), "Projekt nie może być null.");

            _context.Projects.Add(project);
            _context.SaveChanges();
        }

        public void UpdateProject(Project project)
        {
            var existingProject = GetProjectById(project.Id);
            if (existingProject == null)
                throw new KeyNotFoundException("Nie znaleziono projektu o podanym Id.");

            existingProject.Name = project.Name;
            existingProject.Description = project.Description;
            existingProject.TeamId = project.TeamId;
            existingProject.BudgetId = project.BudgetId;
            existingProject.ClientId = project.ClientId;

            _context.Projects.Update(existingProject);
            _context.SaveChanges();
        }

        public void DeleteProjectById(int projectId)
        {
            var project = GetProjectById(projectId);
            if (project == null)
                throw new KeyNotFoundException("Nie znaleziono projektu o podanym Id.");

            _context.Projects.Remove(project);
            _context.SaveChanges();
        }

        public Project GetProjectById(int id)
        {
            return _context.Projects.FirstOrDefault(p => p.Id == id);
        }

        public List<Project> GetAllProjects()
        {
            return _context.Projects.ToList();
        }
    }
}

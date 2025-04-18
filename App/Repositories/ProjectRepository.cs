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

        // Konstruktor repozytorium, inicjalizuje kontekst bazy danych
        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        // Tworzenie nowego projektu
        public void CreateProject(Project project)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project), "Projekt nie może być null.");
            if (IsNameTaken(project.Name))
                throw new ArgumentException("Nazwa projektu jest już zajęta");

            // Dodanie projektu do bazy danych
            _context.Projects.Add(project);
            _context.SaveChanges();
        }

        // Aktualizacja istniejącego projektu
        public void UpdateProject(Project project)
        {
            // Wyszukiwanie istniejącego projektu na podstawie Id
            var existingProject = GetProjectById(project.Id);
            if (existingProject == null)
                throw new KeyNotFoundException("Nie znaleziono projektu o podanym Id.");

            // Aktualizacja danych projektu
            existingProject.Name = project.Name;
            existingProject.Description = project.Description;
            existingProject.TeamId = project.TeamId;
            existingProject.BudgetId = project.BudgetId;
            existingProject.ClientId = project.ClientId;

            // Zapisanie zaktualizowanego projektu w bazie danych
            _context.Projects.Update(existingProject);
            _context.SaveChanges();
        }

        // Usunięcie projektu na podstawie nazwy
        public void DeleteProjectByName(string name)
        {
            var project = GetProjectByName(name);
            if (project == null)
                throw new KeyNotFoundException("Nie znaleziono projektu o podanej nazwie.");

            // Usunięcie projektu z bazy danych
            _context.Projects.Remove(project);
            _context.SaveChanges();
        }

        // Pobranie projektu na podstawie Id
        public Project GetProjectById(int id)
        {
            return _context.Projects.FirstOrDefault(p => p.Id == id);
        }

        // Pobranie projektu na podstawie nazwy
        public Project GetProjectByName(string name)
        {
            return _context.Projects.FirstOrDefault(p => p.Name == name);
        }

        // Pobranie wszystkich projektów
        public List<Project> GetAllProjects()
        {
            return _context.Projects.ToList();
        }

        // Sprawdzenie, czy nazwa projektu jest już zajęta
        bool IsNameTaken(string name)
        {
            return _context.Projects.Any(p => p.Name == name);
        }
    }
}

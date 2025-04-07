using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Controllers
{
    internal class ProjectController
    {
        private readonly ProjectRepository _projectRepository;

        public ProjectController(ProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public void CreateProject(string name, string description, DateTime startDate, DateTime endDate)
        {
            try
            {
                if (endDate <= startDate)
                    throw new ArgumentException("Data zakończenia projektu musi być późniejsza niż data rozpoczęcia.");

                var project = new Project(name, description, startDate, endDate);
                _projectRepository.CreateProject(project);
                Console.WriteLine("Projekt został pomyślnie utworzony.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }

        public void UpdateProject(int projectId, string name, string description, DateTime startDate, DateTime endDate)
        {
            try
            {
                var project = _projectRepository.GetProjectById(projectId);
                if (project == null)
                    throw new KeyNotFoundException($"Nie znaleziono projektu o ID {projectId}.");

                project.Name = name;
                project.Description = description;
                project.StartDate = startDate;
                project.EndDate = endDate;

                _projectRepository.UpdateProject(project);
                Console.WriteLine("Projekt został pomyślnie zaktualizowany.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }

        public void DeleteProject(int projectId)
        {
            try
            {
                _projectRepository.DeleteProjectById(projectId);
                Console.WriteLine("Projekt został pomyślnie usunięty.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }

        public void DisplayAllProjects()
        {
            try
            {
                var projects = _projectRepository.GetAllProjects();
                if (projects.Count == 0)
                {
                    Console.WriteLine("Brak projektów w systemie.");
                    return;
                }

                Console.WriteLine("--- Lista projektów ---");
                foreach (var project in projects)
                {
                    Console.WriteLine($"ID: {project.Id}, Nazwa: {project.Name}, Opis: {project.Description}, Start: {project.StartDate}, Koniec: {project.EndDate}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }
    }
}
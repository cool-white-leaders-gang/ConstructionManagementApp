using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Repositories;

namespace ConstructionManagementApp.App.Controllers
{
    internal class ProjectController
    {
        private readonly ProjectRepository _projectRepository;

        // Konstruktor kontrolera
        public ProjectController(ProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        // Dodaj nowy projekt
        public void CreateProject(string name, string description, int teamId, int budgetId, int clientId)
        {
            try
            {
                var project = new Project(name, description, teamId, budgetId, clientId);
                _projectRepository.CreateProject(project);
                Console.WriteLine("Projekt został pomyślnie utworzony.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas tworzenia projektu: {ex.Message}");
            }
        }

        // Zaktualizuj projekt
        public void UpdateProject(int projectId, string name, string description, int teamId, int budgetId, int clientId)
        {
            try
            {
                var project = _projectRepository.GetProjectById(projectId);
                if (project == null)
                {
                    Console.WriteLine($"Projekt o Id {projectId} nie został znaleziony.");
                    return;
                }

                project.Name = name;
                project.Description = description;
                project.TeamId = teamId;
                project.BudgetId = budgetId;
                project.ClientId = clientId;

                _projectRepository.UpdateProject(project);
                Console.WriteLine("Projekt został pomyślnie zaktualizowany.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas aktualizacji projektu: {ex.Message}");
            }
        }

        // Usuń projekt
        public void DeleteProject(int projectId)
        {
            try
            {
                _projectRepository.DeleteProject(projectId);
                Console.WriteLine("Projekt został pomyślnie usunięty.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas usuwania projektu: {ex.Message}");
            }
        }

        // Wyświetl wszystkie projekty
        public void DisplayAllProjects()
        {
            try
            {
                var projects = _projectRepository.GetAllProjects();
                if (projects.Count == 0)
                {
                    Console.WriteLine("Brak projektów do wyświetlenia.");
                    return;
                }

                foreach (var project in projects)
                {
                    Console.WriteLine(project.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas pobierania projektów: {ex.Message}");
            }
        }

        // Wyświetl szczegóły projektu po Id
        public void DisplayProjectById(int projectId)
        {
            try
            {
                var project = _projectRepository.GetProjectById(projectId);
                if (project == null)
                {
                    Console.WriteLine($"Projekt o Id {projectId} nie został znaleziony.");
                    return;
                }

                Console.WriteLine(project.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas pobierania szczegółów projektu: {ex.Message}");
            }
        }
    }
}
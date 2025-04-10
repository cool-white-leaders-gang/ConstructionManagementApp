using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Enums;

namespace ConstructionManagementApp.App.Controllers
{
    internal class ProjectController
    {
        private readonly ProjectRepository _projectRepository;
        private readonly UserRepository _userRepository;
        private readonly BudgetRepository _budgetRepository;
        private readonly TeamRepository _teamRepository;

        public ProjectController(ProjectRepository projectRepository, UserRepository userRepository, BudgetRepository budgetRepository, TeamRepository teamRepository)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _budgetRepository = budgetRepository;
            _teamRepository = teamRepository;
        }

        public void CreateProject(string name, string description, string teamName, int budgetId, string clientUsername)
        {
            try
            {
                if (_userRepository.GetUserByUsername(clientUsername).Role != Role.Client || _userRepository.GetUserByUsername(clientUsername) == null)
                    throw new KeyNotFoundException($"Nie znaleziono użytkownika {clientUsername}, który jest klientem");
                if (_budgetRepository.GetBudgetById(budgetId) == null)
                    throw new KeyNotFoundException($"Nie znaleziono budżetu o ID: {budgetId}");
                if (_teamRepository.GetTeamByName(teamName).Name == null)
                    throw new KeyNotFoundException($"Nie znaleziono zespołu o nazwie: {teamName}");
                int clientId = _userRepository.GetUserByUsername(clientUsername).Id;
                int teamId = _teamRepository.GetTeamByName(teamName).Id;
                var project = new Project(name, description, teamId, budgetId, clientId);
                
                _projectRepository.CreateProject(project);
                Console.WriteLine("Projekt został pomyślnie utworzony.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }

        public void UpdateProject(string projectName, string name, string description, string teamName, int budgetId, string clientUsername)
        {
            try
            {
                var project = _projectRepository.GetProjectByName(projectName);
                if (project == null)
                    throw new KeyNotFoundException($"Nie znaleziono projektu o nazwie {projectName}.");
                int teamId = _teamRepository.GetTeamByName(teamName).Id;
                if (_teamRepository.GetTeamByName(teamName) == null)
                    throw new KeyNotFoundException($"Nie znaleziono zespołu o nazwie {teamName}");
                int clientId = _userRepository.GetUserByUsername(clientUsername).Id;
                if (_userRepository.GetUserByUsername(clientUsername).Role != Role.Client || _userRepository.GetUserByUsername(clientUsername) == null)
                    throw new KeyNotFoundException($"Nie znaleziono użytkownika {clientUsername}, który jest klientem");

                project.Name = name;
                project.Description = description;
                project.BudgetId = budgetId;
                project.TeamId = teamId;
                project.ClientId = clientId;

                _projectRepository.UpdateProject(project);
                Console.WriteLine("Projekt został pomyślnie zaktualizowany.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }

        public void DeleteProject(string projectName)
        {
            try
            {
                _projectRepository.DeleteProjectByName(projectName);
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
                    Console.WriteLine($"ID: {project.Id}, Nazwa: {project.Name}, Opis: {project.Description}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }
    }
}
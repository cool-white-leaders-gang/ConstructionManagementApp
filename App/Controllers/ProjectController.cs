using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Enums;
using ConstructionManagementApp.App.Services;
using ConstructionManagementApp.Events;
using ConstructionManagementApp.App.Delegates;

namespace ConstructionManagementApp.App.Controllers
{
    internal class ProjectController
    {
        private readonly ProjectRepository _projectRepository;
        private readonly UserRepository _userRepository;
        private readonly BudgetRepository _budgetRepository;
        private readonly TeamRepository _teamRepository;
        private readonly AuthenticationService _authenticationService;

        // Eventy do logowania działań na projektach
        public event LogEventHandler ProjectAdded;
        public event LogEventHandler ProjectDeleted;
        public event LogEventHandler ProjectUpdated;

        // Konstruktor
        public ProjectController(ProjectRepository projectRepository, UserRepository userRepository, BudgetRepository budgetRepository, TeamRepository teamRepository, AuthenticationService authenticationService)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _budgetRepository = budgetRepository;
            _teamRepository = teamRepository;
            _authenticationService = authenticationService;
        }

        // Tworzenie nowego projektu
        public void CreateProject(string name, string description, string teamName, int budgetId, string clientUsername)
        {
            try
            {
                // Walidacja — klient musi istnieć i mieć rolę Client
                if (_userRepository.GetUserByUsername(clientUsername) == null)
                    throw new KeyNotFoundException($"Nie znaleziono użytkownika {clientUsername}");
                else if (_userRepository.GetUserByUsername(clientUsername).Role != Role.Client)
                    throw new InvalidOperationException("Podany użytkownik nie jest klientem.");

                // Sprawdzenie budżetu
                if (_budgetRepository.GetBudgetById(budgetId) == null)
                    throw new KeyNotFoundException($"Nie znaleziono budżetu o ID: {budgetId}");

                // Sprawdzenie zespołu
                if (_teamRepository.GetTeamByName(teamName) == null)
                    throw new KeyNotFoundException($"Nie znaleziono zespołu o nazwie: {teamName}");

                // Pobranie ID klienta i zespołu
                int clientId = _userRepository.GetUserByUsername(clientUsername).Id;
                int teamId = _teamRepository.GetTeamByName(teamName).Id;

                // Utworzenie i zapisanie projektu
                var project = new Project(name, description, teamId, budgetId, clientId);
                _projectRepository.CreateProject(project);

                // Logowanie operacji
                ProjectAdded?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Dodano nowy projekt o nazwie {name}"));
                Console.WriteLine("Projekt został pomyślnie utworzony.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }

        // Aktualizacja istniejącego projektu
        public void UpdateProject(string projectName, string name, string description, string teamName, int budgetId, string clientUsername)
        {
            try
            {
                // Pobranie istniejącego projektu
                var project = _projectRepository.GetProjectByName(projectName);
                if (project == null)
                    throw new KeyNotFoundException($"Nie znaleziono projektu o nazwie {projectName}.");

                // Walidacja zespołu
                if (_teamRepository.GetTeamByName(teamName) == null)
                    throw new KeyNotFoundException($"Nie znaleziono zespołu o nazwie {teamName}");

                // Walidacja klienta
                if (_userRepository.GetUserByUsername(clientUsername) == null || _userRepository.GetUserByUsername(clientUsername).Role != Role.Client)
                    throw new KeyNotFoundException($"Nie znaleziono użytkownika {clientUsername}, który jest klientem");

                // Aktualizacja pól projektu
                project.Name = name;
                project.Description = description;
                project.BudgetId = budgetId;
                project.TeamId = _teamRepository.GetTeamByName(teamName).Id;
                project.ClientId = _userRepository.GetUserByUsername(clientUsername).Id;

                // Aktualizacja w repozytorium
                _projectRepository.UpdateProject(project);

                // Logowanie operacji
                ProjectUpdated?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Zaktualizowano projekt o nazwie {name}"));
                Console.WriteLine("Projekt został pomyślnie zaktualizowany.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }

        // Usuwanie projektu po nazwie
        public void DeleteProject(string projectName)
        {
            try
            {
                _projectRepository.DeleteProjectByName(projectName);
                Console.WriteLine("Projekt został pomyślnie usunięty.");

                // Logowanie usunięcia
                ProjectDeleted?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Usunięto projekt o nazwie {projectName}"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }

        // Wyświetlanie projektów dostępnych dla aktualnego użytkownika
        public void DisplayProjectsForCurrentUser()
        {
            try
            {
                var currentUser = _authenticationService.CurrentSession.User;
                var userRole = currentUser.Role;
                var userId = currentUser.Id;
                List<Project> projects;

                if (userRole == Role.Admin)
                {
                    // Admin widzi wszystkie projekty
                    projects = _projectRepository.GetAllProjects();
                }
                else if (userRole == Role.Manager)
                {
                    // Manager widzi tylko projekty zarządzane przez swój zespół
                    projects = _projectRepository.GetAllProjects()
                        .Where(project => project.TeamId > 0 && _teamRepository.GetTeamById(project.TeamId)?.ManagerId == userId)
                        .ToList();
                }
                else if (userRole == Role.Client)
                {
                    // Klient widzi wyłącznie swoje projekty
                    projects = _projectRepository.GetAllProjects()
                        .Where(project => project.ClientId == userId)
                        .ToList();
                }
                else
                {
                    Console.WriteLine("Nie masz uprawnień do przeglądania projektów.");
                    return;
                }

                if (!projects.Any())
                {
                    Console.WriteLine("Brak projektów do wyświetlenia.");
                    return;
                }

                foreach (var project in projects)
                {
                    var budget = _budgetRepository.GetBudgetById(project.BudgetId);
                    Console.WriteLine($"ID: {project.Id}, Nazwa: {project.Name}, Opis: {project.Description}, Budżet: {budget.TotalAmount}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }
    }
}

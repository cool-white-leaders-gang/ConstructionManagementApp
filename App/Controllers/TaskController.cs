using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Repositories;
using Task = ConstructionManagementApp.App.Models.Task;
using ConstructionManagementApp.App.Enums;
using ConstructionManagementApp.App.Services;
using ConstructionManagementApp.Events;
using ConstructionManagementApp.App.Delegates;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Controllers
{
    internal class TaskController
    {
        private readonly TaskRepository _taskRepository;  // Repozytorium zadań
        private readonly TaskAssignmentRepository _taskAssignmentRepository;  // Repozytorium przypisań zadań do użytkowników
        private readonly ProjectRepository _projectRepository;  // Repozytorium projektów
        private readonly AuthenticationService _authenticationService;  // Obsługa logowania
        private readonly UserRepository _userRepository;  // Repozytorium użytkowników
        private readonly RBACService _rbacService;  // Obsługa kontroli dostępu i uprawnień

        // Zdarzenia logujące różne akcje związane z zadaniami
        public event LogEventHandler TaskAdded;
        public event LogEventHandler TaskDeleted;
        public event LogEventHandler TaskUpdated;
        public event LogEventHandler TaskCompleted;
        public event LogEventHandler TaskStarted;
        public event LogEventHandler TaskAssigned;
        public event LogEventHandler TaskUnassigned;

        // Konstruktor inicjalizujący repozytoria i serwisy
        public TaskController(TaskRepository taskRepository, TaskAssignmentRepository taskAssignmentRepository, AuthenticationService authenticationService, ProjectRepository projectRepository, RBACService rbacService, UserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _taskAssignmentRepository = taskAssignmentRepository;
            _authenticationService = authenticationService;
            _projectRepository = projectRepository;
            _rbacService = rbacService;
            _userRepository = userRepository;
        }

        // Metoda do dodawania zadania
        public void AddTask(string title, string description, TaskPriority priority, TaskProgress progress, string projectName)
        {
            try
            {
                // Sprawdzenie, czy istnieje projekt o podanej nazwie
                var project = _projectRepository.GetProjectByName(projectName);
                if (project == null)
                    throw new KeyNotFoundException($"Nie znaleziono projektu o nazwie {projectName}");
                int projectId = project.Id;

                // Sprawdzanie uprawnień użytkownika (czy jest menedżerem projektu)
                if (!_rbacService.IsProjectManager(_authenticationService.CurrentSession.User, projectId, _projectRepository))
                {
                    throw new UnauthorizedAccessException("Brak uprawnień. Tylko menedżerowie tego projektu lub administratorzy mogą dodawać zadania.");
                }

                // Tworzenie nowego zadania
                var task = new Task(title, description, priority, progress, projectId);
                _taskRepository.AddTask(task);
                Console.WriteLine("Zadanie zostało pomyślnie dodane.");

                // Wywołanie eventu logującego dodanie zadania
                TaskAdded?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Dodano nowe zadanie o tytule {title}"));
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nieoczekiwany błąd: {ex.Message}");
            }
        }

        // Metoda do aktualizacji zadania
        public void UpdateTask(int id, string title, string description, TaskPriority priority, TaskProgress progress)
        {
            try
            {
                // Pobranie zadania po ID
                var task = _taskRepository.GetTaskById(id);
                if (task == null)
                    throw new KeyNotFoundException($"Nie znaleziono zadania o ID {id}.");

                // Pobranie projektu, do którego należy zadanie
                var project = _projectRepository.GetProjectById(task.ProjectId);
                if (project == null)
                    throw new KeyNotFoundException($"Nie znaleziono projektu o ID {id}");
                int projectId = project.Id;

                // Sprawdzanie uprawnień (czy użytkownik jest menedżerem projektu)
                if (!_rbacService.IsProjectManager(_authenticationService.CurrentSession.User, projectId, _projectRepository))
                {
                    throw new UnauthorizedAccessException("Brak uprawnień. Tylko menedżerowie tego projektu lub administratorzy mogą edytować zadania.");
                }

                // Aktualizacja zadania
                task.Title = title;
                task.Description = description;
                task.Priority = priority;
                task.Progress = progress;
                _taskRepository.UpdateTask(task);
                Console.WriteLine("Zadanie zostało pomyślnie zaktualizowane.");

                // Wywołanie eventu logującego aktualizację zadania
                TaskUpdated?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Zaktualizowano zadanie o ID {id} i nazwie {title}"));
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nieoczekiwany błąd: {ex.Message}");
            }
        }

        // Metoda do oznaczenia zadania jako ukończone
        public void CompleteTask(int id)
        {
            try
            {
                // Pobranie zadania po ID
                var task = _taskRepository.GetTaskById(id);
                if (task == null)
                    throw new KeyNotFoundException($"Nie znaleziono zadania o ID {id}.");

                // Sprawdzenie, czy użytkownik jest przypisany do zadania
                var assignedUsers = _taskAssignmentRepository.GetWorkersAssignedToTask(id);
                if (!assignedUsers.Any(worker => worker.Id == _authenticationService.CurrentSession.User.Id))
                    throw new UnauthorizedAccessException("Brak uprawnień. Tylko przypisani użytkownicy mogą zakończyć zadanie.");

                // Zaktualizowanie statusu zadania na "ukończone"
                task.Progress = TaskProgress.Completed;
                _taskRepository.UpdateTask(task);
                Console.WriteLine("Zadanie zostało pomyślnie ukończone");

                // Wywołanie eventu logującego ukończenie zadania
                TaskCompleted?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Zadanie o ID {id} zostało ukończone"));
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nieoczekiwany błąd: {ex.Message}");
            }
        }

        // Metoda do rozpoczęcia zadania
        public void StartTask(int id)
        {
            try
            {
                // Pobranie zadania po ID
                var task = _taskRepository.GetTaskById(id);
                if (task == null)
                    throw new KeyNotFoundException($"Nie znaleziono zadania o ID {id}");

                // Sprawdzenie, czy użytkownik jest przypisany do zadania
                var assignedUsers = _taskAssignmentRepository.GetWorkersAssignedToTask(id);
                if (!assignedUsers.Any(worker => worker.Id == _authenticationService.CurrentSession.User.Id))
                    throw new UnauthorizedAccessException("Brak uprawnień. Tylko przypisani użytkownicy mogą rozpocząć zadanie.");

                // Zaktualizowanie statusu zadania na "w trakcie"
                task.Progress = TaskProgress.InProgress;
                _taskRepository.UpdateTask(task);
                Console.WriteLine("Zadanie zostało rozpoczęte");

                // Wywołanie eventu logującego rozpoczęcie zadania
                TaskStarted?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Zadanie o ID {id} zostało rozpoczęte"));
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nieoczekiwany błąd: {ex.Message}");
            }
        }

        // Metoda do usunięcia zadania
        public void DeleteTask(int id)
        {
            try
            {
                // Pobranie zadania po ID
                var task = _taskRepository.GetTaskById(id);
                if (task == null)
                    throw new KeyNotFoundException($"Nie znaleziono zadania o ID {id}.");

                // Pobranie projektu, do którego należy zadanie
                var project = _projectRepository.GetProjectById(task.ProjectId);
                if (project == null)
                    throw new KeyNotFoundException($"Nie znaleziono projektu o ID {id}");
                int projectId = project.Id;

                // Sprawdzenie uprawnień (czy użytkownik jest menedżerem projektu)
                if (!_rbacService.IsProjectManager(_authenticationService.CurrentSession.User, projectId, _projectRepository))
                {
                    throw new UnauthorizedAccessException("Brak uprawnień. Tylko menedżerowie tego projektu lub administratorzy mogą usuwać zadania.");
                }

                // Usunięcie zadania
                _taskRepository.DeleteTaskById(id);
                Console.WriteLine("Zadanie zostało pomyślnie usunięte.");

                // Wywołanie eventu logującego usunięcie zadania
                TaskDeleted?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Usunięto zadanie o ID {id}"));
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nieoczekiwany błąd: {ex.Message}");
            }
        }

        // Metoda do wyświetlania wszystkich zadań przypisanych do użytkownika
        public void DisplayAllTasks()
        {
            try
            {
                // Pobranie projektów, do których przypisany jest użytkownik
                var userProjectIds = _rbacService.GetProjectsForUserOrManagedBy(_authenticationService.CurrentSession.User, _projectRepository);
                if (!userProjectIds.Any())
                    throw new InvalidOperationException("Nie jesteś przypisany do żadnych projektów.");

                // Pobranie zadań przypisanych do projektów
                var tasks = _taskRepository.GetTasksByProjectIds(userProjectIds);

                if (!tasks.Any())
                {
                    Console.WriteLine("Brak zadań");
                    return;
                }

                // Wyświetlenie szczegółów zadań
                Console.WriteLine("--- Lista zadań ---");
                foreach (var task in tasks)
                {
                    Console.WriteLine($"ID: {task.Id}, Tytuł: {task.Title}, Priorytet: {task.Priority}, Status: {task.Progress}, Opis: {task.Description}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }

        // Metoda do przypisania użytkownika do zadania
        public void AssignTask(int taskId, string username)
        {
            try
            {
                // Pobranie zadania po ID
                var task = _taskRepository.GetTaskById(taskId);
                if (task == null)
                    throw new KeyNotFoundException($"Nie znaleziono zadania o ID {taskId}.");

                // Sprawdzenie, czy użytkownik ma uprawnienia do przypisania pracownika do zadania
                if (!_rbacService.IsProjectManager(_authenticationService.CurrentSession.User, task.ProjectId, _projectRepository))
                    throw new UnauthorizedAccessException("Brak uprawnień. Tylko menedżer projektu może przypisywać pracowników do zadania.");

                // Pobranie użytkownika po nazwie
                var worker = _userRepository.GetUserByUsername(username);
                if (worker == null)
                    throw new KeyNotFoundException($"Nie znaleziono użytkownika o nazwie {username}.");

                // Sprawdzenie, czy użytkownik jest członkiem zespołu projektu
                if (!_rbacService.IsWorkerInProjectTeam(worker, task.ProjectId, _projectRepository))
                    throw new UnauthorizedAccessException("Brak uprawnień. Pracownik musi być członkiem zespołu przypisanego do projektu.");

                // Przypisanie użytkownika do zadania
                _taskAssignmentRepository.AssignWorkerToTask(taskId, username);
                Console.WriteLine($"Użytkownik o nazwie {username} został przypisany do zadania o ID {taskId}.");

                // Wywołanie eventu logującego przypisanie użytkownika do zadania
                TaskAssigned?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Przypisano użytkownika {username} do zadania o ID {taskId}"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas przypisywania użytkownika do zadania: {ex.Message}");
            }
        }

        // Metoda do usunięcia użytkownika z zadania
        public void RemoveWorkerFromTask(int taskId, string username)
        {
            try
            {
                // Pobranie zadania po ID
                var task = _taskRepository.GetTaskById(taskId);
                if (task == null)
                    throw new KeyNotFoundException($"Nie znaleziono zadania o ID {taskId}.");

                // Sprawdzenie uprawnień (czy użytkownik jest menedżerem projektu)
                if (!_rbacService.IsProjectManager(_authenticationService.CurrentSession.User, task.ProjectId, _projectRepository))
                    throw new UnauthorizedAccessException("Brak uprawnień. Tylko menedżer projektu może usuwać pracowników z zadania.");

                // Pobranie użytkownika po nazwie
                var worker = _userRepository.GetUserByUsername(username);
                if (worker == null)
                    throw new KeyNotFoundException($"Nie znaleziono użytkownika o nazwie {username}.");

                // Usunięcie użytkownika z zadania
                _taskAssignmentRepository.RemoveWorkerFromTask(taskId, username);
                Console.WriteLine($"Użytkownik o nazwie {username} został usunięty z zadania o Id {taskId}.");

                // Wywołanie eventu logującego usunięcie użytkownika z zadania
                TaskUnassigned?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Usunięto użytkownika {username} z zadania o ID {taskId}"));
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas usuwania użytkownika z zadania: {ex.Message}");
            }
        }

        // Metoda do wyświetlania użytkowników przypisanych do zadania
        public void DisplayWorkersAssignedToTask(int taskId)
        {
            try
            {
                // Pobranie użytkowników przypisanych do zadania
                var users = _taskAssignmentRepository.GetWorkersAssignedToTask(taskId);
                if (users == null || users.Count == 0)
                {
                    Console.WriteLine($"Brak użytkowników w zadaniu o Id {taskId}.");
                    return;
                }

                // Wyświetlenie szczegółów użytkowników
                foreach (var user in users)
                {
                    Console.WriteLine(user.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas pobierania użytkowników w zadania: {ex.Message}");
            }
        }
    }
}

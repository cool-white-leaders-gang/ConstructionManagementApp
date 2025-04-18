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
        private readonly TaskRepository _taskRepository;
        private readonly TaskAssignmentRepository _taskAssignmentRepository;
        private readonly ProjectRepository _projectRepository;
        private readonly AuthenticationService _authenticationService;
        private readonly UserRepository _userRepository;
        private readonly RBACService _rbacService;
        public event LogEventHandler TaskAdded;
        public event LogEventHandler TaskDeleted;
        public event LogEventHandler TaskUpdated;
        public event LogEventHandler TaskCompleted;
        public event LogEventHandler TaskStarted;
        public event LogEventHandler TaskAssigned;
        public event LogEventHandler TaskUnassigned;


        public TaskController(TaskRepository taskRepository, TaskAssignmentRepository taskAssignmentRepository, AuthenticationService authenticationService, ProjectRepository projectRepository, RBACService rbacService, UserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _taskAssignmentRepository = taskAssignmentRepository;
            _authenticationService = authenticationService;
            _projectRepository = projectRepository;
            _rbacService = rbacService;
            _userRepository = userRepository;
        }

        public void AddTask(string title, string description, TaskPriority priority, TaskProgress progress, string projectName)
        {
            
            try
            {
                var project = _projectRepository.GetProjectByName(projectName);
                if (project == null)
                    throw new KeyNotFoundException($"Nie znaleziono projektu o nazwie {projectName}");
                int projectId = project.Id;
                if (!_rbacService.IsProjectManager(_authenticationService.CurrentSession.User, projectId, _projectRepository))
                {
                    throw new UnauthorizedAccessException("Brak uprawnień. Tylko menedżerowie tego projektu lub administratorzy mogą dodawać zadania.");
                }
                var task = new Task(title, description, priority, progress, projectId);
                _taskRepository.AddTask(task);
                Console.WriteLine("Zadanie zostało pomyślnie dodane.");
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

        public void UpdateTask(int id, string title, string description, TaskPriority priority, TaskProgress progress)
        {
            
            try
            {
                var task = _taskRepository.GetTaskById(id);
                if (task == null)
                    throw new KeyNotFoundException($"Nie znaleziono zadania o ID {id}.");
                var project = _projectRepository.GetProjectById(task.ProjectId);
                if (project == null)
                    throw new KeyNotFoundException($"Nie znaleziono projektu o ID {id}");
                int projectId = project.Id;
                if (!_rbacService.IsProjectManager(_authenticationService.CurrentSession.User, projectId, _projectRepository))
                {
                    throw new UnauthorizedAccessException("Brak uprawnień. Tylko menedżerowie tego projektu lub administratorzy mogą dodawać zadania.");
                }
                task.Title = title;
                task.Description = description;
                task.Priority = priority;
                task.Progress = progress;

                _taskRepository.UpdateTask(task);
                Console.WriteLine("Zadanie zostało pomyślnie zaktualizowane.");
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

        public void CompleteTask(int id)
        {
            try
            {
                var task = _taskRepository.GetTaskById(id);
                if (task == null)
                    throw new KeyNotFoundException($"Nie znaleziono zadania o ID {id}.");
                var assignedUsers = _taskAssignmentRepository.GetWorkersAssignedToTask(id);
                if (!assignedUsers.Any(worker => worker.Id == _authenticationService.CurrentSession.User.Id))
                    throw new UnauthorizedAccessException("Brak uprawnień. Tylko przypisani użytkownicy mogą rozpocząć zadanie.");
                task.Progress = TaskProgress.Completed;
                _taskRepository.UpdateTask(task);
                Console.WriteLine("Zadania zostało pomyślnie ukończone");
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

        public void StartTask(int id)
        {
            try
            {
                var task = _taskRepository.GetTaskById(id);
                if (task == null)
                    throw new KeyNotFoundException($"Nie znaleziono zadania o ID {id}");
                var assignedUsers = _taskAssignmentRepository.GetWorkersAssignedToTask(id);
                if (!assignedUsers.Any(worker => worker.Id == _authenticationService.CurrentSession.User.Id))
                    throw new UnauthorizedAccessException("Brak uprawnień. Tylko przypisani użytkownicy mogą rozpocząć zadanie.");
                task.Progress = TaskProgress.InProgress;
                _taskRepository.UpdateTask(task);
                Console.WriteLine("Zadanie zostało rozpoczęte");
                TaskStarted?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Zdanie o ID {id} zostało rozpoczęte"));
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

        public void DeleteTask(int id)
        {
            try
            {
                var task = _taskRepository.GetTaskById(id);
                if (task == null)
                    throw new KeyNotFoundException($"Nie znaleziono zadania o ID {id}.");
                var project = _projectRepository.GetProjectById(task.ProjectId);
                if (project == null)
                    throw new KeyNotFoundException($"Nie znaleziono projektu o ID {id}");
                int projectId = project.Id;
                if (!_rbacService.IsProjectManager(_authenticationService.CurrentSession.User, projectId, _projectRepository))
                {
                    throw new UnauthorizedAccessException("Brak uprawnień. Tylko menedżerowie tego projektu lub administratorzy mogą dodawać zadania.");
                }
                _taskRepository.DeleteTaskById(id);
                Console.WriteLine("Zadanie zostało pomyślnie usunięte.");
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

        public void DisplayAllTasks()
        {
            try
            {
                var userProjectIds = _rbacService.GetProjectsForUserOrManagedBy(_authenticationService.CurrentSession.User, _projectRepository);
                if (!userProjectIds.Any())
                    throw new InvalidOperationException("Nie jesteś przypisany do żadnych projektów.");
                var tasks = _taskRepository.GetTasksByProjectIds(userProjectIds);

                if (!tasks.Any())
                {
                    Console.WriteLine("Brak zadań");
                    return;
                }

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

        public void AssignTask(int taskId, string username)
        {
            try
            {
                var task = _taskRepository.GetTaskById(taskId);
                if (task == null)
                    throw new KeyNotFoundException($"Nie znaleziono zadania o ID {taskId}.");
                if (!_rbacService.IsProjectManager(_authenticationService.CurrentSession.User, task.ProjectId, _projectRepository))
                    throw new UnauthorizedAccessException("Brak uprawnień. Tylko manager projektu może przypisywać pracowników do zadania.");
                var worker = _userRepository.GetUserByUsername(username);
                if (worker == null)
                    throw new KeyNotFoundException($"Nie znaleziono użytkownika o nazwie {username}.");
                if (!_rbacService.IsWorkerInProjectTeam(worker, task.ProjectId, _projectRepository))
                    throw new UnauthorizedAccessException("Brak uprawnień. Pracownik musi być członkiem zespołu przypisanego do projektu.");
                _taskAssignmentRepository.AssignWorkerToTask(taskId, username);
                Console.WriteLine($"Użytkownik o nazwie {username} został przypisany do zadania o ID {taskId}.");
                TaskAssigned?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Przypisano użytkownika {username} do zadania o ID {taskId}"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas przypisywania użytkownika do zadania: {ex.Message}");
            }
        }

        public void RemoveWorkerFromTask(int taskId, string username)
        {
            try
            {
                var task = _taskRepository.GetTaskById(taskId);
                if (task == null)
                    throw new KeyNotFoundException($"Nie znaleziono zadania o ID {taskId}.");
                if (!_rbacService.IsProjectManager(_authenticationService.CurrentSession.User, task.ProjectId, _projectRepository))
                    throw new UnauthorizedAccessException("Brak uprawnień. Tylko manager projektu może usuwać pracowników z zadania.");
                var worker = _userRepository.GetUserByUsername(username);
                if (worker == null)
                    throw new KeyNotFoundException($"Nie znaleziono użytkownika o nazwie {username}.");

                _taskAssignmentRepository.RemoveWorkerFromTask(taskId, username);
                Console.WriteLine($"Użytkownik o nazwie {username} został usunięty z zadania o Id {taskId}.");
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

        public void DisplayWorkersAssignedToTask(int taskId)
        {
            try
            {
                var users = _taskAssignmentRepository.GetWorkersAssignedToTask(taskId);
                if (users.Count == 0 || users == null)
                {
                    Console.WriteLine($"Brak użytkowników w zadaniu o Id {taskId}.");
                    return;
                }

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
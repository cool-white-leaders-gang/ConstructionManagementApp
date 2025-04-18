using System;
using ConstructionManagementApp.App.Controllers;
using ConstructionManagementApp.App.Enums;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Services;

namespace ConstructionManagementApp.App.Views
{
    internal class TaskView
    {
        private readonly TaskController _taskController;
        private readonly RBACService _rbacService;
        private readonly User _currentUser;

        public TaskView(TaskController taskController, RBACService rbacService, User currentUser)
        {
            _taskController = taskController;
            _rbacService = rbacService;
            _currentUser = currentUser;
        }

        public void ShowView()
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("--- Zarządzanie zadaniami ---");
                Console.WriteLine("1. Wyświetl wszystkie zadania");
                Console.WriteLine("2. Dodaj nowe zadanie");
                Console.WriteLine("3. Zaktualizuj zadanie");
                Console.WriteLine("4. Usuń zadanie");
                Console.WriteLine("5. Przypisz zadanie");
                Console.WriteLine("6. Usuń przypisanie");
                Console.WriteLine("7. Wyświetl pracowników przypisanych do zadania");
                Console.WriteLine("8. Zakończ przypisane zadanie");
                Console.WriteLine("9. Powrót do menu głównego");

                Console.Write("\nTwój wybór: ");
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Niepoprawny wybór. Naciśnij dowolny klawisz, aby spróbować ponownie.");
                    Console.ReadKey();
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        if (HasPermission(Permission.ViewTasks)) DisplayAllTasks();
                        break;
                    case 2:
                        if (HasPermission(Permission.CreateTask)) AddTask();
                        break;
                    case 3:
                        if (HasPermission(Permission.UpdateTask)) UpdateTask();
                        break;
                    case 4:
                        if (HasPermission(Permission.DeleteTask)) DeleteTask();
                        break;
                    case 5:
                        if (HasPermission(Permission.AssignTask)) AssignTask();
                        break;
                    case 6:
                        if (HasPermission(Permission.DeleteTask)) RemoveFromTask();
                        break;
                    case 7:
                        if (HasPermission(Permission.ViewTaskAssignment)) DisplayWorkersAssignedToTask();
                        break;
                    case 8:
                        if (HasPermission(Permission.CompleteTask)) CompleteTask();
                        break;
                    case 9:
                        Console.WriteLine("Naciśnij dowolny przycisk, aby kontynuować");

                        isRunning = false; // Powrót do menu głównego
                        break;
                    default:
                        Console.WriteLine("Niepoprawny wybór. Naciśnij dowolny klawisz, aby spróbować ponownie.");
                        Console.ReadKey();
                        break;
                }
            }
        }

  

        private bool HasPermission(Permission permission)
        {
            if (!_rbacService.HasPermission(_currentUser, permission))
            {
                Console.WriteLine("Brak uprawnień do wykonania tej operacji.");
                Console.ReadKey();
                return false;
            }
            return true;
        }

        private void DisplayAllTasks()
        {
            Console.Clear();
            _taskController.DisplayAllTasks();
            ReturnToMenu();
        }

        private void AddTask()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Dodaj nowe zadanie ---");

                Console.Write("Podaj tytuł zadania: ");
                var title = Console.ReadLine();

                Console.Write("Podaj opis zadania: ");
                var description = Console.ReadLine();

                Console.WriteLine("Wybierz priorytet zadania: 1. Low, 2. Medium, 3. High");
                var priorityChoice = Console.ReadLine();
                TaskPriority priority = priorityChoice switch
                {
                    "1" => TaskPriority.Low,
                    "2" => TaskPriority.Medium,
                    "3" => TaskPriority.High,
                    _ => throw new ArgumentException("Wybrano nieprawidłowy priorytet.")
                };

                Console.WriteLine("Wybierz status zadania: 1. New, 2. In Progress, 3. Completed");
                var progressChoice = Console.ReadLine();
                TaskProgress progress = progressChoice switch
                {
                    "1" => TaskProgress.New,
                    "2" => TaskProgress.InProgress,
                    "3" => TaskProgress.Completed,
                    _ => throw new ArgumentException("Wybrano nieprawidłowy status.")
                };

                Console.WriteLine("Podaj nazwę projektu, do którego dodać zadanie");

                var projectName = Console.ReadLine();

                _taskController.AddTask(title, description, priority, progress, projectName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            finally
            {
                ReturnToMenu();
            }
        }

        private void UpdateTask()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Zaktualizuj zadanie ---");

                Console.Write("Podaj ID zadania: ");
                if (!int.TryParse(Console.ReadLine(), out var taskId))
                {
                    Console.WriteLine("Niepoprawne ID.");
                    return;
                }

                Console.Write("Podaj nowy tytuł zadania: ");
                var title = Console.ReadLine();

                Console.Write("Podaj nowy opis zadania: ");
                var description = Console.ReadLine();

                Console.WriteLine("Wybierz nowy priorytet zadania: 1. Low, 2. Medium, 3. High");
                var priorityChoice = Console.ReadLine();
                TaskPriority priority = priorityChoice switch
                {
                    "1" => TaskPriority.Low,
                    "2" => TaskPriority.Medium,
                    "3" => TaskPriority.High,
                    _ => throw new ArgumentException("Wybrano nieprawidłowy priorytet.")
                };

                Console.WriteLine("Wybierz nowy status zadania: 1. New, 2. In Progress, 3. Completed");
                var progressChoice = Console.ReadLine();
                TaskProgress progress = progressChoice switch
                {
                    "1" => TaskProgress.New,
                    "2" => TaskProgress.InProgress,
                    "3" => TaskProgress.Completed,
                    _ => throw new ArgumentException("Wybrano nieprawidłowy status.")
                };

                _taskController.UpdateTask(taskId, title, description, priority, progress);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            finally
            {
                ReturnToMenu();
            }
        }

        private void DeleteTask()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Usuń zadanie ---");

                Console.Write("Podaj ID zadania: ");
                if (!int.TryParse(Console.ReadLine(), out var taskId))
                {
                    Console.WriteLine("Niepoprawne ID.");
                    return;
                }

                _taskController.DeleteTask(taskId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            finally
            {
                ReturnToMenu();
            }
        }

        private void AssignTask()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Dodaj członka do zespołu ---");

                Console.Write("Podaj ID zadania: ");
                if (!int.TryParse(Console.ReadLine(), out var taskId))
                {
                    Console.WriteLine("Niepoprawne ID zadania.");
                    return;
                }

                Console.Write("Podaj nazwę użytkownika: ");
                string username = Console.ReadLine();

                _taskController.AssignTask(taskId, username);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            finally
            {
                ReturnToMenu();
            }
        }

        private void RemoveFromTask()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Usuń członka z zadania ---");

                Console.Write("Podaj ID zadania: ");
                if (!int.TryParse(Console.ReadLine(), out var taskId))
                {
                    Console.WriteLine("Niepoprawne ID zadania.");
                    return;
                }

                Console.Write("Podaj nazwę użytkownika: ");
                string username = Console.ReadLine();

                _taskController.RemoveWorkerFromTask(taskId, username);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            finally
            {
                ReturnToMenu();
            }
        }

        private void DisplayWorkersAssignedToTask()
        {
            Console.Clear ();
            Console.WriteLine("--- Wyświetl pracowników przypisanych do zadania ---");

            Console.Write("Podaj ID zadania: ");
            if (!int.TryParse(Console.ReadLine(), out var taskId))
            {
                Console.WriteLine("Niepoprawne ID zadania.");
                return;
            }
            _taskController.DisplayWorkersAssignedToTask (taskId);
            Console.ReadKey();
        }

        private void CompleteTask()
        {
            Console.Clear();
            Console.WriteLine("--- Zakończ zadanie ---");

            Console.Write("Podaj ID zadania: ");
            if (!int.TryParse(Console.ReadLine(), out var taskId))
            {
                Console.WriteLine("Niepoprawne ID zadania.");
                return;
            }
            _taskController.CompleteTask(taskId);
            Console.ReadKey();
        }
        private void ReturnToMenu()
        {
            Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu zadań.");
            Console.ReadKey();
        }
    }
}
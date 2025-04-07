using System;
using ConstructionManagementApp.App.Controllers;
using ConstructionManagementApp.App.Enums;

namespace ConstructionManagementApp.App.Views
{
    internal class TaskView
    {
        private readonly TaskController _taskController;

        public TaskView(TaskController taskController)
        {
            _taskController = taskController;
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
                Console.WriteLine("5. Powrót do menu głównego");

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
                        DisplayAllTasks();
                        break;
                    case 2:
                        AddTask();
                        break;
                    case 3:
                        UpdateTask();
                        break;
                    case 4:
                        DeleteTask();
                        break;
                    case 5:
                        isRunning = false; // Powrót do menu głównego
                        break;
                    default:
                        Console.WriteLine("Niepoprawny wybór. Naciśnij dowolny klawisz, aby spróbować ponownie.");
                        Console.ReadKey();
                        break;
                }
            }
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

                _taskController.AddTask(title, description, priority, progress);
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

        private void ReturnToMenu()
        {
            Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu zadań.");
            Console.ReadKey();
        }
    }
}
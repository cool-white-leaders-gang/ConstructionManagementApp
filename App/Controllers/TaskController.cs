using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Enums;
using TaskPriority = ConstructionManagementApp.App.Enums.TaskPriority;
using Task = ConstructionManagementApp.App.Models.Task;

namespace ConstructionManagementApp.App.Controllers
{
    internal class TaskController
    {
        private readonly TaskRepository _taskRepository;

        // Konstruktor kontrolera, przyjmuje repozytorium jako zależność
        public TaskController(TaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        // Metoda do tworzenia nowego zadania
        public void CreateTask(string title, string description, TaskPriority priority, TaskProgress progress)
        {
            try
            {
                // Tworzenie nowego obiektu Task
                var task = new Task
                {
                    Title = title,
                    Description = description,
                    Priority = priority,
                    Progress = progress
                };

                // Zapisanie zadania w bazie danych
                _taskRepository.CreateTask(task);
                Console.WriteLine("Zadanie zostało pomyślnie utworzone.");
            }
            catch (Exception ex)
            {
                // Obsługa błędów
                Console.WriteLine($"Błąd podczas tworzenia zadania: {ex.Message}");
            }
        }

        // Metoda do aktualizowania istniejącego zadania
        public void UpdateTask(Task task)
        {
            try
            {
                // Aktualizacja zadania w bazie danych
                _taskRepository.UpdateTask(task);
                Console.WriteLine("Zadanie zostało pomyślnie zaktualizowane.");
            }
            catch (Exception ex)
            {
                // Obsługa błędów
                Console.WriteLine($"Błąd podczas aktualizacji zadania: {ex.Message}");
            }
        }

        // Metoda do usuwania zadania na podstawie ID
        public void DeleteTask(int taskId)
        {
            try
            {
                // Usunięcie zadania z bazy danych
                _taskRepository.DeleteTask(taskId);
                Console.WriteLine("Zadanie zostało pomyślnie usunięte.");
            }
            catch (Exception ex)
            {
                // Obsługa błędów
                Console.WriteLine($"Błąd podczas usuwania zadania: {ex.Message}");
            }
        }

        // Metoda do wyświetlania wszystkich zadań
        public void DisplayAllTasks()
        {
            try
            {
                // Pobranie wszystkich zadań z bazy danych
                var tasks = _taskRepository.GetAllTasks();
                if (tasks.Count == 0)
                {
                    Console.WriteLine("Brak zadań do wyświetlenia.");
                    return;
                }

                // Wyświetlenie każdego zadania
                foreach (var task in tasks)
                {
                    Console.WriteLine(task.ToString());
                }
            }
            catch (Exception ex)
            {
                // Obsługa błędów
                Console.WriteLine($"Błąd podczas pobierania zadań: {ex.Message}");
            }
        }

        // Metoda do wyświetlania zadania na podstawie ID
        public void DisplayTaskById(int taskId)
        {
            try
            {
                // Pobranie zadania z bazy danych
                var task = _taskRepository.GetTaskById(taskId);
                if (task == null)
                {
                    Console.WriteLine($"Zadanie o ID {taskId} nie zostało znalezione.");
                    return;
                }

                // Wyświetlenie szczegółów zadania
                Console.WriteLine(task.ToString());
            }
            catch (Exception ex)
            {
                // Obsługa błędów
                Console.WriteLine($"Błąd podczas pobierania zadania: {ex.Message}");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Repositories;
using Task =  ConstructionManagementApp.App.Models.Task;
using ConstructionManagementApp.App.Enums;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Controllers
{
    internal class TaskController
    {
        private readonly TaskRepository _taskRepository;
        private readonly TaskAssignmentRepository _taskAssignmentRepository;

        public TaskController(TaskRepository taskRepository, TaskAssignmentRepository taskAssignmentRepository)
        {
            _taskRepository = taskRepository;
            _taskAssignmentRepository = taskAssignmentRepository;
        }

        public void AddTask(string title, string description, TaskPriority priority, TaskProgress progress, int projectId)
        {
            try
            {
                var task = new Task(title, description, priority, progress, projectId);
                _taskRepository.AddTask(task);
                Console.WriteLine("Zadanie zostało pomyślnie dodane.");
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

        public void UpdateTask(int taskId, string title, string description, TaskPriority priority, TaskProgress progress)
        {
            try
            {
                var task = _taskRepository.GetTaskById(taskId);
                if (task == null)
                    throw new KeyNotFoundException($"Nie znaleziono zadania o ID {taskId}.");

                task.Title = title;
                task.Description = description;
                task.Priority = priority;
                task.Progress = progress;

                _taskRepository.UpdateTask(task);
                Console.WriteLine("Zadanie zostało pomyślnie zaktualizowane.");
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

        public void DeleteTask(int taskId)
        {
            try
            {
                _taskRepository.DeleteTaskById(taskId);
                Console.WriteLine("Zadanie zostało pomyślnie usunięte.");
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
                var tasks = _taskRepository.GetAllTasks();
                if (tasks.Count == 0)
                {
                    Console.WriteLine("Brak zadań w systemie.");
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

        public void AssignTask(int taskId, int userId)
        {
            try
            {
                _taskAssignmentRepository.AssignWorkerToTask(taskId, userId);
                Console.WriteLine($"Użytkownik o Id {userId} został przypisany do zadania o Id {taskId}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas przypisywania użytkownika do zadania: {ex.Message}");
            }
        }

        public void RemoveWorkerFromTask(int taskId, int userId)
        {
            try
            {
                _taskAssignmentRepository.RemoveWorkerFromTask(taskId, userId);
                Console.WriteLine($"Użytkownik o Id {userId} został usunięty z zadania o Id {taskId}.");
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
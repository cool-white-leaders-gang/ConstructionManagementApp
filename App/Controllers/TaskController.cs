using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Enums;

namespace ConstructionManagementApp.App.Controllers
{
    internal class TaskController
    {
        private readonly TaskRepository _taskRepository;

        public TaskController(TaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public void AddTask(string title, string description, TaskPriority priority, TaskProgress progress)
        {
            try
            {
                var task = new Task(title, description, priority, progress);
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
    }
}
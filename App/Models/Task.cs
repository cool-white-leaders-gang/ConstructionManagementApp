using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Enums;

namespace ConstructionManagementApp.App.Models
{
    internal class Task
    {
        public int Id { get; private set; } // Id zadania

        private string _title;
        private string _description;
        private TaskPriority _priority;
        private TaskProgress _progress;
        private readonly DateTime _createdAt; // Prywatne pole dla daty utworzenia

        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Tytuł zadania nie może być pusty.");
                _title = value;
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Opis zadania nie może być pusty.");
                _description = value;
            }
        }

        public TaskPriority Priority
        {
            get => _priority;
            set
            {
                if (!Enum.IsDefined(typeof(TaskPriority), value))
                    throw new ArgumentException("Nieprawidłowy priorytet zadania.");
                _priority = value;
            }
        }

        public TaskProgress Progress
        {
            get => _progress;
            set
            {
                if (!Enum.IsDefined(typeof(TaskProgress), value))
                    throw new ArgumentException("Nieprawidłowy status zadania.");
                _progress = value;
            }
        }

        public DateTime CreatedAt => _createdAt;

        // Relacja wiele do wielu z użytkownikami (przez TaskAssignment)
        public List<TaskAssignment> TaskAssignments { get; private set; }

        public Task(string title, string description, TaskPriority priority, TaskProgress progress)
        {
            Title = title;
            Description = description;
            Priority = priority;
            Progress = progress;
            _createdAt = DateTime.Now;
            TaskAssignments = new List<TaskAssignment>();
        }

        public Task()
        {
            TaskAssignments = new List<TaskAssignment>();
        }

        public override string ToString()
        {
            return $"Task: {Title}, Priority: {Priority}, Progress: {Progress}, CreatedAt: {CreatedAt}";
        }
    }
}
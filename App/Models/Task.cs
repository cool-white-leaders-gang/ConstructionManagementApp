using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Enums;

namespace ConstructionManagementApp.App.Models
{
    internal class Task
    {
        private string _title;
        private string _description;

        public int Id { get; private set; } // tylko do odczytu, baza sama ustawia id
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
            set => _description = value ?? string.Empty; // Opis zadania może być pusty
        }

        public TaskPriority Priority { get; set; }
        public TaskProgress Progress { get; set; }

        public List<TaskAssignment> TaskAssignments { get; private set; }

        public Task(string title, string description, TaskPriority priority, TaskProgress progress)
        {
            Title = title;
            Description = description;
            Priority = priority;
            Progress = progress;
            TaskAssignments = new List<TaskAssignment>();
        }

        public Task() { }
    }
}
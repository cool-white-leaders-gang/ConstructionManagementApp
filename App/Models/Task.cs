using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Enums;

namespace ConstructionManagementApp.App.Models
{
    internal class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskPriority Priority { get; set; }
        public TaskProgress Progress { get; set; }

        // Lista przypisanych u¿ytkowników (przez TaskAssignment)
        public List<TaskAssignment> TaskAssignments { get; set; }

        public Task(string title, string description, TaskPriority priority, TaskProgress progress)
        {
            Title = title;
            Description = description;
            Priority = priority;
            Progress = progress;
            TaskAssignments = new List<TaskAssignment>();
        }

        public override string ToString()
        {
            return $"Task: {Title}, Description: {Description}, Priority: {Priority}, Progress: {Progress}, Assigned Workers: {1}";
        }
    }
}
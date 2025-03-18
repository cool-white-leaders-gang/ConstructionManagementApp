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
        public List<User> AssignedWorkers { get; set; }

        public Task(string title, string description, TaskPriority priority, TaskProgress progress, List<User> assignedWorkers)
        {
            Title = title;
            Description = description;
            Priority = priority;
            Progress = progress;
            AssignedWorkers = assignedWorkers;
        }

        public override string ToString()
        {
            string workers = string.Join(", ", AssignedWorkers.Select(w => w.Username));
            return $"Task: {Title}, Description: {Description}, Priority: {Priority}, Progress: {Progress}, Assigned Workers: {workers}";
        }
    }
}
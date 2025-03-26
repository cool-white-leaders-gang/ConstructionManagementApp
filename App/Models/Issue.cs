using System;
using ConstructionManagementApp.App.Enums;

namespace ConstructionManagementApp.App.Models
{
    internal class Issue : Report
    {
        public TaskPriority Priority { get; set; } // Priorytet problemu (np. Low, Medium, High)
        public string Status { get; private set; } // Status problemu (np. "Open", "In Progress", "Resolved")
        public DateTime? ResolvedAt { get; private set; } // Data rozwiązania problemu (null, jeśli problem nie został rozwiązany)

        // Konstruktor
        public IssueReport(string title, string content, User createdBy, Project project, TaskPriority priority)
            : base(title, content, createdBy, project)
        {
            Priority = priority;
            Status = "Open"; // Domyślny status problemu to "Open"
            ResolvedAt = null; // Problem nie jest jeszcze rozwiązany
        }

        // Konstruktor bezparametrowy (wymagany przez Entity Framework)
        public IssueReport() { }
    }
}
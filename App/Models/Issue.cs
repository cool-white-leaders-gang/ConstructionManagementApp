using System;
using ConstructionManagementApp.App.Enums;

namespace ConstructionManagementApp.App.Models
{
    internal class Issue : Report
    {
        public TaskPriority Priority { get; set; } // Priorytet problemu (np. Low, Medium, High)
        public IssueStatus Status { get; set; } // Status problemu (np. "Open", "In Progress", "Resolved")
        public DateTime? ResolvedAt { get; set; } // Data rozwiązania problemu (null, jeśli problem nie został rozwiązany)

        // Konstruktor
        public Issue(string title, string content, int userId, int projectId, TaskPriority priority)
            : base(title, content, userId, projectId)
        {
            Priority = priority;
            Status = IssueStatus.Open; // Domyślny status problemu to "Open"
            ResolvedAt = null; // Problem nie jest jeszcze rozwiązany
        }

        // Konstruktor bezparametrowy (wymagany przez Entity Framework)
        public Issue() { }
    }
}
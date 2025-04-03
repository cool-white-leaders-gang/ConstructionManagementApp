using System;
using ConstructionManagementApp.App.Enums;

namespace ConstructionManagementApp.App.Models
{
    internal class Issue : Report
    {
        private TaskPriority _priority;
        private IssueStatus _status;
        private DateTime? _resolvedAt;

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

        public IssueStatus Status
        {
            get => _status;
            set
            {
                if (!Enum.IsDefined(typeof(IssueStatus), value))
                    throw new ArgumentException("Nieprawidłowy status problemu.");
                _status = value;
            }
        }

        public DateTime? ResolvedAt
        {
            get => _resolvedAt;
            set
            {
                 if (value > DateTime.Now)
                    throw new ArgumentException("Data rozwiązania problemu nie może być w przyszłości.");
                _resolvedAt = value;
            }
        }

        public Issue(string title, string content, int userId, int projectId, TaskPriority priority)
            : base(title, content, userId, projectId)
        {
            Priority = priority;
            Status = IssueStatus.Open;
            ResolvedAt = null;
        }

        public Issue() { }
    }
}
using System;

namespace ConstructionManagementApp.App.Models
{
    internal abstract class Report
    {
        public int Id { get; private set; } // Id raportu

        private string _title;
        private string _content;
        private DateTime _createdAt;
        private int _createdByUserId;
        private int _projectId;

        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Tytuł raportu nie może być pusty.");
                _title = value;
            }
        }

        public string Content
        {
            get => _content;
            set => _content = value ?? string.Empty; // Treść raportu może być pusta, ale nie null
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            private set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Data utworzenia raportu nie może być w przyszłości.");
                _createdAt = value;
            }
        }

        public int CreatedByUserId
        {
            get => _createdByUserId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Id autora raportu musi być większe od zera.");
                _createdByUserId = value;
            }
        }

        public int ProjectId
        {
            get => _projectId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Id projektu musi być większe od zera.");
                _projectId = value;
            }
        }

        protected Report(string title, string content, int userId, int projectId)
        {
            Title = title;
            Content = content;
            CreatedAt = DateTime.Now;
            CreatedByUserId = userId;
            ProjectId = projectId;
        }

        protected Report() { }
    }
}
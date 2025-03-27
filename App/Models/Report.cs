using System;

namespace ConstructionManagementApp.App.Models
{
    internal abstract class Report
    {
        public int Id { get; set; } // Id raportu
        public string Title { get; set; } // Tytuł raportu
        public string Content { get; set; } // Treść raportu
        public DateTime CreatedAt { get; set; } // Data utworzenia raportu
        public int CreatedByUserId { get; set; } // Autor raportu
        public int ProjectId { get; set; } // Projekt, do którego raport należy

        protected Report(string title, string content, int userId, int projectId)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Content = content ?? string.Empty;
            CreatedAt = DateTime.Now;
            CreatedByUserId = userId;
            ProjectId = projectId;
        }

        protected Report() { }
    }
}
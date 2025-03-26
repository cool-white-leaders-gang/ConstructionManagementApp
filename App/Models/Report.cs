using System;

namespace ConstructionManagementApp.App.Models
{
    internal abstract class Report
    {
        public int Id { get; set; } // Id raportu
        public string Title { get; set; } // Tytuł raportu
        public string Content { get; set; } // Treść raportu
        public DateTime CreatedAt { get; set; } // Data utworzenia raportu
        public User CreatedBy { get; set; } // Autor raportu
        public Project Project { get; set; } // Projekt, do którego raport należy

        protected Report(string title, string content, User createdBy, Project project)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Content = content ?? string.Empty;
            CreatedAt = DateTime.Now;
            CreatedBy = createdBy ?? throw new ArgumentNullException(nameof(createdBy));
            Project = project ?? throw new ArgumentNullException(nameof(project));
        }

        protected Report() { }
    }
}
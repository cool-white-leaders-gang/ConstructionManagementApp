using System;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Models
{
    internal class ProgressReport : Report
    {
        public int CompletionPercentage { get; set; } // Procent ukończenia projektu

        // Konstruktor
        public ProgressReport(string title, string content, int createdByUserId, int projectId, int completionPercentage)
            : base(title, content, createdByUserId, projectId)
        {
            CompletionPercentage = completionPercentage;
        }

        // Konstruktor bezparametrowy (wymagany przez EF)
        public ProgressReport() { }
    }
}
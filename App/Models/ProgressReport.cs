using System;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Models
{
    internal class ProgressReport : Report
    {
        private int _completionPercentage;

        public int CompletionPercentage
        {
            get => _completionPercentage;
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentException("Procent ukończenia musi być w zakresie od 0 do 100.");
                _completionPercentage = value;
            }
        }

        public ProgressReport(string title, string content, int createdByUserId, int projectId, int completionPercentage)
            : base(title, content, createdByUserId, projectId)
        {
            CompletionPercentage = completionPercentage;
        }

        public ProgressReport() { }
    }
}
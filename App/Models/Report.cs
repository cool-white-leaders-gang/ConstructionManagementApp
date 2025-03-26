using System;

namespace ConstructionManagementApp.App.Models
{
    internal class Report
    {
        public int Id { get; private set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedByUserId { get; set; }
        public int ProjectId { get; set; }

        public Report(string title, string content, int createdByUserId, int projectId)
        {
            Title = title;
            Content = content;
            CreatedAt = DateTime.Now;
            CreatedByUserId = createdByUserId;
            ProjectId = projectId;
        }

        public override string ToString()
        {
            return $"Raport: {Title}, stworzony: {CreatedAt}, przez u¿ytkownika o ID: {CreatedByUserId}";
        }
    }
}
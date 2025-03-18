using System;

namespace ConstructionManagementApp.App.Models
{
    internal class Report
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedByUserId { get; set; }

        public Report(string title, string content, int createdByUserId)
        {
            Title = title;
            Content = content;
            CreatedAt = DateTime.Now;
            CreatedByUserId = createdByUserId;
        }

        public override string ToString()
        {
            return $"Report: {Title}, Created At: {CreatedAt}, Created By User ID: {CreatedByUserId}";
        }
    }
}
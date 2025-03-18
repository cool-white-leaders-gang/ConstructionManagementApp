using System;

namespace ConstructionManagementApp.App.Models
{
    internal class Issue
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime ReportedAt { get; set; }
        public int ReportedByUserId { get; set; }

        public Issue(string description, int reportedByUserId)
        {
            Description = description;
            ReportedAt = DateTime.Now;
            ReportedByUserId = reportedByUserId;
        }

        public override string ToString()
        {
            return $"Issue: {Description}, Reported At: {ReportedAt}, Reported By User ID: {ReportedByUserId}";
        }
    }
}
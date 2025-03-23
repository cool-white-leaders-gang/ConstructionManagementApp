using System;
using System.Collections.Generic;

namespace ConstructionManagementApp.App.Models
{
    internal class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Task> Tasks { get; set; }
        public Team Team { get; set; }
        public Budget Budget { get; set; }
        public List<Material> Materials { get; set; }
        public List<Equipment> Equipment { get; set; }
        public List<Report> Reports { get; set; }
        public List<Issue> Issues { get; set; }
        public User Client { get; set; }

        public Project(string name, string description, Team team, Budget budget, User client)
        {
            Name = name;
            Description = description;
            Tasks = new List<Task>();
            Team = team;
            Budget = budget;
            Materials = new List<Material>();
            Equipment = new List<Equipment>();
            Reports = new List<Report>();
            Issues = new List<Issue>();
            Client = client;
        }

        public void AddTask(Task task)
        {
            Tasks.Add(task);
        }

        public void AddMaterial(Material material)
        {
            Materials.Add(material);
        }

        public void AddEquipment(Equipment equipment)
        {
            Equipment.Add(equipment);
        }

        public void AddReport(Report report)
        {
            Reports.Add(report);
        }

        public void AddIssue(Issue issue)
        {
            Issues.Add(issue);
        }

        public override string ToString()
        {
            return $"Project: {Name}, Description: {Description}, Number of Tasks: {Tasks.Count}, Team: {Team.Name}, Budget: {Budget.TotalAmount}, Client: {Client.Username}";
        }
    }
}
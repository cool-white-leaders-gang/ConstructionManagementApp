using System;
using System.Collections.Generic;

namespace ConstructionManagementApp.App.Models
{
    internal class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TeamId { get; set; }
        public int BudgetId { get; set; }
        public int ClientId { get; set; }

        public Project(string name, string description, int teamId, int budgetId, int clientId)
        {
            Name = name;
            Description = description;
            TeamId = teamId;
            BudgetId = budgetId;
            ClientId = clientId;
        }

       

        public override string ToString()
        {
            return $"Project: {Name}, Description: {Description}, Team Id: {TeamId}, Budget Id: {BudgetId}, Client Id: {ClientId}";
        }
    }
}
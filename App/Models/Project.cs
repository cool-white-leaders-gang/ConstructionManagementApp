using System;

namespace ConstructionManagementApp.App.Models
{
    internal class Project
    {
        public int Id { get; private set; }

        private string _name;
        private string _description;
        private int _teamId;
        private int _budgetId;
        private int _clientId;

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Nazwa projektu nie może być pusta.");
                _name = value;
            }
        }

        public string Description
        {
            get => _description;
            set => _description = value ?? string.Empty;
        }

        public int TeamId
        {
            get => _teamId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Id zespołu musi być większe od zera.");
                _teamId = value;
            }
        }

        public int BudgetId
        {
            get => _budgetId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Id budżetu musi być większe od zera.");
                _budgetId = value;
            }
        }

        public int ClientId
        {
            get => _clientId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Id klienta musi być większe od zera.");
                _clientId = value;
            }
        }

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
using System;
using System.Collections.Generic;

namespace ConstructionManagementApp.App.Models
{
    internal class Team
    {
        public int Id { get; private set; } // Id zespołu

        private string _name;
        private int _managerId;

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Nazwa zespołu nie może być pusta.");
                _name = value;
            }
        }

        public int ManagerId
        {
            get => _managerId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Id menedżera musi być większe od zera.");
                _managerId = value;
            }
        }

        // Relacja jeden do wielu z użytkownikami (przez TeamMembers)
        public List<TeamMembers> TeamMembers { get; private set; }

        public Team(string name, int managerId)
        {
            Name = name;
            ManagerId = managerId;
            TeamMembers = new List<TeamMembers>();
        }

        public Team()
        {
            TeamMembers = new List<TeamMembers>();
        }

        public override string ToString()
        {
            return $"Team: {Name}, ManagerId: {ManagerId}";
        }
    }
}
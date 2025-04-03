using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Enums;

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


        public Team(string name, int managerId)
        {
            Name = name;
            ManagerId = managerId;
        }

        public override string ToString()
        {
            return $"Team: {Name}, ManagerId: {ManagerId}";
        }
    }
}
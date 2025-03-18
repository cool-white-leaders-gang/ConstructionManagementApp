using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Enums;

namespace ConstructionManagementApp.App.Models
{
    internal class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public User Manager { get; set; }
        public List<User> Workers { get; set; }

        public Team(string name, User manager)
        {
            Name = name;
            Manager = manager;
            Workers = new List<User>();
        }

        public void AddWorker(User worker)
        {
            if (worker.Role == Role.Worker)
            {
                Workers.Add(worker);
            }
            else
            {
                throw new ArgumentException("Only users with the Worker role can be added as workers.");
            }
        }

        public override string ToString()
        {
            return $"Team: {Name}, Manager: {Manager.Username}, Number of Workers: {Workers.Count}";
        }
    }
}
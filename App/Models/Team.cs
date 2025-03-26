using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Enums;

namespace ConstructionManagementApp.App.Models
{
    internal class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ManagerId { get; set; } // Zmieniono z user na int

        public List<TeamMembers> TeamMembers { get; set; } //tabela ³¹cnzikowa Teammembers
        public Team(string name, int managerId)
        {
            Name = name;
            ManagerId = managerId;
            TeamMembers = new List<TeamMembers>();
        }

        //TRZEBA ZROBIÆ INN¥ METODÊ DODANIA, POKOMBINUJ, TO TRZEBA ZROBIÆ W TABELI TEAMMEMBERS
        //public void AddWorker(User worker)
        //{
        //    if (worker.Role == Role.Worker)
        //    {
        //        Workers.Add(worker);
        //    }
        //    else
        //    {
        //        throw new ArgumentException("Only users with the Worker role can be added as workers.");
        //    }
        //}

        public override string ToString()
        {
            return $"Team: {Name}, ManagerId: {ManagerId}";   //tu te¿ siê pobaw
        }
    }
}
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

        public List<TeamMembers> TeamMembers { get; set; } //tabela ��cnzikowa Teammembers
        public Team(string name, int managerId)
        {
            Name = name;
            ManagerId = managerId;
            TeamMembers = new List<TeamMembers>();
        }

        //TRZEBA ZROBI� INN� METOD� DODANIA, POKOMBINUJ, TO TRZEBA ZROBI� W TABELI TEAMMEMBERS
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
            return $"Team: {Name}, ManagerId: {ManagerId}";   //tu te� si� pobaw
        }
    }
}
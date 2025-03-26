using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionManagementApp.App.Models
{
    internal class TeamMembers  //dodanie klasy ktora jest tabela laczaca
    {
        
        public int TeamId { get; set; }
        public Team team { get; set; }
        public int UserId { get; set; }
        public User user { get; set; }

        public TeamMembers(int teamId, int userId)
        {
            TeamId = teamId;
            UserId = userId;
        }

        public TeamMembers() { }
    }
}

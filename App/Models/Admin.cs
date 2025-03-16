using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Models
{
    internal class Admin : User
    {
        public string AdminKey { get; set; }
        public Admin(string username, string passwordHash, string email, string adminKey)
            : base(username, passwordHash, email, Enums.Role.Admin)
        {
            AdminKey = adminKey;
        }
        public override string ToString()
        {
            return base.ToString() + $", Klucz administracyjny: {AdminKey}";
        }
    }
}

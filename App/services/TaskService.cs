using ConstructionManagementApp.App.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionManagementApp.App.Services
{
    internal class TaskService
    {
        private Dictionary<Role, List<Permissions>> _rolePermissions { get; set; }

        public TaskService()
        {
            _rolePermissions = new Dictionary<Role, List<Permissions>>
            {
                {Role.Admin, new List<Permissions>{Permissions.Read, Permissions.Write, Permissions.Delete, Permissions.ManageUsers} },
                {Role.Manager, new List<Permissions>{Permissions.Read, Permissions.Write, Permissions.Delete} },
                {Role.Worker, new List<Permissions>{Permissions.Read} }
            };
        }

        public void HasPermission(Role role, Permissions permission)
        {
            if (_rolePermissions[role].Contains(permission))
            {
                Console.WriteLine("User has permission");
            }
            else
            {
                Console.WriteLine("User does not have permission");
            }
        }
    }
}

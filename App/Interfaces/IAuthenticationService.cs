using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Services;

namespace ConstructionManagementApp.App.Interfaces
{
    internal interface IAuthenticationService
    {
        bool Login(string email, string password);
        void Logout();
        User GetCurrentUser();
    }
}

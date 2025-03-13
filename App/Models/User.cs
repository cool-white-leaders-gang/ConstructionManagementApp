using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConstructionManagementApp.App.Enums;

namespace ConstructionManagementApp.App.Models
{
    //klasa user
    internal class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

        //konstruktor
        public User(string username, string password, string email, Role role)
        {
            Username = username;
            Email = email;
            Password = password;
            Role = role;
        }

        public string GetAllData()
        {
            return $"Username: {Username}, Email: {Email}, Role: {Role}";
        }
    }
}

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
        public string PasswordHash { get; set; }
        public Role Role { get; set; }

        //konstruktor
        public User(string username, string passwordHash, string email, Role role)
        {
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
        }

        public override string ToString()
        {
            return $"Nazwa użytkownika: {Username}, email: {Email}, rola: {Role}";
        }

    }
}

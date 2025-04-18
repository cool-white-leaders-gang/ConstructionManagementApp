using System;
using System.Text.RegularExpressions;
using ConstructionManagementApp.App.Enums;

namespace ConstructionManagementApp.App.Models
{
    internal class User
    {
        public int Id { get; private set; } // Id użytkownika

        private string _username;
        private string _email;
        private string _passwordHash;
        private Role _role;
        public List<TaskAssignment> TaskAssignments;
        public List<TeamMembers> TeamMembers;

        public string Username
        {
            get => _username;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Nazwa użytkownika nie może być pusta.");
                _username = value;
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Email nie może być pusty.");
                if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    throw new ArgumentException("Podany adres e-mail jest nieprawidłowy.");
                _email = value;
            }
        }

        public string PasswordHash
        {
            get => _passwordHash;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Hasło nie może być puste.");
                _passwordHash = value;
            }
        }

        public Role Role
        {
            get => _role;
            set
            {
                if (!Enum.IsDefined(typeof(Role), value))
                    throw new ArgumentException("Nieprawidłowa rola użytkownika.");
                _role = value;
            }
        }

        public User(string username, string email, string passwordHash, Role role)
        {
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            TaskAssignments = new List<TaskAssignment>();
            TeamMembers = new List<TeamMembers>();
        }

        public override string ToString()
        {
            return $"\tUżytkownik: {Username}, Email: {Email}, Rola: {Role}";
        }
    }
}

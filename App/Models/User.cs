using System;
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
                if (!value.Contains("@"))
                    throw new ArgumentException("Email musi zawierać znak '@'.");
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
                if (value.Length < 6)
                    throw new ArgumentException("Hasło musi mieć co najmniej 6 znaków.");
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
        }

        public override string ToString()
        {
            return $"User: {Username}, Email: {Email}, Role: {Role}";
        }
    }
}

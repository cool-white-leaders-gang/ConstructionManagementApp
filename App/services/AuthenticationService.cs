using System;
using System.Security.Cryptography;
using System.Text;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Repositories;

namespace ConstructionManagementApp.App.Services
{
    internal class AuthenticationService
    {
        private readonly UserRepository _userRepository;
        private Session _currentSession;

        public AuthenticationService(UserRepository userRepository)
        {
            _userRepository = userRepository;
            _currentSession = null;
        }

        public bool Login(string email, string password)
        {
            var user = _userRepository.GetUsersByEmail(email).FirstOrDefault();
            if (user == null)
            {
                Console.WriteLine("Nie znaleziono użytkownika o podanym adresie email.");
                return false;
            }

            if (!VerifyPassword(password, user.PasswordHash))
            {
                Console.WriteLine("Nieprawidłowe hasło.");
                return false;
            }

            _currentSession = new Session(user);
            Console.WriteLine($"Zalogowano pomyślnie jako {user.Username}.");
            return true;
        }

        public void Logout()
        {
            if (_currentSession == null)
            {
                Console.WriteLine("Brak aktywnej sesji.");
                return;
            }

            Console.WriteLine($"Wylogowano użytkownika {_currentSession.User.Username}.");
            _currentSession = null;
        }

        public User GetCurrentUser()
        {
            return _currentSession?.User;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }
    }
}
using System;
using System.Security.Cryptography;
using System.Text;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Utilities;

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

        public AuthenticationService(){

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

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return PasswordHasher.HashPassword(password) == hashedPassword;
        }
    }
}
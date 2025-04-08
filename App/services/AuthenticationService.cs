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
        public Session? CurrentSession { get; set; }

        public AuthenticationService(UserRepository userRepository)
        {
            _userRepository = userRepository;
            CurrentSession = null;
        }

        public AuthenticationService(){

        }

        public bool Login(string email, string password)
        {
            var user = _userRepository.GetUserByEmail(email);
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

            CurrentSession = new Session(user);
            Console.WriteLine($"Zalogowano pomyślnie jako {CurrentSession.User.Username}.");
            return true;
        }

        public void Logout()
        {
            if (CurrentSession == null)
            {
                Console.WriteLine("Brak aktywnej sesji.");
                return;
            }
            string username = CurrentSession.User.Username;
            CurrentSession = null;
            Console.WriteLine($"Wylogowano użytkownika {username}.");

        }

        public User GetCurrentUser()
        {
            return CurrentSession?.User;
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return PasswordHasher.HashPassword(password) == hashedPassword;
        }
    }
}
using System;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Utilities;
using ConstructionManagementApp.Events;
using ConstructionManagementApp.App.Interfaces;
using ConstructionManagementApp.App.Delegates;

namespace ConstructionManagementApp.App.Services
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly UserRepository _userRepository;
        public Session? CurrentSession { get; set; }

        public event LogEventHandler UserLoggedIn;

        public AuthenticationService(UserRepository userRepository)
        {
            _userRepository = userRepository;
            CurrentSession = null;
        }

        // Logowanie użytkownika
        public bool Login(string email, string password)
        {
            // Sprawdzenie, czy użytkownik istnieje
            var user = _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                Console.WriteLine("Nie znaleziono użytkownika o podanym adresie email.");
                return false;
            }

            // Weryfikacja hasła
            if (!VerifyPassword(password, user.PasswordHash))
            {
                Console.WriteLine("Nieprawidłowe hasło.");
                return false;
            }

            // Tworzenie sesji po pomyślnym zalogowaniu
            CurrentSession = new Session(user);
            Console.WriteLine($"Zalogowano pomyślnie jako {CurrentSession.User.Username}.");

            // Wysyłanie zdarzenia logowania
            UserLoggedIn?.Invoke(this, new LogEventArgs(CurrentSession.User.Username, "Użytkownik zalogował się do systemu"));
            return true;
        }

        // Wylogowanie użytkownika
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

        // Pobranie aktualnie zalogowanego użytkownika
        public User GetCurrentUser()
        {
            return CurrentSession?.User;
        }

        // Weryfikacja poprawności hasła
        private bool VerifyPassword(string password, string hashedPassword)
        {
            return PasswordHasher.HashPassword(password) == hashedPassword;
        }
    }
}

using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Enums;
using ConstructionManagementApp.App.Utilities;
using ConstructionManagementApp.Events;
using ConstructionManagementApp.App.Services;
using ConstructionManagementApp.App.Delegates;

namespace ConstructionManagementApp.App.Controllers
{
    internal class UserController
    {
        private readonly UserRepository _userRepository;
        private readonly AuthenticationService _authenticationService;

        // Zdarzenia do logowania działań związanych z użytkownikami
        public event LogEventHandler UserAdded;
        public event LogEventHandler UserUpdated;
        public event LogEventHandler UserDeleted;

        // Konstruktor kontrolera, inicjalizujący repozytoria i serwisy
        public UserController(UserRepository userRepository, AuthenticationService authenticationService)
        {
            _userRepository = userRepository;
            _authenticationService = authenticationService;
        }

        // Pobranie użytkownika po jego ID
        public User GetUserById(int userId)
        {
            try
            {
                var user = _userRepository.GetUserById(userId);
                if (user == null)
                    throw new KeyNotFoundException($"Nie znaleziono użytkownika o ID {userId}.");
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas szukanie użytkownika: {ex.Message}");
                throw;
            }
        }

        // Pobranie użytkownika po nazwie użytkownika
        public User GetUserByUsername(string username)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                    throw new ArgumentException("Nazwa użytkownika nie może być pusta.");

                return _userRepository.GetUserByUsername(username);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas szukanie użytkownika: {ex.Message}");
                throw;
            }
        }

        // Pobranie użytkownika po adresie e-mail
        public User GetUserByEmail(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    throw new ArgumentException("Adres e-mail nie może być pusty.");

                var user = _userRepository.GetUserByEmail(email);
                if (user == null)
                    throw new Exception($"Nie znaleziono użytkownika o adresie e-mail '{email}'.");

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas szukanie użytkownika: {ex.Message}");
                throw;
            }
        }

        // Dodanie nowego użytkownika
        public void AddUser(string username, string email, string passwordHash, Role role)
        {
            try
            {
                // Tworzenie użytkownika i dodanie do repozytorium
                var user = new User(username, email, PasswordHasher.HashPassword(passwordHash), role);
                _userRepository.CreateUser(user);
                Console.WriteLine("Użytkownik został pomyślnie dodany.");

                // Logowanie akcji dodania użytkownika
                UserAdded?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Dodano nowego użytkownika o nazwie {username}"));
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nieoczekiwany błąd: {ex.Message}");
            }
        }

        // Aktualizacja danych użytkownika
        public void UpdateUser(string userNameUpdate, string username, string email, string passwordHash, Role role)
        {
            try
            {
                var user = _userRepository.GetUserByUsername(userNameUpdate);
                if (user == null)
                    throw new KeyNotFoundException($"Nie znaleziono użytkownika o nazwie {userNameUpdate}.");

                // Aktualizacja danych użytkownika
                user.Username = username;
                user.Email = email;
                user.PasswordHash = passwordHash;
                user.Role = role;

                _userRepository.UpdateUser(user);
                Console.WriteLine("Dane użytkownika zostały pomyślnie zaktualizowane.");

                // Logowanie akcji aktualizacji użytkownika
                UserUpdated?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Zaktualizowano użytkownika o nazwie {userNameUpdate}"));
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Błąd podczas szukanie użytkownika: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nieoczekiwany błąd: {ex.Message}");
            }
        }

        // Usunięcie użytkownika
        public void DeleteUser(string username)
        {
            try
            {
                // Usunięcie użytkownika z repozytorium
                _userRepository.DeleteUserByName(username);
                Console.WriteLine("Użytkownik został pomyślnie usunięty.");

                // Logowanie akcji usunięcia użytkownika
                UserDeleted?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Usunięto użytkownika o nazwie {username}"));
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nieoczekiwany błąd: {ex.Message}");
            }
        }

        // Wyświetlenie wszystkich użytkowników
        public void DisplayAllUsers()
        {
            try
            {
                var users = _userRepository.GetAllUsers();
                if (users.Count == 0)
                {
                    Console.WriteLine("Brak użytkowników w systemie.");
                    return;
                }

                Console.WriteLine("--- Lista użytkowników ---");
                foreach (var user in users)
                {
                    Console.WriteLine($"ID: {user.Id}, Nazwa: {user.Username}, E-mail: {user.Email}, Rola: {user.Role}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }
    }
}

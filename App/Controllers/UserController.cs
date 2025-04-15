using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Enums;
using ConstructionManagementApp.App.Utilities;

namespace ConstructionManagementApp.App.Controllers
{
    internal class UserController
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

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

        public void AddUser(string username, string email, string passwordHash, Role role)
        {
            try
            {
                var user = new User(username, email,PasswordHasher.HashPassword(passwordHash), role);
                _userRepository.CreateUser(user);
                Console.WriteLine("Użytkownik został pomyślnie dodany.");
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

        public void UpdateUser(string userNameUpdate, string username, string email, string passwordHash, Role role)
        {
            try
            {
                var user = _userRepository.GetUserByUsername(userNameUpdate);
                if (user == null)
                    throw new KeyNotFoundException($"Nie znaleziono użytkownika o nazwie {userNameUpdate}.");

                user.Username = username;
                user.Email = email;
                user.PasswordHash = passwordHash;
                user.Role = role;

                _userRepository.UpdateUser(user);
                Console.WriteLine("Dane użytkownika zostały pomyślnie zaktualizowane.");
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

        public void DeleteUser(string username)
        {
            try
            {
                _userRepository.GetUserByUsername(username);
                Console.WriteLine("Użytkownik został pomyślnie usunięty.");
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

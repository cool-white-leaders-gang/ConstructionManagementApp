using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Controllers
{
    internal class UserController
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Pobierz użytkownika po ID
        public User GetUserById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID użytkownika musi być większe od zera.");

            var user = _userRepository.GetUserById(id);
            if (user == null)
                throw new Exception($"Nie znaleziono użytkownika o ID {id}.");

            return user;
        }

        // Pobierz użytkownika po nazwie użytkownika
        public List<User> GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Nazwa użytkownika nie może być pusta.");

            List<User> users = _userRepository.GetUsersByUsername(username);
            if (users == null)
                throw new Exception($"Nie znaleziono użytkownika o nazwie '{username}'.");

            return users;
        }

        // Pobierz użytkownika po adresie e-mail
        public User GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Adres e-mail nie może być pusty.");

            if (!IsValidEmail(email))
                throw new ArgumentException("Podano nieprawidłowy adres e-mail.");

            var user = _userRepository.GetUserByEmail(email);
            if (user == null)
                throw new Exception($"Nie znaleziono użytkownika o adresie e-mail '{email}'.");

            return user;
        }

        // Dodaj nowego użytkownika
        public void AddUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "Użytkownik nie może być null.");

            ValidateUser(user);

            _userRepository.CreateUser(user);
        }

        // Zaktualizuj dane użytkownika
        public void UpdateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "Użytkownik nie może być null.");

            ValidateUser(user);

            _userRepository.UpdateUser(user);
        }

        // Usuń użytkownika po ID
        public void DeleteUser(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID użytkownika musi być większe od zera.");

            var user = _userRepository.GetUserById(id);
            if (user == null)
                throw new Exception($"Nie znaleziono użytkownika o ID {id}.");

            _userRepository.DeleteUserById(id);
        }

        // Wyświetl listę wszystkich użytkowników
        public void DisplayAllUsers()
        {
            var users = _userRepository.GetAllUsers();
            if (users == null || users.Count == 0)
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

        // Walidacja użytkownika
        private void ValidateUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username))
                throw new ArgumentException("Nazwa użytkownika nie może być pusta.");

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ArgumentException("Adres e-mail nie może być pusty.");

            if (!IsValidEmail(user.Email))
                throw new ArgumentException("Podano nieprawidłowy adres e-mail.");

            if (user.PasswordHash == null || user.PasswordHash.Length == 0)
                throw new ArgumentException("Hasło użytkownika nie może być puste.");
        }

        // Sprawdź poprawność adresu e-mail
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}

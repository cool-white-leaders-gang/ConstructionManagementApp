using System;
using System.Collections.Generic;
using System.Linq;
using ConstructionManagementApp.App.Database;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Repositories
{
    internal class UserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        // Dodanie nowego użytkownika do bazy danych
        public void CreateUser(User user)
        {
            // Sprawdzenie, czy email lub nazwa użytkownika są już zajęte
            if (IsEmailTaken(user.Email))
                throw new ArgumentException("Podany adres email jest już zajęty.");

            if (IsUsernameTaken(user.Username))
                throw new ArgumentException("Podana nazwa użytkownika jest już zajęta.");

            // Dodanie użytkownika do bazy danych
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        // Aktualizacja istniejącego użytkownika
        public void UpdateUser(User user)
        {
            // Pobranie istniejącego użytkownika na podstawie Id
            var existingUser = GetUserById(user.Id);

            // Sprawdzenie, czy zmiana emaila lub nazwy użytkownika nie powoduje konfliktu
            if (existingUser.Email != user.Email && IsEmailTaken(user.Email))
                throw new ArgumentException("Podany adres email jest już zajęty.");

            if (existingUser.Username != user.Username && IsUsernameTaken(user.Username))
                throw new ArgumentException("Podana nazwa użytkownika jest już zajęta.");

            // Zaktualizowanie użytkownika w bazie danych
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        // Usunięcie użytkownika z bazy danych na podstawie nazwy użytkownika
        public void DeleteUserByName(string username)
        {
            var user = GetUserByUsername(username);
            if (user == null)
                throw new KeyNotFoundException("Nie znaleziono użytkownika o podanej nazwie.");

            // Usunięcie użytkownika z bazy danych
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        // Pobranie użytkownika na podstawie jego Id
        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        // Pobranie użytkownika na podstawie jego nazwy użytkownika
        public User GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        // Pobranie użytkownika na podstawie jego adresu email
        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        // Pobranie wszystkich użytkowników z bazy danych
        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        // Sprawdzenie, czy nazwa użytkownika jest już zajęta
        public bool IsUsernameTaken(string username)
        {
            return _context.Users.Any(u => u.Username == username);
        }

        // Sprawdzenie, czy adres email jest już zajęty
        public bool IsEmailTaken(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }
    }
}

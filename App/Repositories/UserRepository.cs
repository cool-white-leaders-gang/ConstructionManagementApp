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

        // Tworzenie nowego użytkownika
        public void CreateUser(User user)
        {
            if (IsEmailTaken(user.Email))
                throw new ArgumentException("Podany adres email jest już zajęty.");
            
            if (IsUsernameTaken(user.Username))
                throw new ArgumentException("Podana nazwa użytkownika jest już zajęta.");

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        // Aktualizacja istniejącego użytkownika
        public void UpdateUser(User user)
        {
            var existingUser = GetUserById(user.Id);
            if (existingUser == null)
                throw new KeyNotFoundException("Nie znaleziono użytkownika o podanym Id.");

            if (existingUser.Email != user.Email && IsEmailTaken(user.Email))
                throw new ArgumentException("Podany adres email jest już zajęty.");

            if (existingUser.Username != user.Username && IsUsernameTaken(user.Username))
                throw new ArgumentException("Podana nazwa użytkownika jest już zajęta.");

            _context.Users.Update(user);
            _context.SaveChanges();
        }

        // Usuwanie użytkownika po Id
        public void DeleteUserById(int userId)
        {
            var user = GetUserById(userId);
            if (user == null)
                throw new KeyNotFoundException("Nie znaleziono użytkownika o podanym Id.");

            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        // Pobieranie użytkownika po Id
        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        // Pobieranie użytkowników po nazwie użytkownika
        public List<User> GetUsersByUsername(string username)
        {
            return _context.Users.Where(u => u.Username == username).ToList();
        }

        // Pobieranie użytkowników po adresie email
        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        // Pobieranie wszystkich użytkowników
        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        // Sprawdzenie, czy nazwa użytkownika jest zajęta
        public bool IsUsernameTaken(string username)
        {
            return _context.Users.Any(u => u.Username == username);
        }

        // Sprawdzenie, czy adres email jest zajęty
        public bool IsEmailTaken(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }
    }
}

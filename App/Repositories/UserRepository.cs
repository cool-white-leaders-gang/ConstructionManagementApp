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

        public void CreateUser(User user)
        {
            if (IsEmailTaken(user.Email))
                throw new ArgumentException("Podany adres email jest już zajęty.");

            if (IsUsernameTaken(user.Username))
                throw new ArgumentException("Podana nazwa użytkownika jest już zajęta.");

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            var existingUser = GetUserById(user.Id);

            if (existingUser.Email != user.Email && IsEmailTaken(user.Email))
                throw new ArgumentException("Podany adres email jest już zajęty.");

            if (existingUser.Username != user.Username && IsUsernameTaken(user.Username))
                throw new ArgumentException("Podana nazwa użytkownika jest już zajęta.");

            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void DeleteUserByName(string username)
        {
            var user = GetUserByUsername(username);
            if (user == null)
                throw new KeyNotFoundException("Nie znaleziono użytkownika o podanej nazwie.");

            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public bool IsUsernameTaken(string username)
        {
            return _context.Users.Any(u => u.Username == username);
        }

        public bool IsEmailTaken(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }
    }
}

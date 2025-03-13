using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConstructionManagementApp.App.Database;
using ConstructionManagementApp.App.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionManagementApp.App.Repositories
{
    //klasa UserRepository zarządzająca użytkownikami w bazie danych
    internal class UserRepository
    {
        //właściwość tylko do odczytu, któa jest kontkstem do bazy danych
        private readonly AppDbContext _context;

        //konstruktor
        public UserRepository()
        {
            _context = new AppDbContext();
        }

        //metoda dodająca użytkownika do bazy danych
        public void CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            Console.WriteLine("Użytkownik dodany do bazy danych");
        }

        //metoda aktualizująca użytkownika w bazie danych
        public void UpdateUser(User user)
        {
            try
            {
                _context.Users.Update(user);
                _context.SaveChanges();
                Console.WriteLine("Użytkownik zaktualizowany");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine("Próba aktualizacji nieistniejącego użytkownika " + ex.Message);
                //Łapanie błędu aktualizacji usuniętego użytkownika
            }
        }

        //metoda usuwająca użytkownika z bazy danych, jeśli nie ma użytkownika to wypisuje komunikat
        public void DeleteUserById(int userId)
        {
            User user = GetUserById(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                Console.WriteLine("Użytkownik usunięty");
                user = null;         //nadawanie użytkownikowi wartości null, żeby usunąć go z pamięci
                GC.Collect();         //wywołanie Garbage Collectora, żeby usunąć użytkownika z pamięci
            }
            return;
        }

        //metoda szukająca użytkownika po id
        public User GetUserById(int id)
        {
            if(_context.Users.FirstOrDefault(u => u.Id == id) == null)
            {
                Console.WriteLine("Nie znaleziono użytkownika");
                return null;
            }
            return _context.Users.FirstOrDefault(u => u.Id == id);

        }

        //metoda szukająca użytkowników po nazwie
        public List<User> GetUsersByUsername(string username)
        {
            if (_context.Users.FirstOrDefault(u => u.Username == username) == null)
            {
                Console.WriteLine("Nie znaleziono użytkownika");
                return null;
            }
            return _context.Users.ToList().FindAll(u => u.Username == username);
        }

        //metoda szukająca użytkowników po emailu
        public List<User> GetUserByEmail(string email)
        {
            if (_context.Users.FirstOrDefault(u => u.Email == email) == null)
            {
                Console.WriteLine("Nie znaleziono użytkownika");
                return null;
            }
            return _context.Users.ToList().FindAll(u => u.Email == email);
        }

        //metoda zwracająca wszystkich użytkowników
        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }


    }
}

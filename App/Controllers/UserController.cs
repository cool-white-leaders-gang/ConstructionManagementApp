using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Enums;


namespace ConstructionManagementApp.App.Controllers
{
    internal class UserController
    {
        private UserRepository _userRepository { get; set; }

        public UserController(UserRepository userRepo)
        {
            _userRepository = userRepo;
        }
        

        public void AddUser(User user)
        {
            _userRepository.CreateUser(user);
        }
        public void UpdateUser(User user)
        {
            _userRepository.UpdateUser(user);
        }
        public void DeleteUser(int userId)
        {
            _userRepository.DeleteUserById(userId);
        }
        public void GetUserById(int userId)
        {
            var user = _userRepository.GetUserById(userId);
            if (user != null)
            {
                Console.WriteLine(user.ToString());
            }
            else
            {
                return;
            }
        }
        public List<User> GetUsersByUserName(string username)
        {
            return _userRepository.GetUsersByUsername(username);
        }
        public List<User> GetUsersByEmail(string email)
        {
            return _userRepository.GetUsersByEmail(email);
        }
        public List<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }
        public void DisplayAllUsers()
        {
            var users = _userRepository.GetAllUsers();
            if (users.Count == 0)
            {
                Console.WriteLine("Brak użytkowników");
                return;
            }
            foreach (var user in users)
            {
                Console.WriteLine(user.ToString());
            }
        }

        public int GetUserId(User user)
        {
            return user.Id;
        }
    }
}

using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Enums;
using ConstructionManagementApp.App.Database;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Controllers;


namespace ConstructionManagementApp.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //   ---TESTY USER REPOSITORY---
            ////test czy wszystko działa (działa)
            //UserRepository userRepository = new UserRepository();
            //User user = new User("user", "email", "password", 0);
            //userRepository.CreateUser(user);
            //List<User> users = userRepository.GetAllUsers();
            //foreach (User u in users)
            //{
            //    Console.WriteLine(u.GetAllData());
            //}

            ////test sprawdzający czy działa usuwanie użytkownika
            //userRepository.DeleteUserById(12);

            ////test sprawdzający czy działa aktualizacja użytkownika
            //User user2 = new User("user", "email", "password", Role.Worker);
            //userRepository.CreateUser(user2);
            //user2.Username = "newUsername";
            //userRepository.UpdateUser(user);

            // ---TESTY USER CONTROLLER---
            User user = new User("user", "email", "password", Role.Worker);
            UserController userController = new UserController();
            userController.AddUser(user);
            userController.ChangeRole(1, Role.Manager);
            userController.DisplayAllUsers();
            userController.ChangeUsername(1, "newUsername");
            userController.DisplayAllUsers();
            userController.ChangeRole(1, Role.Worker);
            userController.DisplayAllUsers();
            userController.DeleteUser(1);
            userController.DisplayAllUsers();

        }
    }
}

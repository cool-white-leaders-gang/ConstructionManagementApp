using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Enums;
using ConstructionManagementApp.App.Database;
using ConstructionManagementApp.App.Repositories;


namespace ConstructionManagementApp.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //test czy wszystko działa (działa)
            UserRepository userRepository = new UserRepository();
            User user = new User("user", "email", "password", 0);
            userRepository.CreateUser(user);
            List<User> users = userRepository.GetAllUsers();
            foreach (User u in users)
            {
                Console.WriteLine(u.GetAllData());
            }

            //test sprawdzający czy działa usuwanie użytkownika
            userRepository.DeleteUserById(12);

            //test sprawdzający czy działa aktualizacja użytkownika
            User user2 = new User("user", "email", "password", Role.Worker);
            userRepository.CreateUser(user2);
            user2.Username = "newUsername";
            userRepository.UpdateUser(user);

        }
    }
}

using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Enums;
using ConstructionManagementApp.App.Database;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Controllers;
using Task = ConstructionManagementApp.App.Models.Task;


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
            //User user = new User("user", "email", "password", Role.Worker);
            //UserController userController = new UserController();
            //userController.AddUser(user);
            //userController.ChangeRole(1, Role.Manager);
            //userController.DisplayAllUsers();
            //userController.ChangeUsername(1, "newUsername");
            //userController.DisplayAllUsers();
            //userController.ChangeRole(1, Role.Worker);
            //userController.DisplayAllUsers();
            //userController.DeleteUser(1);
            //userController.DisplayAllUsers();



            // ---TESTY TABELI TASKASSIGNMENTS---
            //var context = new AppDbContext();
            //var taskRepository = new TaskRepository(context);
            //var userRepository = new UserRepository(context);
            //var userController = new UserController(userRepository);
            //User user1 = new User("user1", "email1", "password1", Role.Worker);
            //User user2 = new User("user2", "email2", "password2", Role.Worker);
            //Task task = new Task(
            //                "Task1",
            //                "Description1",
            //                TaskPriority.High,
            //                TaskProgress.InProgress
            //            );

            //userController.AddUser(user1);
            //userController.AddUser(user2);


            //context.Add(task);
            //context.SaveChanges();

            //var taskAssignment1 = new TaskAssignment
            //{
            //    UserId = userController.GetUserId(user1),
            //    TaskId = task.Id
            //};
            //var taskAssignment2 = new TaskAssignment
            //{
            //    UserId = userController.GetUserId(user2),
            //    TaskId = task.Id
            //};
            //context.Add(taskAssignment1);
            //context.Add(taskAssignment2);
            //context.SaveChanges();




            //var context = new AppDbContext();
            //var zbyszek = new User("Zbyszek", "email1", "password1", Role.Manager);
            //var userReposiotry = new UserRepository(context);
            //var userController = new UserController(userReposiotry);
            //userController.AddUser(zbyszek);

            //Team team = new Team("team1", userController.GetUserId(zbyszek));
            //context.Add(team);
            //context.SaveChanges();

            //var teamMembers = new TeamMembers
            //{
            //    UserId = userController.GetUserId(zbyszek),
            //        TeamId = team.Id
            //    };
            //var teammembers2 = new TeamMembers
            //{
            //    UserId = 11,
            //    TeamId = 2
            //};
            //context.Add(teammembers2);
            //context.SaveChanges();

            //---TEST KLASY BUDGET I EQUIPMENT---
            //var context = new AppDbContext();
            //Budget budget = new Budget(100);
            //context.Add(budget);
            //context.SaveChanges();

            //Equipment equipment = new Equipment("Koparka", EquipmentStatus.Available);
            //context.Add(equipment);
            //context.SaveChanges();


            // ---TESTY Project---
            //var context = new AppDbContext();
            //var userRepository = new UserRepository(context);
            //var userController = new UserController(userRepository);

           

            //var project = new Project("Budowa domu", "Budowa domu jednorodzinnego", 1, 1, 10);
            //context.Add(project);
            //context.SaveChanges();

            //Equipment equipment = new Equipment("Koparka", EquipmentStatus.Available, 2);
            //context.Add(equipment);
            //context.SaveChanges();
        }
    }
}

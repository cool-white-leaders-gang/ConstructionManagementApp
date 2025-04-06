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


            // ---TESTY PORJECT---
            //var context = new AppDbContext();
            //var userRepository = new UserRepository(context);
            //var userController = new UserController(userRepository);
            //var project = new Project("Budowa domu", "Budowa domu jednorodzinnego", 1, 1, 10);
            //context.Add(project);
            //context.SaveChanges();


            //---WPISYWANIE EQUIPMENT DO BAZY---
            //Equipment equipment = new Equipment("Koparka", EquipmentStatus.Available, 2);
            //context.Add(equipment);
            //context.SaveChanges();

            //---WPISYWANIE ISSUE DO BAZY---
            //var issue = new Issue("Menadzer mnie bije", 10, 2);
            //var context = new AppDbContext();
            //context.Add(issue);
            //context.SaveChanges();


            //---WPISYWANIE REPORT DO BAZY---
            //var context = new AppDbContext();
            //int useridexampledoitwithusercontroller = 10;
            //var Report = new Report("Zawiadomienie", "zbyszek sie obija", useridexampledoitwithusercontroller, 2);
            //context.Add(Report);
            //context.SaveChanges();


            //--WPISYWANIE EQUIPMENT ZA POMOCA REPOSITORY---
            //var context = new AppDbContext();
            //var equipmentRepository = new EquipmentRepository(context);
            //var equipment = new Equipment("Koparka", EquipmentStatus.Available, 2);
            //equipmentRepository.CreateEquipment(equipment);

            //var AllEquipment = equipmentRepository.GetAllEquipment();
            //foreach (var e in AllEquipment)
            //{
            //    Console.WriteLine(e.Name);
            //}



            //--WPISYWANIE ISSUE ZA POMOCA REPOSITORY---
            //var context = new AppDbContext();
            //var issueRepository = new IssueRepository(context);
            ////10 to id usera(PRZYKLADOWE)
            ////2 to id projektu(PRZYKLADOWE)
            //var issue = new Issue("Menadzer mnie bije", 10, 2);
            //issueRepository.CreateIssue(issue);
            //issue.Description = "Menadzer mnie bije i kopie i obraza";
            //issueRepository.UpdateIssue(issue);

            //var AllIssues = issueRepository.GetAllIssues();
            //foreach (var i in AllIssues)
            //{
            //    Console.WriteLine(i.Description);
            //}


            //---WPISYWANIE MATERIAL ZA POMOCA REPOSITORY---
            //var context = new AppDbContext();
            //var materialRepository = new MaterialRepository(context);
            //var material = new Material("Śruby", 1000, "szt", 2);
            //materialRepository.CreateMaterial(material);
            //var allMaterials = materialRepository.GetAllMaterials();
            //int i = 1;
            //foreach (var m in allMaterials)
            //{
            //    Console.WriteLine($"{i}) {m.ToString()}");
            //    i++;
            //}

            //---WPISYWANIE PROJECT ZA POMOCA REPOSITORY---
            //var context = new AppDbContext();
            //var projectRepository = new ProjectRepository(context);
            //var project = new Project("Budowa wieżowca", "duzy dom ", 1, 1, 10);
            //projectRepository.CreateProject(project);
            //var allProjects = projectRepository.GetAllProjects();
            //int i = 1;
            //foreach (var p in allProjects)
            //{
            //    Console.WriteLine($"{i}) {p.ToString()}");
            //    i++;
            //}

            //---WPISYWANIE RAPORTÓW ZA POMOCA REPOSITORY---
            //var context = new AppDbContext();
            //var reportRepository = new ReportRepository(context);
            //var report = new Report("Raport", "wszystko w porządku zabrakjło materiałów", 10, 7);
            //reportRepository.CreateReport(report);
            //var allReports = reportRepository.GetAllReports();
            //int i = 1;
            //foreach (var r in allReports)
            //{
            //    Console.WriteLine($"{i}) {r.ToString()}");
            //    i++;
            //}

            //---WPISYWANIE TEAM ZA POMOCA REPOSITORY---
            // var context = new AppDbContext();
            // var teamRepository = new TeamRepository(context);
            // var team = new Team("Zespół 1", 15);
            // teamRepository.CreateTeam(team);
            // var allTeams = teamRepository.GetAllTeams();
            // int i = 1;
            // foreach (var t in allTeams)
            // {
            //    Console.WriteLine($"{i}) {t.ToString()}");
            //    i++;

            // }
            Console.WriteLine("hello world");




        }
    }
}

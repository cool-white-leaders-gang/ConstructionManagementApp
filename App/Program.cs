﻿using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Enums;
using ConstructionManagementApp.App.Database;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Controllers;
using ConstructionManagementApp.App.Views;
using Task = ConstructionManagementApp.App.Models.Task;
using ConstructionManagementApp.App.Services;
using ConstructionManagementApp.App.Utilities;



namespace ConstructionManagementApp.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AppDbContext Context = new AppDbContext();

            // Initialize repositories
            BudgetRepository budgetRepository = new BudgetRepository(Context);
            EquipmentRepository equipmentRepository = new EquipmentRepository(Context);
            IssueRepository issueRepository = new IssueRepository(Context);
            LogRepository logRepository = new LogRepository(Context);
            MaterialRepository materialRepository = new MaterialRepository(Context);
            MessageRepository messageRepository = new MessageRepository(Context);
            ProgressReportRepository progressReportRepository = new ProgressReportRepository(Context);
            ProjectRepository projectRepository = new ProjectRepository(Context);
            TaskAssignmentRepository taskAssignmentRepository = new TaskAssignmentRepository(Context);
            TaskRepository taskRepository = new TaskRepository(Context);
            TeamMembersRepository teamMembersRepository = new TeamMembersRepository(Context);
            TeamRepository teamRepository = new TeamRepository(Context);
            UserRepository userRepository = new UserRepository(Context);

            // Initialize controllers
            UserController userController = new UserController(userRepository);
            BudgetController budgetController = new BudgetController(budgetRepository);
            EquipmentController equipmentController = new EquipmentController(equipmentRepository, projectRepository);
            IssueController issueController = new IssueController(issueRepository);
            LogController logController = new LogController(logRepository);
            MaterialController materialController = new MaterialController(materialRepository);
            MessageController messageController = new MessageController(messageRepository);
            ProgressReportController progressReportController = new ProgressReportController(progressReportRepository);
            ProjectController projectController = new ProjectController(projectRepository, userRepository, budgetRepository, teamRepository);
            TaskController taskController = new TaskController(taskRepository, taskAssignmentRepository);
            TeamController teamController = new TeamController(teamRepository, teamMembersRepository, userController);

            //Admin USer

            //userController.AddUser("Admin", "admin@construction.com", "123", Role.Admin);
            //userController.AddUser("Manager", "manager@construction.com", "123", Role.Manager);
            //userController.AddUser("Majster", "majster@construction.com", "123", Role.Worker);
            userController.AddUser("Klient", "klihhhhhhhent@email.com", "123", Role.Client);

            userController.AddUser("Klient", "klihhhhhhhent@email.com", "123", Role.Worker);


            //initialize views
            AuthenticationService authenticationService = new AuthenticationService(userRepository);

            string choice;
            do
            {
                Console.Clear();
                Console.WriteLine("===Panel logowania===");
                Console.WriteLine("1. Zaloguj się do systemu: ");
                Console.WriteLine("2. Wyjdź: \n");

                Console.Write("Twój wybór: ");
                choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Write("Podaj email: ");
                        string email = "admin@construction.com";//Console.ReadLine();
                        Console.Write("Podaj hasło: ");
                        string password = "123";//Console.ReadLine();
                        if (!authenticationService.Login(email, password))
                        {
                            Console.WriteLine("Nie udało się zalogować.");
                        }
                        break;
                    case "2":
                        Environment.Exit(0);
                        return;
                    default:
                        Console.WriteLine("Niepoprawny wybór. Naciśnij dowolny klawisz, aby spróbować ponownie.");
                        Console.ReadKey();
                        break;
                }
            } while (authenticationService.CurrentSession == null);



            Console.WriteLine("Naciśnij dowolny przycisk aby przejść dalej");
            Console.ReadKey();
            RBACService rbac = new RBACService();
            UserView userView = new UserView(userController, rbac, authenticationService.CurrentSession.User);
            BudgetView budgetView = new BudgetView(budgetController, rbac, authenticationService.CurrentSession.User);
            EquipmentView equipmentView = new EquipmentView(equipmentController, rbac, authenticationService.CurrentSession.User);
            IssueView issueView = new IssueView(issueController, rbac, authenticationService.CurrentSession.User);
            LogView logView = new LogView(logController, rbac, authenticationService.CurrentSession.User);
            MaterialView materialView = new MaterialView(materialController, rbac, authenticationService.CurrentSession.User);
            MessageView messageView = new MessageView(messageController, rbac, authenticationService.CurrentSession.User);
            ProgressReportView progressReportView = new ProgressReportView(progressReportController, rbac, authenticationService.CurrentSession.User);
            ProjectView projectView = new ProjectView(projectController, rbac, authenticationService.CurrentSession.User);
            TaskView taskView = new TaskView(taskController, rbac, authenticationService.CurrentSession.User);
            TeamView teamView = new TeamView(teamController, rbac, authenticationService.CurrentSession.User, userController, teamMembersRepository);

            MainMenu mainMenu = new MainMenu(authenticationService, userView, budgetView, equipmentView, issueView, logView, materialView, messageView, progressReportView, projectView, taskView, teamView);
            mainMenu.Show();




        }
    }
}

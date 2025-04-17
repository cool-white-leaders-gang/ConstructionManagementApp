using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Enums;
using ConstructionManagementApp.App.Database;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Controllers;
using ConstructionManagementApp.App.Views;
using Task = ConstructionManagementApp.App.Models.Task;
using ConstructionManagementApp.App.Services;
using ConstructionManagementApp.App.Utilities;
using ConstructionManagementApp.Services;



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
            TeamRepository teamRepository = new TeamRepository(Context);
            UserRepository userRepository = new UserRepository(Context);
            TeamMembersRepository teamMembersRepository = new TeamMembersRepository(Context, userRepository);

            //initialize services
            AuthenticationService authenticationService = new AuthenticationService(userRepository);
            LogService logService = new LogService(logRepository);
            RBACService rbac = new RBACService(projectRepository, teamRepository, teamMembersRepository);

            // Initialize controllers
            UserController userController = new UserController(userRepository, authenticationService);
            BudgetController budgetController = new BudgetController(budgetRepository, authenticationService);
            EquipmentController equipmentController = new EquipmentController(equipmentRepository, projectRepository, authenticationService);
            IssueController issueController = new IssueController(issueRepository, authenticationService, projectRepository);
            LogController logController = new LogController(logRepository);
            MaterialController materialController = new MaterialController(materialRepository, projectRepository ,authenticationService);
            MessageController messageController = new MessageController(userRepository, messageRepository, authenticationService);
            ProgressReportController progressReportController = new ProgressReportController(progressReportRepository, authenticationService, projectRepository);
            ProjectController projectController = new ProjectController(projectRepository, userRepository, budgetRepository, teamRepository, authenticationService);
            TaskController taskController = new TaskController(taskRepository, taskAssignmentRepository, authenticationService, projectRepository, rbac, userRepository);
            TeamController teamController = new TeamController(teamRepository, teamMembersRepository, userController, authenticationService);

            //Admin USer

            //userController.AddUser("Admin", "admin@construction.com", "123", Role.Admin);
            //userController.AddUser("Manager", "manager@construction.com", "123", Role.Manager);
            //userController.AddUser("Majster", "majster@construction.com", "123", Role.Worker);
            //userController.AddUser("klient", "klinet@email.com", "123", Role.Client);

            //subscribe events
            EventSubscriptionManager.SubscribeEvents(
                authenticationService,
                userController,
                budgetController,
                equipmentController,
                issueController,
                materialController,
                messageController,
                progressReportController,
                projectController,
                taskController,
                teamController,
                logService
                
            );

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
                        string email = Console.ReadLine();
                        Console.Write("Podaj hasło: ");
                        string password = Console.ReadLine();
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
            
            UserView userView = new UserView(userController, rbac, authenticationService.CurrentSession.User);
            BudgetView budgetView = new BudgetView(budgetController, rbac, authenticationService.CurrentSession.User, logController);
            EquipmentView equipmentView = new EquipmentView(equipmentController, rbac, authenticationService.CurrentSession.User, logController);
            IssueView issueView = new IssueView(issueController, rbac, authenticationService.CurrentSession.User, logController);
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

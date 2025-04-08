using ConstructionManagementApp.App.Models;
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
            BudgetController budgetController = new BudgetController(budgetRepository);
            EquipmentController equipmentController = new EquipmentController(equipmentRepository);
            IssueController issueController = new IssueController(issueRepository);
            LogController logController = new LogController(logRepository);
            MaterialController materialController = new MaterialController(materialRepository);
            MessageController messageController = new MessageController(messageRepository);
            ProgressReportController progressReportController = new ProgressReportController(progressReportRepository);
            ProjectController projectController = new ProjectController(projectRepository);
            TaskController taskController = new TaskController(taskRepository);
            TeamController teamController = new TeamController(teamRepository, teamMembersRepository);
            UserController userController = new UserController(userRepository);

            //Admin USer

            //userController.AddUser("Admin", "admin@construction.com", "123", Role.Admin);

            //initialize views
            AuthenticationService authenticationService = new AuthenticationService(userRepository);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("===Panel logowania===");
                Console.WriteLine("1. Zaloguj się do systemu: ");
                Console.WriteLine("2. Wyjdź: \n");

                Console.Write("Twój wybór: ");

                string choice = Console.ReadLine();
                bool loggedOut = true;
                while (loggedOut)
                {
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
                                return;
                            }
                            loggedOut = false;
                            break;
                        case "2":
                            Environment.Exit(0);
                            return;
                        default:
                            Console.WriteLine("Niepoprawny wybór. Naciśnij dowolny klawisz, aby spróbować ponownie.");
                            Console.ReadKey();
                            continue;
                    }
                }
                Console.WriteLine("Naciśnij dowolny przycisk aby przejść dalej");
                Console.ReadKey();
                RBACService rbac = new RBACService();
                UserView userView = new UserView(userController, rbac, authenticationService.CurrentSession.User);

                MainMenu mainMenu = new MainMenu(authenticationService.CurrentSession, userView);
                mainMenu.Show();

            }


        }
    }
}

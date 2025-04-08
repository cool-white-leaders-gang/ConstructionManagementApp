using System;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Views;
using ConstructionManagementApp.App.Services;

namespace ConstructionManagementApp.App.Views
{
    internal class MainMenu
    {
        private readonly UserView _userView;
        private readonly BudgetView _budgetView;
        private readonly EquipmentView _equipmentView;
        private readonly IssueView _issueView;
        private readonly LogView _logView;
        private readonly MaterialView _materialView;
        private readonly MessageView _messageView;
        private readonly ProgressReportView _progressReportView;
        private readonly ProjectView _projectView;
        private readonly TaskView _taskView;
        private TeamView _teamView;

        private readonly AuthenticationService _authenticationService;

        public MainMenu(AuthenticationService authenticationService, UserView userView, BudgetView budgetView, EquipmentView equipmentView, IssueView issueView, LogView logView, MaterialView materialView, MessageView messageView, ProgressReportView progressReportView, ProjectView projectView, TaskView taskView, TeamView teamView)
        {
            _authenticationService = authenticationService;
            _userView = userView;
            _budgetView = budgetView;
            _equipmentView = equipmentView;
            _issueView = issueView;
            _logView = logView;
            _materialView = materialView;
            _messageView = messageView;
            _progressReportView = progressReportView;
            _projectView = projectView;
            _taskView = taskView;
            _teamView = teamView;
        }

        public void Show()
        {
            while (_authenticationService.CurrentSession != null)
            {
                Console.Clear();
                Console.WriteLine($"Witaj!");
                Console.WriteLine("Wybierz opcję:");
                Console.WriteLine("1. Zarządzanie użytkownikami");
                Console.WriteLine("2. Zarządzanie projektami");
                Console.WriteLine("3. Zarządzanie zespołami");
                Console.WriteLine("4. Zarządzanie zadaniami");
                Console.WriteLine("5. Zarządzanie materiałami");
                Console.WriteLine("6. Zarządzanie sprzętem");
                Console.WriteLine("7. Zarządzanie budżetami");
                Console.WriteLine("8. Zarządzanie raportami postępu");
                Console.WriteLine("9. Zarządzanie zgłoszeniami problemów");
                Console.WriteLine("10. Wiadomości");
                Console.WriteLine("11. Logi");
                Console.WriteLine("0. Wyloguj się");

                Console.Write("\nTwój wybór: ");
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Niepoprawny wybór. Naciśnij dowolny klawisz, aby spróbować ponownie.");
                    Console.ReadKey();
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        _userView.ShowView();
                        break;
                    case 2:
                        _projectView.ShowView();
                        break;
                    case 3:
                        _teamView.ShowView();
                        Console.ReadKey();
                        break;
                    case 4:
                        _taskView.ShowView();
                        Console.ReadKey();
                        break;
                    case 5:
                        _materialView.ShowView();
                        Console.ReadKey();
                        break;
                    case 6:
                        _equipmentView.ShowView();
                        Console.ReadKey();
                        break;
                    case 7:
                        _budgetView.ShowView();
                        Console.ReadKey();
                        break;
                    case 8:
                        _progressReportView.ShowView();
                        Console.ReadKey();
                        break;
                    case 9:
                        _issueView.ShowView();
                        Console.ReadKey();
                        break;
                    case 10:
                        _messageView.ShowView(_authenticationService.CurrentSession.User);
                        Console.ReadKey();
                        break;
                    case 11:
                        _logView.ShowView();
                        Console.ReadKey();
                        break;
                    case 0:
                        Console.WriteLine("Wylogowywanie...");
                        _authenticationService.Logout();
                        break;
                    default:
                        Console.WriteLine("Niepoprawny wybór. Naciśnij dowolny klawisz, aby spróbować ponownie.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
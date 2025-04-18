using System;
using ConstructionManagementApp.App.Controllers;
using ConstructionManagementApp.App.Enums;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Services;

namespace ConstructionManagementApp.App.Views
{
    internal class ProjectView
    {
        private readonly ProjectController _projectController;
        private readonly RBACService _rbacService;
        private readonly User _currentUser;

        public ProjectView(ProjectController projectController, RBACService rbacService, User currentUser)
        {
            _projectController = projectController;
            _rbacService = rbacService;
            _currentUser = currentUser;
        }

        public void ShowView()
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("--- Zarządzanie projektami ---");
                Console.WriteLine("1. Wyświetl wszystkie projekty");
                Console.WriteLine("2. Utwórz nowy projekt");
                Console.WriteLine("3. Zaktualizuj projekt");
                Console.WriteLine("4. Usuń projekt");
                Console.WriteLine("5. Powrót do menu głównego");

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
                        if (HasPermission(Permission.ViewProjects)) DisplayProjectsForCurrentUser();
                        break;
                    case 2:
                        if (HasPermission(Permission.CreateProject)) CreateProject();
                        break;
                    case 3:
                        if (HasPermission(Permission.UpdateProject)) UpdateProject();
                        break;
                    case 4:
                        if (HasPermission(Permission.DeleteProject)) DeleteProject();
                        break;
                    case 5:
                        isRunning = false; // Powrót do menu głównego
                        break;
                    default:
                        Console.WriteLine("Niepoprawny wybór. Naciśnij dowolny klawisz, aby spróbować ponownie.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private bool HasPermission(Permission permission)
        {
            if (!_rbacService.HasPermission(_currentUser, permission))
            {
                Console.WriteLine("Brak uprawnień do wykonania tej operacji.");
                Console.ReadKey();
                return false;
            }
            return true;
        }

        private void DisplayProjectsForCurrentUser()
        {
            Console.Clear();
            Console.WriteLine("--- Wyświetl projekty ---");
            _projectController.DisplayProjectsForCurrentUser();
            ReturnToMenu();
        }

        private void CreateProject()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Utwórz nowy projekt ---");

                Console.Write("Podaj nazwę projektu: ");
                var name = Console.ReadLine();

                Console.Write("Podaj opis projektu: ");
                var description = Console.ReadLine();

                Console.Write("Podaj nazwę zespołu zajmującego się projektem: ");
                var teamName = Console.ReadLine();
                Console.Write("Podaj Id budżetu projektu: ");
                if (!int.TryParse(Console.ReadLine(), out var budgetId))
                {
                    Console.WriteLine("Niepoprawne ID budżetu.");
                    return;
                }
                Console.Write("Podaj nazwę klienta: ");
                var clientName = Console.ReadLine();

                _projectController.CreateProject(name, description, teamName, budgetId, clientName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            finally
            {
                ReturnToMenu();
            }
        }

        private void UpdateProject()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Zaktualizuj projekt ---");

                Console.Write("Podaj nazwę projektu do zaktualizowania: ");
                var projectName = Console.ReadLine();

                Console.Write("Podaj nową nazwę projektu: ");
                var name = Console.ReadLine();

                Console.Write("Podaj nowy opis projektu: ");
                var description = Console.ReadLine();

                Console.Write("Podaj nazwę zespołu zajmującego się projektem: ");
                var teamName = Console.ReadLine();
                Console.Write("Podaj Id budżetu projektu: ");
                if (!int.TryParse(Console.ReadLine(), out var budgetId))
                {
                    Console.WriteLine("Niepoprawne ID budżetu.");
                    return;
                }
                Console.Write("Podaj nazwę klienta: ");
                var clientName = Console.ReadLine();


                _projectController.UpdateProject(projectName, name, description, teamName, budgetId, clientName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            finally
            {
                ReturnToMenu();
            }
        }

        private void DeleteProject()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Usuń projekt ---");

                Console.Write("Podaj nazwę projektu: ");
                string projectName = Console.ReadLine();

                _projectController.DeleteProject(projectName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            finally
            {
                ReturnToMenu();
            }
        }

        private void ReturnToMenu()
        {
            Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu projektów.");
            Console.ReadKey();
        }
    }
}
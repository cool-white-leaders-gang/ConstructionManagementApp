using System;
using ConstructionManagementApp.App.Controllers;
using ConstructionManagementApp.App.Enums;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Services;

namespace ConstructionManagementApp.App.Views
{
    internal class TeamView
    {
        private readonly TeamController _teamController;
        private readonly RBACService _rbacService;
        private readonly User _currentUser;

        public TeamView(TeamController teamController, RBACService rBACService, User currentUser)
        {
            _teamController = teamController;
            _currentUser = currentUser;
            _rbacService = rBACService;
        }

        public void ShowView()
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("--- Zarządzanie zespołami ---");
                Console.WriteLine("1. Utwórz nowy zespół");
                Console.WriteLine("2. Dodaj członka do zespołu");
                Console.WriteLine("3. Usuń członka z zespołu");
                Console.WriteLine("4. Wyświetl członków zespołu");
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
                        if (HasPermission(Permission.CreateTeam)) CreateTeam();
                        break;
                    case 2:
                        if (HasPermission(Permission.UpdateTeam)) UpdateTeam();
                        break;
                    case 3:
                        if (HasPermission(Permission.DeleteTeam)) DeleteTeam();
                        break;
                    case 4:
                        if (HasPermission(Permission.AddMember)) AddMemberToTeam();
                        break;
                    case 5:
                        if (HasPermission(Permission.RemoveMember)) RemoveMemberFromTeam();
                        break;
                    case 6:
                        if (HasPermission(Permission.ViewTeam)) DisplayTeamMembers();
                        break;
                    case 7:
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

        private void CreateTeam()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Utwórz nowy zespół ---");

                Console.Write("Podaj nazwę zespołu: ");
                var name = Console.ReadLine();

                Console.Write("Id managera zespołu: ");
                if(!int.TryParse(Console.ReadLine(), out int managerId))
                {
                    Console.WriteLine("Niepoprawne ID managera.");
                    return;
                }
                _teamController.CreateTeam(name, managerId);
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

        private void AddMemberToTeam()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Dodaj członka do zespołu ---");

                Console.Write("Podaj ID zespołu: ");
                if (!int.TryParse(Console.ReadLine(), out var teamId))
                {
                    Console.WriteLine("Niepoprawne ID zespołu.");
                    return;
                }

                Console.Write("Podaj ID użytkownika: ");
                if (!int.TryParse(Console.ReadLine(), out var userId))
                {
                    Console.WriteLine("Niepoprawne ID użytkownika.");
                    return;
                }

                _teamController.AddUserToTeam(teamId, userId);
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

        private void RemoveMemberFromTeam()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Usuń członka z zespołu ---");

                Console.Write("Podaj ID zespołu: ");
                if (!int.TryParse(Console.ReadLine(), out var teamId))
                {
                    Console.WriteLine("Niepoprawne ID zespołu.");
                    return;
                }

                Console.Write("Podaj ID użytkownika: ");
                if (!int.TryParse(Console.ReadLine(), out var userId))
                {
                    Console.WriteLine("Niepoprawne ID użytkownika.");
                    return;
                }

                _teamController.RemoveUserFromTeam(teamId, userId);
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

        private void DisplayTeamMembers()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Wyświetl członków zespołu ---");

                Console.Write("Podaj ID zespołu: ");
                if (!int.TryParse(Console.ReadLine(), out var teamId))
                {
                    Console.WriteLine("Niepoprawne ID zespołu.");
                    return;
                }

                _teamController.DisplayUsersInTeam(teamId);
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

        private void DeleteTeam()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Usuń zespół ---");

                Console.Write("Podaj ID zespołu: ");
                if (!int.TryParse(Console.ReadLine(), out var teamId))
                {
                    Console.WriteLine("Niepoprawne ID.");
                    return;
                }

                _teamController.DeleteTeam(teamId);
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

        private void UpdateTeam()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Zaktualizuj zespół ---");

                Console.Write("Podaj ID zespołu: ");
                if (!int.TryParse(Console.ReadLine(), out var taskId))
                {
                    Console.WriteLine("Niepoprawne ID.");
                    return;
                }

                Console.Write("Podaj nową nazwę zespołu: ");
                var name = Console.ReadLine();

                Console.Write("Podaj ID nowego managera zespołu: ");
                if (!int.TryParse(Console.ReadLine(), out var managerId))
                {
                    Console.WriteLine("Niepoprawne ID.");
                    return;
                }

                _teamController.UpdateTeam(taskId, name, managerId);
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
            Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu zespołów.");
            Console.ReadKey();
        }
    }
}
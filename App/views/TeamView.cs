using System;
using ConstructionManagementApp.App.Controllers;
using ConstructionManagementApp.App.Enums;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Services;

namespace ConstructionManagementApp.App.Views
{
    internal class TeamView
    {
        private readonly TeamController _teamController;
        private readonly RBACService _rbacService;
        private readonly User _currentUser;
        private readonly UserController _userController;
        private readonly TeamMembersRepository _teamMembersRepository;

        public TeamView(TeamController teamController, RBACService rBACService, User currentUser , UserController userController, TeamMembersRepository teamMembersRepository)
        {
            _teamController = teamController;
            _currentUser = currentUser;
            _rbacService = rBACService;
            _userController = userController;
            _teamMembersRepository = teamMembersRepository;

        }

        public void ShowView()
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("--- Zarządzanie zespołami ---");
                Console.WriteLine("1. Utwórz nowy zespół");
                Console.WriteLine("2. Edytuj zespół");
                Console.WriteLine("3. Usuń zespół");
                Console.WriteLine("4. Dodaj nowetgo członka zaspołu");
                Console.WriteLine("5. Usuń członka z zespołu");
                Console.WriteLine("6. Wyświetl członków zespołu");
                Console.WriteLine("7. Wyświetl wszystkie zespoły");
                Console.WriteLine("8. Powrót do menu głównego");

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
                        if(HasPermission(Permission.ViewTeam)) DisplayAllTeams();
                        break;
                    case 8:
                        isRunning = false; // Powrót do menu głównego
                        break;
                    default:
                        Console.WriteLine("Niepoprawny wybór. Naciśnij dowolny klawisz, aby spróbować ponownie.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void DisplayAllTeams()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Wszystkie zespoły ---");
                Console.WriteLine();

                
                List<Team> teams = _teamController.GetAllTeams();
                foreach (var team in teams)
                {
                    Console.WriteLine($"Nazwa: {team.Name}, Manager: {team.ManagerId}");
                    _teamController.DisplayUsersInTeam(team.Id);
                }


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
                User user = _userController.GetUserById(userId);
                if (user.Role != Role.Worker)
                {
                    throw new InvalidOperationException($"Nie można dodać użytkownika o roli {user.Role}");
                }
                List<User> teamMembers = _teamMembersRepository.GetMembersOfTeam(teamId);
                if (teamMembers.Any(member => member.Id == userId))
                {
                    throw new InvalidOperationException("Użytkownik już jest dodany do zespołu");
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
                var team = _teamController.GetTeamById(teamId);


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
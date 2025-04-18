using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Delegates;
using ConstructionManagementApp.App.Enums;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Services;
using ConstructionManagementApp.Events;

namespace ConstructionManagementApp.App.Controllers
{
    internal class TeamController
    {
        private readonly TeamRepository _teamRepository;
        private readonly TeamMembersRepository _teamMembersRepository;
        private readonly UserController _userController;
        private readonly AuthenticationService _authenticationService;

        // Zdarzenia do logowania działań związanych z zespołami
        public event LogEventHandler TeamAdded;
        public event LogEventHandler TeamUpdated;
        public event LogEventHandler TeamDeleted;
        public event LogEventHandler UserAddedToTeam;
        public event LogEventHandler UserRemovedFromTeam;

        // Konstruktor kontrolera, inicjalizujący repozytoria i serwisy
        public TeamController(TeamRepository teamRepository, TeamMembersRepository teamMembersRepository, UserController userController, AuthenticationService authenticationService)
        {
            _teamRepository = teamRepository;
            _teamMembersRepository = teamMembersRepository;
            _userController = userController;
            _authenticationService = authenticationService;
        }

        // Tworzenie nowego zespołu i przypisanie menadżera
        public void CreateTeam(string name, string managerName)
        {
            try
            {
                // Pobranie użytkownika (menadżera) po nazwie
                User manager = _userController.GetUserByUsername(managerName);
                if (manager == null)
                    throw new KeyNotFoundException($"Nie znaleziono menadżera o podanej nazwie: {managerName}");
                if (manager.Role != Enums.Role.Manager)
                {
                    throw new InvalidOperationException($"Użytkownik {managerName} ma inną rolę");
                }

                // Tworzenie zespołu
                var team = new Team(name, manager.Id);
                _teamRepository.CreateTeam(team);
                Console.WriteLine("Zespół został pomyślnie utworzony.");
                // Logowanie akcji dodania zespołu
                TeamAdded?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Dodano nowy zespół o nazwie {name}"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas tworzenia zespołu: {ex.Message}");
            }
        }

        // Aktualizacja danych zespołu
        public void UpdateTeam(int teamId, string name, int managerId)
        {
            try
            {
                // Pobranie zespołu po Id
                var team = _teamRepository.GetTeamById(teamId);
                if (team == null)
                {
                    Console.WriteLine($"Zespół o Id {teamId} nie został znaleziony.");
                    return;
                }

                // Aktualizacja nazwy i menadżera zespołu
                team.Name = name;
                team.ManagerId = managerId;

                _teamRepository.UpdateTeam(team);
                Console.WriteLine("Zespół został pomyślnie zaktualizowany.");
                // Logowanie akcji aktualizacji zespołu
                TeamUpdated?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Zaktualizowano zespół o ID {teamId} i nazwie {name}"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas aktualizacji zespołu: {ex.Message}");
            }
        }

        // Usunięcie zespołu po jego ID
        public void DeleteTeam(int teamId)
        {
            try
            {
                // Usunięcie zespołu
                _teamRepository.DeleteTeamById(teamId);
                Console.WriteLine("Zespół został pomyślnie usunięty.");
                // Logowanie akcji usunięcia zespołu
                TeamDeleted?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Usunięto zespół o ID {teamId}"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas usuwania zespołu: {ex.Message}");
            }
        }

        // Dodanie użytkownika do zespołu
        public void AddUserToTeam(int teamId, string userName)
        {
            try
            {
                var currentUser = _authenticationService.GetCurrentUser();
                var team = _teamRepository.GetTeamById(teamId);
                if (team == null)
                    throw new KeyNotFoundException($"Nie znaleziono zespołu o ID {teamId}");
                if (currentUser.Role != Role.Admin && currentUser.Id != team.ManagerId)
                    throw new UnauthorizedAccessException("Tylko administrator lub menadżer zespołu może dodawać członków do zespołu.");
                _teamMembersRepository.AddMemberToTeam(teamId, userName);
                Console.WriteLine($"Użytkownik o Id {userName} został dodany do zespołu o Id {teamId}.");
                // Logowanie akcji dodania użytkownika do zespołu
                UserAddedToTeam?.Invoke(this, new LogEventArgs(currentUser.Username, $"Dodano użytkownika {userName} do zespołu o ID {teamId}"));
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas dodawania użytkownika do zespołu: {ex.Message}");
            }
        }

        // Usunięcie użytkownika z zespołu
        public void RemoveUserFromTeam(int teamId, string userName)
        {
            try
            {
                var currentUser = _authenticationService.GetCurrentUser();
                var team = _teamRepository.GetTeamById(teamId);
                if (team == null)
                    throw new KeyNotFoundException($"Nie znaleziono zespołu o ID {teamId}");
                if (currentUser.Role != Role.Admin && currentUser.Id != team.ManagerId)
                    throw new UnauthorizedAccessException("Tylko administrator lub menadżer zespołu może usuwać członków z zespołu.");
                _teamMembersRepository.RemoveMemberFromTeam(teamId, userName);
                Console.WriteLine($"Użytkownik o {userName} został usunięty z zespołu o Id {teamId}.");
                // Logowanie akcji usunięcia użytkownika z zespołu
                UserRemovedFromTeam?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Usunięto użytkownika {userName} z zespołu o ID {teamId}"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas usuwania użytkownika z zespołu: {ex.Message}");
            }
        }

        // Wyświetlenie zespołów, do których należy użytkownik
        public void DisplayTeamsForUser()
        {
            try
            {
                var currentUser = _authenticationService.CurrentSession.User;
                List<Team> teams;
                if (currentUser.Role == Enums.Role.Admin)
                {
                    // Jeśli użytkownik jest administratorem, pokazujemy wszystkie zespoły
                    teams = _teamRepository.GetAllTeams();
                }
                else
                {
                    // Filtrujemy zespoły, w których użytkownik jest menadżerem lub członkiem
                    teams = _teamRepository.GetAllTeams()
                    .Where(team => team.ManagerId == currentUser.Id ||
                                   _teamMembersRepository.GetMembersOfTeam(team.Id)
                                       .Any(member => member.Id == currentUser.Id))
                    .ToList();
                }

                if (teams.Count == 0)
                {
                    Console.WriteLine("Nie należysz do żadnego zespołu.");
                }
                else
                {
                    foreach (var team in teams)
                    {
                        Console.WriteLine($"Nazwa: {team.Name}, Manager: {team.ManagerId}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas pobierania zespołów: {ex.Message}");
            }
        }

        // Wyświetlenie szczegółów zespołu na podstawie jego ID
        public void DisplayTeamById(int teamId)
        {
            try
            {
                var currentUser = _authenticationService.CurrentSession.User;
                var team = _teamRepository.GetTeamById(teamId);

                if (team == null)
                {
                    Console.WriteLine($"Zespół o Id {teamId} nie został znaleziony.");
                    return;
                }

                // Sprawdzanie, czy użytkownik ma dostęp do zespołu
                if (currentUser.Role != Role.Admin &&
                    team.ManagerId != currentUser.Id &&
                    !_teamMembersRepository.GetMembersOfTeam(teamId).Any(member => member.Id == currentUser.Id))
                {
                    Console.WriteLine("Nie masz dostępu do tego zespołu.");
                    return;
                }

                // Wyświetlanie szczegółów zespołu
                Console.WriteLine($"Nazwa zespołu: {team.Name}");
                Console.WriteLine($"Manager Id: {team.ManagerId}");
                Console.WriteLine("Członkowie zespołu:");

                var users = _teamMembersRepository.GetMembersOfTeam(teamId);
                if (users.Count == 0)
                {
                    Console.WriteLine("Brak członków w tym zespole.");
                }
                else
                {
                    foreach (var user in users)
                    {
                        Console.WriteLine($"- {user.ToString()}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas pobierania szczegółów zespołu: {ex.Message}");
            }
        }

        // Wyświetlenie użytkowników w danym zespole
        public void DisplayUsersInTeam(int teamId)
        {
            try
            {
                var currentUser = _authenticationService.GetCurrentUser();
                var team = _teamRepository.GetTeamById(teamId);

                if (team == null)
                {
                    Console.WriteLine($"Zespół o Id {teamId} nie został znaleziony.");
                    return;
                }

                // Sprawdzenie, czy użytkownik ma uprawnienia do wyświetlania członków zespołu
                if (currentUser.Role != Role.Admin && team.ManagerId != currentUser.Id && !_teamMembersRepository.GetMembersOfTeam(teamId).Any(member => member.Id == currentUser.Id))
                {
                    throw new UnauthorizedAccessException("Nie masz uprawnień do wyświetlenia członków tego zespołu.");
                }

                var users = _teamMembersRepository.GetMembersOfTeam(teamId);
                if (users.Count == 0)
                {
                    Console.WriteLine($"Brak użytkowników w zespole o Id {teamId}.");
                    return;
                }

                foreach (var user in users)
                {
                    Console.WriteLine(user.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas pobierania użytkowników w zespole: {ex.Message}");
            }
        }
    }
}

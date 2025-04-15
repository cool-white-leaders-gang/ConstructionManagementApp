using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Repositories;

namespace ConstructionManagementApp.App.Controllers
{
    internal class TeamController
    {
        private readonly TeamRepository _teamRepository;
        private readonly TeamMembersRepository _teamMembersRepository;
        private readonly UserController _userController;

        // Konstruktor kontrolera
        public TeamController(TeamRepository teamRepository, TeamMembersRepository teamMembersRepository, UserController userController)
        {
            _teamRepository = teamRepository;
            _teamMembersRepository = teamMembersRepository;
            _userController = userController;
        }

        // Dodaj nowy zespół
        public void CreateTeam(string name, string managerName)
        {
            try
            {
                User manager = _userController.GetUserByUsername(managerName);
                if (manager == null)
                    throw new KeyNotFoundException($"Nie znaleziono menadżera o podanej nazwie: {managerName}");
                if (manager.Role != Enums.Role.Manager)
                {
                    throw new InvalidOperationException($"Użytkownik {managerName} ma inną rolę");
                }

                var team = new Team(name, manager.Id);
                _teamRepository.CreateTeam(team);
                Console.WriteLine("Zespół został pomyślnie utworzony.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas tworzenia zespołu: {ex.Message}");
            }
        }

        // Zaktualizuj zespół
        public void UpdateTeam(int teamId, string name, int managerId)
        {
            try
            {
                var team = _teamRepository.GetTeamById(teamId);
                if (team == null)
                {
                    Console.WriteLine($"Zespół o Id {teamId} nie został znaleziony.");
                    return;
                }

                team.Name = name;
                team.ManagerId = managerId;

                _teamRepository.UpdateTeam(team);
                Console.WriteLine("Zespół został pomyślnie zaktualizowany.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas aktualizacji zespołu: {ex.Message}");
            }
        }

        // Usuń zespół
        public void DeleteTeam(int teamId)
        {
            try
            {
                _teamRepository.DeleteTeamById(teamId);
                Console.WriteLine("Zespół został pomyślnie usunięty.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas usuwania zespołu: {ex.Message}");
            }
        }

        // Dodaj użytkownika do zespołu
        public void AddUserToTeam(int teamId, int userId)
        {
            try
            {
                _teamMembersRepository.AddMemberToTeam(teamId, userId);
                Console.WriteLine($"Użytkownik o Id {userId} został dodany do zespołu o Id {teamId}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas dodawania użytkownika do zespołu: {ex.Message}");
            }
        }

        // Usuń użytkownika z zespołu
        public void RemoveUserFromTeam(int teamId, int userId)
        {
            try
            {
                _teamMembersRepository.RemoveMemberFromTeam(teamId, userId);
                Console.WriteLine($"Użytkownik o Id {userId} został usunięty z zespołu o Id {teamId}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas usuwania użytkownika z zespołu: {ex.Message}");
            }
        }

        // Wyświetl wszystkie zespoły
        public void DisplayAllTeams()
        {
            try
            {
                var teams = _teamRepository.GetAllTeams();
                if (teams.Count == 0)
                {
                    Console.WriteLine("Brak zespołów do wyświetlenia.");
                    return;
                }

                foreach (var team in teams)
                {
                    Console.WriteLine(team.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas pobierania zespołów: {ex.Message}");
            }
        }

        // Wyświetl szczegóły zespołu po Id
        public void DisplayTeamById(int teamId)
        {
            try
            {
                var team = _teamRepository.GetTeamById(teamId);
                if (team == null)
                {
                    Console.WriteLine($"Zespół o Id {teamId} nie został znaleziony.");
                    return;
                }

                Console.WriteLine(team.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas pobierania szczegółów zespołu: {ex.Message}");
            }
        }

        // Wyświetl użytkowników w zespole
        public void DisplayUsersInTeam(int teamId)
        {
            try
            {
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

        public Team GetTeamById(int teamId)
        {
            try
            {
                var team = _teamRepository.GetTeamById(teamId);
                if (team == null)
                    throw new KeyNotFoundException($"Zespół o Id {teamId} nie został znaleziony.");


                return team;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas pobierania zespołu: {ex.Message}");
                throw;
            }
        }

        public List<Team> GetAllTeams()
        {
            try
            {
                return _teamRepository.GetAllTeams();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas pobierania zespołów: {ex.Message}");
                throw;
            }
        }
    }
}
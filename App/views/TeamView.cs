using System;
using ConstructionManagementApp.App.Controllers;

namespace ConstructionManagementApp.App.Views
{
    internal class TeamView
    {
        private readonly TeamController _teamController;

        public TeamView(TeamController teamController)
        {
            _teamController = teamController;
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
                        CreateTeam();
                        break;
                    case 2:
                        AddMemberToTeam();
                        break;
                    case 3:
                        RemoveMemberFromTeam();
                        break;
                    case 4:
                        DisplayTeamMembers();
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

        private void CreateTeam()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Utwórz nowy zespół ---");

                Console.Write("Podaj nazwę zespołu: ");
                var name = Console.ReadLine();

                Console.Write("Podaj opis zespołu: ");
                var description = Console.ReadLine();

                _teamController.CreateTeam(name, description);
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

                _teamController.AddMemberToTeam(teamId, userId);
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

                _teamController.RemoveMemberFromTeam(teamId, userId);
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

                _teamController.DisplayTeamMembers(teamId);
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
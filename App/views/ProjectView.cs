using System;
using ConstructionManagementApp.App.Controllers;

namespace ConstructionManagementApp.App.Views
{
    internal class ProjectView
    {
        private readonly ProjectController _projectController;

        public ProjectView(ProjectController projectController)
        {
            _projectController = projectController;
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
                        DisplayAllProjects();
                        break;
                    case 2:
                        CreateProject();
                        break;
                    case 3:
                        UpdateProject();
                        break;
                    case 4:
                        DeleteProject();
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

        private void DisplayAllProjects()
        {
            Console.Clear();
            _projectController.DisplayAllProjects();
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

                Console.Write("Podaj datę rozpoczęcia projektu (yyyy-MM-dd): ");
                if (!DateTime.TryParse(Console.ReadLine(), out var startDate))
                {
                    Console.WriteLine("Niepoprawny format daty.");
                    return;
                }

                Console.Write("Podaj datę zakończenia projektu (yyyy-MM-dd): ");
                if (!DateTime.TryParse(Console.ReadLine(), out var endDate))
                {
                    Console.WriteLine("Niepoprawny format daty.");
                    return;
                }

                _projectController.CreateProject(name, description, startDate, endDate);
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

                Console.Write("Podaj ID projektu: ");
                if (!int.TryParse(Console.ReadLine(), out var projectId))
                {
                    Console.WriteLine("Niepoprawne ID.");
                    return;
                }

                Console.Write("Podaj nową nazwę projektu: ");
                var name = Console.ReadLine();

                Console.Write("Podaj nowy opis projektu: ");
                var description = Console.ReadLine();

                Console.Write("Podaj nową datę rozpoczęcia projektu (yyyy-MM-dd): ");
                if (!DateTime.TryParse(Console.ReadLine(), out var startDate))
                {
                    Console.WriteLine("Niepoprawny format daty.");
                    return;
                }

                Console.Write("Podaj nową datę zakończenia projektu (yyyy-MM-dd): ");
                if (!DateTime.TryParse(Console.ReadLine(), out var endDate))
                {
                    Console.WriteLine("Niepoprawny format daty.");
                    return;
                }

                _projectController.UpdateProject(projectId, name, description, startDate, endDate);
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

                Console.Write("Podaj ID projektu: ");
                if (!int.TryParse(Console.ReadLine(), out var projectId))
                {
                    Console.WriteLine("Niepoprawne ID.");
                    return;
                }

                _projectController.DeleteProject(projectId);
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
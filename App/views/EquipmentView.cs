using System;
using ConstructionManagementApp.App.Controllers;
using ConstructionManagementApp.App.Services;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Enums;

namespace ConstructionManagementApp.App.Views
{
    internal class EquipmentView
    {
        private readonly EquipmentController _equipmentController;
        private readonly RBACService _rbacService;
        private readonly User _currentUser;
        private readonly LogController _logController;

        public EquipmentView(EquipmentController equipmentController, RBACService rbacService, User currentUser, LogController logController)
        {
            _equipmentController = equipmentController;
            _rbacService = rbacService;
            _currentUser = currentUser;
            _logController = logController;
        }

        public void ShowView()
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("--- Zarządzanie sprzętem ---");
                Console.WriteLine("1. Wyświetl wszystkie sprzęty");
                Console.WriteLine("2. Dodaj nowy sprzęt");
                Console.WriteLine("3. Zaktualizuj sprzęt");
                Console.WriteLine("4. Usuń sprzęt");
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
                        if (HasPermission(Permission.ViewEquipment)) DisplayAllEquipments();
                        break;
                    case 2:
                        if (HasPermission(Permission.CreateEquipment)) AddEquipment();
                        break;
                    case 3:
                        if (HasPermission(Permission.UpdateEquipment)) UpdateEquipment();
                        break;
                    case 4:
                        if (HasPermission(Permission.DeleteEquipment)) DeleteEquipment();
                        break;
                    case 5:
                        isRunning = false; // Powrót do menu głównego
                        Console.WriteLine("Naciśnij dowolny klawisz aby przejść dalej");
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

        private void DisplayAllEquipments()
        {
            Console.Clear();
            _equipmentController.DisplayAllEquipments();
            ReturnToMenu();
        }

        private void AddEquipment()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Dodaj nowy sprzęt ---");

                Console.Write("Podaj nazwę sprzętu: ");
                var name = Console.ReadLine();

                Console.WriteLine("Wybierz status sprzętu: 1. Dostępny, 2. Niedostępny");
                var statusChoice = Console.ReadLine();
                EquipmentStatus status = statusChoice switch
                {
                    "1" => EquipmentStatus.Available,
                    "2" => EquipmentStatus.Unavailable,
                    _ => throw new ArgumentException("Nieprawidłowy status sprzętu.")
                };

                Console.Write("Podaj nazwę projektu: ");
                string projectName = Console.ReadLine();
                _equipmentController.AddEquipment(name, status, projectName);
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

        private void UpdateEquipment()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Zaktualizuj sprzęt ---");

                Console.Write("Podaj Id sprzętu do zaktualizowania: ");
                int equipmentId;
                while (!int.TryParse(Console.ReadLine(), out equipmentId))
                {
                    Console.WriteLine("Niepoprawny wybór. Naciśnij dowolny klawisz, aby spróbować ponownie.");
                    Console.ReadKey();
                }
                Console.Write("Podaj nową nazwę sprzętu: ");
                var name = Console.ReadLine();

                Console.WriteLine("Wybierz nowy status sprzętu: 1. Dostępny, 2. Niedostępny");
                var statusChoice = Console.ReadLine();
                EquipmentStatus status = statusChoice switch
                {
                    "1" => EquipmentStatus.Available,
                    "2" => EquipmentStatus.Unavailable,
                    _ => throw new ArgumentException("Nieprawidłowy status sprzętu.")
                };

                Console.Write("Podaj nazwę projektu: ");
                int projectId;
                while(!int.TryParse(Console.ReadLine(), out projectId))
                {
                    Console.WriteLine("Niepoprawny wybór. Naciśnij dowolny klawisz, aby spróbować ponownie.");
                    Console.ReadKey();
                }

                _equipmentController.UpdateEquipment(equipmentId, name, status, projectId);
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

        private void DeleteEquipment()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Usuń sprzęt ---");

                Console.Write("Podaj ID sprzętu: ");
                int equipmentId;
                while(!int.TryParse(Console.ReadLine(), out equipmentId))
                {
                    Console.WriteLine("Niepoprawny wybór. Naciśnij dowolny klawisz, aby spróbować ponownie.");
                    Console.ReadKey();
                }

                _equipmentController.DeleteEquipment(equipmentId);
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
            Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu sprzętu.");
            Console.ReadKey();
        }
    }
}
using System;
using ConstructionManagementApp.App.Controllers;

namespace ConstructionManagementApp.App.Views
{
    internal class EquipmentView
    {
        private readonly EquipmentController _equipmentController;

        public EquipmentView(EquipmentController equipmentController)
        {
            _equipmentController = equipmentController;
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
                        DisplayAllEquipments();
                        break;
                    case 2:
                        AddEquipment();
                        break;
                    case 3:
                        UpdateEquipment();
                        break;
                    case 4:
                        DeleteEquipment();
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

                Console.Write("Podaj ilość sprzętu: ");
                if (!int.TryParse(Console.ReadLine(), out var quantity))
                {
                    Console.WriteLine("Niepoprawna ilość.");
                    return;
                }

                _equipmentController.AddEquipment(name, quantity);
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

                Console.Write("Podaj ID sprzętu: ");
                if (!int.TryParse(Console.ReadLine(), out var equipmentId))
                {
                    Console.WriteLine("Niepoprawne ID.");
                    return;
                }

                Console.Write("Podaj nową nazwę sprzętu: ");
                var name = Console.ReadLine();

                Console.Write("Podaj nową ilość sprzętu: ");
                if (!int.TryParse(Console.ReadLine(), out var quantity))
                {
                    Console.WriteLine("Niepoprawna ilość.");
                    return;
                }

                _equipmentController.UpdateEquipment(equipmentId, name, quantity);
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
                if (!int.TryParse(Console.ReadLine(), out var equipmentId))
                {
                    Console.WriteLine("Niepoprawne ID.");
                    return;
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
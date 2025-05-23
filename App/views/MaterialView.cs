using System;
using ConstructionManagementApp.App.Controllers;
using ConstructionManagementApp.App.Enums;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Services;

namespace ConstructionManagementApp.App.Views
{
    internal class MaterialView
    {
        private readonly MaterialController _materialController;
        private readonly RBACService _rbacService;
        private readonly User _currentUser;

        public MaterialView(MaterialController materialController, RBACService rbacService, User currentUser)
        {
            _materialController = materialController;
            _rbacService = rbacService;
            _currentUser = currentUser;
        }

        public void ShowView()
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("--- Zarządzanie materiałami ---");
                Console.WriteLine("1. Wyświetl materiały");
                Console.WriteLine("2. Dodaj nowy materiał");
                Console.WriteLine("3. Zaktualizuj materiał");
                Console.WriteLine("4. Usuń materiał");
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
                        if (HasPermission(Permission.ViewMaterials)) DisplayMaterialsForCurrentUser();
                        break;
                    case 2:
                        if (HasPermission(Permission.CreateMaterial)) AddMaterial();
                        break;
                    case 3:
                        if (HasPermission(Permission.UpdateMaterial)) UpdateMaterial();
                        break;
                    case 4:
                        if (HasPermission(Permission.DeleteMaterial)) DeleteMaterial();
                        break;
                    case 5:
                        Console.WriteLine("Naciśnij dowolny przycisk, aby kontynuować");
                        
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

        private void DisplayMaterialsForCurrentUser()
        {
            Console.Clear();
            Console.WriteLine("--- Wyświetl materiały przypisane do Twoich projektów ---");
            _materialController.DisplayMaterialsForUser();
            ReturnToMenu();
        }

        private void AddMaterial()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Dodaj nowy materiał ---");

                Console.Write("Podaj nazwę materiału: ");
                var name = Console.ReadLine();

                Console.Write("Podaj ilość materiału: ");
                if (!int.TryParse(Console.ReadLine(), out var quantity))
                {
                    Console.WriteLine("Niepoprawna ilość.");
                    return;
                }

                Console.Write("Podaj jednostkę materiału: ");
                var unit = Console.ReadLine();

                Console.Write("Podaj nazwę projektu do którego materiał zostanie przypisany: ");
                string projectName = Console.ReadLine();

                _materialController.AddMaterial(name, quantity, unit, projectName);
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

        private void UpdateMaterial()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Zaktualizuj materiał ---");

                Console.Write("Podaj ID materiału: ");
                if (!int.TryParse(Console.ReadLine(), out var materialId))
                {
                    Console.WriteLine("Niepoprawne ID.");
                    return;
                }

                Console.Write("Podaj nową nazwę materiału: ");
                var name = Console.ReadLine();

                Console.Write("Podaj nową ilość materiału: ");
                if (!int.TryParse(Console.ReadLine(), out var quantity))
                {
                    Console.WriteLine("Niepoprawna ilość.");
                    return;
                }

                _materialController.UpdateMaterial(materialId, name, quantity);
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

        private void DeleteMaterial()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Usuń materiał ---");

                Console.Write("Podaj ID materiału: ");
                if (!int.TryParse(Console.ReadLine(), out var materialId))
                {
                    Console.WriteLine("Niepoprawne ID.");
                    return;
                }

                _materialController.DeleteMaterial(materialId);
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
            Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu materiałów.");
            Console.ReadKey();
        }
    }
}
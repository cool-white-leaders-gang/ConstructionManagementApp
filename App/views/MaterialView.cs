using System;
using ConstructionManagementApp.App.Controllers;

namespace ConstructionManagementApp.App.Views
{
    internal class MaterialView
    {
        private readonly MaterialController _materialController;

        public MaterialView(MaterialController materialController)
        {
            _materialController = materialController;
        }

        public void ShowView()
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("--- Zarządzanie materiałami ---");
                Console.WriteLine("1. Wyświetl wszystkie materiały");
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
                        DisplayAllMaterials();
                        break;
                    case 2:
                        AddMaterial();
                        break;
                    case 3:
                        UpdateMaterial();
                        break;
                    case 4:
                        DeleteMaterial();
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

        private void DisplayAllMaterials()
        {
            Console.Clear();
            _materialController.DisplayAllMaterials();
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

                Console.Write("Podaj ID projektu do którego materiał zostanie przypisany: ");
                if(!int.TryParse(Console.ReadLine(), out var projectId))
                {
                    Console.WriteLine("Niepoprawne ID projektu.");
                    return;
                }
                _materialController.AddMaterial(name, quantity, unit, projectId);
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
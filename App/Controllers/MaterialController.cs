using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Repositories;

namespace ConstructionManagementApp.App.Controllers
{
    internal class MaterialController
    {
        private readonly MaterialRepository _materialRepository;

        // Konstruktor kontrolera
        public MaterialController(MaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }

        // Dodaj nowy materiał
        public void CreateMaterial(string name, int quantity, string unit, int projectId)
        {
            try
            {
                var material = new Material(name, quantity, unit, projectId);
                _materialRepository.CreateMaterial(material);
                Console.WriteLine("Materiał został pomyślnie dodany.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas dodawania materiału: {ex.Message}");
            }
        }

        // Zaktualizuj materiał
        public void UpdateMaterial(int materialId, string name, int quantity, string unit, int projectId)
        {
            try
            {
                var material = _materialRepository.GetMaterialById(materialId);
                if (material == null)
                {
                    Console.WriteLine($"Materiał o Id {materialId} nie został znaleziony.");
                    return;
                }

                material.Name = name;
                material.Quantity = quantity;
                material.Unit = unit;
                material.ProjectId = projectId;

                _materialRepository.UpdateMaterial(material);
                Console.WriteLine("Materiał został pomyślnie zaktualizowany.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas aktualizacji materiału: {ex.Message}");
            }
        }

        // Usuń materiał
        public void DeleteMaterial(int materialId)
        {
            try
            {
                _materialRepository.DeleteMaterial(materialId);
                Console.WriteLine("Materiał został pomyślnie usunięty.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas usuwania materiału: {ex.Message}");
            }
        }

        // Wyświetl wszystkie materiały
        public void DisplayAllMaterials()
        {
            try
            {
                var materials = _materialRepository.GetAllMaterials();
                if (materials.Count == 0)
                {
                    Console.WriteLine("Brak materiałów do wyświetlenia.");
                    return;
                }

                foreach (var material in materials)
                {
                    Console.WriteLine(material.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas pobierania materiałów: {ex.Message}");
            }
        }

        // Wyświetl materiał po Id
        public void DisplayMaterialById(int materialId)
        {
            try
            {
                var material = _materialRepository.GetMaterialById(materialId);
                if (material == null)
                {
                    Console.WriteLine($"Materiał o Id {materialId} nie został znaleziony.");
                    return;
                }

                Console.WriteLine(material.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas pobierania materiału: {ex.Message}");
            }
        }
    }
}
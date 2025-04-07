using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Controllers
{
    internal class MaterialController
    {
        private readonly MaterialRepository _materialRepository;

        public MaterialController(MaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public void AddMaterial(string name, int quantity)
        {
            try
            {
                if (quantity <= 0)
                    throw new ArgumentException("Ilość materiału musi być większa od zera.");

                var material = new Material(name, quantity);
                _materialRepository.AddMaterial(material);
                Console.WriteLine("Materiał został pomyślnie dodany.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nieoczekiwany błąd: {ex.Message}");
            }
        }

        public void UpdateMaterial(int materialId, string name, int quantity)
        {
            try
            {
                if (quantity <= 0)
                    throw new ArgumentException("Ilość materiału musi być większa od zera.");

                var material = _materialRepository.GetMaterialById(materialId);
                if (material == null)
                    throw new KeyNotFoundException($"Nie znaleziono materiału o ID {materialId}.");

                material.Name = name;
                material.Quantity = quantity;

                _materialRepository.UpdateMaterial(material);
                Console.WriteLine("Materiał został pomyślnie zaktualizowany.");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nieoczekiwany błąd: {ex.Message}");
            }
        }

        public void DeleteMaterial(int materialId)
        {
            try
            {
                _materialRepository.DeleteMaterialById(materialId);
                Console.WriteLine("Materiał został pomyślnie usunięty.");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nieoczekiwany błąd: {ex.Message}");
            }
        }

        public void DisplayAllMaterials()
        {
            try
            {
                var materials = _materialRepository.GetAllMaterials();
                if (materials.Count == 0)
                {
                    Console.WriteLine("Brak materiałów w systemie.");
                    return;
                }

                Console.WriteLine("--- Lista materiałów ---");
                foreach (var material in materials)
                {
                    Console.WriteLine($"ID: {material.Id}, Nazwa: {material.Name}, Ilość: {material.Quantity}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Services;
using ConstructionManagementApp.Events;

namespace ConstructionManagementApp.App.Controllers
{
    internal class MaterialController
    {
        private readonly MaterialRepository _materialRepository;
        private readonly AuthenticationService _authenticationService;
        public event LogEventHandler MaterialAdded;
        public event LogEventHandler MaterialDeleted;
        public event LogEventHandler MaterialUpdated;

        public MaterialController(MaterialRepository materialRepository, AuthenticationService authenticationService)
        {
            _materialRepository = materialRepository;
            _authenticationService = authenticationService;
        }

        public void AddMaterial(string name, int quantity, string unit, int projectId)
        {
            try
            {
                var material = new Material(name, quantity, unit, projectId);
                _materialRepository.AddMaterial(material);
                Console.WriteLine("Materiał został pomyślnie dodany.");
                MaterialAdded?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Dodano nowy materiał o ID {material.Id} i nazwie {name}"));
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

                var material = _materialRepository.GetMaterialById(materialId);
                if (material == null)
                    throw new KeyNotFoundException($"Nie znaleziono materiału o ID {materialId}.");

                material.Name = name;
                material.Quantity = quantity;

                _materialRepository.UpdateMaterial(material);
                Console.WriteLine("Materiał został pomyślnie zaktualizowany.");
                MaterialUpdated?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Zaktualizowano materiał o ID {materialId} i nazwie {name}"));
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
                MaterialDeleted?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Usunięto materiał o ID {materialId}"));
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
                if (materials.Count == 0 || materials == null)
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
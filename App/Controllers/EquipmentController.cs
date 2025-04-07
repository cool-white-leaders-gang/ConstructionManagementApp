using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Enums;

namespace ConstructionManagementApp.App.Controllers
{
    internal class EquipmentController
    {
        private readonly EquipmentRepository _equipmentRepository;

        public EquipmentController(EquipmentRepository equipmentRepository)
        {
            _equipmentRepository = equipmentRepository;
        }

        public void AddEquipment(string name, int quantity)
        {
            try
            {
                if (quantity <= 0)
                    throw new ArgumentException("Ilość sprzętu musi być większa od zera.");

                var equipment = new Equipment(name, quantity);
                _equipmentRepository.AddEquipment(equipment);
                Console.WriteLine("Sprzęt został pomyślnie dodany.");
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

        public void UpdateEquipment(int equipmentId, string name, int quantity)
        {
            try
            {
                if (quantity <= 0)
                    throw new ArgumentException("Ilość sprzętu musi być większa od zera.");

                var equipment = _equipmentRepository.GetEquipmentById(equipmentId);
                if (equipment == null)
                    throw new KeyNotFoundException($"Nie znaleziono sprzętu o ID {equipmentId}.");

                equipment.Name = name;
                equipment.Quantity = quantity;

                _equipmentRepository.UpdateEquipment(equipment);
                Console.WriteLine("Sprzęt został pomyślnie zaktualizowany.");
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

        public void DeleteEquipment(int equipmentId)
        {
            try
            {
                _equipmentRepository.DeleteEquipmentById(equipmentId);
                Console.WriteLine("Sprzęt został pomyślnie usunięty.");
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

        public void DisplayAllEquipments()
        {
            try
            {
                var equipments = _equipmentRepository.GetAllEquipments();
                if (equipments.Count == 0)
                {
                    Console.WriteLine("Brak sprzętu w systemie.");
                    return;
                }

                Console.WriteLine("--- Lista sprzętu ---");
                foreach (var equipment in equipments)
                {
                    Console.WriteLine($"ID: {equipment.Id}, Nazwa: {equipment.Name}, Ilość: {equipment.Quantity}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }
    }
}
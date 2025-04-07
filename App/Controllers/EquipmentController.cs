using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Models;
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

        public void AddEquipment(string name, EquipmentStatus status, int projectId)
        {
            try
            {
                var equipment = new Equipment(name, status, projectId);
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

        public void UpdateEquipment(int equipmentId, string name, EquipmentStatus status, int projectId)
        {
            try
            {
                var equipment = _equipmentRepository.GetEquipmentById(equipmentId);
                if (equipment == null)
                    throw new KeyNotFoundException($"Nie znaleziono sprzętu o ID {equipmentId}.");

                equipment.Name = name;
                equipment.Status = status;
                equipment.ProjectId = projectId;

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
                    Console.WriteLine($"ID: {equipment.Id}, Nazwa: {equipment.Name}, Status: {equipment.Status}, Projekt ID: {equipment.ProjectId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }
    }
}
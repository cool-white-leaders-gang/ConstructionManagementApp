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

        // Konstruktor kontrolera
        public EquipmentController(EquipmentRepository equipmentRepository)
        {
            _equipmentRepository = equipmentRepository;
        }

        // Dodaj nowy sprzęt
        public void CreateEquipment(string name, EquipmentStatus status, int projectId)
        {
            try
            {
                var equipment = new Equipment(name, status, projectId);
                _equipmentRepository.CreateEquipment(equipment);
                Console.WriteLine("Sprzęt został pomyślnie dodany.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas dodawania sprzętu: {ex.Message}");
            }
        }

        // Zaktualizuj sprzęt
        public void UpdateEquipment(int equipmentId, string name, EquipmentStatus status, int projectId)
        {
            try
            {
                var equipment = _equipmentRepository.GetEquipmentById(equipmentId);
                if (equipment == null)
                {
                    Console.WriteLine($"Sprzęt o Id {equipmentId} nie został znaleziony.");
                    return;
                }

                equipment.Name = name;
                equipment.Status = status;
                equipment.ProjectId = projectId;

                _equipmentRepository.UpdateEquipment(equipment);
                Console.WriteLine("Sprzęt został pomyślnie zaktualizowany.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas aktualizacji sprzętu: {ex.Message}");
            }
        }

        // Usuń sprzęt
        public void DeleteEquipment(int equipmentId)
        {
            try
            {
                _equipmentRepository.DeleteEquipment(equipmentId);
                Console.WriteLine("Sprzęt został pomyślnie usunięty.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas usuwania sprzętu: {ex.Message}");
            }
        }

        // Pobierz wszystkie sprzęty
        public void DisplayAllEquipment()
        {
            try
            {
                var equipmentList = _equipmentRepository.GetAllEquipment();
                if (equipmentList.Count == 0)
                {
                    Console.WriteLine("Brak sprzętu do wyświetlenia.");
                    return;
                }

                foreach (var equipment in equipmentList)
                {
                    Console.WriteLine(equipment.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas pobierania sprzętu: {ex.Message}");
            }
        }

        // Pobierz sprzęt po Id
        public void DisplayEquipmentById(int equipmentId)
        {
            try
            {
                var equipment = _equipmentRepository.GetEquipmentById(equipmentId);
                if (equipment == null)
                {
                    Console.WriteLine($"Sprzęt o Id {equipmentId} nie został znaleziony.");
                    return;
                }

                Console.WriteLine(equipment.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas pobierania sprzętu: {ex.Message}");
            }
        }
    }
}
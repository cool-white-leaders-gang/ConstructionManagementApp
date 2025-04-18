using System;
using System.Collections.Generic;
using System.Linq;
using ConstructionManagementApp.App.Database;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Repositories
{
    internal class EquipmentRepository
    {
        private readonly AppDbContext _context;

        // Konstruktor repozytorium, inicjalizuje kontekst bazy danych
        public EquipmentRepository(AppDbContext context)
        {
            _context = context;
        }

        // Dodanie nowego sprzętu do bazy danych
        public void AddEquipment(Equipment equipment)
        {
            if (equipment == null)
                throw new ArgumentNullException(nameof(equipment), "Sprzęt nie może być null.");

            // Dodanie sprzętu i zapisanie zmian w bazie
            _context.Equipment.Add(equipment);
            _context.SaveChanges();
        }

        // Aktualizacja istniejącego sprzętu
        public void UpdateEquipment(Equipment equipment)
        {
            // Sprawdzenie, czy sprzęt o podanym Id istnieje
            var existingEquipment = GetEquipmentById(equipment.Id);
            if (existingEquipment == null)
                throw new KeyNotFoundException("Nie znaleziono sprzętu o podanym Id.");

            // Aktualizacja wartości sprzętu
            existingEquipment.Name = equipment.Name;
            existingEquipment.Status = equipment.Status;
            existingEquipment.ProjectId = equipment.ProjectId;

            // Zaktualizowanie sprzętu w bazie danych
            _context.Equipment.Update(existingEquipment);
            _context.SaveChanges();
        }

        // Usunięcie sprzętu na podstawie jego nazwy
        public void DeleteEquipmentByName(string name)
        {
            // Pobranie sprzętu na podstawie nazwy
            var equipment = GetEquipmentByName(name);
            if (equipment == null)
                throw new KeyNotFoundException("Nie znaleziono sprzętu o podanej nazwie.");

            // Usunięcie sprzętu z bazy
            _context.Equipment.Remove(equipment);
            _context.SaveChanges();
        }

        // Usunięcie sprzętu na podstawie jego ID
        public void DeleteEquipmentById(int id)
        {
            // Pobranie sprzętu na podstawie ID
            var equipment = GetEquipmentById(id);
            if (equipment == null)
                throw new KeyNotFoundException("Nie znaleziono sprzętu o podanym Id.");

            // Usunięcie sprzętu z bazy
            _context.Equipment.Remove(equipment);
            _context.SaveChanges();
        }

        // Pobranie sprzętu na podstawie ID
        public Equipment GetEquipmentById(int id)
        {
            // Zwrócenie sprzętu lub null, jeśli nie znaleziono
            return _context.Equipment.FirstOrDefault(e => e.Id == id);
        }

        // Pobranie sprzętu na podstawie nazwy
        public Equipment GetEquipmentByName(string name)
        {
            // Zwrócenie sprzętu lub null, jeśli nie znaleziono
            return _context.Equipment.FirstOrDefault(e => e.Name == name);
        }

        // Pobranie wszystkich sprzętów z bazy danych
        public List<Equipment> GetAllEquipments()
        {
            // Zwrócenie listy wszystkich sprzętów
            return _context.Equipment.ToList();
        }
    }
}

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

        public EquipmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddEquipment(Equipment equipment)
        {
            if (equipment == null)
                throw new ArgumentNullException(nameof(equipment), "Sprzęt nie może być null.");

            _context.Equipment.Add(equipment);
            _context.SaveChanges();
        }

        public void UpdateEquipment(Equipment equipment)
        {
            var existingEquipment = GetEquipmentById(equipment.Id);
            if (existingEquipment == null)
                throw new KeyNotFoundException("Nie znaleziono sprzętu o podanym Id.");

            existingEquipment.Name = equipment.Name;
            existingEquipment.Status = equipment.Status;
            existingEquipment.ProjectId = equipment.ProjectId;

            _context.Equipment.Update(existingEquipment);
            _context.SaveChanges();
        }

        public void DeleteEquipmentByName(string name)
        {
            var equipment = GetEquipmentByName(name);
            if (equipment == null)
                throw new KeyNotFoundException("Nie znaleziono sprzętu o podanej nazwie.");

            _context.Equipment.Remove(equipment);
            _context.SaveChanges();
        }

        public Equipment GetEquipmentById(int id)
        {
            return _context.Equipment.FirstOrDefault(e => e.Id == id);
        }

        public Equipment GetEquipmentByName(string name)
        {
            return _context.Equipment.FirstOrDefault(e => e.Name == name);
        }

        public List<Equipment> GetAllEquipments()
        {
            return _context.Equipment.ToList();
        }
    }
}

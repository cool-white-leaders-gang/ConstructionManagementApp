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

        public void CreateEquipment(Equipment equipment)
        {
            _context.Equipment.Add(equipment);
            _context.SaveChanges();
        }

        public void UpdateEquipment(Equipment equipment)
        {
            var existingEquipment = GetEquipmentById(equipment.Id);
            if (existingEquipment == null)
                throw new KeyNotFoundException("Nie znaleziono sprzętu o podanym Id.");

            _context.Equipment.Update(equipment);
            _context.SaveChanges();
        }

        public void DeleteEquipment(int equipmentId)
        {
            var equipment = GetEquipmentById(equipmentId);
            if (equipment == null)
                throw new KeyNotFoundException("Nie znaleziono sprzętu o podanym Id.");

            _context.Equipment.Remove(equipment);
            _context.SaveChanges();
        }

        public Equipment GetEquipmentById(int equipmentId)
        {
            return _context.Equipment.FirstOrDefault(e => e.Id == equipmentId);
        }

        public List<Equipment> GetAllEquipment()
        {
            return _context.Equipment.ToList();
        }
    }
}

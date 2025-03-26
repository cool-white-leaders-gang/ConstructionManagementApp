using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Console.WriteLine("Sprzęt dodany do bazy danych");
        }

        public void UpdateEquipment(Equipment equipment)
        {
            try
            {
                _context.Equipment.Update(equipment);
                _context.SaveChanges();
                Console.WriteLine("Sprzęt zaktualizowany");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Próba aktualizacji nieistniejącego sprzętu: " + ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void DeleteEquipment(int equipmentId)
        {
            var equipment = _context.Equipment.FirstOrDefault(e => equipmentId == e.Id);
            if (equipment == null)
            {
                Console.WriteLine("Nie ma takiego sprzętu");
                return;
            }
            _context.Equipment.Remove(equipment);
            equipment = null;   
            GC.Collect();
            _context.SaveChanges();
            Console.WriteLine("Sprzęt usunięty");
        }

        public List<Equipment> GetAllEquipment()
        {
            return _context.Equipment.ToList();
        }

        public Equipment GetEquipmentById(int equipmentId)
        {
            return _context.Equipment.FirstOrDefault(e => equipmentId == e.Id);
        }
    }
}

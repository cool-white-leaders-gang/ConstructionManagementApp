using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConstructionManagementApp.App.Database;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Repositories
{
    internal class MaterialRepository
    {
        private readonly AppDbContext _context;
        public MaterialRepository(AppDbContext context)
        {
            _context = context;
        }
        public void CreateMaterial(Material material)
        {
            _context.Materials.Add(material);
            _context.SaveChanges();
            Console.WriteLine("Dodano nowy materiał");
        }
        public void UpdateMaterial(Material material)
        {
            try
            {
                _context.Materials.Update(material);
                _context.SaveChanges();
                Console.WriteLine("Materiał zaktualizowany");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Próba aktualizacji nieistniejącego materiału: " + ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        public void DeleteMaterial(int materialId)
        {
            var material = _context.Materials.FirstOrDefault(e => materialId == e.Id);
            if (material == null)
            {
                Console.WriteLine("Nie ma takiego materiału");
                return;
            }
            _context.Materials.Remove(material);
            material = null;
            GC.Collect();
            _context.SaveChanges();
            Console.WriteLine("Materiał usunięty");
        }
        public List<Material> GetAllMaterials()
        {
            return _context.Materials.ToList();
        }

        public Material GetMaterialById(int materialId)
        {
            return _context.Materials.FirstOrDefault(e => materialId == e.Id);
        }
    }
}

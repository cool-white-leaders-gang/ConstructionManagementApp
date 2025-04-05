using System.Collections.Generic;
using System.Linq;
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
        }

        public void UpdateMaterial(Material material)
        {
            var existingMaterial = GetMaterialById(material.Id);
            if (existingMaterial == null)
                throw new KeyNotFoundException("Nie znaleziono materiału o podanym Id.");

            _context.Materials.Update(material);
            _context.SaveChanges();
        }

        public void DeleteMaterial(int materialId)
        {
            var material = GetMaterialById(materialId);
            if (material == null)
                throw new KeyNotFoundException("Nie znaleziono materiału o podanym Id.");

            _context.Materials.Remove(material);
            _context.SaveChanges();
        }

        public Material GetMaterialById(int materialId)
        {
            return _context.Materials.FirstOrDefault(m => m.Id == materialId);
        }

        public List<Material> GetAllMaterials()
        {
            return _context.Materials.ToList();
        }
    }
}

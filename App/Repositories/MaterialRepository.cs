using System;
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

        public void AddMaterial(Material material)
        {
            if (material == null)
                throw new ArgumentNullException(nameof(material), "Materiał nie może być null.");

            _context.Materials.Add(material);
            _context.SaveChanges();
        }

        public void UpdateMaterial(Material material)
        {
            var existingMaterial = GetMaterialById(material.Id);

            existingMaterial.Name = material.Name;
            existingMaterial.Quantity = material.Quantity;

            _context.Materials.Update(existingMaterial);
            _context.SaveChanges();
        }

        public void DeleteMaterialById(int materialId)
        {
            var material = GetMaterialById(materialId);
            if (material == null)
                throw new KeyNotFoundException("Nie znaleziono materiału o podanym Id.");

            _context.Materials.Remove(material);
            _context.SaveChanges();
        }

        public Material GetMaterialById(int id)
        {
            return _context.Materials.FirstOrDefault(m => m.Id == id);
        }

        public List<Material> GetAllMaterials()
        {
            return _context.Materials.ToList();
        }
    }
}

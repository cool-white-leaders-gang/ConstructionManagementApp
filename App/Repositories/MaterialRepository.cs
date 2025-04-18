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

        // Konstruktor repozytorium, inicjalizuje kontekst bazy danych
        public MaterialRepository(AppDbContext context)
        {
            _context = context;
        }

        // Dodanie nowego materiału do bazy danych
        public void AddMaterial(Material material)
        {
            if (material == null)
                throw new ArgumentNullException(nameof(material), "Materiał nie może być null.");

            // Dodanie materiału i zapisanie zmian w bazie
            _context.Materials.Add(material);
            _context.SaveChanges();
        }

        // Aktualizacja istniejącego materiału w bazie danych
        public void UpdateMaterial(Material material)
        {
            // Wyszukiwanie istniejącego materiału po Id
            var existingMaterial = GetMaterialById(material.Id);
            if (existingMaterial == null)
                throw new KeyNotFoundException("Nie znaleziono materiału o podanym Id.");

            // Aktualizacja właściwości materiału
            existingMaterial.Name = material.Name;
            existingMaterial.Quantity = material.Quantity;

            // Zapisanie zaktualizowanego materiału w bazie
            _context.Materials.Update(existingMaterial);
            _context.SaveChanges();
        }

        // Usunięcie materiału z bazy danych na podstawie jego Id
        public void DeleteMaterialById(int materialId)
        {
            var material = GetMaterialById(materialId);
            if (material == null)
                throw new KeyNotFoundException("Nie znaleziono materiału o podanym Id.");

            // Usunięcie materiału z bazy
            _context.Materials.Remove(material);
            _context.SaveChanges();
        }

        // Pobranie materiału z bazy danych po Id
        public Material GetMaterialById(int id)
        {
            return _context.Materials.FirstOrDefault(m => m.Id == id);
        }

        // Pobranie wszystkich materiałów z bazy danych
        public List<Material> GetAllMaterials()
        {
            return _context.Materials.ToList();
        }
    }
}

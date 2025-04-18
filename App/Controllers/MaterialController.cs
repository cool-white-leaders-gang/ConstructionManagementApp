using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Services;
using ConstructionManagementApp.Events;
using ConstructionManagementApp.App.Delegates;

namespace ConstructionManagementApp.App.Controllers
{
    internal class MaterialController
    {
        private readonly MaterialRepository _materialRepository;
        private readonly AuthenticationService _authenticationService;
        private readonly ProjectRepository _projectRepository;
        private readonly RBACService _rbacService;
        public event LogEventHandler MaterialAdded;
        public event LogEventHandler MaterialDeleted;
        public event LogEventHandler MaterialUpdated;

        public MaterialController(MaterialRepository materialRepository, ProjectRepository projectRepository, AuthenticationService authenticationService, RBACService rbacService)
        {
            _materialRepository = materialRepository;
            _projectRepository = projectRepository;
            _authenticationService = authenticationService;
            _rbacService = rbacService;
        }

        public void AddMaterial(string name, int quantity, string unit, string projectName)
        {
            try
            {
                var currentUser = _authenticationService.CurrentSession.User;

                // Pobierz projekt na podstawie nazwy
                var project = _projectRepository.GetProjectByName(projectName);
                if (project == null)
                    throw new KeyNotFoundException($"Nie znaleziono projektu o nazwie {projectName}.");

                // Sprawdź czy użytkownik jest managerem projektu
                if (!_rbacService.IsProjectManager(currentUser, project.Id, _projectRepository))
                    throw new UnauthorizedAccessException("Nie masz uprawnień do dodawania materiałów do tego projektu.");

                var material = new Material(name, quantity, unit, project.Id);
                _materialRepository.AddMaterial(material);
                Console.WriteLine("Materiał został pomyślnie dodany.");
                MaterialAdded?.Invoke(this, new LogEventArgs(currentUser.Username, $"Dodano nowy materiał o ID {material.Id} i nazwie {name}"));
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

        public void UpdateMaterial(int materialId, string name, int quantity)
        {
            try
            {
                var currentUser = _authenticationService.CurrentSession.User;

                var material = _materialRepository.GetMaterialById(materialId);
                if (material == null)
                    throw new KeyNotFoundException($"Nie znaleziono materiału o ID {materialId}.");

                // Pobierz projekt na podstawie nazwy
                var project = _projectRepository.GetProjectById(material.ProjectId);
                if (project == null)
                    throw new KeyNotFoundException($"Nie znaleziono projektu o ID {material.ProjectId}.");

                // Sprawdź czy użytkownik jest managerem projektu
                if (!_rbacService.IsProjectManager(currentUser, project.Id, _projectRepository))
                    throw new UnauthorizedAccessException("Nie masz uprawnień do aktualizacji materiałów w tym projekcie.");

                // Aktualizuj materiał

                material.Name = name;
                material.Quantity = quantity;

                _materialRepository.UpdateMaterial(material);
                Console.WriteLine("Materiał został pomyślnie zaktualizowany.");
                MaterialUpdated?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Zaktualizowano materiał o ID {materialId} i nazwie {name}"));
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

        public void DeleteMaterial(int materialId)
        {
            try
            {
                var currentUser = _authenticationService.CurrentSession.User;

                // Pobierz materiał na podstawie ID
                var material = _materialRepository.GetMaterialById(materialId);
                if (material == null)
                    throw new KeyNotFoundException($"Nie znaleziono materiału o ID {materialId}.");

                // Sprawdź czy użytkownik jest managerem projektu
                var project = _projectRepository.GetProjectById(material.ProjectId);
                if (project == null || !_rbacService.IsProjectManager(currentUser, project.Id, _projectRepository))
                    throw new UnauthorizedAccessException("Nie masz uprawnień do usuwania materiałów w tym projekcie.");

                _materialRepository.DeleteMaterialById(materialId);
                Console.WriteLine("Materiał został pomyślnie usunięty.");
                MaterialDeleted?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Usunięto materiał o ID {materialId}"));
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

        public void DisplayMaterialsForUser()
        {
            try
            {
                var currentUser = _authenticationService.CurrentSession.User;
                var userProjects = _rbacService.GetProjectsForUserOrManagedBy(currentUser, _projectRepository);

                var materials = _materialRepository.GetAllMaterials()
                    .Where(material => userProjects.Contains(material.ProjectId))
                    .ToList();

                if (!materials.Any())
                {
                    Console.WriteLine("Brak materiałów przypisanych do Twoich projektów.");
                    return;
                }

                Console.WriteLine("--- Lista materiałów ---");
                foreach (var material in materials)
                {
                    Console.WriteLine($"ID: {material.Id}, Nazwa: {material.Name}, Ilość: {material.Quantity}, Jednostka: {material.Unit}, Projekt ID: {material.ProjectId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }
    }
}
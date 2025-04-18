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

        // Eventy do logowania operacji na materiałach.
        public event LogEventHandler MaterialAdded;
        public event LogEventHandler MaterialDeleted;
        public event LogEventHandler MaterialUpdated;

        // Konstruktor 
        public MaterialController(MaterialRepository materialRepository, ProjectRepository projectRepository, AuthenticationService authenticationService, RBACService rbacService)
        {
            _materialRepository = materialRepository;
            _projectRepository = projectRepository;
            _authenticationService = authenticationService;
            _rbacService = rbacService;
        }

        // Dodaje nowy materiał do projektu.
        public void AddMaterial(string name, int quantity, string unit, string projectName)
        {
            try
            {
                var currentUser = _authenticationService.CurrentSession.User;

                // Szuka projektu po nazwie.
                var project = _projectRepository.GetProjectByName(projectName);
                if (project == null)
                    throw new KeyNotFoundException($"Nie znaleziono projektu o nazwie {projectName}.");

                // Sprawdza, czy użytkownik jest managerem tego projektu.
                if (!_rbacService.IsProjectManager(currentUser, project.Id, _projectRepository))
                    throw new UnauthorizedAccessException("Nie masz uprawnień do dodawania materiałów do tego projektu.");

                // Tworzy i dodaje materiał.
                var material = new Material(name, quantity, unit, project.Id);
                _materialRepository.AddMaterial(material);
                Console.WriteLine("Materiał został pomyślnie dodany.");

                // Wywołuje event logujący dodanie.
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

        // Aktualizuje istniejący materiał.
        public void UpdateMaterial(int materialId, string name, int quantity)
        {
            try
            {
                var currentUser = _authenticationService.CurrentSession.User;

                // Pobiera materiał po ID.
                var material = _materialRepository.GetMaterialById(materialId);
                if (material == null)
                    throw new KeyNotFoundException($"Nie znaleziono materiału o ID {materialId}.");

                // Pobiera projekt, do którego należy materiał.
                var project = _projectRepository.GetProjectById(material.ProjectId);
                if (project == null)
                    throw new KeyNotFoundException($"Nie znaleziono projektu o ID {material.ProjectId}.");

                // Sprawdza, czy użytkownik jest managerem tego projektu.
                if (!_rbacService.IsProjectManager(currentUser, project.Id, _projectRepository))
                    throw new UnauthorizedAccessException("Nie masz uprawnień do aktualizacji materiałów w tym projekcie.");

                // Aktualizuje dane materiału.
                material.Name = name;
                material.Quantity = quantity;
                _materialRepository.UpdateMaterial(material);
                Console.WriteLine("Materiał został pomyślnie zaktualizowany.");

                // Wywołuje event logujący aktualizację.
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

        // Usuwa materiał na podstawie ID.
        public void DeleteMaterial(int materialId)
        {
            try
            {
                var currentUser = _authenticationService.CurrentSession.User;

                // Pobiera materiał po ID.
                var material = _materialRepository.GetMaterialById(materialId);
                if (material == null)
                    throw new KeyNotFoundException($"Nie znaleziono materiału o ID {materialId}.");

                // Sprawdza, czy użytkownik jest managerem projektu.
                var project = _projectRepository.GetProjectById(material.ProjectId);
                if (project == null || !_rbacService.IsProjectManager(currentUser, project.Id, _projectRepository))
                    throw new UnauthorizedAccessException("Nie masz uprawnień do usuwania materiałów w tym projekcie.");

                // Usuwa materiał.
                _materialRepository.DeleteMaterialById(materialId);
                Console.WriteLine("Materiał został pomyślnie usunięty.");

                // Wywołuje event logujący usunięcie.
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

        // Wyświetla listę materiałów przypisanych do projektów użytkownika.
        public void DisplayMaterialsForUser()
        {
            try
            {
                var currentUser = _authenticationService.CurrentSession.User;

                // Pobiera projekty użytkownika.
                var userProjects = _rbacService.GetProjectsForUserOrManagedBy(currentUser, _projectRepository);

                // Filtruje materiały przypisane do projektów użytkownika.
                var materials = _materialRepository.GetAllMaterials()
                    .Where(material => userProjects.Contains(material.ProjectId))
                    .ToList();

                if (!materials.Any())
                {
                    Console.WriteLine("Brak materiałów przypisanych do Twoich projektów.");
                    return;
                }

                // Wyświetla listę materiałów.
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

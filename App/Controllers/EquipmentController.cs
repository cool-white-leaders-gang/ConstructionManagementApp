using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Enums;
using ConstructionManagementApp.App.Services;
using ConstructionManagementApp.Events;
using System.Xml.Linq;
using ConstructionManagementApp.App.Delegates;

namespace ConstructionManagementApp.App.Controllers
{
    internal class EquipmentController
    {
        private readonly EquipmentRepository _equipmentRepository;
        private readonly ProjectRepository _projectRepository;
        private readonly AuthenticationService _authenticationService;
        private readonly RBACService _rbacService;
        public event LogEventHandler EquipmentAdded;
        public event LogEventHandler EquipmentDeleted;
        public event LogEventHandler EquipmentUpdated;

        public EquipmentController(EquipmentRepository equipmentRepository, ProjectRepository projectRepository, AuthenticationService authenticationService, RBACService rbacService)
        {
            _equipmentRepository = equipmentRepository;
            _projectRepository = projectRepository;
            _authenticationService = authenticationService;
            _rbacService = rbacService;
        }

        public void AddEquipment(string name, EquipmentStatus status, string projectName)
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
                    throw new UnauthorizedAccessException("Nie masz uprawnień do dodawania sprzętu do tego projektu.");
                var equipment = new Equipment(name, status, project.Id);
                _equipmentRepository.AddEquipment(equipment);
                Console.WriteLine("Sprzęt został pomyślnie dodany.");
                EquipmentAdded?.Invoke(this, new LogEventArgs(currentUser.Username, $"Dodano nowe wyposażenie o nazwie {name}"));
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

        public void UpdateEquipment(int equipmentId, string newName, EquipmentStatus status)
        {
            try
            {
                var currentUser = _authenticationService.CurrentSession.User;

                var equipment = _equipmentRepository.GetEquipmentById(equipmentId);
                if (equipment == null)
                    throw new KeyNotFoundException($"Nie znaleziono sprzętu o ID {equipmentId}.");

                // Pobierz projekt na podstawie nazwy
                var project = _projectRepository.GetProjectById(equipment.ProjectId);
                if (project == null)
                    throw new KeyNotFoundException($"Nie znaleziono projektu o ID {equipment.ProjectId}.");

                // Sprawdź czy użytkownik jest managerem projektu
                if (!_rbacService.IsProjectManager(currentUser, project.Id, _projectRepository))
                    throw new UnauthorizedAccessException("Nie masz uprawnień do aktualizacji sprzętu w tym projekcie.");

                // Aktualizuj sprzęt
                

                equipment.Name = newName;
                equipment.Status = status;


                _equipmentRepository.UpdateEquipment(equipment);
                Console.WriteLine("Sprzęt został pomyślnie zaktualizowany.");
                EquipmentUpdated?.Invoke(this, new LogEventArgs(currentUser.Username, $"Zaktualizowano wyposażenie o ID {equipmentId}"));

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

        public void DeleteEquipment(int equipmentId)
        {
            try
            {
                var currentUser = _authenticationService.CurrentSession.User;

                // Pobierz sprzęt na podstawie ID
                var equipment = _equipmentRepository.GetEquipmentById(equipmentId);
                if (equipment == null)
                    throw new KeyNotFoundException($"Nie znaleziono sprzętu o ID {equipmentId}.");

                // Sprawdź czy użytkownik jest managerem projektu
                var project = _projectRepository.GetProjectById(equipment.ProjectId);
                if (project == null || !_rbacService.IsProjectManager(currentUser, project.Id, _projectRepository))
                    throw new UnauthorizedAccessException("Nie masz uprawnień do usuwania sprzętu w tym projekcie.");

                _equipmentRepository.DeleteEquipmentById(equipmentId);
                Console.WriteLine("Sprzęt został pomyślnie usunięty.");
                EquipmentDeleted?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Usunięto wyposażenie o ID {equipmentId}"));

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

        public void DisplayEquipmentsForUser()
        {
            try
            {
                var currentUser = _authenticationService.CurrentSession.User;
                var userProjects = _rbacService.GetProjectsForUserOrManagedBy(currentUser, _projectRepository);

                var equipments = _equipmentRepository.GetAllEquipments()
                    .Where(equipment => userProjects.Contains(equipment.ProjectId))
                    .ToList();

                if (!equipments.Any())
                {
                    Console.WriteLine("Brak sprzętu przypisanego do Twoich projektów.");
                    return;
                }

                Console.WriteLine("--- Lista sprzętu ---");
                foreach (var equipment in equipments)
                {
                    Console.WriteLine($"ID: {equipment.Id}, Nazwa: {equipment.Name}, Status: {equipment.Status}, Projekt ID: {equipment.ProjectId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }


    }
}
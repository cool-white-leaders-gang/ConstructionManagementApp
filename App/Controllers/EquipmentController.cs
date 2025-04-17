using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Enums;
using ConstructionManagementApp.App.Services;
using ConstructionManagementApp.Events;
using System.Xml.Linq;

namespace ConstructionManagementApp.App.Controllers
{
    internal class EquipmentController
    {
        private readonly EquipmentRepository _equipmentRepository;
        private readonly ProjectRepository _projectRepository;
        private readonly AuthenticationService _authenticationService;
        public event LogEventHandler EquipmentAdded;
        public event LogEventHandler EquipmentDeleted;
        public event LogEventHandler EquipmentUpdated;

        public EquipmentController(EquipmentRepository equipmentRepository, ProjectRepository projectRepository, AuthenticationService authenticationService)
        {
            _equipmentRepository = equipmentRepository;
            _projectRepository = projectRepository;
            _authenticationService = authenticationService;
        }

        public void AddEquipment(string name, EquipmentStatus status, string projectName)
        {
            try
            {
                var project = _projectRepository.GetProjectByName(projectName);
                if (project == null)
                    throw new KeyNotFoundException($"Nie znaleziono projektu o Id {projectName}");
                var equipment = new Equipment(name, status, project.Id);
                _equipmentRepository.AddEquipment(equipment);
                Console.WriteLine("Sprzęt został pomyślnie dodany.");
                EquipmentAdded?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Dodano nowe wyposażenie o nazwie {name}"));
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

        public void UpdateEquipment(int equipmentId, string newName, EquipmentStatus status, string projectName)
        {
            try
            {
                var equipment = _equipmentRepository.GetEquipmentById(equipmentId);
                var project = _projectRepository.GetProjectByName(projectName);
                if (equipment == null)
                    throw new KeyNotFoundException($"Nie znaleziono sprzętu o Id {equipmentId}.");
             
                if (_projectRepository.GetProjectById(project.Id) == null)
                    throw new KeyNotFoundException($"Nie znaleziono projektu o Id {project.Id}");

                equipment.Name = newName;
                equipment.Status = status;
                equipment.ProjectId = project.Id;

                _equipmentRepository.UpdateEquipment(equipment);
                Console.WriteLine("Sprzęt został pomyślnie zaktualizowany.");
                EquipmentUpdated?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Zaktualizowano wyposażenie o ID {equipmentId}"));

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

        public void DisplayAllEquipments()
        {
            try
            {
                var equipments = _equipmentRepository.GetAllEquipments();
                if (equipments.Count == 0)
                {
                    Console.WriteLine("Brak sprzętu w systemie.");
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
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
    // Klasa kontrolera odpowiedzialna za zarządzanie sprzętem w projektach budowlanych
    internal class EquipmentController
    {
        private readonly EquipmentRepository _equipmentRepository;   // Repozytorium sprzętu
        private readonly ProjectRepository _projectRepository;       // Repozytorium projektów
        private readonly AuthenticationService _authenticationService; // Usługa autoryzacji użytkownika
        private readonly RBACService _rbacService;                     // Usługa kontroli dostępu opartej na rolach

        // Zdarzenia logujące operacje na sprzęcie
        public event LogEventHandler EquipmentAdded;
        public event LogEventHandler EquipmentDeleted;
        public event LogEventHandler EquipmentUpdated;

        // Konstruktor kontrolera inicjalizujący repozytoria i serwisy
        public EquipmentController(EquipmentRepository equipmentRepository, ProjectRepository projectRepository, AuthenticationService authenticationService, RBACService rbacService)
        {
            _equipmentRepository = equipmentRepository;
            _projectRepository = projectRepository;
            _authenticationService = authenticationService;
            _rbacService = rbacService;
        }

        // Dodaje nowy sprzęt do wskazanego projektu, jeśli użytkownik ma odpowiednie uprawnienia
        public void AddEquipment(string name, EquipmentStatus status, string projectName)
        {
            try
            {
                var currentUser = _authenticationService.CurrentSession.User;

                // Pobiera projekt według nazwy
                var project = _projectRepository.GetProjectByName(projectName);
                if (project == null)
                    throw new KeyNotFoundException($"Nie znaleziono projektu o nazwie {projectName}.");

                // Sprawdza, czy użytkownik jest managerem tego projektu
                if (!_rbacService.IsProjectManager(currentUser, project.Id, _projectRepository))
                    throw new UnauthorizedAccessException("Nie masz uprawnień do dodawania sprzętu do tego projektu.");

                // Tworzy nowy sprzęt i zapisuje w repozytorium
                var equipment = new Equipment(name, status, project.Id);
                _equipmentRepository.AddEquipment(equipment);

                Console.WriteLine("Sprzęt został pomyślnie dodany.");

                // Wywołanie zdarzenia logowania
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

        // Aktualizuje istniejący sprzęt, po weryfikacji uprawnień użytkownika
        public void UpdateEquipment(int equipmentId, string newName, EquipmentStatus status)
        {
            try
            {
                var currentUser = _authenticationService.CurrentSession.User;

                // Pobiera sprzęt na podstawie ID
                var equipment = _equipmentRepository.GetEquipmentById(equipmentId);
                if (equipment == null)
                    throw new KeyNotFoundException($"Nie znaleziono sprzętu o ID {equipmentId}.");

                // Pobiera projekt, do którego sprzęt należy
                var project = _projectRepository.GetProjectById(equipment.ProjectId);
                if (project == null)
                    throw new KeyNotFoundException($"Nie znaleziono projektu o ID {equipment.ProjectId}.");

                // Sprawdza, czy użytkownik jest managerem projektu
                if (!_rbacService.IsProjectManager(currentUser, project.Id, _projectRepository))
                    throw new UnauthorizedAccessException("Nie masz uprawnień do aktualizacji sprzętu w tym projekcie.");

                // Aktualizacja danych sprzętu
                equipment.Name = newName;
                equipment.Status = status;

                _equipmentRepository.UpdateEquipment(equipment);
                Console.WriteLine("Sprzęt został pomyślnie zaktualizowany.");

                // Logowanie zdarzenia
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

        // Usuwa sprzęt, jeśli użytkownik ma odpowiednie uprawnienia do projektu
        public void DeleteEquipment(int equipmentId)
        {
            try
            {
                var currentUser = _authenticationService.CurrentSession.User;

                // Pobiera sprzęt do usunięcia
                var equipment = _equipmentRepository.GetEquipmentById(equipmentId);
                if (equipment == null)
                    throw new KeyNotFoundException($"Nie znaleziono sprzętu o ID {equipmentId}.");

                // Sprawdza, czy użytkownik jest managerem projektu
                var project = _projectRepository.GetProjectById(equipment.ProjectId);
                if (project == null || !_rbacService.IsProjectManager(currentUser, project.Id, _projectRepository))
                    throw new UnauthorizedAccessException("Nie masz uprawnień do usuwania sprzętu w tym projekcie.");

                // Usunięcie sprzętu z repozytorium
                _equipmentRepository.DeleteEquipmentById(equipmentId);
                Console.WriteLine("Sprzęt został pomyślnie usunięty.");

                // Logowanie zdarzenia
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

        // Wyświetla sprzęt przypisany do projektów, którymi zarządza lub w których uczestniczy aktualny użytkownik
        public void DisplayEquipmentsForUser()
        {
            try
            {
                var currentUser = _authenticationService.CurrentSession.User;

                // Pobiera listę projektów, do których użytkownik ma dostęp
                var userProjects = _rbacService.GetProjectsForUserOrManagedBy(currentUser, _projectRepository);

                // Filtruje sprzęt przypisany do projektów użytkownika
                var equipments = _equipmentRepository.GetAllEquipments()
                    .Where(equipment => userProjects.Contains(equipment.ProjectId))
                    .ToList();

                // Komunikat jeśli brak sprzętu
                if (!equipments.Any())
                {
                    Console.WriteLine("Brak sprzętu przypisanego do Twoich projektów.");
                    return;
                }

                // Wyświetlanie listy sprzętu
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

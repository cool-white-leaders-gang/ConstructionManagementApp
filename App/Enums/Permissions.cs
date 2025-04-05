using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionManagementApp.App.Enums
{
    internal enum Permission
    {
        // Użytkownicy
        CreateUser,         // Tworzenie użytkownika
        UpdateUser,         // Aktualizowanie użytkownika
        DeleteUser,         // Usuwanie użytkownika
        ViewUsers,          // Przeglądanie listy użytkowników

        // Zadania
        CreateTask,         // Tworzenie zadania
        UpdateTask,         // Aktualizowanie zadania
        DeleteTask,         // Usuwanie zadania
        ViewTasks,          // Przeglądanie listy zadań
        AssignTask,         // Przypisywanie zadania do użytkownika
        CompleteTask,       // Oznaczanie zadania jako ukończone

        // Materiały
        CreateMaterial,     // Dodawanie materiału
        UpdateMaterial,     // Aktualizowanie materiału
        DeleteMaterial,     // Usuwanie materiału
        ViewMaterials,      // Przeglądanie listy materiałów

        // Sprzęt
        CreateEquipment,    // Dodawanie sprzętu
        UpdateEquipment,    // Aktualizowanie sprzętu
        DeleteEquipment,    // Usuwanie sprzętu
        ViewEquipment,      // Przeglądanie listy sprzętu

        // Raporty postępu
        CreateProgressReport,   // Tworzenie raportu postępu
        UpdateProgressReport,   // Aktualizowanie raportu postępu
        DeleteProgressReport,   // Usuwanie raportu postępu
        ViewProgressReports,    // Przeglądanie listy raportów postępu

        // Budżet
        CreateBudget,       // Tworzenie budżetu
        UpdateBudget,       // Aktualizowanie budżetu
        DeleteBudget,       // Usuwanie budżetu
        ViewBudget,         // Przeglądanie budżetu

        // Logi
        CreateLog,          // Tworzenie logu (np. ręczne dodanie logu)
        ViewLogs,           // Przeglądanie logów

        // Powiadomienia
        CreateNotification, // Tworzenie powiadomienia
        SendNotification,   // Wysyłanie powiadomienia
        ViewNotifications,  // Przeglądanie powiadomień

        // Zgłoszenia problemów
        CreateIssue,        // Tworzenie zgłoszenia problemu
        UpdateIssue,        // Aktualizowanie zgłoszenia problemu
        DeleteIssue,        // Usuwanie zgłoszenia problemu
        ViewIssues,         // Przeglądanie zgłoszeń problemów

        // Projekty
        CreateProject,      // Tworzenie projektu
        UpdateProject,      // Aktualizowanie projektu
        DeleteProject,      // Usuwanie projektu
        ViewProjects,       // Przeglądanie listy projektów

        // Zespoły
        CreateTeam,         // Tworzenie zespołu
        UpdateTeam,         // Aktualizowanie zespołu
        DeleteTeam,         // Usuwanie zespołu
        ViewTeams           // Przeglądanie listy zespołów
    }
}

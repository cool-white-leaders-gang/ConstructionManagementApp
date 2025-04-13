using System;
using System.Collections.Generic;
using System.Data;
using ConstructionManagementApp.App.Enums;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Services
{
    internal class RBACService
    {
        // Słownik przypisujący role do listy permisji
        private readonly Dictionary<Role, List<Permission>> _rolePermissions;

        public RBACService()
        {
            // Definicja permisji dla każdej roli
            _rolePermissions = new Dictionary<Role, List<Permission>>
            {
                {
                    Role.Admin, new List<Permission>
                    {
                        // Użytkownicy
                        Permission.CreateUser, Permission.UpdateUser, Permission.DeleteUser, Permission.ViewUsers,

                        // Zadania
                        Permission.CreateTask, Permission.UpdateTask, Permission.DeleteTask, Permission.ViewTasks,
                        Permission.AssignTask, Permission.RemoveFromTask,Permission.ViewTaskAssignment,Permission.CompleteTask,

                        // Materiały
                        Permission.CreateMaterial, Permission.UpdateMaterial, Permission.DeleteMaterial, Permission.ViewMaterials,

                        // Sprzęt
                        Permission.CreateEquipment, Permission.UpdateEquipment, Permission.DeleteEquipment, Permission.ViewEquipment,

                        // Raporty postępu
                        Permission.CreateProgressReport, Permission.UpdateProgressReport, Permission.DeleteProgressReport, Permission.ViewProgressReports,

                        // Budżet
                        Permission.CreateBudget, Permission.UpdateBudget, Permission.DeleteBudget, Permission.ViewBudget,

                        // Logi
                        Permission.CreateLog, Permission.ViewLogs,

                        // Powiadomienia
                        Permission.SendMessage, Permission.ViewMessages,

                        // Zgłoszenia problemów
                        Permission.CreateIssue, Permission.UpdateIssue, Permission.DeleteIssue, Permission.ViewIssues,

                        // Projekty
                        Permission.CreateProject, Permission.UpdateProject, Permission.DeleteProject, Permission.ViewProjects,

                        // Zespoły
                        Permission.CreateTeam, Permission.UpdateTeam, Permission.DeleteTeam, Permission.ViewTeam, Permission.AddMember, Permission.RemoveMember
                    }
                },
                {
                    Role.Manager, new List<Permission>
                    {
                        // Użytkownicy
                        Permission.ViewUsers,

                        // Zadania
                        Permission.CreateTask, Permission.UpdateTask, Permission.ViewTasks, Permission.AssignTask,

                        // Materiały
                        Permission.CreateMaterial, Permission.UpdateMaterial, Permission.ViewMaterials,

                        // Sprzęt
                        Permission.CreateEquipment, Permission.UpdateEquipment, Permission.ViewEquipment,

                        // Raporty postępu
                        Permission.CreateProgressReport, Permission.UpdateProgressReport, Permission.ViewProgressReports,

                        // Budżet
                        Permission.ViewBudget,

                        // Powiadomienia
                        Permission.SendMessage, Permission.ViewMessages,

                        // Zgłoszenia problemów
                        Permission.CreateIssue, Permission.UpdateIssue, Permission.ViewIssues,

                        // Projekty
                        Permission.CreateProject, Permission.UpdateProject, Permission.ViewProjects,

                        // Zespoły
                        Permission.CreateTeam, Permission.UpdateTeam, Permission.ViewTeam, Permission.AddMember, Permission.RemoveMember
                    }
                },
                {
                    Role.Worker, new List<Permission>
                    {
                        // Zadania
                        Permission.ViewTasks, Permission.CompleteTask,

                        // Materiały
                        Permission.ViewMaterials,

                        // Sprzęt
                        Permission.ViewEquipment,

                        // Raporty postępu
                        Permission.CreateProgressReport, Permission.ViewProgressReports,

                        // Powiadomienia
                        Permission.ViewMessages, Permission.SendMessage,

                        // Zgłoszenia problemów
                        Permission.CreateIssue, Permission.ViewIssues,

                        // Projekty
                        Permission.ViewProjects
                    }
                },
                {
                    Role.Client, new List<Permission>
                    {
                        // Raporty postępu
                        Permission.ViewProgressReports,

                        // Powiadomienia
                        Permission.ViewMessages, Permission.SendMessage,

                        // Projekty
                        Permission.ViewProjects
                    }
                }
            };
        }

        // Sprawdza, czy użytkownik ma określone uprawnienie
        public bool HasPermission(User user, Permission permission)
        {
            Role role = user.Role;
            if (_rolePermissions.ContainsKey(role) && _rolePermissions[role].Contains(permission))
            {
                return true;
            }
            return false;
        }

        // Wyświetla wszystkie uprawnienia dla danej roli użytkownika
        public void PrintPermissions(User user)
        {
            if (!_rolePermissions.ContainsKey(user.Role))
            {
                Console.WriteLine($"Rola {user.Role} nie ma zdefiniowanych uprawnień.");
                return;
            }

            Console.WriteLine($"Uprawnienia dla roli {user.Role}:");
            foreach (var permission in _rolePermissions[user.Role])
            {
                Console.WriteLine($"- {permission}");
            }
        }
    }
}
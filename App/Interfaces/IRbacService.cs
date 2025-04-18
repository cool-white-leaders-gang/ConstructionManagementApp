using System.Collections.Generic;
using ConstructionManagementApp.App.Enums;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Repositories;

namespace ConstructionManagementApp.App.Interfaces
{
    internal interface IRBACService
    {
        bool HasPermission(User user, Permission permission);
        void PrintPermissions(User user);
        bool IsProjectManager(User user, int projectId, ProjectRepository projectRepository);
        bool IsWorkerInProjectTeam(User user, int projectId, ProjectRepository projectRepository);
        List<int> GetProjectsForUserOrManagedBy(User user, ProjectRepository projectRepository);
        List<int> GetProjectsManagedBy(int managerId);
    }
}
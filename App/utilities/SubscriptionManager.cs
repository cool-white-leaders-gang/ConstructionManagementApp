using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConstructionManagementApp.App.Controllers;
using ConstructionManagementApp.App.Services;
using ConstructionManagementApp.Services;

namespace ConstructionManagementApp.App.Utilities
{
    internal static class EventSubscriptionManager
    {
        public static void SubscribeEvents(
            AuthenticationService authenticationService,
            UserController userController,
            BudgetController budgetController,
            EquipmentController equipmentController,
            IssueController issueController,
            MaterialController materialController,
            MessageController messageController,
            ProgressReportController progressReportController,
            ProjectController projectController,
            TaskController taskController,
            TeamController teamController,
            LogService logService)
        {
            // Subscribe to events
            authenticationService.UserLoggedIn += logService.OnActionOccurred;
            userController.UserAdded += logService.OnActionOccurred;
            userController.UserUpdated += logService.OnActionOccurred;
            userController.UserDeleted += logService.OnActionOccurred;
            budgetController.BudgetAdded += logService.OnActionOccurred;
            budgetController.BudgetUpdated += logService.OnActionOccurred;
            budgetController.BudgetDeleted += logService.OnActionOccurred;
            equipmentController.EquipmentAdded += logService.OnActionOccurred;
            equipmentController.EquipmentUpdated += logService.OnActionOccurred;
            equipmentController.EquipmentDeleted += logService.OnActionOccurred;
            issueController.IssueAdded += logService.OnActionOccurred;
            issueController.IssueUpdated += logService.OnActionOccurred;
            issueController.IssueDeleted += logService.OnActionOccurred;
            materialController.MaterialAdded += logService.OnActionOccurred;
            materialController.MaterialUpdated += logService.OnActionOccurred;
            materialController.MaterialDeleted += logService.OnActionOccurred;
            messageController.MessageSent += logService.OnActionOccurred;
            progressReportController.ProgressReportAdded += logService.OnActionOccurred;
            progressReportController.ProgressReportUpdated += logService.OnActionOccurred;
            progressReportController.ProgressReportDeleted += logService.OnActionOccurred;
            projectController.ProjectAdded += logService.OnActionOccurred;
            projectController.ProjectUpdated += logService.OnActionOccurred;
            projectController.ProjectDeleted += logService.OnActionOccurred;
            taskController.TaskAdded += logService.OnActionOccurred;
            taskController.TaskUpdated += logService.OnActionOccurred;
            taskController.TaskDeleted += logService.OnActionOccurred;
            taskController.TaskCompleted += logService.OnActionOccurred;
            taskController.TaskAssigned += logService.OnActionOccurred;
            taskController.TaskUnassigned += logService.OnActionOccurred;
            teamController.TeamAdded += logService.OnActionOccurred;
            teamController.TeamUpdated += logService.OnActionOccurred;
            teamController.TeamDeleted += logService.OnActionOccurred;
            teamController.UserAddedToTeam += logService.OnActionOccurred;
            teamController.UserRemovedFromTeam += logService.OnActionOccurred;
        }
    }
}

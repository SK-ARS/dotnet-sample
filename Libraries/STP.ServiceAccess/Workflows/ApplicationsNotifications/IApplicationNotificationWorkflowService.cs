using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.SecurityAndUsers;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.Domain.Workflow;
using STP.Domain.Workflow.Models;
using System.Collections.Generic;

namespace STP.ServiceAccess.Workflows.ApplicationsNotifications
{
    public interface IApplicationNotificationWorkflowService
    {
        WorkflowProcessDetails StartProcess(PlanMvmntPayLoad mvmntPayLoad, int taskId=4);
        bool ProcessActivity(string activityId, WorkflowActivityPostModel workflowActivityPostModel, bool completeTask = true);
        List<WorkflowVariableModel> SearchPayloadItem(string processKey, string variableName);
        string LoadActivities(int stepNumber);
        string GetCurrentTask(string activityId);
        string GetProcessKey(long appRevisionId);
        WorkflowProcessModel GetWorkflowDetailsByEsdalKey(long esdalKey);
        WorkflowActivityRoute GetWorkflowActivityRoute(string activityName, string processDefenition);
    }
}

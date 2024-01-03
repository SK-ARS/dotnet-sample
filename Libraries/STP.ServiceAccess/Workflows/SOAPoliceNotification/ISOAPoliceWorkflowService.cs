using STP.Domain.VehiclesAndFleets;
using STP.Domain.Workflow;
using STP.Domain.Workflow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.Workflows
{
    public interface ISOAPoliceWorkflowService
    {
        WorkflowProcessModel CheckIfProcessExit(string esdalReferenceNumber);
        WorkflowProcessDetails StartProcess(string esdalReferenceNumber, bool setApplicationKey = false, string organizationId = null, decimal esdalKey = 0);
        bool ProcessActivity(string activityId, WorkflowActivityPostModel workflowActivityPostModel, bool completeTask = true);
        string GetCurrentTask(string activityId);
        string LoadActivities(int stepNumber);
        List<WorkflowVariableModel> SearchPayloadItem(string processKey, string variableName);
        WorkflowProcessModel GetWorkflowDetailsByProcessId(string processId, long workflowId);
        WorkflowActivityRoute GetWorkflowActivityRoute(string activityName, string processDefenition);
    }
}

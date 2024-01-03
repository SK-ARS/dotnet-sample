using STP.Domain.VehiclesAndFleets;
using STP.Domain.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.Workflows
{
    public interface IFleetManagementWorkflowService
    {
        WorkflowProcessDetails StartProcess(bool isImportFleet, bool setApplicationKey = false, string organizationId = null, string workflowKeyVehicleName = null, decimal esdalKey = 0);
        bool ProcessActivity(string activityId, VehicleWorkFlowParams vehicleWorkFlowParams, object workflowPayload = null, bool completeTask = true);
        bool ProcessActivity(string activityId, List<VehicleComponentsModel> vehicleComponentsList, object workflowPayload = null, bool completeTask = true);
        List<WorkflowVariableModel> SearchPayloadItem(string processKey, string variableName);
        string LoadActivities(int stepNumber);
        string GetCurrentTask(string processId);
        bool TerminateProcess(string applicationId);
    }
}

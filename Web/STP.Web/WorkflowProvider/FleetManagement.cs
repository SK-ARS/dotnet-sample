using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.Domain.VehiclesAndFleets;
using STP.Domain.Workflow;
using STP.ServiceAccess.Workflows;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace STP.Web.WorkflowProvider
{
    public class FleetManagement
    {
        private readonly IFleetManagementWorkflowService fleetManagementWorkflowService;
        public FleetManagement(IFleetManagementWorkflowService fleetManagementWorkflowService)
        {
            this.fleetManagementWorkflowService = fleetManagementWorkflowService;
        }

        public string StartWorkflow(bool isImportFleet, string organizationId, string vehicleName)
        {
            try
            {
                WorkflowProcessDetails workflowProcessDetails = fleetManagementWorkflowService.StartProcess(isImportFleet, true, organizationId, vehicleName, 0);
                new SessionData().Wf_Fm_FleetManagementId = workflowProcessDetails == null || workflowProcessDetails.applicationFileId == null || workflowProcessDetails.applicationFileId.Length == 0
                    ? WorkflowActivityConstants.Gn_Failed
                    : workflowProcessDetails.applicationFileId;
                new SessionData().Wf_Fm_CurrentExecuted = WorkflowActivityTypes.Fm_ManualEntryComponent;
                return new SessionData().Wf_Fm_FleetManagementId;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Workflow/FleetManagement/StartWorkflow,  Fleet Management Workflow Application Id not found", ex.Message));
            }
            return string.Empty;
        }

        public bool ProcessWorkflowActivity(VehicleWorkFlowParams vehicleWorkFlowParams, List<VehicleComponentsModel> vehicleComponentsList, WorkflowFleetMgmtFlowTypes workflowFleetMgmtFlowTypes, string sessionId, bool addComponent, bool decideNextTask, bool isImportFromFleet, bool moveToEnterVehicleDetails, bool isEditConfiguration = false)
        {
            bool completeTask = true;

            if (new SessionData().Wf_Fm_FleetManagementId == WorkflowActivityConstants.Gn_Failed)
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],FleetManagement/ProcessWorkflowActivity,  Failed due to FleetManagement Id invalid", sessionId));
                return false;
            }
            if (vehicleWorkFlowParams != null && vehicleWorkFlowParams.VehicleComponentsModels != null)
            {
                vehicleWorkFlowParams.TotalComponents = vehicleWorkFlowParams.VehicleComponentsModels.Count;
            }
            if (vehicleComponentsList != null && vehicleComponentsList.Count > 0 && vehicleWorkFlowParams == null)
            {
                vehicleWorkFlowParams = new VehicleWorkFlowParams
                {
                    VehicleComponentsModels = vehicleComponentsList,
                    TotalComponents = vehicleComponentsList.Count
                };
                completeTask = false;

            }
            if (workflowFleetMgmtFlowTypes != WorkflowFleetMgmtFlowTypes.Vehicle)
            {
                if (workflowFleetMgmtFlowTypes == WorkflowFleetMgmtFlowTypes.VehicleConfig)
                {
                    if (new SessionData().Wf_Fm_FleetManagementId != null && new SessionData().Wf_Fm_FleetManagementId.Length > 0)
                    {
                        string workflowNextActivity = moveToEnterVehicleDetails || isEditConfiguration ? "Activity_EnterVehicleDetails" : GetNextActivityToExecute(new SessionData().Wf_Fm_CurrentExecuted);
                        dynamic workflowPayload = new ExpandoObject();
                        workflowPayload.decideNextTask = decideNextTask;
                        workflowPayload.nextTaskKey = workflowNextActivity;
                        if (isImportFromFleet)
                        {
                            workflowPayload.isValueChange = false;
                        }
                        return fleetManagementWorkflowService.ProcessActivity(new SessionData().Wf_Fm_FleetManagementId, vehicleWorkFlowParams, workflowPayload, completeTask);
                    }
                    else
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Vehicle/CreateComponent,  Fleet Management Workflow Application Id not found", sessionId));
                    }
                }
            }
            else
            {
                completeTask = false;
                return fleetManagementWorkflowService.ProcessActivity(new SessionData().Wf_Fm_FleetManagementId, vehicleWorkFlowParams, null, completeTask);
            }
            return false;
        }

        private string GetNextActivityToExecute(WorkflowActivityTypes workflowActivityTypes)
        {
            return WorkflowTaskFinder.FindNextTask("FleetManagement", workflowActivityTypes, out dynamic workflowPayload);

        }

        public List<WorkflowVariableModel> SearchPayloadItem(string variableName)
        {
            if (new SessionData().Wf_Fm_FleetManagementId == WorkflowActivityConstants.Gn_Failed)
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "FleetManagement/SearchPayloadItem,  Failed due to FleetManagement Id invalid");
                return new List<WorkflowVariableModel>();
            }
            return fleetManagementWorkflowService.SearchPayloadItem(new SessionData().Wf_Fm_FleetManagementId, variableName);
        }

        public bool TerminateProcess(string applicationId)
        {
            return fleetManagementWorkflowService.TerminateProcess(applicationId);
        }
    }
}
using Newtonsoft.Json;
using STP.Common.Constants;
using STP.Common.Logger;
using STP.Domain.VehiclesAndFleets;
using STP.Domain.Workflow;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Net.Http;

namespace STP.ServiceAccess.Workflows
{
    public class FleetManagementWorkflowService : IFleetManagementWorkflowService
    {
        readonly IWorkflowService workflowService;

        public FleetManagementWorkflowService()
        {
            HttpClient httpClient = new HttpClient { BaseAddress = new Uri(ConfigurationManager.AppSettings["APIGatewayUrl"]) };
            workflowService = new WorkflowService(httpClient);
        }
        public WorkflowProcessDetails StartProcess(bool isImportFleet, bool setApplicationKey = false, string organizationId = null, string workflowKeyVehicleName = null, decimal esdalKey = 0)
        {
            var workflow = workflowService.GetWorkflowByName(WorkflowProcessKey.Process_Haulier_Fleet_Management_v05.ToString());
            if (workflow != null)
            {
                dynamic startProcessApplicationPayload = new ExpandoObject();
                startProcessApplicationPayload.importComponent = isImportFleet;
                var workflowResponse = workflowService.StartProcess(WorkflowProcessKey.Process_Haulier_Fleet_Management_v05,
                                                                    payload: startProcessApplicationPayload,
                                                                    applicationId: setApplicationKey ? workflowKeyVehicleName : null,
                                                                    orgId: organizationId,
                                                                    subjectId: WorkflowActivityConstants.Wf_Subject_FleetManagement);
                if (workflowResponse != null && workflowResponse.applicationFileId.Length > 1)
                {
                    WorkflowProcessModel workflowProcessModel = new WorkflowProcessModel
                    {
                        PROCESS_ID = workflowResponse.applicationFileId,
                        WFP_ESDALKEY = esdalKey,
                        WFP_STATUS = true,
                        WORKFLOW_ID = workflow.WORKFLOW_ID,
                        WORKFLOW_KEY = workflowKeyVehicleName ?? string.Empty,
                        WORKFLOW_TYPE = 923003
                    };
                    workflowService.SaveWorkflow(workflowProcessModel);
                }
                return workflowResponse;

            }
            return null;
        }
        public bool ProcessActivity(string activityId, VehicleWorkFlowParams vehicleWorkFlowParams, object workflowPayload = null, bool completeTask = true)
        {
            if (activityId.Length == 0)
            {
                return false;
            }
            var nextTaskByWorkflow = workflowService.GetCurrentWorkflowActivity(activityId);
            if (nextTaskByWorkflow==null|| nextTaskByWorkflow.Length == 0)
            {
                return false;
            }
            var nextActivity = nextTaskByWorkflow;

            WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel
            {
                data = vehicleWorkFlowParams ?? new object(),
                taskKey = nextActivity,
                workflowData = workflowPayload ?? new object(),
            };
            return workflowService.ProcessActivity(activityId, workflowActivityPostModel, completeTask);
        }
        public bool ProcessActivity(string activityId, List<VehicleComponentsModel> vehicleComponentsList, object workflowPayload = null, bool completeTask = true)
        {
            if(activityId.Length==0)
            {
                return false;
            }
            var nextTaskByWorkflow = workflowService.GetCurrentWorkflowActivity(activityId);
            if (nextTaskByWorkflow == null || nextTaskByWorkflow.Length == 0)
            {
                return false;
            }
            var nextActivity = nextTaskByWorkflow;

            dynamic objectToPass = new ExpandoObject();
            objectToPass.VehicleComponentsModels = vehicleComponentsList;
            objectToPass.TotalComponents = vehicleComponentsList != null ? vehicleComponentsList.Count : 0;

            WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel
            {
                data = objectToPass,
                taskKey = nextActivity,
                workflowData = workflowPayload ?? new object(),
            };
            return workflowService.ProcessActivity(activityId, workflowActivityPostModel, false);
        }
        public string GetCurrentTask(string processId)
        {
            var nextTaskByWorkflow = workflowService.GetCurrentWorkflowActivity(processId);
            return nextTaskByWorkflow;
        }
        public string LoadActivities(int stepNumber)
        {
            var workflow = workflowService.GetWorkflowByName(WorkflowProcessKey.Process_Haulier_Fleet_Management_v05.ToString());
            if (workflow != null)
            {
                var workflowActivitiesList = workflowService.GetWorkflowActivitiesByWorkflowId(workflow.WORKFLOW_ID).OrderBy(x => x.WFA_ACTIVITYORDER).ToList();
                if (workflowActivitiesList.Count != 0)
                {
                    return workflowActivitiesList.Where(x => x.WFA_ACTIVITYORDER == stepNumber).Select(x => x.ACTIVITY_NAME).SingleOrDefault();
                }
            }
            return string.Empty;
        }
        public List<WorkflowVariableModel> SearchPayloadItem(string processKey, string variableName)
        {
            return workflowService.SearchPayloadItem(processKey, variableName);
        }
        public bool TerminateProcess(string applicationId)
        {
            return workflowService.TerminateProcess(applicationId);
        }
    }
}

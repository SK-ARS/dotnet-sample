using STP.Common.Constants;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.SecurityAndUsers;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.Domain.Workflow;
using STP.Domain.Workflow.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Net.Http;

namespace STP.ServiceAccess.Workflows.ApplicationsNotifications
{
    public class ApplicationNotificationWorkflowService : IApplicationNotificationWorkflowService
    {
        readonly IWorkflowService workflowService;

        public ApplicationNotificationWorkflowService()
        {
            HttpClient httpClient = new HttpClient { BaseAddress = new Uri(ConfigurationManager.AppSettings["APIGatewayUrl"]) };
            workflowService = new WorkflowService(httpClient);
        }
        private long GetWorkFlowId()
        {
            var workflowDetails = workflowService.GetWorkflowByName(WorkflowProcessKey.Process_HauliersNotificationApplicationsCombined07.ToString());
            return workflowDetails != null ? workflowDetails.WORKFLOW_ID : 0;
        }
        
        public WorkflowProcessDetails StartProcess(PlanMvmntPayLoad mvmntPayLoad, int taskId=4)
        {
            dynamic startProcessApplicationPayload = new ExpandoObject();
            mvmntPayLoad.IsSupplimentarySaved = false;
            mvmntPayLoad.IsSoOverView = false;
            mvmntPayLoad.IsNotifGeneralSaved = false;
            string applicationType = string.Empty;
            if (mvmntPayLoad.IsApp)
            {
                applicationType = mvmntPayLoad.IsVr1App ? "VR1 Application" : "SO Application";

            }
            WorkflowLog workflowLog = new WorkflowLog
            {
                WorkflowLogModels = new List<WorkflowLogModel>()
            {
                new WorkflowLogModel()
                {
                ActivityKey = "Process Started",
                ActivityOn = DateTime.Now
                }
            }
            };
            startProcessApplicationPayload.applicationType = applicationType;
            startProcessApplicationPayload.processType = mvmntPayLoad.IsApp ? "Application" : "Notification";
            startProcessApplicationPayload.workflowActivityLog = workflowLog;
            startProcessApplicationPayload.decideNextTask = mvmntPayLoad.NextActivity.Length > 0;
            startProcessApplicationPayload.PlanMvmntPayLoad = mvmntPayLoad;
            startProcessApplicationPayload.nextTaskKey = mvmntPayLoad.NextActivity;
            startProcessApplicationPayload.taskId = taskId;
            var workflowResponse = workflowService.StartProcess(WorkflowProcessKey.Process_HauliersNotificationApplicationsCombined07,
                                                                payload: startProcessApplicationPayload,
                                                                applicationId: null,
                                                                orgId: mvmntPayLoad.OrgId.ToString(),
                                                                subjectId: WorkflowActivityConstants.Wf_Subject_ApplicationNotification);

            if (workflowResponse != null && workflowResponse.applicationFileId.Length > 1)
            {
                WorkflowProcessModel workflowProcessModel = new WorkflowProcessModel
                {
                    PROCESS_ID = workflowResponse.applicationFileId,
                    WFP_ESDALKEY = (int)mvmntPayLoad.MovementKey,
                    WFP_STATUS = true,
                    WORKFLOW_ID = GetWorkFlowId(),
                    WORKFLOW_KEY = mvmntPayLoad.IsApp ? $"A_{mvmntPayLoad.MovementKey}" : $"N_{mvmntPayLoad.MovementKey}",
                    WORKFLOW_TYPE= mvmntPayLoad.IsApp ? 923001: 923002
                };
                workflowService.SaveWorkflow(workflowProcessModel);
            }
            return workflowResponse;
        }
        public bool ProcessActivity(string activityId, WorkflowActivityPostModel workflowActivityPostModel, bool completeTask = true)
        {
            var currentWorkflowActivity = GetCurrentTask(activityId);
            if (currentWorkflowActivity != null && currentWorkflowActivity.Length == 0)
            {
                return false;
            }
            workflowActivityPostModel.taskKey = currentWorkflowActivity;           
            return workflowService.ProcessActivity(activityId, workflowActivityPostModel, completeTask);
        }
        public string GetCurrentTask(string activityId)
        {
            if (activityId.Length == 0)
            {
                return string.Empty;
            }
            var nextTaskByWorkflow = workflowService.GetCurrentWorkflowActivity(activityId);
            return nextTaskByWorkflow;
        }
        public string LoadActivities(int stepNumber)
        {
            var workflow = workflowService.GetWorkflowByName(WorkflowProcessKey.Process_HauliersNotificationApplicationsCombined07.ToString());
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
        public string GetProcessKey(long appRevisionId)
        {
            string urlParameters = "/workflow/process?esdalKey=" + appRevisionId + "&workflowId=" + GetWorkFlowId();
            return workflowService.ExecuteGet(urlParameters);
        }
        public WorkflowProcessModel GetWorkflowDetailsByEsdalKey(long esdalKey)
        {
            var workflow = workflowService.GetWorkflowByName(WorkflowProcessKey.Process_HauliersNotificationApplicationsCombined07.ToString());
            return workflowService.GetWorkflowDetailsByEsdalKey(esdalKey, workflow.WORKFLOW_ID);
        }
        public WorkflowActivityRoute GetWorkflowActivityRoute(string activityName, string processDefenition)
        {
            return workflowService.GetWorkflowActivityRoute(activityName, processDefenition);
        }
        public WorkflowProcessModel CheckIfProcessExit(long esdalKey)
        {
            var workflow = workflowService.GetWorkflowByName(WorkflowProcessKey.Process_HauliersNotificationApplicationsCombined07.ToString());
            WorkflowProcessModel workflowProcessModelRespone = new WorkflowProcessModel();
            if (esdalKey > 0 && workflow != null)
            {
                workflowProcessModelRespone = GetWorkflowDetailsByEsdalKey(esdalKey);
            }
            return workflowProcessModelRespone;
        }
    }
}

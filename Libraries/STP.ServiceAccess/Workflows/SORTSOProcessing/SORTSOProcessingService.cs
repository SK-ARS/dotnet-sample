using STP.Common.Constants;
using STP.Domain.VehiclesAndFleets;
using STP.Domain.Workflow;
using STP.Domain.Workflow.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.Workflows.SORTSOProcessing
{
    public class SORTSOProcessingService : ISORTSOProcessingService
    {
        readonly IWorkflowService workflowService;

        public SORTSOProcessingService()
        {
            HttpClient httpClient = new HttpClient { BaseAddress = new Uri(ConfigurationManager.AppSettings["APIGatewayUrl"]) };
            workflowService = new WorkflowService(httpClient);
        }
        public WorkflowProcessModel CheckIfProcessExit(string esdalReferenceNumber)
        {
            var workflow = workflowService.GetWorkflowByName(WorkflowProcessKey.Process_SORTSOApplicationApprovalV08.ToString());
            WorkflowProcessModel workflowProcessModelRespone = new WorkflowProcessModel();
            if (esdalReferenceNumber.Length > 0 && workflow != null)
            {
                workflowProcessModelRespone = GetWorkflowDetailsByProcessId(esdalReferenceNumber, workflow.WORKFLOW_ID);
            }
            return workflowProcessModelRespone;
        }
        public WorkflowProcessDetails StartProcess(string esdalReferenceNumber, bool setApplicationKey = false, string organizationId = null, decimal esdalKey = 0)
        {
            var workflow = workflowService.GetWorkflowByName(WorkflowProcessKey.Process_SORTSOApplicationApprovalV08.ToString());
            var workflowProcessModelRespone = CheckIfProcessExit(esdalReferenceNumber);

            if ((workflow != null && workflow.WORKFLOW_ID > 0) && (workflowProcessModelRespone.PROCESS_ID == null || workflowProcessModelRespone.PROCESS_ID.Length == 0))
            {
                return StartSOProcess(esdalReferenceNumber, workflow, setApplicationKey, organizationId, esdalKey);
            }
            else if (workflowProcessModelRespone.PROCESS_ID != null && workflowProcessModelRespone.PROCESS_ID.Length > 0)
            {
                var currentTask = GetCurrentTask(esdalReferenceNumber);
                if (currentTask == null)
                {
                    return StartSOProcess(esdalReferenceNumber, workflow, setApplicationKey, organizationId, esdalKey);
                }
                WorkflowProcessDetails workflowProcessDetails = new WorkflowProcessDetails()
                {
                    applicationFileId = workflowProcessModelRespone.PROCESS_ID
                };
                return workflowProcessDetails;
            }
            return null;
        }
        private WorkflowProcessDetails StartSOProcess(string esdalReferenceNumber, WorkflowModel workflow, bool setApplicationKey = false, string organizationId = null, decimal esdalKey = 0)
        {
            dynamic startProcessApplicationPayload = new ExpandoObject();
            var workflowResponse = workflowService.StartProcess(WorkflowProcessKey.Process_SORTSOApplicationApprovalV08,
                                                                payload: startProcessApplicationPayload,
                                                                applicationId: setApplicationKey ? esdalReferenceNumber : null,
                                                                orgId: organizationId,
                                                                subjectId: WorkflowActivityConstants.Wf_Subject_SortSoApplicationProcess);
            if (workflowResponse != null && workflowResponse.applicationFileId.Length > 1)
            {
                WorkflowProcessModel workflowProcessModel = new WorkflowProcessModel
                {
                    PROCESS_ID = workflowResponse.applicationFileId,
                    WFP_ESDALKEY = esdalKey,
                    WFP_STATUS = true,
                    WORKFLOW_ID = workflow.WORKFLOW_ID,
                    WORKFLOW_KEY = esdalReferenceNumber ?? string.Empty
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
            var workflow = workflowService.GetWorkflowByName(WorkflowProcessKey.Process_SORTSOApplicationApprovalV08.ToString());
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
        public WorkflowActivityRoute GetWorkflowActivityRoute(string activityName, string processDefenition)
        {
            return workflowService.GetWorkflowActivityRoute(activityName, processDefenition);
        }
        public List<WorkflowVariableModel> SearchPayloadItem(string processKey, string variableName)
        {
            return workflowService.SearchPayloadItem(processKey, variableName);
        }
        public WorkflowProcessModel GetWorkflowDetailsByProcessId(string processId, long workflowId)
        {
            return workflowService.GetWorkflowDetailsByProcessId(processId, workflowId);
        }
    }
}

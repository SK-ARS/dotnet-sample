using Newtonsoft.Json;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.Domain.MovementsAndNotifications.Movements;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.Domain.Workflow;
using STP.Domain.Workflow.Models;
using STP.ServiceAccess.Workflows.ApplicationsNotifications;
using System;
using System.Collections.Generic;

namespace STP.Web.WorkflowProvider
{
    public class ApplicationNotificationManagement
    {
        private readonly IApplicationNotificationWorkflowService applicationNotificationWorkflowService;
        public ApplicationNotificationManagement(IApplicationNotificationWorkflowService applicationNotificationWorkflowService)
        {
            this.applicationNotificationWorkflowService = applicationNotificationWorkflowService;
        }
        public string StartWorkflow(PlanMvmntPayLoad mvmntPayLoad, int taskId=4)
        {
            try
            {
                WorkflowProcessDetails workflowProcessDetails = applicationNotificationWorkflowService
                    .StartProcess(mvmntPayLoad, taskId);


                new SessionData().Wf_An_ApplicationWorkflowId = workflowProcessDetails == null || workflowProcessDetails.applicationFileId == null || workflowProcessDetails.applicationFileId.Length == 0
                    ? WorkflowActivityConstants.Gn_Failed
                    : workflowProcessDetails.applicationFileId;

                new SessionData().Wf_An_CurrentExecuted = WorkflowActivityTypes.An_Activity_ConfirmMovementType;
                return new SessionData().Wf_An_ApplicationWorkflowId;
            }
            catch (Exception ex)
            {
                new SessionData().Wf_An_ApplicationWorkflowId = WorkflowActivityConstants.Gn_Failed;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Workflow/ApplicationNotification/StartWorkflow,  Application Notification Workflow Application Id not found", ex.Message));
            }
            return string.Empty;
        }
        public bool ProcessWorkflowActivity(string sessionId, WorkflowActivityPostModel workflowActivityPostModel, bool completeTask = true)
        {
            if (new SessionData().Wf_An_ApplicationWorkflowId == WorkflowActivityConstants.Gn_Failed)
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Workflow/ApplicationNotification/ProcessWorkflowActivity,  Failed due to ApplicationNotification Id invalid", sessionId));
                return false;
            }
            var currentActivity = applicationNotificationWorkflowService.GetCurrentTask(new SessionData().Wf_An_ApplicationWorkflowId);
            //if (currentActivity == "TheEnd")
            //{
            //    var iamhere = "Yes";
            //}
            if (new SessionData().Wf_An_ApplicationWorkflowId != null
                && new SessionData().Wf_An_ApplicationWorkflowId.Length > 0)
            {
                return applicationNotificationWorkflowService.ProcessActivity(new SessionData().Wf_An_ApplicationWorkflowId, workflowActivityPostModel, completeTask);
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Workflow/ApplicationNotification/ProcessWorkflowActivity,  Application Notification Workflow Application Id not found", sessionId));
            }

            return false;
        }
        public string GetCurrentActivity()
        {
            if (new SessionData().Wf_An_ApplicationWorkflowId != null
              && new SessionData().Wf_An_ApplicationWorkflowId.Length > 0)
            {
                return applicationNotificationWorkflowService.GetCurrentTask(new SessionData().Wf_An_ApplicationWorkflowId);
            }
            return string.Empty;
        }
        public List<WorkflowVariableModel> SearchPayloadItem(string variableName)
        {
            if (new SessionData().Wf_An_ApplicationWorkflowId == WorkflowActivityConstants.Gn_Failed)
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "ApplicationNotificationManagement/SearchPayloadItem,  Failed due to ApplicationNotificationManagement Id invalid");
                return new List<WorkflowVariableModel>();
            }
            return applicationNotificationWorkflowService.SearchPayloadItem(new SessionData().Wf_An_ApplicationWorkflowId, variableName);
        }
        public PlanMvmntPayLoad GetPlanMvmtPayload()
        {
            PlanMvmntPayLoad mvmntPayLoad = new PlanMvmntPayLoad();
            var payloadData = SearchPayloadItem("PlanMvmntPayLoad");
            if (payloadData != null)
            {
                foreach (WorkflowVariableModel workflowVariableModel in payloadData)
                {
                    mvmntPayLoad = JsonConvert.DeserializeObject<PlanMvmntPayLoad>(workflowVariableModel.value);
                }
            }
            mvmntPayLoad.NextAction = GetTaskId();
            new SessionData().E4_AN_PlanMovement = mvmntPayLoad;
            return mvmntPayLoad;
        }
        public bool GetSupplimentaryOverviewStatus(bool supplimetaryDetails)
        {
            bool resultFlag = false;
            var payloadData = SearchPayloadItem(supplimetaryDetails ? "IsSupplimentarySaved" : "IsOverviewSaved");
            if (payloadData != null)
            {
                foreach (WorkflowVariableModel workflowVariableModel in payloadData)
                {
                    resultFlag = JsonConvert.DeserializeObject<bool>(workflowVariableModel.value);
                }
            }
            return resultFlag;
        }
        public double GetTaskId()
        {
            double taskId = 0;
            var payloadData = SearchPayloadItem("taskId");
            if (payloadData != null)
            {
                foreach (WorkflowVariableModel workflowVariableModel in payloadData)
                {
                    taskId = JsonConvert.DeserializeObject<double>(workflowVariableModel.value);
                }
            }
            return taskId;
        }
        public WorkflowLog SetWorkflowLog(string activityKey)
        {
            try
            {
                var workflowLog = GetWorkflowLog();
                if (workflowLog != null)
                {
                    if (workflowLog.WorkflowLogModels != null)
                    {
                        workflowLog.WorkflowLogModels.Add(new WorkflowLogModel()
                        {
                            ActivityKey = activityKey,
                            ActivityOn = DateTime.Now
                        });
                    }
                    else
                    {
                        workflowLog.WorkflowLogModels = new List<WorkflowLogModel>()
                            {
                                new WorkflowLogModel()
                                {
                                    ActivityKey = activityKey,
                                    ActivityOn = DateTime.Now
                                }
                            };
                    }
                }
                else
                {
                    workflowLog = new WorkflowLog
                    {
                        WorkflowLogModels = new List<WorkflowLogModel>()
                            {
                                new WorkflowLogModel()
                                {
                                    ActivityKey = activityKey,
                                    ActivityOn = DateTime.Now
                                }
                            }
                    };
                }
                return workflowLog;
            }
            catch
            {
                return new WorkflowLog();
            }
        }
        private WorkflowLog GetWorkflowLog()
        {
            var workflowLogPayload = SearchPayloadItem("workflowActivityLog");
            WorkflowLog workflowLog = new WorkflowLog();
            if (workflowLogPayload != null)
            {
                foreach (WorkflowVariableModel workflowVariableModel in workflowLogPayload)
                {
                    workflowLog = JsonConvert.DeserializeObject<WorkflowLog>(workflowVariableModel.value);
                }
            }
            return workflowLog;
        }
        public WorkflowProcessModel GetWorkflowDetailsByEsdalKey(long esdalKey)
        {
            return applicationNotificationWorkflowService.GetWorkflowDetailsByEsdalKey(esdalKey);
        }
        public bool IsThisMovementExist(long notificationId, long applicationId, out string workflowKey)
        {
            workflowKey = string.Empty;
            var response = GetWorkflowDetailsByEsdalKey(notificationId > 0 ? notificationId : applicationId);
            if (response != null && response.WFP_ESDALKEY > 0)
            {
                workflowKey = response.PROCESS_ID;
                var typeApplication = response.WORKFLOW_KEY.ToLower().Substring(0, 2);
                if (applicationId > 0 && typeApplication.Equals("a_"))
                {
                    return true;
                }
                else if (notificationId > 0 && typeApplication.Equals("n_"))
                {
                    return true;

                }

            }
            return false;
        }

        public string GetProcessKey(long appRevisionId)
        {
            return applicationNotificationWorkflowService.GetProcessKey(appRevisionId);
        }
        public WorkflowActivityRoute GetWorkflowActivityRoute(string activityName, string processDefenition)
        {
            return applicationNotificationWorkflowService.GetWorkflowActivityRoute(activityName, processDefenition);
        }
    }
}
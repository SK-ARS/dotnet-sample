using Newtonsoft.Json;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.Domain.Workflow;
using STP.Domain.Workflow.Models;
using STP.ServiceAccess.Workflows;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace STP.Web.WorkflowProvider
{
    public class SOAPoliceNotificationManagement
    {
        private readonly ISOAPoliceWorkflowService soaPoliceProcessingService;
        public SOAPoliceNotificationManagement(ISOAPoliceWorkflowService soaPoliceProcessingService)
        {
            this.soaPoliceProcessingService = soaPoliceProcessingService;
        }
        public bool CheckIfProcessExit(string notificationNumber, long organizationId = -1, decimal esdalKey = 0, bool startProcess = true)
        {
            if (notificationNumber.Length > 0)
            {
                var workflowProcessModel = soaPoliceProcessingService.CheckIfProcessExit(notificationNumber);
                if (workflowProcessModel != null && workflowProcessModel.PROCESS_ID != null && workflowProcessModel.PROCESS_ID.Length > 0)
                {
                    new SessionData().Wf_Ap_SOANotificationWorkflowId = workflowProcessModel.PROCESS_ID;
                    var nextTask = GetCurrentActivity(notificationNumber);
                    if (nextTask == null && startProcess && StartWorkflow(organizationId, "", true, notificationNumber, esdalKey).Length > 2
                        && WorkflowTaskFinder.FindNextTask("SOANotification", WorkflowActivityTypes.Sp_Activity_SelectaNotification, out dynamic workflowPayload, false) != string.Empty)
                    {
                        dynamic dataPayload = new ExpandoObject();
                        dataPayload.workflowActivityLog = SetWorkflowLog("STARTED ON MIDDLE");
                        WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                        {
                            data = dataPayload,
                            workflowData = workflowPayload
                        };
                        ProcessWorkflowActivity(string.Empty, workflowActivityPostModel, true);
                    }
                    return true;
                }
                else if (startProcess && StartWorkflow(organizationId, "", true, notificationNumber, esdalKey).Length > 2)
                {
                    if (WorkflowTaskFinder.FindNextTask("SAP", WorkflowActivityTypes.Sp_Activity_SelectaNotification, out dynamic workflowPayload, false) != string.Empty)
                    {

                        dynamic dataPayload = new ExpandoObject();
                        dataPayload.workflowActivityLog = SetWorkflowLog("STARTED ON MIDDLE");
                        WorkflowActivityPostModel workflowActivityPostModel = new WorkflowActivityPostModel()
                        {
                            data = dataPayload,
                            workflowData = workflowPayload
                        };
                        ProcessWorkflowActivity(string.Empty, workflowActivityPostModel, true);
                    }
                    return true;
                }
            }
            return false;
        }
        public string StartWorkflow(long organizationId, string organizationName, bool setApplication, string notificationNumber = "", decimal esdalKey = 0)
        {
            try
            {

                WorkflowProcessDetails workflowProcessDetails = soaPoliceProcessingService
                    .StartProcess(notificationNumber, setApplication, organizationId.ToString(), esdalKey);

                new SessionData().Wf_Ap_SOANotificationWorkflowId = workflowProcessDetails == null || workflowProcessDetails.applicationFileId == null || workflowProcessDetails.applicationFileId.Length == 0
                    ? WorkflowActivityConstants.Gn_Failed
                    : workflowProcessDetails.applicationFileId;
                new SessionData().Wf_Ap_CurrentExecuted = WorkflowActivityTypes.Sp_Activity_SelectaNotification;
                return new SessionData().Wf_Ap_SOANotificationWorkflowId;
            }
            catch (Exception ex)
            {
                new SessionData().Wf_Ap_SOANotificationWorkflowId = WorkflowActivityConstants.Gn_Failed;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Workflow/SOANotification/StartWorkflow,  SOANotification Workflow Application Id not found", ex.Message));
            }
            return string.Empty;
        }
        public bool ProcessWorkflowActivity(string sessionId, WorkflowActivityPostModel workflowActivityPostModel, bool completeTask = true)
        {

            if (new SessionData().Wf_Ap_SOANotificationWorkflowId == WorkflowActivityConstants.Gn_Failed)
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Workflow/SOANotification/ProcessWorkflowActivity,  Failed due to Application Id invalid", sessionId));
                return false;
            }
            if (new SessionData().Wf_Ap_SOANotificationWorkflowId != null
                && new SessionData().Wf_Ap_SOANotificationWorkflowId.Length > 0)
            {
                return soaPoliceProcessingService.ProcessActivity(new SessionData().Wf_Ap_SOANotificationWorkflowId, workflowActivityPostModel, completeTask);

            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Workflow/SOANotification/ProcessWorkflowActivity,  SOANotification Workflow Application Id not found", sessionId));
            }

            return false;
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
        public List<WorkflowVariableModel> SearchPayloadItem(string variableName)
        {
            if (new SessionData().Wf_Ap_SOANotificationWorkflowId == WorkflowActivityConstants.Gn_Failed)
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "SORTSOApplicationManagement/SearchPayloadItem,  Failed due to SORTSOApplication Id invalid");
                return new List<WorkflowVariableModel>();
            }
            return soaPoliceProcessingService.SearchPayloadItem(new SessionData().Wf_Ap_SOANotificationWorkflowId, variableName);
        }
        public WorkflowActivityRoute GetWorkflowActivityRoute(string activityName, string processDefenition)
        {
            return soaPoliceProcessingService.GetWorkflowActivityRoute(activityName, processDefenition);
        }
        public string GetCurrentActivity(string esdalReferenceNumber)
        {
            return soaPoliceProcessingService.GetCurrentTask(esdalReferenceNumber);
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
    }
}
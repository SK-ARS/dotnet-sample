using Newtonsoft.Json;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.Domain.Applications;
using STP.Domain.Workflow;
using STP.Domain.Workflow.Models;
using STP.ServiceAccess.Workflows.ApplicationsNotifications;
using STP.ServiceAccess.Workflows.SORTSOProcessing;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace STP.Web.WorkflowProvider
{
    public class SORTSOApplicationManagement
    {
        private readonly ISORTSOProcessingService sortSOProcessingService;
        public SORTSOApplicationManagement(ISORTSOProcessingService sortSOProcessingService)
        {
            this.sortSOProcessingService = sortSOProcessingService;
        }
        public bool CheckIfProcessExit(string esdalReferenceNumber, long organizationId = -1, decimal esdalKey = 0, bool startProcess = true)
        {
            if (esdalReferenceNumber.Length > 0)
            {
                var workflowProcessModel = sortSOProcessingService.CheckIfProcessExit(esdalReferenceNumber);
                if (workflowProcessModel != null && workflowProcessModel.PROCESS_ID != null && workflowProcessModel.PROCESS_ID.Length > 0)
                {
                    new SessionData().Wf_Ap_SortSoProcessingWorkflowId = workflowProcessModel.PROCESS_ID;
                    var nextTask = GetCurrentActivity(esdalReferenceNumber);
                    if (nextTask == null && startProcess && StartWorkflow(organizationId, "", true, esdalReferenceNumber, esdalKey).Length > 2 
                        && WorkflowTaskFinder.FindNextTask("SAP", WorkflowActivityTypes.Ap_Activity_AllocateApplication2RoutingOfficers, out dynamic workflowPayload, false) != string.Empty)
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
                else if (startProcess && StartWorkflow(organizationId, "", true, esdalReferenceNumber, esdalKey).Length > 2)
                {
                    if (WorkflowTaskFinder.FindNextTask("SAP", WorkflowActivityTypes.Ap_Activity_AllocateApplication2RoutingOfficers, out dynamic workflowPayload, false) != string.Empty)
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
        public string StartWorkflow(long organizationId, string organizationName, bool setApplication, string esdalReferenceNumber = "", decimal esdalKey = 0)
        {
            try
            {

                WorkflowProcessDetails workflowProcessDetails = sortSOProcessingService
                    .StartProcess(esdalReferenceNumber, setApplication, organizationId.ToString(), esdalKey);

                new SessionData().Wf_Ap_SortSoProcessingWorkflowId = workflowProcessDetails == null || workflowProcessDetails.applicationFileId == null || workflowProcessDetails.applicationFileId.Length == 0
                    ? WorkflowActivityConstants.Gn_Failed
                    : workflowProcessDetails.applicationFileId;
                new SessionData().Wf_Ap_CurrentExecuted = WorkflowActivityTypes.Ap_Activity_AllocateApplication2RoutingOfficers;
                return new SessionData().Wf_Ap_SortSoProcessingWorkflowId;
            }
            catch (Exception ex)
            {
                new SessionData().Wf_Ap_SortSoProcessingWorkflowId = WorkflowActivityConstants.Gn_Failed;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Workflow/SORTSOProcessing/StartWorkflow,  SORTSOProcessing Workflow Application Id not found", ex.Message));
            }
            return string.Empty;
        }
        public bool ProcessWorkflowActivity(string sessionId, WorkflowActivityPostModel workflowActivityPostModel, bool completeTask = true)
        {

            if (new SessionData().Wf_Ap_SortSoProcessingWorkflowId == WorkflowActivityConstants.Gn_Failed)
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Workflow/SORTSOProcessing/ProcessWorkflowActivity,  Failed due to Application Id invalid", sessionId));
                return false;
            }
            if (new SessionData().Wf_Ap_SortSoProcessingWorkflowId != null
                && new SessionData().Wf_Ap_SortSoProcessingWorkflowId.Length > 0)
            {
                return sortSOProcessingService.ProcessActivity(new SessionData().Wf_Ap_SortSoProcessingWorkflowId, workflowActivityPostModel, completeTask);

            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Workflow/SORTSOProcessing/ProcessWorkflowActivity,  SORTSOProcessing Workflow Application Id not found", sessionId));
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
            if (new SessionData().Wf_Ap_SortSoProcessingWorkflowId == WorkflowActivityConstants.Gn_Failed)
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "SORTSOApplicationManagement/SearchPayloadItem,  Failed due to SORTSOApplication Id invalid");
                return new List<WorkflowVariableModel>();
            }
            return sortSOProcessingService.SearchPayloadItem(new SessionData().Wf_Ap_SortSoProcessingWorkflowId, variableName);
        }
        public WorkflowActivityRoute GetWorkflowActivityRoute(string activityName, string processDefenition)
        {
            return sortSOProcessingService.GetWorkflowActivityRoute(activityName, processDefenition);
        }
        public string GetCurrentActivity(string esdalReferenceNumber)
        {
            return sortSOProcessingService.GetCurrentTask(esdalReferenceNumber);
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
        public SortAppPayload GetSortAppPayload()
        {
            SortAppPayload sortAppPayload = new SortAppPayload();
            var payloadData = SearchPayloadItem("SortAppPayload");
            if (payloadData != null)
            {
                foreach (WorkflowVariableModel workflowVariableModel in payloadData)
                {
                    sortAppPayload = JsonConvert.DeserializeObject<SortAppPayload>(workflowVariableModel.value);
                }
            }
            return sortAppPayload;
        }

    }
}
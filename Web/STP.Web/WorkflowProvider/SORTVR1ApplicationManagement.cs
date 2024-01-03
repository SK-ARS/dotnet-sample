using Newtonsoft.Json;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.Domain.Applications;
using STP.Domain.Workflow;
using STP.Domain.Workflow.Models;
using STP.ServiceAccess.Workflows.SORTVR1Processing;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace STP.Web.WorkflowProvider
{
    public class SORTVR1ApplicationManagement
    {
        private readonly ISORTVR1ProcessingService sortVR1ProcessingService;
        public SORTVR1ApplicationManagement(ISORTVR1ProcessingService sortVR1ProcessingService)
        {
            this.sortVR1ProcessingService = sortVR1ProcessingService;
        }

        public bool CheckIfProcessExit(string esdalReferenceNumber, long organizationId = -1, decimal esdalKey = 0, bool startProcess = true)
        {
            if (esdalReferenceNumber.Length > 0)
            {
                var workflowProcessModel = sortVR1ProcessingService.CheckIfProcessExit(esdalReferenceNumber);
                if (workflowProcessModel != null && workflowProcessModel.PROCESS_ID != null && workflowProcessModel.PROCESS_ID.Length > 0)
                {
                    new SessionData().Wf_Ap_SortVR1ProcessingWorkflowId = workflowProcessModel.PROCESS_ID;
                    var nextTask = GetCurrentActivity(esdalReferenceNumber);
                    if (nextTask == null && startProcess && StartWorkflow(organizationId, "", esdalReferenceNumber != "", esdalReferenceNumber, esdalKey).Length > 2
                        && WorkflowTaskFinder.FindNextTask("SVR1P", WorkflowActivityTypes.Vr_Activity_AllocateApplication2RoutingOfficers, out dynamic workflowPayload, false) != string.Empty)
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
                else if (startProcess && StartWorkflow(organizationId, "", esdalReferenceNumber != "", esdalReferenceNumber, esdalKey).Length > 2)
                {
                    if (WorkflowTaskFinder.FindNextTask("SVR1P", WorkflowActivityTypes.Vr_Activity_AllocateApplication2RoutingOfficers, out dynamic workflowPayload, false) != string.Empty)
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

                WorkflowProcessDetails workflowProcessDetails = sortVR1ProcessingService
                    .StartProcess(esdalReferenceNumber, setApplication, organizationId.ToString(), esdalKey);

                new SessionData().Wf_Ap_SortVR1ProcessingWorkflowId = workflowProcessDetails == null || workflowProcessDetails.applicationFileId == null || workflowProcessDetails.applicationFileId.Length == 0
                    ? WorkflowActivityConstants.Gn_Failed
                    : workflowProcessDetails.applicationFileId;
                new SessionData().Wf_Ap_VR1CurrentExecuted = WorkflowActivityTypes.Vr_Activity_AllocateApplication2RoutingOfficers;
                return new SessionData().Wf_Ap_SortVR1ProcessingWorkflowId;
            }
            catch (Exception ex)
            {
                new SessionData().Wf_Ap_SortVR1ProcessingWorkflowId = WorkflowActivityConstants.Gn_Failed;
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Workflow/SORTVR1Processing/StartWorkflow,  SORTVR1Processing Workflow Application Id not found", ex.Message));
            }
            return string.Empty;
        }
        public string GetCurrentActivity(string esdalReferenceNumber)
        {
            return sortVR1ProcessingService.GetCurrentTask(esdalReferenceNumber);
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
        public bool ProcessWorkflowActivity(string sessionId, WorkflowActivityPostModel workflowActivityPostModel, bool completeTask = true)
        {

            if (new SessionData().Wf_Ap_SortVR1ProcessingWorkflowId == WorkflowActivityConstants.Gn_Failed)
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Workflow/SORTVR1Processing/ProcessWorkflowActivity,  Failed due to Application Id invalid", sessionId));
                return false;
            }
            if (new SessionData().Wf_Ap_SortVR1ProcessingWorkflowId != null
                && new SessionData().Wf_Ap_SortVR1ProcessingWorkflowId.Length > 0)
            {
                return sortVR1ProcessingService.ProcessActivity(new SessionData().Wf_Ap_SortVR1ProcessingWorkflowId, workflowActivityPostModel, completeTask);

            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("[{0}],Workflow/SORTVR1Processing/ProcessWorkflowActivity,  SORTVR1Processing Workflow Application Id not found", sessionId));
            }

            return false;
        }
        public WorkflowActivityRoute GetWorkflowActivityRoute(string activityName, string processDefenition)
        {
            return sortVR1ProcessingService.GetWorkflowActivityRoute(activityName, processDefenition);
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
        public List<WorkflowVariableModel> SearchPayloadItem(string variableName)
        {
            if (new SessionData().Wf_Ap_SortVR1ProcessingWorkflowId == WorkflowActivityConstants.Gn_Failed)
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, "SORTVR1Application/SearchPayloadItem,  Failed due to SORTVR1Application Id invalid");
                return new List<WorkflowVariableModel>();
            }
            return sortVR1ProcessingService.SearchPayloadItem(new SessionData().Wf_Ap_SortVR1ProcessingWorkflowId, variableName);
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
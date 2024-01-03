using Newtonsoft.Json;
using STP.Common.Logger;
using STP.Domain.VehiclesAndFleets;
using STP.Domain.Workflow;
using STP.Domain.Workflow.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace STP.ServiceAccess.Workflows
{
    public class WorkflowService : IWorkflowService
    {
        private readonly HttpClient httpClient;
        const string m_RouteName = " -WorkflowService";
        public WorkflowService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public WorkflowProcessDetails StartProcess(WorkflowProcessKey workflowProcessKey, object payload = null, string applicationId = null, string orgId = null, string subjectId = null, bool removeMiddleware = false)
        {
            return StartProcess(workflowProcessKey.ToString(), payload, applicationId, orgId, subjectId,
                                removeMiddleware);
        }
        public WorkflowProcessDetails StartProcess(string workflowProcessName, object payload = null, string applicationId = null, string orgId = null, string subjectId = null, bool removeMiddleware = false)
        {
            WorkflowProcessDetails workflowProcessDetails = new WorkflowProcessDetails();
            try
            {
                WorkflowInitializeModel workflowInitializeModel = new WorkflowInitializeModel()
                {
                    applicationFileId = applicationId ?? "",
                    processKey = workflowProcessName,
                    startProcessPayload = payload,
                    orgId = orgId,
                    subjectId = subjectId

                };                
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                            $"{ConfigurationManager.AppSettings["Workflow"]}" +
                            $"/workflow",
                            workflowInitializeModel).Result;

                if (response.IsSuccessStatusCode)
                {
                    workflowProcessDetails = response.Content.ReadAsAsync<WorkflowProcessDetails>().Result;
                }
                else
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        var jsonValue = JsonConvert.SerializeObject(workflowInitializeModel);
                    }
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/StartProcess, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/StartProcess, Exception: {0}", ex);
            }
            return workflowProcessDetails;
        }
        public WorkflowActivityResponseModel SearchTask(string activityId, bool removeMiddleware = false)
        {
            string urlParameters = $"/{activityId}";
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Workflow"]}" +
                            $"/workflow/tasks{ urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<WorkflowActivityResponseModel>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/SearchTask, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return null;
        }
        public List<WorkflowVariableModel> SearchPayloadItem(string processKey, string variableName)
        {
            List<WorkflowVariableModel> listWorkflowVariableModel = new List<WorkflowVariableModel>();
            string urlParameters = variableName.Length > 0 ? $"?variableName={variableName}" : "";
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Workflow"]}" +
                            $"/workflow/variables/{processKey}{urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                var workflowVariableResponseModel = response.Content.ReadAsAsync<WorkflowVariableResponseModel>().Result;
                listWorkflowVariableModel = workflowVariableResponseModel.content;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/SearchPayloadItem, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return listWorkflowVariableModel;
        }
        public string ExecuteGet(string urlEndpointWithParameter)
        {

            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["WorkflowInternal"]}" +
                            $"{urlEndpointWithParameter}").Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/ExecuteGet, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return null;
        }
        
        public string ExecutePost(string urlEndpoint, string parameterJsonString)
        {
            var content = new StringContent(parameterJsonString, Encoding.UTF8, "application/json");
            HttpResponseMessage response = httpClient.PostAsync(
                              $"{ConfigurationManager.AppSettings["Workflow"]}" +
                              $"{urlEndpoint}",
                              content).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content != null ? response.Content.ReadAsStringAsync().Result : string.Empty;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/StartProcess, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                return string.Empty;
            }
        }
        public bool SaveWorkflow(WorkflowProcessModel workflowProcessModel, bool removeMiddleware = false)
        {
            try
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                               $"{(removeMiddleware ? string.Empty : ConfigurationManager.AppSettings["WorkflowInternal"])}" +
                               $"/workflow/process",
                               workflowProcessModel).Result;

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/StartProcess, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/StartProcess, Exception: {0}", ex);
            }
            return false;
        }
        public WorkflowProcessModel GetWorkflowDetailsByProcessId(string processId, long workflowId)
        {
            string urlParameters = $"?processId={processId}&workflowId={workflowId}";
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["WorkflowInternal"]}" +
                            $"/workflow/process/id{ urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<WorkflowProcessModel>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/GetWorkflowByName, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return null;
        }
        public WorkflowProcessModel GetWorkflowDetailsByEsdalKey(long esdalKey, long workflowId)
        {
            string urlParameters = $"?esdalKey={esdalKey}&workflowId={workflowId}";
            HttpResponseMessage response = httpClient.GetAsync($"{ConfigurationManager.AppSettings["WorkflowInternal"]}" +
                            $"/workflow/process{ urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<WorkflowProcessModel>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/GetWorkflowByName, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return null;
        }
        public WorkflowModel GetWorkflowByName(string workflowName, bool updateStartName = false, bool removeMiddleware = false)
        {
            string urlParameters = $"?workflowName={workflowName}&updateStartName={updateStartName}";
            HttpResponseMessage response = httpClient.GetAsync($"{(removeMiddleware ? string.Empty : ConfigurationManager.AppSettings["WorkflowInternal"])}" +
                            $"/workflow/name{ urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<WorkflowModel>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/GetWorkflowByName, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return null;
        }
        public WorkflowActivityRoute GetWorkflowActivityRoute(string activityName, string processDefenition)
        {
            //curl -X GET "http://10.10.6.6:8080/workflow/routes?activity=Activity_AllocateApplication2RoutingOfficers&processDef=Process_SORTSOApplicationApprovalV08" -H "accept: */*"
            string urlParameters = $"?activity={activityName}&processDef={processDefenition}";
            HttpResponseMessage response = httpClient.GetAsync($"{(ConfigurationManager.AppSettings["Workflow"])}" +
                            $"/workflow/routes{ urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsAsync<List<WorkflowActivityRoute>>().Result;
                if (result != null)
                    return result.FirstOrDefault();
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/GetWorkflowActivityRoute, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return null;
        }
        public List<WorkflowActivityModel> GetWorkflowActivitiesByWorkflowId(long workflowId, bool removeMiddleware = false)
        {
            string urlParameters = $"?workflowId={workflowId}";
            HttpResponseMessage response = httpClient.GetAsync($"{(removeMiddleware ? string.Empty : ConfigurationManager.AppSettings["WorkflowInternal"])}" +
                            $"/workflow/activities{ urlParameters}").Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<List<WorkflowActivityModel>>().Result;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/GetWorkflowActivitiesByWorkflowId, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return new List<WorkflowActivityModel>();
        }

        public bool TerminateProcess(string applicationId)
        {
            HttpResponseMessage response = httpClient.PostAsJsonAsync(
                                $"{ConfigurationManager.AppSettings["Workflow"]}" +
                                $"/workflow/suspend-by-bk/{applicationId}","").Result;

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/TerminateProcess, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
            return false;
        }

        public bool ProcessActivity(string applicationId, WorkflowActivityPostModel workflowActivityPostModel, bool completeTask = true)
        {
            try
            {
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    var payloadJson = JsonConvert.SerializeObject(workflowActivityPostModel);
                }
                HttpResponseMessage response = httpClient.PostAsJsonAsync(
                                $"{ConfigurationManager.AppSettings["Workflow"]}" +
                                $"/workflow/tasks/{applicationId}?completeTask={completeTask}",
                                workflowActivityPostModel).Result;

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/CompleteActivity, Error: {(int)response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{m_RouteName }/CompleteActivity, Exception: {0}", ex);
            }
            return false;
        }
        public string GetCurrentWorkflowActivity(string activityId, bool removeMiddleware = false)
        {
            if (activityId == null)
            {
                return null;
            }
            var responseList = SearchTask(activityId);
            if (responseList == null || !responseList.content.Any())
            {
                return null;
            }
            return responseList.content[0].activityKey;
        }
        public string SetWorkflowOrder(WorkflowActivityOrderPostModel workflowActivityOrderPostModel)
        {
            return ExecutePost("/default-activity-order", JsonConvert.SerializeObject(workflowActivityOrderPostModel));
        }

    }
}

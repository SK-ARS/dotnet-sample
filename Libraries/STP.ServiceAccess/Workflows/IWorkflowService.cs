using STP.Domain.Workflow;
using STP.Domain.Workflow.Models;
using System.Collections.Generic;

namespace STP.ServiceAccess.Workflows
{
    public interface IWorkflowService
    {
        WorkflowProcessDetails StartProcess(WorkflowProcessKey workflowProcessKey, object payload = null, string applicationId = null, string orgId = null, string subjectId = null, bool removeMiddleware = false);
        WorkflowProcessDetails StartProcess(string workflowProcessName, object payload = null, string applicationId = null, string orgId = null, string subjectId = null, bool removeMiddleware = false);
        WorkflowActivityResponseModel SearchTask(string activityId, bool removeMiddleware = false);
        string GetCurrentWorkflowActivity(string activityId, bool removeMiddleware = false);
        WorkflowModel GetWorkflowByName(string workflowName, bool updateStartName = false, bool removeMiddleware = false);
        List<WorkflowActivityModel> GetWorkflowActivitiesByWorkflowId(long workflowId, bool removeMiddleware = false);       
        bool ProcessActivity(string applicationId, WorkflowActivityPostModel workflowActivityPostModel, bool completeTask = true);
        bool SaveWorkflow(WorkflowProcessModel workflowProcessModel, bool removeMiddleware = false);
        string ExecuteGet(string urlEndpointWithParameter);
        string ExecutePost(string urlEndpoint, string parameterJsonString);
        string SetWorkflowOrder(WorkflowActivityOrderPostModel workflowActivityOrderPostModel);
        List<WorkflowVariableModel> SearchPayloadItem(string processKey, string variableName);
        WorkflowProcessModel GetWorkflowDetailsByProcessId(string processId, long workflowId);
        WorkflowProcessModel GetWorkflowDetailsByEsdalKey(long esdalKey, long workflowId);
        WorkflowActivityRoute GetWorkflowActivityRoute(string activityName, string processDefenition);
        bool TerminateProcess(string applicationId);
    }
}

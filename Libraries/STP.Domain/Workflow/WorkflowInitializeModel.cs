using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.Workflow
{
    public class WorkflowInitializeModel
    {
        public string applicationFileId { get; set; }
        public string orgId { get; set; }
        public string processKey { get; set; }
        public dynamic startProcessPayload { get; set; }
        public string subjectId { get; set; }
    }
    public class WorkflowActivityPostDataModel
    {
        public bool vehicleConfigurationType { get; set; }
        public bool movementType { get; set; }
        public bool vehicleInfoValid { get; set; }
        public object formData { get; set; }
        public string nextTaskKey { get; set; }
    }
    public class WorkflowActivityPostModel
    {
        public string taskKey { get; set; }
        public object data { get; set; }
        public dynamic workflowData { get; set; }
    }
    public class WorkflowActivityResponseModel
    {

        public WorkflowActivityResponseContentModel[] content { get; set; }
        public int page { get; set; }
        public int size { get; set; }
        public int totalElements { get; set; }
        public int totalPages { get; set; }
        public bool last { get; set; }
    }
    public class WorkflowVariableModel
    {
        public string name { get; set; }
        public string type { get; set; }
        public string value { get; set; }
        public string processInstanceId { get; set; }
    }
    
    public class WorkflowVariableResponseModel
    {
        public List<WorkflowVariableModel> content { get; set; }
        public bool last { get; set; }
        public int size { get; set; }
    }
    public class WorkflowActivityResponseContentModel
    {
        public string activityKey { get; set; }
        //      "tenantId": null,
        //      "activityKey": "Activity_VehicleComponentUI",
        //      "processKey": null,
        //      "processInstanceId": "be8e53e8-3186-11ec-9c89-02001701523e",
        //      "processDefinitionId": "5134c045-2f85-11ec-a590-02001701523e",
        //      "priority": 50,
        //      "parentTaskId": null,
        //      "owner": null,
        //      "name": "Vehicle Component UI",
        //      "id": "be8ef054-3186-11ec-9c89-02001701523e",
        //      "followUpDate": null,
        //      "executionId": "be8e53e8-3186-11ec-9c89-02001701523e",
        //      "dueDate": null,
        //      "description": null,
        //      "createTime": "2021-10-20T09:18:53.406+00:00",
        //      "caseInstanceId": null,
        //      "caseExecutionId": null,
        //      "caseDefinitionId": null,
        //      "assignee": null,
        //      "rootProcessInstanceId": null,
        //      "formStructure": null,
        //      "endTime": null,
        //      "durationInMillis": null
    }
}

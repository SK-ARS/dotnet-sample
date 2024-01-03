using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.Workflow
{
    public class WorkflowActivityModelTemp
    {
        public enum FleetManagementWorkFlowActivity
        {
            VehicleComponent,
            VehicelConfigrationType,
            MovementType,
            VehicleDetails,
            ValidateData,
            SaveVehicleData
        }
        public List<Tuple<string, string, FleetManagementWorkFlowActivity>> UIMapper { get; set; }

        public WorkflowActivityModelTemp()
        {
            UIMapper = new List<Tuple<string, string, FleetManagementWorkFlowActivity>>
            {
                Tuple.Create("Activity_VehicleComponentUI", "", FleetManagementWorkFlowActivity.VehicleComponent),
                Tuple.Create("Activity_ChooseVehicleConfigurationType", "", FleetManagementWorkFlowActivity.VehicelConfigrationType),
                Tuple.Create("Activity_ChooseMovementType", "", FleetManagementWorkFlowActivity.MovementType),
                Tuple.Create("Activity_EnterVehicleDetails", "", FleetManagementWorkFlowActivity.VehicleDetails),
                Tuple.Create("Activity_ChooseVehicleConfigurationType", "", FleetManagementWorkFlowActivity.ValidateData),
                Tuple.Create("Activity_ChooseVehicleConfigurationType", "", FleetManagementWorkFlowActivity.SaveVehicleData)
            };
        }

        public string GetNextActivityScreen(string activityId)
        {
            return UIMapper.FirstOrDefault(x => x.Item1 == activityId).Item2;
        }
        public FleetManagementWorkFlowActivity CheckValidWorkflow(string activityId)
        {
            return UIMapper.FirstOrDefault(x => x.Item1 == activityId).Item3;
        }
    }
}

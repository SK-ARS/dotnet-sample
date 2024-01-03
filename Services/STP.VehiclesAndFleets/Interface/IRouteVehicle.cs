using STP.Domain;
using STP.Domain.VehiclesAndFleets.Component;
using STP.Domain.VehiclesAndFleets.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace STP.VehiclesAndFleets.Interface
{
    public    interface IRouteVehicle
    {
        #region vehicle Configuration
        ConfigurationModel GetRouteConfigInfo(int componentId, string userSchema);
        ConfigurationModel GetRouteConfigInfoForVR1(int componentId, string userSchema, int isEdit = 0);
        List<VehicleConfigList> GetRouteVehicleConfigVhclID(int vehicleId, string userSchema);
        List<VehicleRegistration> GetRouteVehicleRegistrationDetails(int vehicleId, string userSchema);
        ConfigurationModel GetNotifVehicleConfig(int vehicleId, int isSimple);
        bool CheckNotifValidVehicle(int vehicleId);
        long CheckValidVehicleCreated(int vehicleId);
        bool CheckWheelWithSumOfAxel(int vehicleId, string userSchema, int applnRev, bool isNotif, bool isVR1);

        #endregion
        #region Vehicle Components
        ComponentModel GetRouteComponent(int componentId, string userSchema);
        List<VehicleRegistration> GetRouteComponentRegistrationDetails(int componentId, string userSchema);
        List<Axle> ListRouteComponentAxle(int componentId, string userSchema);
        #endregion
        List<MovementVehicleList> GetRouteVehicleList(long revisionId, long versionId, string cont_Ref_No, string userSchema, int isHistoric);
        List<VehicleDetails> GetVehicleList(long routePartId, string userSchema, int isHistoric = 0);
    }
}

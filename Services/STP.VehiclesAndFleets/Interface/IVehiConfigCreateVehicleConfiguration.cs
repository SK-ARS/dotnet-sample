
using STP.Domain;
using STP.Domain.VehiclesAndFleets.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static STP.Domain.VehiclesAndFleets.Configuration.VehicleModel;

namespace STP.VehiclesAndFleets.Interface
{
  
   public  interface IVehiConfigCreateVehicleConfiguration 
    {
        double InsertVehicleConfiguration(NewConfigurationModel configurationModel);
        bool UpdateVehicleConfiguration(NewConfigurationModel configurationModel);
        VehicleConfigList InsertVehicleConfigPosition(VehicleConfigList ConfigList);
        bool SaveVehicleConfiguration(int configId, string userSchema, int applnRev, bool isNotif, bool isVR1);
        bool UpdateVR1vehicleconfig(NewConfigurationModel configurationModel, string userschema);
        bool Updateappvehicleconfig(NewConfigurationModel configurationModel, string userSchema);      
        VehicleConfigList CreateAppConfigPosn(VehicleConfigList configList, int isImportFromFleet, string userSchema);
        VehicleConfigList CreateVR1ConfigPosn(VehicleConfigList configList, string userSchema);
        VehicleConfigList CreateApplVehConfigPosn(VehicleConfigList configList, string userSchema);        
        long ImportVehicleFromList(ImportVehicleListModel model);
        NewConfigurationModel InsertVR1VehicleConfiguration(NewConfigurationModel configurationModel, string userSchema);
        long CopyVehicleFromList(ImportVehicleListModel model);
        List<MovementVehicleConfig> InsertConfigurationTemp(NewConfigurationModel configurationModel, string userSchema);
        bool UpdateMovementVehicle(NewConfigurationModel configurationModel, string userSchema);
        long SaveVehicleComponents(VehicleConfigDetail vehicleDetail, string userSchema);
        long UpdateVehicleComponents(VehicleConfigDetail vehicleDetail, string userSchema);
        long AddComponentToFleet(List<VehicleComponentModel> componentList);
    }
}

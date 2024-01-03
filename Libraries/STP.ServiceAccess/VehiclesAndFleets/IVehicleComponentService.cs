using STP.Common.Constants;
using STP.Domain;
using STP.Domain.VehiclesAndFleets.Component;
using STP.Domain.VehiclesAndFleets.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static STP.Domain.VehiclesAndFleets.Configuration.VehicleGlobalConfig;

namespace STP.ServiceAccess.VehiclesAndFleets
{
    public interface IVehicleComponentService
    {
        int CheckConfigNameExists(string internalName, int organisationId);
        double CreateComponent(ComponentModel componentObj);
        double InsertAppVehicleComponent(ComponentModel componentObj, string userSchema);
        double InsertVR1VehicleComponent(ComponentModel componentObj, string userSchema=UserSchema.Portal);
        VehicleConfigList CreateConfPosnComponent(VehicleConfigList configPosn);
        List<ComponentGridList> GetComponentByOrganisationId(int organisationId, int pageNumber, int pageSize, string componentName, string componentType, string vehicleIntent, int filterFavourites, string userSchema = UserSchema.Portal,int presetFilter=1, int? sortOrder = null);
        ComponentModel GetVehicleComponent(int componentId);
        VehicleConfigList GetConfigForComponent(int componentId);
        List<VehicleRegistration> GetRegistrationDetails(int compId);
        List<VehicleRegistration> GetVR1RegistrationDetails(int compId, string userSchema);
        List<VehicleRegistration> GetApplRegistrationDetails(int compId, string userSchema);
        int CreateRegistration(int compId, string registrationValue, string fleetId);
        int CreateVR1CompRegistration(int compId, string registrationValue, string fleetId, string userSchema);
        int CreateAppCompRegistration(int compId, string registrationValue, string fleetId, string userSchema);
        bool UpdateComponent(ComponentModel componentObj);
        int CreateVehicleRegFromCompReg(int compId, int vhclId);
        bool UpdateVR1VehComponent(ComponentModel componentObj, string userSchema);
        bool UpdateAppVehComponent(ComponentModel componentObj, string userSchema);
        ComponentModel GetVR1VehicleComponent(int componentId, string userSchema);
        ComponentModel GetAppVehicleComponent(int componentId, string userSchema);
        void UpdateAxle(Axle axle, int componentId);
        void UpdateVR1Axle(Axle axle, int componentId, string userSchema);
        void UpdateAppAxle(Axle axle, int componentId, string userSchema);
        List<Axle> ListAxle(int componentId);
        List<Axle> ListVR1vehAxle(int componentId, string userSchema);
        List<Axle> ListAppvehAxle(int componentId, string userSchema);
        ComponentModel GetRouteComponent(int componentId, string userSchema);
        List<VehicleRegistration> GetRouteComponentRegistrationDetails(int compId, string userSchema);
        List<Axle> ListRouteComponentAxle(int componentId, string userSchema);
        bool DeleteVehComponent(int componentId);
        bool DeleteComponentRegister(int compId, int IdNumber, int flag);
        bool DeleteVR1VehComponentRegister(int compId, int IdNumber, string UserSchema);
        bool DeleteAppVehComponentRegister(int compId, int IdNumber, string UserSchema);
        List<uint> VehicleComponentMovementClassification(int componentTypeId, int componentSubTypeId);
        VehicleComponentConfiguration VehicleComponentValidation(int componentTypeId, int componentSubTypeId, int movementClassificationId);
        double InsertComponentToTemp(ComponentModel componentModel);
        int UpdateComponentSubTypeToTemp(ComponentModel componentModel);
        int CreateRegistrationTemp(int compId, string registrationValue, string fleetId, bool movement, string userSchema);
        void InsertAxleDetailsTemp(Axle axle, int componentId, bool movement, string userSchema);
        bool UpdateComponentTemp(ComponentModel componentObj);
        List<VehicleConfigList> GetComponentIdTemp(string GUID, int vehicleId, string userSchema);
        ComponentModel GetComponentTemp(int componentId, string GUID, string userSchema);
        List<VehicleRegistration> GetRegistrationTemp(int compId, bool movement, string userSchema);
        List<Axle> ListAxleTemp(int componentId, bool movement, string userSchema);
        int InsertComponentConfigPosn(int componentId, int vehicleId);
        int AddToFleetTemp(string GUID, int componentId, int vehicleId);
        int DeleteComponentTemp(int componentId);
        int DeleteComponentConfig(int componentId, int vehicleId, bool movement, string userSchema);
        List<ComponentGridList> GetComponentFavourite(int organisationId, int movementId);
        List<uint> VehicleComponentType(int movementClassificationId, string userSchema);
        List<uint> VehicleSubComponentType(int movementClassificationId, int componentTypeId, string userSchema);
        bool UpdateMovementComponentTemp(ComponentModel componentObj, string userSchema);
        int CheckComponentInternalnameExists(string componentName, int organisationId);
        int UpdateConventionalTractorAxleCount(int axleCount, int vehicleId, int fromComponentId, int toComponentId, string userSchema);
    }
}

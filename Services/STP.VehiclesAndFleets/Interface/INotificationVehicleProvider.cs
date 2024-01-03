using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain;
using STP.Domain.VehiclesAndFleets.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STP.Domain.Routes;

namespace STP.VehiclesAndFleets.Interface
{
    public interface INotificationVehicleProvider
    {
        ListRouteVehicleId ImportFleetRouteVehicle(int VehicleId, string ContentRefNo, int simple, int RoutePartId);
        long ImportReturnRouteVehicle(int routePartId, string contentRefNo);
        int UpdateNotifRouteVehicle(NotificationGeneralDetails obj, int RoutePartId, int vehicleUnits);
        double InsertRouteVehicleConfiguration(NewConfigurationModel configurationModel, string contentRefNo, int isNotif);
        int SaveNotifVehicleRegistrationId(int vehicleId, string registrationValue, string fleetId);
        int SaveNotifVehicleConfiguration(int vehicleId, int componentId, int componentType);
        bool SaveNotifAxel(AxleDetails axle);
        bool UpdateMaxAxleWeight(long vehicleId);
        List<AxleDetails> ListCloneAxelDetails(int VehicleId);
        bool UpdateNewAxleDetails(AxleDetails axle);
        List<VehicleDetailSummary> GetNotificationVehicle(long partId);
        long ImportRouteVehicle(int routePartId, int vehicleId, string contentRefNo, int simple);
        List<NotifVehicleRegistration> ListVehRegDetails(string contentReferenceNo);
        List<NotifVehicleImport> ListVehicleImportDetails(string contentReferenceNo);
        List<NotifVehicleWeight> ListNotiVehWeightDetails(string contentReferenceNo);
        List<NotifVehicleImport> ListVehicleLengthDetails(string contentReferenceNo);
        List<NotifVehicleImport> ListVehicleGrossWeightDetails(string contentReferenceNo);
        List<NotifVehicleImport> ListVehicleWidthDetails(string contentReferenceNo, int reqVR1);
        List<NotifVehicleImport> ListVehicleAxleWeightDetails(string contentReferenceNo);
        List<NotifVehicleImport> ListVehicleRigidLengthDetails(string contentReferenceNo);
        int DeletePrevVehicle(int routePartId);
    }
}

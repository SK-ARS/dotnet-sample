using STP.VehiclesAndFleets.Interface;
using STP.Domain;
using STP.VehiclesAndFleets.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.Domain.MovementsAndNotifications.Notification;
using STP.Domain.Routes;

namespace STP.VehiclesAndFleets.Providers
{
    public class NotificationVehicleProvider: INotificationVehicleProvider
    {
        #region NotificationVehicleProvider Singleton
        private NotificationVehicleProvider()
        {
        }
        public static NotificationVehicleProvider Instance
        {
            [DebuggerStepThrough]
            get
            {
                return Nested.instance;
            }
        }
        /// <summary>
        /// Not to be called while using logic
        /// </summary>
        internal class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
            internal static readonly NotificationVehicleProvider instance = new NotificationVehicleProvider();
        }
        #region Logger instance
        private const string PolicyName = "NotificationVehicleProvider";
        #endregion
        #endregion

        #region Vehicle Managemnet in Notification
        #region ImportFleetRouteVehicle
        public ListRouteVehicleId ImportFleetRouteVehicle(int VehicleId, string ContentRefNo, int simple, int RoutePartId)
        {
            return VehicleConfigDAO.ImportFleetRouteVehicle(VehicleId, ContentRefNo, simple, RoutePartId);
        }
        #endregion
        #region ImportReturnRouteVehicle
        public long ImportReturnRouteVehicle(int routePartId, string contentRefNo)
        {
            return VehicleConfigDAO.ImportReturnRouteVehicle(routePartId, contentRefNo);
        }
        #endregion
        #region UpdateNotifRouteVehicle
        public int UpdateNotifRouteVehicle(NotificationGeneralDetails obj, int RoutePartId, int vehicleUnits)
        {
            return VehicleConfigDAO.UpdateNotifRouteVehicle(obj, RoutePartId, vehicleUnits);
        }
        #endregion
        #region InsertRouteVehicleConfiguration
        public double InsertRouteVehicleConfiguration(NewConfigurationModel configurationModel, string contentRefNo, int isNotif)
        {
            return VehicleConfigDAO.InsertRouteVehicleConfiguration(configurationModel, contentRefNo, isNotif);
        }
        #endregion
        #region SaveNotifVehicleRegistrationId
        public int SaveNotifVehicleRegistrationId(int vehicleId, string registrationValue, string fleetId)
        {
            return VehicleConfigDAO.SaveNotifVehicleRegistrationId(vehicleId, registrationValue, fleetId);
        }
        #endregion
        #region SaveNotifVehicleConfiguration
        public int SaveNotifVehicleConfiguration(int vehicleId, int componentId, int componentType)
        {
            return VehicleConfigDAO.SaveNotifVehicleConfiguration(vehicleId, componentId, componentType);
        }
        #endregion
        #region SaveNotifAxel
        public bool SaveNotifAxel(AxleDetails axle)
        {
            return VehicleConfigDAO.SaveNotifAxel(axle);
        }
        #endregion
        #region UpdateMaxAxleWeight
        public bool UpdateMaxAxleWeight(long vehicleId)
        {
            return VehicleConfigDAO.UpdateMaxAxleWeight(vehicleId);
        }
        #endregion
        #region ListCloneAxelDetails
        public List<AxleDetails> ListCloneAxelDetails(int VehicleId)
        {
            return VehicleConfigDAO.ListCloneAxelDetails(VehicleId);
        }
        #endregion
        #region UpdateNewAxleDetails
        public bool UpdateNewAxleDetails(AxleDetails axle)
        {
            return VehicleConfigDAO.UpdateNewAxleDetails(axle);
        }
        #endregion
        #region GetNotificationVehicle
        public List<VehicleDetailSummary> GetNotificationVehicle(long partId)
        {
            return VehicleConfigDAO.GetNotificationVehicle(partId);
        }
        #endregion

        #region ImportRouteVehicle
        public long ImportRouteVehicle(int routePartId, int vehicleId, string contentRefNo, int simple)
        {
            return VehicleConfigDAO.ImportRouteVehicle(routePartId, vehicleId, contentRefNo, simple);
        }
        #endregion
        #region checking vehicle registraion validation
        public List<NotifVehicleRegistration> ListVehRegDetails(string contentReferenceNo)
        {
            return VehicleConfigDAO.ListVehRegDetails(contentReferenceNo);
        }
        #endregion
        #region checking vehicle import validation
        public List<NotifVehicleImport> ListVehicleImportDetails(string contentReferenceNo)
        {
            return VehicleConfigDAO.ListVehImportDetails(contentReferenceNo);
        }
        #endregion
        #region checking vehicle weight against axle weight validation
        public List<NotifVehicleWeight> ListNotiVehWeightDetails(string contentReferenceNo)
        {
            return VehicleConfigDAO.ListNotiVehWeightDetails(contentReferenceNo);
        }
        #endregion
        #region checking vehicle length validation
        public List<NotifVehicleImport> ListVehicleLengthDetails(string contentReferenceNo)
        {
            return VehicleConfigDAO.ListVehLenDetails(contentReferenceNo);
        }
        #endregion
        #region checking vehicle gross weight validation based on vehicle category
        public List<NotifVehicleImport> ListVehicleGrossWeightDetails(string contentReferenceNo)
        {
            return VehicleConfigDAO.ListVehGrossWgtDetails(contentReferenceNo);
        }
        #endregion
        #region checking vehicle width validation based on vehicle category
        public List<NotifVehicleImport> ListVehicleWidthDetails(string contentReferenceNo, int reqVR1)
        {
            return VehicleConfigDAO.ListVehWdhDetails(contentReferenceNo, reqVR1);
        }
        #endregion
        #region checking Axle Weight validation based on vehicle category
        public List<NotifVehicleImport> ListVehicleAxleWeightDetails(string contentReferenceNo)
        {
            return VehicleConfigDAO.ListVehAxleWgtDetails(contentReferenceNo);
        }
        #endregion
        #region checking Rigid Length validation based on vehicle category
        public List<NotifVehicleImport> ListVehicleRigidLengthDetails(string contentReferenceNo)
        {
            return VehicleConfigDAO.ListVehRigidLenDetails(contentReferenceNo);
        }
        #endregion
        #region DeletePrevVehicle
        public int DeletePrevVehicle(int routePartId)
        {
            return VehicleConfigDAO.DeletePrevVehicle(routePartId);
        }
        #endregion

        #endregion
    }
}
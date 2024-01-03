using STP.Domain.DocumentsAndContents;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.LoggingAndReporting
{
    public class MovementActionIdentifiers
    {


        public long HaulierID { get; set; }
        public string HaulierName { get; set; }
        public string FailToSendUser { get; set; }
        public string ESDALRef { get; set; }
        public string NotificationCode { get; set; }
        public string OrganisationNameSender { get; set; }
        public string SenderContactName { get; set; }
        public string OrganisationNameReceiver { get; set; }
        public string ReciverContactName { get; set; }
        public string FailToSendOrganisation { get; set; }
        public long OrganisationId { get; set; }
        public string Title { get; set; }
        public long ContactId { get; set; }
        public long OrganisationIdReciver { get; set; }
        public long ContactIdReciver { get; set; }
        public string ContactName { get; set; }
        public long CollaborationStatus { get; set; }
        public long TransmissionId { get; set; }
        public string TransmissionDocType { get; set; }
        public DateTime DateTime { get; set; }
        public string TransmissionErrorMsg { get; set; }
        public string FullName { get; set; }
        public string DocType { get; set; }
        public int ItemTypeNo { get; set; }
        public string ManuallyAddedContName { get; set; }
        public string ManuallyAddedOrgName { get; set; }
        public string ApplicantName { get; set; }
        public string AllocateUser { get; set; }
        public string AllocateToName { get; set; }
        public string ReallocateUser { get; set; }
        public int MovementVer { get; set; }
        public int RevisionNo { get; set; }
        public string ProjectStatus { get; set; }
        public string SpecialOrderNo { get; set; }
        public bool ReviseBySort { get; set; }
        public string JobFileRef { get; set; }
        public int VR1GenNum { get; set; }
        public int CandVerNo { get; set; }
        public string CheckerName { get; set; }
        public long ApplicationId { get; set; }
        public string OrganisatioName { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public int RevisionId { get; set; }
        public int NewRevisionId { get; set; }
        public int VehicleId { get; set; }
        public int VehicleIdNo { get; set; }
        public int ComponentId { get; set; }
        public int ComponentIdNo { get; set; }
        public long FleetVehicleId { get; set; }
        public long FleetComponentId { get; set; }
        public string FleetVehicleName { get; set; }
        public string FleetComponentName { get; set; }
        public int FleetComponentIdNo { get; set; }
        public int FleetVehicleIdNo { get; set; }
        public int RouteId { get; set; }
        public int ReturnRouteId { get; set; }
        public int PrevMovRouteId { get; set; }
        public int LibRouteId { get; set; }
        public int NewLibRouteId { get; set; }
        public int EditFlag { get; set; }
        public string NewNotificationCode { get; set; }
        public ContactPreference ContactPreference { get; set; }

        public TransmissionModelFilter TransmissionModelFilter { get; set; }

        public TransmissionModel TransmissionModel { get; set; }

        public MovementnActionType MovementActionType { get; set; }

        public SysEventType SystemEventType { get; set; }

        public MovementActionIdentifiers()
        {
            ContactPreference = new ContactPreference();
            TransmissionModelFilter = new TransmissionModelFilter();
            TransmissionModel = new TransmissionModel();
            MovementActionType = new MovementnActionType();
            SystemEventType = new SysEventType();
            ESDALRef = null;
            VR1App = "false";
        }

        public string HaulierMnemonic { get; set; }

        public long ProjectId { get; set; }

        public string VR1App { get; set; }

        public long AnalysisId { get; set; }

        public string RouteType { get; set; }

        public string Routename { get; set; }

        public long CandRouteId { get; set; }

        public long CandVersionId { get; set; }

        public string Status { get; set; }

        public long NotificationID { get; set; }
        public long NewNotificationID { get; set; }
        public string ContentRefNo { get; set; }
        public int PrevMovVehicleId { get; set; }

        public int organisationId { get; set; }
        public string NENRouteName { get; set; }
        public int NENNotificationNo { get; set; }
        public string CollaborationNotes { get; set; }
    }   

}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Notification
{
    public class NENAuditLogList
    {
        public int AuditLoginId { get; set; }
        public string ESDALReferenceNumber { get; set; }
        public string History { get; set; }
        public string HistoryDate { get; set; }
        public long InboxItemId { get; set; }
        public decimal RecordCount { get; set; } 
        public string UserName { get; set; }
        public string SearchType { get; set; }
        public string SearchName { get; set; }
        public int SortFlag { get; set; }
        public string NotificationSource { get; set; }
    }

    public class OrganisationUser
    {
        public string OrganisationUserName { get; set; }
        public long OrganisationUserId { get; set; }

        public int ScrutinyUserId { get; set; }
    }

    public class ErrorList
    {
        public string Point { get; set; }
        public int AddIndex { get; set; }
        public string Error { get; set; }
    }
    public class NonErrorList
    {
        public int Point { get; set; }
        public string Address { get; set; }
        public long Northing { get; set; }
        public long Easting { get; set; }
        public int PointIndex { get; set; }
        public string Moniker { get; set; }
        public bool DifferentQASSearchFlag { get; set; }
    }


    public class NENGeneralDetails
    {
        public string MovementName { get; set; }
        public string ESDALReference { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Classification { get; set; }
        public string MovementDateFrom { get; set; }
        public string MovementDateTo { get; set; }
        public int NoOfMovements { get; set; }
        public int MaximamPiecesPerLoad { get; set; }
        public int VehicleCode { get; set; }
        public long RoutePartIdS { get; set; }
        public long LibRoutePartID { get; set; }
        public long VehicleId { get; set; }        
        public string MyReference { get; set; }
        public string ClientName { get; set; }
        public string HaulierOprLicence { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string LoadDescription { get; set; }
        public string Notes { get; set; }
        public string NotesOnEscort { get; set; }
        public string NotificationDate { get; set; }
        public string VR1Number { get; set; }
        public string SONumbers { get; set; }
        public string VSONumber { get; set; }

       
        public string FromDateTime { get; set; }
        public string ToDateTime { get; set; }

        public string CountryId { get; set; }
        public string ReceivedOn { get; set; }
        public string GRouteDescription { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string OnBehalf { get; set; }
        public string DescriptionReturnLeg { get; set; }
        public long NotificationId { get; set; }
        public long InboxItemId { get; set; }

        public int RoutePartNo { get; set; }
        public long OrganisationId { get; set; }
        public long NENId { get; set; }
        public short IsMostRecent { get; set; }
        public string HaulierOrganisationName { get; set; }

        public string HaulierAddress { get; set; }
        public string HaulierAddress1 { get; set; }
        public string HaulierAddress2 { get; set; }
        public string HaulierAddress3 { get; set; }
        public string HaulierAddress4 { get; set; }
        public string HaulierAddress5 { get; set; }

        public string OtherContactDetails { get; set; }
        public int IndemnityConfirmation { get; set; }
        public string Indemnity { get; set; }
        public string HaulierContactName { get; set; }

        public string ContentRefNo { get; set; }
    }

    public class NENHaulierRouteDesc
    {
        public string HualierGRouteDescription { get; set; }
        public string HualierDescriptionReturnLeg { get; set; }
        public string MainStartAddress { get; set; }
        public string MainEndAddress { get; set; }
    }

    public class NENAuditGridList
    {
        public string DateTime { get; set; }
        public string User { get; set; }
        public string Log { get; set; }
        public decimal RecordCount { get; set; } 
        public string NotificationSource { get; set; }
    }

    public class NENRouteStatusList
    {
        public int RouteStatus { get; set; }
    }

    public class NENUpdateRouteDet
    {
        public long RouteId { get; set; }
        public string RouteName { get; set; }
        public string RouteDescription { get; set; }
        public long PartNo { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }

    }
    public class NeHaulierList
    {
        public string HaulierName { get; set; }

        public string AuthenticationKey { get; set; }

        public string OrgniseName { get; set; }

        public long NeLimit { get; set; }

        public bool IsValNen { get; set; }

        public long NeAuthKey { get; set; }
        public long TotalCount { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace STP.Domain.ExternalAPI
{
    public class ExportNotification
    {
        public NotifGeneralDetail GeneralDetails { get; set; }
        public List<Vehicle> Vehicles { get; set; }
        public List<Route> Routes { get; set; }
    }
    public class NotifGeneralDetail
    {
        public string Classification { get; set; }
        public HaulierDetail HaulierDetails { get; set; }
        public LoadDetail LoadDetails { get; set; }
        public MovementDetail MovementDetails { get; set; }
        public string ApplicationReference { get; set; }
        public string NotesOnEscort { get; set; }
        public string Notes { get; set; }
        public string HaulierReference { get; set; }
        public string Client { get; set; }
        public string IsVR1 { get; set; }
        public string Indemnity { get; set; }
    }

    public class ExportNotifGeneralDetails
    {
        public string Classification { get; set; }
        public string HaulierOrgName { get; set; }
        public string HaulierContact { get; set; }
        public string HaulierAddressLine1 { get; set; }
        public string HaulierAddressLine2 { get; set; }
        public string HaulierAddressLine3 { get; set; }
        public string HaulierAddressLine4 { get; set; }
        public string HaulierAddressLine5 { get; set; }
        public string HaulierPostCode { get; set; }
        public string HaulierCountry { get; set; }
        public string HaulierEmail { get; set; }
        public string HaulierTelephoneNumber { get; set; }
        public string HaulierFaxNumber { get; set; }
        public string HaulierLicence { get; set; }
        public string LoadDescription { get; set; }
        public int TotalMoves { get; set; }
        public int MaxPiecesPerMove { get; set; }
        public string FromSummary { get; set; }
        public string ToSummary { get; set; }
        public DateTime MovementStart { get; set; }
        public DateTime MovementEnd { get; set; }
        public string NotesOnEscort { get; set; }
        public string Notes { get; set; }
        public string HauliersReference { get; set; }
        public string Client { get; set; }
        public string Indemnity { get; set; }
        public string ApplicationReference { get; set; }
        public string RequireVR1 { get; set; }
        public int VSOType { get; set; }
        public string ContentReferenceNo { get; set; }
        public int NotificationType { get; set; }
        public long AnalysisId { get; set; }
    }
}

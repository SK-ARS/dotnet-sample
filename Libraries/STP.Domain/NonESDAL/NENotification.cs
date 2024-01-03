using STP.Domain.ExternalAPI;
using System;
using System.Collections.Generic;

namespace STP.Domain.NonESDAL
{
    public class NENotification
    {
        public GeneralDetail GeneralDetails { get; set; }
        public List<Vehicle> Vehicles { get; set; }
        public List<Route> Routes { get; set; }
        public List<string> AffectedParties { get; set; } 
        public string AuthenticationKey { get; set; }
    }
    public class NENotifOuput
    {
        public string ESDALReferenceNumber { get; set; }
        public List<string> AffectedParties { get; set; }
        public AffectedPartiesValidation PartiesNotReceivedNen { get; set; }
    }
    public class NENotificationStatusOutput
    {
        public string OrganisationName { get; set; }
        public List<string> CollaborationNotes { get; set; }
    }
    public class NENotifGeneralDetails
    {
        public int Classification { get; set; }
        public string HaulierOrgName { get; set; }
        public string HaulierContact { get; set; }
        public string HaulierAddressLine { get; set; }
        public string HaulierPostCode { get; set; }
        public int HaulierCountry { get; set; }
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
        public int Indemnity { get; set; }
        public string ApplicationReference { get; set; }
        public int RequireVR1 { get; set; }
        public int RequireSo { get; set; }
        public int VSOType { get; set; }
        public long NonEsdalKeyId { get; set; }
        public string PrevEsdalRef { get; set; }
        public ValidationError GeneralDetailError { get; set; }
    }
    public class ValidNERenotif
    {
        public bool IsRenotified { get; set; }
        public long NonEsdalKeyId { get; set; }
        public bool IsNotExist { get; set; }
        public bool InValid { get; set; }
    }
    public class AffectedPartiesValidation
    {
        public string ErrorMessage { get; set; }
        public List<string> InvalidAffectedParties { get; set; }
    }
}

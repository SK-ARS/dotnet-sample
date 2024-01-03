using STP.Domain.Applications;
using STP.Domain.ExternalAPI;
using System;
using System.Collections.Generic;

namespace STP.Domain.NonESDAL
{
    public class NEApplication
    {
        public GeneralDetail GeneralDetails { get; set; }
        public List<Vehicle> Vehicles { get; set; }
        public List<Route> Routes { get; set; }
        public string AuthenticationKey { get; set; }
    }
    public class NEApplicationOuput
    {
        public string ESDALReferenceNumber { get; set; }
    }
    public class NEApplicationStatus
    {
        public string ApplicationStatus { get; set; }
    }

    public class NEAppGeneralDetails
    {
        public int Classification { get; set; }
        public string HaulierOrgName { get; set; }
        public string HaulierContact { get; set; }
        public string HaulierAddressLine1 { get; set; }
        public string HaulierAddressLine2 { get; set; }
        public string HaulierAddressLine3 { get; set; }
        public string HaulierAddressLine4 { get; set; }
        public string HaulierAddressLine5 { get; set; }
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
        public string ApplicationNotes { get; set; }
        public string HauliersReference { get; set; }
        public string ApplicationDesc { get; set; }
        public string AgentName { get; set; }
        public string Client { get; set; }
        public long NonEsdalKeyId { get; set; }
        /*Fields only Required for VR1 Application*/
        public bool IsVR1 { get; set; }
        public SupplimentaryInfo SupplimentaryInfo { get; set; }
        public ValidationError GeneralDetailError { get; set; }
    }
}

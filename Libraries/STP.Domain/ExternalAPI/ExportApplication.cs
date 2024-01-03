using STP.Domain.Applications;
using System;
using System.Collections.Generic;

namespace STP.Domain.ExternalAPI
{
    public class ExportApplication
    {
        public AppGeneralDetails GeneralDetails { get; set; }
        public List<Vehicle> Vehicles { get; set; }
        public List<Route> Routes { get; set; }
    }
    public class AppGeneralDetails
    {
        public string Classification { get; set; }
        public HaulierDetail HaulierDetails { get; set; }
        public LoadDetail LoadDetails { get; set; }
        public MovementDetail MovementDetails { get; set; }
        public string NotesOnEscort { get; set; }
        public string Notes { get; set; }
        public string HaulierReference { get; set; }
        public string ApplicationDesc { get; set; }
        public string AgentName { get; set; }
        public string Client { get; set; }
        public SupplimentaryDetails SupplimentaryDetails { get; set; }
    }
    public class SupplimentaryDetails
    {
        public decimal DistanceOfRoad { get; set; }
        public string ValueOfLoad { get; set; }
        public string DateOfAuthority { get; set; }
        public string AdditionalCost { get; set; }
        public string RiskNature { get; set; }
        public string PortNames { get; set; }
        public string SeaQuotation { get; set; }
        public string ProposedMovementDetails { get; set; }
        public string CostOfMovement { get; set; }
        public string AdditionalConsideration { get; set; }
    }

    public class ExportAppGeneralDetails
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
        public string ApplicationNotes { get; set; }
        public string HauliersReference { get; set; }
        public string ApplicationDesc { get; set; }
        public string AgentName { get; set; }
        public string Client { get; set; }
        /*Fields only Required for VR1 Application*/
        public bool IsVR1 { get; set; }
        public SupplimentaryInfo SupplimentaryInfo { get; set; }
        public long VersionId { get; set; }
        public long ApplicationRevisionId { get; set; }
        public long AnalysisId { get; set; }
    }
}

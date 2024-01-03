using System.Collections.Generic;

namespace STP.Domain.ExternalAPI
{
    public class GeneralDetail
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
        public string Indemnity { get; set; }
        public string ApplicationReference { get; set; }
        public string ESDALReferenceNumber { get; set; }
        public SupplimentaryDetails SupplimentaryDetails { get; set; }
    }

    public class HaulierDetail
    {
        public string HaulierOrgName { get; set; }
        public HaulierAddress HaulierAddress { get; set; }
        public string HaulierContact { get; set; }
        public string Email { get; set; }
        public string TelephoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Licence { get; set; }
    }

    public class HaulierAddress
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string AddressLine5 { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }
    }

    public class LoadDetail
    {
        public string Description { get; set; }
        public long TotalMoves { get; set; }
        public long MaxPiecesPerMove { get; set; }
    }

    public class MovementDetail
    {
        public string FromSummary { get; set; }
        public string ToSummary { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }

    public class ValidateAuthentication
    {
        public int OrganisationId { get; set; }
        public int UserTypeId { get; set; }
    }

    public class AuthKeyValid
    {
        public long OrganisationId { get; set; }
        public string OrganisationName { get; set; }
    }

    public class AuthorizedOrganisation
    {
        public int ValidCount { get; set; }
        public int SortValid { get; set; }
        public long SenderId { get; set; }
        public List<long> Receivers { get; set; }
        public List<long> SortOrgIds { get; set; } 
    }
}

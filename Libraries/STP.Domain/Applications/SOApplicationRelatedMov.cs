namespace STP.Domain.Applications
{
    public class SOApplicationRelatedMov
    {
        public long ProjectID { get; set; }
        public string HaulierMnemonic { get; set; }
        public int ReferenceNo { get; set; }
        public long ApplicationRevisionId { get; set; }
        public long VersionID { get; set; }
        public int OrganisationID { get; set; }
        public int VersionNo { get; set; }
        public decimal EnteredBySORT { get; set; }
        public int RevisionNo { get; set; }
        public int LastVersionNo { get; set; }
        public string OwnerName { get; set; }
        public long CandidateRouteID { get; set; }
        public string CandidateRouteName { get; set; }
        public long CandidateAnalysisID { get; set; }
        public long CandidateRevisionID { get; set; }
        public int CandidateRevisionNo { get; set; }
        public int LastCandidateRevisionNo { get; set; }
        public string ESDALReference { get; set; }
    }
}
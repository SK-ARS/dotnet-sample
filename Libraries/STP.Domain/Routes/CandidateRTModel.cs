using System.Collections.Generic;

namespace STP.Domain.Routes
{
    public class CandidateRTModel
    {
        public long RouteID { get; set; }
        public string Name { get; set; }
        public long AnalysisID { get; set; }
        public long RevisionID { get; set; }
        public int RevisionNo { get; set; }
        public string CandidateDate { get; set; }       
    }
    public class CandidateRT
    {
        public CandidateRT()
        {
            this.Versions = new List<CRVersion>();
        }
        public long RouteId { get; set; }
        public string Name { get; set; }
        public long AnalysisId { get; set; }
        public string CandidateDate { get; set; }
        public List<CRVersion> Versions { get; set; }
    }
    public class CRVersion
    {
        //public CRVersion()
        //{
        //    this.RouteParts = new List<CRRoutePart>();
        //}
        public long ReviosionId { get; set; }
        public int RevisionNo { get; set; }
        //public List<CRRoutePart> RouteParts { get; set; }
    }
    public class CRRoutePart
    {
        public long RoutePartId { get; set; }
        public string RoutePartName { get; set; }
    }
    public class CRAppRoutes
    {
        public long RouteId { get; set; }
        public string Name { get; set; }
    }
    public class CRPoints
    {
        public long RoutePointId { get; set; }
        public long LinkId { get; set; }
    }
    
}
using System.Collections.Generic;
namespace STP.Domain.MovementsAndNotifications.HaulierMovementsAPI
{
    public class Movement
    {       
        public string ESDALReferenceNumber { get; set; }
        public string MovementType { get; set; }
        public string Status { get; set; }
        public string FromSummary { get; set; }
        public string ToSummary { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string HaulierReference { get; set; }
        public string MovementCategory { get; set; }
        public string IsVR1 { get; set; }
    }

    public class HaulierMovementDetails
    {
        public int TotalRecords { get; set; }
        public int NumberOfPages { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<Movement>  Movements { get; set; }
    }
}

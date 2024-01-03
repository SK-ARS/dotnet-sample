using Newtonsoft.Json;
using System.Collections.Generic;

namespace STP.Domain.MovementsAndNotifications.SOAPoliceMovementsAPI
{
    public  class Movement
    {
        public string ESDALReferenceNumber { get; set; }
        public string InboxStatus { get; set; }
        public string MovementDate { get; set; }
        public string ReceivedDate { get; set; }
        public string MessageType { get; set; }
        public string MovementType { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Suitablity { get; set; }
        public string IsVR1 { get; set; }
     }

    public class SoaPoliceDetails
    {
        public int TotalRecords { get; set; }
        public int NumberOfPages { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<Movement> Movements { get; set; }
    }
}

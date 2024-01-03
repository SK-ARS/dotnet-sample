namespace STP.Domain.Routes
{
    public class CloneRTPartsInsertParams
    {       
        public long OldRevisionID { get; set; }
        public long RtRevisionID { get; set; }
        public int Flag { get; set; }
    }
}
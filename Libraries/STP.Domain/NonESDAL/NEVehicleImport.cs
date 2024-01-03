namespace STP.Domain.NonESDAL
{
    public class NEVehicleImport
    {
        public long MovementId { get; set; }
        public long RevisionId { get; set; }
        public long NotificationId { get; set; }
        public bool IsVr1 { get; set; }
        public bool IsNotif { get; set; }
    }
}

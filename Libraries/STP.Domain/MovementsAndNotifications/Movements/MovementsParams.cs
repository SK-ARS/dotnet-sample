using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Movements
{
    public class GetInboxMovementsParams
    {
        public int presetFilter;

        public int OrganisationId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public MovementsInboxFilter InboxFilter { get; set; }
        public MovementsInboxAdvancedFilter InboxAdvancedFilter { get; set; }
        public int UserType { get; set; }
        public int UserId { get; set; }
        public string UserSchema { get; set; }
    }
    public class PlanMovementType
    {
        public long MovementId { get; set; }
        public int OrganisationId { get; set; }
        public long ContactId { get; set; }
        public long VehicleClass { get; set; }
        public int MoveType { get; set; }
        public string FromDesc { get; set; }
        public string ToDesc { get; set; }
        public string HaulierRef { get; set; }
        public DateTime MovementStart { get; set; }
        public DateTime MovementEnd { get; set; }
        public long NotificationId { get; set; }
        public long RevisionId { get; set; }
        public string UserSchema { get; set; }
        public long AllocateUserId { get; set; }
        public int ApplicationType { get; set; }
        public string HaulierName { get; set; }
        public string HaulierContact { get; set; }
        public string HaulierOnBehalfOf { get; set; }
        public DateTime NotificationDate { get; set; }
        public int IsVehicleEdit { get; set; }
        public int IsVehicleAmended { get; set; }
    }
}
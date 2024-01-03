using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Movements
{
    public class MovementModelParams
    {
        public string Route { get; set; }

        public string Mnemonic { get; set; }

        public string ESDALReferenceNumber { get; set; }

        public string Version { get; set; }

        public long InboxId { get; set; }

        public string ESDALReference { get; set; }

        public long ContactId { get; set; }

        public long OrganisationId { get; set; }

        public long Notificationid { get; set; }
    }
    public class NotifGeneralDetails
    {
        public long NotificationId { get; set; }
        public string ContentRefNum { get; set; }
        public long VersionId { get; set; }
        public long ProjectId { get; set; }
        public long AnalysisId { get; set; }
    }
}
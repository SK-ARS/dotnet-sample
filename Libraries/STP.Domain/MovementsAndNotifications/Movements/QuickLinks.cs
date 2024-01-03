using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Movements
{
    public class QuickLinks
    {
        public int UserId { get; set; }

        public string NotificationCode { get; set; }

        public int VehicleClassification { get; set; }

        public int NotificationNumber { get; set; }

        public int VersionNumber { get; set; }

        public double VersionId { get; set; }

        public double LinkNumber { get; set; }

        public int RevisionNumber { get; set; }

        public string HaulierMnemonic { get; set; }

        public int ESDALReferenceNumber { get; set; }

        public int NotificationVersionNumber { get; set; }

        public string ESDALReference { get; set; }

        public int NotificationId { get; set; }

        public int RevisionId { get; set; }

        public int ProjectId { get; set; }

        public int VersionStatus { get; set; }

        public int ApplicationStatus { get; set; }

        public int IsNotified { get; set; }
    }
}
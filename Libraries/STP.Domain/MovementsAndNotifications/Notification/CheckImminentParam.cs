using STP.Domain.VehiclesAndFleets.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Notification
{
    public class CheckImminentParam
    {
        public int VehicleClass { get; set; }
        public decimal VehicleWidth { get; set; }
        public decimal VehicleLength { get; set; }
        public decimal RigidLength { get; set; }
        public decimal GrossWeight { get; set; }
        public int WorkingDays { get; set; }
        public decimal FrontPRJ { get; set; }
        public decimal RearPRJ { get; set; }
        public decimal LeftPRJ { get; set; }
        public decimal RightPRJ { get; set; }
        public GetImminentChkDetailsDomain ImminentCheckDetails { get; set; }
        public string NotificationType { get; set; }
    }
}
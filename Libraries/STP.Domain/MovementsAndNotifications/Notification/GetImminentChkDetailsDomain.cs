using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Notification
{
    public class GetImminentChkDetailsDomain
    {
        public int VehicleCode { get; set; }
        public string MovementStartDate { get; set; }
        public decimal VehicleWidth { get; set; }
        public decimal VehicleLength { get; set; }
        public decimal RigidLength { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal Width { get; set; }
        public decimal Length { get; set; }
        public decimal MaximamAxleWeight { get; set; }
        public int AxleCount { get; set; }

        public decimal FrontPRJ { get; set; }
        public decimal RearPRJ { get; set; }
        public decimal LeftPRJ { get; set; }
        public decimal RightPRJ { get; set; }

    }
}
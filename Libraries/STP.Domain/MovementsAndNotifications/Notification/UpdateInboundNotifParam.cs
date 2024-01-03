using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Notification
{
    public class UpdateInboundNotifParam
    {
        public int NotificationId { get; set; }
        public byte[] InboundNotification { get; set; }
    }
}
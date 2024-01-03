using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.LoggingAndReporting.Models
{
    /// <summary>
    /// class to store contact details regarding the notification to be sent.
    /// </summary>
    public class NotificationContacts
    {
        public long contactId { get; set; }

        public long orgId { get; set; }

        public long notificationId { get; set; }//variable added to identify the contact specific notification

        public string contactName { get; set; }

        public string email { get; set; }

        public string fax { get; set; }

        public string phone { get; set; }

        public string extension { get; set; }

        public string reason { get; set; }

        public contactPreference contactPreference { get; set; }

        public string organistationName { get; set; }

        public NotificationContacts()
        { }
    }

}
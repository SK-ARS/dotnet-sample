using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.SecurityAndUsers
{
    /// <summary>
    /// class to store contact details regarding the notification to be sent.
    /// </summary>
    public class NotificationContacts
    {
        public long ContactId { get; set; }

        public long OrganisationId { get; set; }

        public long NotificationId { get; set; }

        public string ContactName { get; set; }

        public string Email { get; set; }

        public string Fax { get; set; }

        public string PhoneNumber { get; set; }

        public string Extension { get; set; }

        public string Reason { get; set; }

        public ContactPreference ContactPreference { get; set; }

        public string OrganistationName { get; set; }
        public long ProjectId { get; set; }

        public NotificationContacts()
        { }
    }
  
}
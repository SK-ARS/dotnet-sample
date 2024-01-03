using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.SecurityAndUsers
{
    public class ContactModel
    {
        public bool IsHaulier { get; set; }

        public bool ISPolice { get; set; }

        public bool IsRecipient { get; set; }

        public bool IsRetainedNotificationOnly { get; set; }

        public int ContactId { get; set; }

        public string FullName { get; set; }

        public int OrganisationId { get; set; }

        public string Organisation { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }


        public string Reason { get; set; }

        public Byte[] AffectedParties { get; set; }

        public int DelegationId { get; set; }

        public int DelegatorsContactId { get; set; }

        public int DelegatorsOrganisationId { get; set; }

        public bool RetainNotification { get; set; }

        public bool WantsFailureAlert { get; set; }

        public string DelegatorsOrganisationName { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactType { get; set; }
        public string OnBehalfOf { get; set; }
        public int IsReceiveNen { get; set; }
    }
}
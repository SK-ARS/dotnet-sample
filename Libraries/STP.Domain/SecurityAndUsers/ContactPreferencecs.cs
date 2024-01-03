using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.SecurityAndUsers
{
    //Dependency - Vehicle Configuration in Vehicle and Fleet
    public enum ContactPreference
    {
        fax,

        emailText,

        emailHtml,

        onlineInboxOnly,

        OnlineInboxPlusEmail,
    }
}
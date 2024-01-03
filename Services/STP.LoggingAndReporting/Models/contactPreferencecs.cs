using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.LoggingAndReporting.Models
{
    //Dependency - Vehicle Configuration in Vehicle and Fleet
    public enum contactPreference
    {
        fax,

        emailText,

        emailHtml,

        onlineInboxOnly,

        OnlineInboxPlusEmail,
    }
}
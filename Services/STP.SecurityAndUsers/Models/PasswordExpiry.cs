using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.SecurityAndUsers.Models
{
    public class PasswordExpiry
    {
        public DateTime LastPasswordChange { get; set; }
        public string PasswordLife { get; set; }
        public string ReminderPeriod { get; set; }
    }
}
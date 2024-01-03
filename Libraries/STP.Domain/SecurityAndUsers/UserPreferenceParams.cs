using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.SecurityAndUsers
{
    public class UserPreferenceParams
    {
        public UserPreferences ObjUserPreference { get; set; }
        public int UserId { get; set; }
        public string EmailUpdate { get; set; }
        public string FaxNumber { get; set; }
    }
}

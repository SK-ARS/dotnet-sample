using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.SecurityAndUsers
{
    public class OTPPasswordUpdation
    {
        public int UserId { get; set; }
        public string OTPPassword { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }
}

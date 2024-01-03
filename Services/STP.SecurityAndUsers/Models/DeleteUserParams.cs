using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.SecurityAndUsers.Models
{
    public class DeleteUserParams
    {
        public int DeleteVal { get; set; }
        public int ? UserId { get; set; }
        public int ? ContactId { get; set; }
    }
}
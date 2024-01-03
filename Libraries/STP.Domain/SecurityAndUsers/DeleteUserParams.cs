using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.SecurityAndUsers
{
    public class DeleteUserParams
    {
        public int DeleteValue { get; set; }
        public int ? UserId { get; set; }
        public int ? ContactId { get; set; }
    }
}
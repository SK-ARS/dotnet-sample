using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class AllocateSortUserInsertParams
    {
        public long ProjectId { get; set; }
        public long PlnUserId { get; set; }
        public string DueDate { get; set; }
        public int RevisionNo { get; set; }
    }
}
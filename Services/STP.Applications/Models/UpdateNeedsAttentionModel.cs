using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class UpdateNeedsAttentionModel
    {
        public int NotificationID { get; set; }
        public int RevisionID { get; set; }
        public int NA_flag { get; set; }
    }
}
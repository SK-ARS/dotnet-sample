using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Applications
{
    public class UpdateNeedsAttentionModel
    {
        public int NotificationId { get; set; }
        public int RevisionId { get; set; }
        public int NAFlag { get; set; }
    }
}
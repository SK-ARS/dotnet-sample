using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.Applications
{
    public class IndemnityConfirmation
    {
        public long NotificationId { get; set; }
        public string HaulierName { get; set; }
        public string OrganisationName { get; set; }
        public string HaulierDetails { get; set; }
        public DateTime FirstMoveDate { get; set; }
        public DateTime LastMoveDate { get; set; }
        public DateTime SentDateTime { get; set; }
        public string HaulierContact { get; set; }

        public string OnBehalfOf { get; set; }
    }
}

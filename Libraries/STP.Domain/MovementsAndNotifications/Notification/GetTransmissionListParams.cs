using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.MovementsAndNotifications.Notification
{
    public class GetTransmissionListParams
    {
        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public string ESDALRefNo { get; set; }
        public string Status { get; set; }
        public int StatusItemCount { get; set; }
        public int IsHistoric { get; set; }
        public string UserSchema { get; set; }
    }
}

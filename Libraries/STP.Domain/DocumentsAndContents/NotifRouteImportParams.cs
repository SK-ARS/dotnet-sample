using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.DocumentsAndContents
{
    public class NotifRouteImportParams
    {
        public long NENId { get; set; }

        public int IUserId { get; set; }

        public long InboxItemId { get; set; }

        public int IOrgId { get; set; }
    }
}
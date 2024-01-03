using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Applications
{
    public class SORTSOApplicationParams
    {
        public int ApplicationRevisionId { get; set; }
        public int EditFlag { get; set; }
    }
    public class SortAppPayload
    {
        public int applicationrevisionId { get; set; }
        public long ProjectId { get; set; }
        public int LastRevisionNo { get; set; }
        public int LastVersionNo { get; set; }
    }
}
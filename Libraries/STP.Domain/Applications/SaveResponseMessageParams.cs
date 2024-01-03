using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.Applications
{
    public class SaveResponseMessageParams
    {
        public long UserId { get; set; }
        public int AutoResponseId { get; set; }
        public long OrganisationId { get; set; }
        public byte[] ResponseMessage { get; set; }
        public string ResponsePdf { get; set; }

    }
}

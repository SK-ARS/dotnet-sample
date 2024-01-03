using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Applications
{
    public class UpdateSpecialOrderInsertParams
    {       
        public int ProjectId { get; set; }
        public int VersionId { get; set; }
        public string ESDALReference { get; set; }
    }
}
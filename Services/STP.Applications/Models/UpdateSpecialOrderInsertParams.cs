using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class UpdateSpecialOrderInsertParams
    {       
        public int ProjectID { get; set; }
        public int VersionID { get; set; }
        public string ESDALReference { get; set; }
    }
}
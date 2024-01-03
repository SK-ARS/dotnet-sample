using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class ProjectFolderModel
    {
        public double OrgId { get; set; }
        public string FolderName { get; set; }
        public long ProjectID { get; set; }
        public long FolderID { get; set; }
    }
}
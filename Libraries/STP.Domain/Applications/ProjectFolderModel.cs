using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Applications
{
    public class ProjectFolderModel
    {
        public double OrgId { get; set; }
        public string FolderName { get; set; }
        public long ProjectId { get; set; }
        public long FolderId { get; set; }
    }
}
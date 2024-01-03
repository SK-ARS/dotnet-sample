using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class ProjectFolderModelParams
    {
        public int flag { get; set; }

        public long folderID { get; set; }

        public long projectId { get; set; }

        public string hauliermnemonic { get; set; }

        public int esdalref { get; set; }

        public long notificationId { get; set; }

        public long revisionID { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Applications
{
    public class ProjectFolderModelParams
    {
        public int Flag { get; set; }

        public long FolderId { get; set; }

        public long ProjectId { get; set; }

        public string HaulierMnemonic { get; set; }

        public int ESDALReference { get; set; }

        public long NotificationId { get; set; }

        public long RevisionId { get; set; }
    }
}
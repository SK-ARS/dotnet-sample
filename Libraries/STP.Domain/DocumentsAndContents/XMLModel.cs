using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.DocumentsAndContents
{
    public class XMLModel
    {
        public string OLDXML { get; set; }

        public string ReturnXML { get; set; }

        public long NotificationID { get; set; }

        public string ESDALReference { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Common.Content
{
    public class XSLTPath
    {
        public string xsltPath { get; set; }
        public string xsltSOAPath { get; set; }
        public string xsltPolicePath { get; set; }
        public string xsltRouteDetail { get; set; }
        public string xsltRouteDetailsImperial { get; set; }

        public XSLTPath()
        {
            xsltPath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Notification_FAX_SOA_PDF.xsl";
            xsltSOAPath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Notification_FAX_SOA_PDF.xsl";
            xsltPolicePath = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\Notification_Fax_Police.xslt";
            xsltRouteDetail = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\RouteDetails.xslt";
            xsltRouteDetailsImperial = AppDomain.CurrentDomain.BaseDirectory + "Content\\XSLT\\RouteDetailsImperial.xslt";
        }
    }     
}

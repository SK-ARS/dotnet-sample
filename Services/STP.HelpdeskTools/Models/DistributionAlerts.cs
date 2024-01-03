using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.HelpdeskTools.Models
{
    public class DistributionAlerts
    {
        public int TransmissionId { get; set; }

        public string DateTime { get; set; }

        public string Start_date { get; set; }

        public string End_date { get; set; }

        public string AlertType { get; set; }

        public string ESDALReference { get; set; }

        public string Details { get; set; }

        public int TotalCount { get; set; }


        //For showing from and TO Organisation in distribution alert        
        //distribution alert date:11/5/2015
        public string ToORGNAME { get; set; }

        public string ToContactNAME { get; set; }

        public string FromORGNAME { get; set; }

        public string ORG_TYPE_NAME { get; set; }

        public int OrganisationId { get; set; }

        public int Contact_id { get; set; }

        public string userID { get; set; }

        public int IS_MANUALLY_ADDED { get; set; }

        //Check as SOA and Police
        public string Item_Type { get; set; }
        public int Inbox_Id { get; set; }
        public int notif_id { get; set; }

        //Check as Haulier
        public int Project_id { get; set; }
        public int Version_id { get; set; }
        public int Version_no { get; set; }
        public int Revision_id { get; set; }
        public int Revision_no { get; set; }
        public int Veh_purpose { get; set; }
        public int Version_status { get; set; }

        //for notification        
        public long VehicleType { get; set; }

        //for distributionAlertfilter
        public string movementData { get; set; }
        public string showalert { get; set; }
        public byte[] OutboundDocument { get; set; }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.VehiclesAndFleets.Models.Configuration
{
  public class NewConfigurationModel
    {
        public int? vehicle_id { get; set; }
        public string vehicle_name { get; set; }
        public string vehicle_int_desc { get; set; }
        public int? vehicle_type { get; set; }
        public int? vehicle_purpose { get; set; }
        public int? organisationid { get; set; }
        public string vehicledesc { get; set; }
        public double?  len { get; set; }
        public int? lenunit{get;set;}
        public double? lenmtr { get; set; }

        public double? regidlen { get; set; }

        public int? rigidlenunit { get; set; }
        public int? rigidlenmtr { get; set; }

        public double? width { get; set; }
        public int? widthunit { get; set; }
        public double? widthmtr { get; set; }
        public double? grossweight { get; set; }
        public int? grossweightunit { get; set; }
        public int? grossweightkg { get; set; }
        public double? maxheight { get; set; }
        public int? maxheightunit { get; set; }
        public double? maxheightmtr { get; set; }
        public double? redheightmtr { get; set; }
        public double? maxaxlewight { get; set; }

        public int? maxaxlewightunit { get; set; }
        public int? maxaxleweightkg { get; set; }
        public double?  wheelbase { get; set; }
        public int? wheelbaseunit { get; set; }
        public double? speed { get; set; }
        public int? speedunit { get; set; }

        public double? tyrespacing { get; set; }
        public int? tyrespacingunit { get; set; }

      // for application vehicle added 2 nre parameter

        public int? applicationrevisionid { get; set; }
        public int? partid { get; set; }

      // for vr1 application vehicle
        public long routepartid { get; set; }
        public int RevisionID { get; set; }
        public string ContentRefNo { get; set; }
        public int versionId { get; set; }
        public int CandRevisionID { get; set; }
    }
}

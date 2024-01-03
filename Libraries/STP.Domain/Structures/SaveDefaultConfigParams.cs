using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.Structures
{
   public class SaveDefaultConfigParams
    {
      public int OrgId { get; set; }
      public  double? OrgMinWeight { get; set; }
      public double? OrgMaxWeight { get; set; }
      public  double? OrgMinSV { get; set; }
      public double? OrgMaxSV { get; set; }
      public string UserName { get; set; }
    }
}

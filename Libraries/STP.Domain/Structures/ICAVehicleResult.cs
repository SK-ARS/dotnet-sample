using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.Structures
{
   public class ICAVehicleResult
    {
      public ICAVehicleModel ObjICAVehicleModel { get; set; } 
      public ManageStructureICA ObjManageStructureICA { get; set; }
      public int MovementClassConfig { get; set; }
      public int ConfigType { get; set; }
      public int CompNum { get; set; }
      public int? TractorType { get; set; }
      public int? TrailerType { get; set; }
      public int StructureId { get; set; }
      public int SectionId { get; set; }
      public int OrgId { get; set; }
    }
}

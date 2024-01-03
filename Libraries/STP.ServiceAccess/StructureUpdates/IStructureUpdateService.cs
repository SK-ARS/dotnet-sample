using STP.Domain.StructureUpdate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.StructureUpdates
{
 public   interface IStructureUpdateService
    {
        List<SUProject> GetSUProjectList(SUProjectListParams suListParam);
    }
}

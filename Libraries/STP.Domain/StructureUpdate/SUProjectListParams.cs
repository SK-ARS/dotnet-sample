using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.StructureUpdate
{
    public class SUProjectListParams
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public int State { get; set; }

        public string DataOwnerName { get;set;}

        public string ProjectName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GpxLibrary.ConvertGpx;

namespace STP.Domain.Routes
{
    public class RouteExportList
    {
        public long TotalRecords { get; set; }
        public int NumberOfPages { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<Route> Routes { get; set; }
    }
}

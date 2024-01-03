using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.LoggingAndReporting
{
  public  class LogEventParam
    {
        public int EventType { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
    }
}

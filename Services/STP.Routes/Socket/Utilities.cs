using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.RoutePlannerInterface.Socket
{
    public static class Utilities
    {
        public static double GetCurrentTime()
        {
            //create Timespan by subtracting the value provided from
            //the Unix Epoch
            TimeSpan span = (DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToUniversalTime());
            //return the total seconds (which is a UNIX timestamp)
            return span.TotalSeconds;
        }
    }
}
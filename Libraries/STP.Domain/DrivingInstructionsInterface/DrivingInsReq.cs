using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace STP.Domain.DrivingInstructionsInterface
{
    public enum DBSchema
    {
        portal = 0,
        sort
    }
    public class DrivingInsReq
    {
        public DrivingInsReq()
        {
            ListRouteParts = new List<UInt64>();
        }
        public ulong AnalysisID { get; set; }               //Analysis ID
        public List<UInt64> ListRouteParts { get; set; } //List of Route Parts
        public string ToString()
        {
            string strMessage = "Route Parts:";
            StringBuilder bld = new StringBuilder();
            foreach (UInt64 nRouteParts in ListRouteParts)
            {
                bld.Append(nRouteParts + ""); 
            }
            string str = bld.ToString();
            strMessage += str;
            strMessage += "Analysis ID: " + AnalysisID;
            return strMessage;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.LoggingAndReporting
{
    public class SessionLengthModel
    {
        public long ID { get; set; }  //Auto increment
        public long LoginId { get; set; }  //Logged in user id
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string MachineName { get; set; }
        public string Description { get; set; }
        public decimal TotalRecordCount { get; set; }
        public decimal SOASessionCount { get; set; }
        public decimal PoliceSessionCount { get; set; }
        public decimal HaulierSessionCount { get; set; }
        public decimal AllSessionCount { get; set; }
        public int NumericMonth { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public long EventType { get; set; }
        public string Organisation { get; set; }
        public decimal TotalSessionInMonth { get; set; }
        public decimal NoofSessionInPeakDay { get; set; }
        public Int32 UserTypeID { get; set; }
        public string UserType { get; set; }
        public decimal MostSessionInDay { get; set; }
        public Int32 AvgSessionLength { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LogOutTime { get; set; }
        public double SessionDuration { get; set; }
    }
}

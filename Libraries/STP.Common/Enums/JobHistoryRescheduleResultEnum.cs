using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STP.Domain.SalesCommission.Enums
{
    public enum JobHistoryRescheduleResultEnum : int
    {
        Success = 1, 
        Failed_Because_of_some_Error = 2,
    }
}

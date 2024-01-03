using STP.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Movements
{
    public class InsertMovementVersionInsertParams
    {      
        public int ProjectID { get; set; }
        public string ApplicationReference { get; set; }
        public int FromPrevious { get; set; } = 0;
        public string UserSchema { get; set; } 
    }
}
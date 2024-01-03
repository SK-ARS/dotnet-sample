using STP.Applications.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class InsertMovementVersionInsertParams
    {      
        public int ProjectID { get; set; }
        public string AppRef { get; set; }
        public int FromPrevious { get; set; } = 0;
        public string UserSchema { get; set; } = ApplicationConstants.DbUserSchema_STPSORT;
    }
}
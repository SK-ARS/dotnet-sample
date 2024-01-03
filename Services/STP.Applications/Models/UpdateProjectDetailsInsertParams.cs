using STP.Applications.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class UpdateProjectDetailsInsertParams
    {       
        public long ProjectID { get; set; }
        public int IsSO { get; set; }
        public string UserSchema { get; set; } = ApplicationConstants.DbUserSchema_STPSORT;
    }
}
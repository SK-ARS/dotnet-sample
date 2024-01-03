using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using STP.Common.Constants;

namespace STP.Domain.Applications
{
    public class UpdateProjectDetailsInsertParams
    {       
        public long ProjectId { get; set; }
        public int IsSO { get; set; }
        public string UserSchema { get; set; } = ApplicationConstants.DbUserSchemaSTPSORT;
    }
}
﻿using STP.Applications.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class ReviseVR1Params
    {       
        public long AppRevID { get; set; }
        public string UserSchema { get; set; } = ApplicationConstants.DbUserSchema_STPSORT;
    }
}
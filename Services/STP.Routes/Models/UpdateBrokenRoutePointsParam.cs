﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static STP.Routes.Models.RouteModel;

namespace STP.Routes.Models
{
    public class UpdateBrokenRoutePointsParam
    {
        public RoutePoint routePoint { get; set; }
        public int is_lib { get; set; }
        public string userSchema { get; set; }
    }
}
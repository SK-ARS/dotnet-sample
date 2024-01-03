using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.DrivingInstructionsInterface
{
    public class DrivInstParams
    {
        public DrivingInsReq DrivingInstructionReq { get; set; }
        public string UserSchema { get; set; }
    }
}
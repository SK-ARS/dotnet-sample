using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class HaulierMovNotesInsertParams
    {        
        public long MovementVersionID { get; set; }
        public byte[] HaulierNote { get; set; }
        public string UserSchema { get; set; }
    }
}
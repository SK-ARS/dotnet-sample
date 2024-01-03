using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.DocumentsAndContents
{
    public class Enums
    {
        public enum SOTemplateType
        {
            SO2D1,
            SO2D4,
            SO2D4a,
            SO2D4b,
            SO2D5,
            SO2D7,
            SO2D7a,
            SO2D8,
            SO2D9
        }
        public enum DocumentType
        {
            PDF,
            WORD,
            EMAIL
        }
        public enum PortalType
        {
            SOA,
            POLICE,
            SPECIALORDER
        }
      
    }
}
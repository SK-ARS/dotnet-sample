using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Xml;
using STP.Domain.Structures;

namespace STP.Domain.VehicleAndFleets.Component
{
    public class IFXProperty
    {
        private IFXProperty oProperty;

        public IFXProperty(IFXProperty oProperty)
        {
            this.DisplayString = new string(oProperty.DisplayString.ToCharArray());
            this.ParamModel = new string(oProperty.ParamModel.ToCharArray());
            
            if (oProperty.ParamValue != null)
            {
                this.ParamValue = new string(oProperty.ParamValue.ToCharArray());

            }
            this.ParamType = new string(oProperty.ParamType.ToCharArray());
            this.ParamMaxLength = new string(oProperty.ParamMaxLength.ToCharArray());
            if (oProperty.StrRange != null)
            {
                this.StrRange = new string(oProperty.StrRange.ToCharArray());
            }
            this.InputType = new string(oProperty.InputType.ToCharArray());
            this.IsRequired = oProperty.IsRequired;
            this.ShowText = oProperty.ShowText;
            this.TextId = oProperty.TextId;
            if (oProperty.ValidRegex != null)
            {
                this.ValidRegex = new string(oProperty.ValidRegex.ToCharArray());
            }
            if(oProperty.DropDownList!=null)
                this.DropDownList = new List<string>(oProperty.DropDownList);
        }

        public IFXProperty()
        {
            // TODO: Complete member initialization
        }
        public string DisplayString { get; set; }
        public string ParamModel { get; set; }
        public string ParamValue { get; set; }
        public string ParamType { get; set; }
        public string ParamMaxLength { get; set; }
        public string StrRange { get; set; }
        public List<string> DropDownList { get; set; }
        public string InputType { get; set; }
        public int IsRequired { get; set; }
        public string ValidRegex { get; set; }
        public int ShowText { get; set; }
        public string TextId { get; set; }

    }
    
}

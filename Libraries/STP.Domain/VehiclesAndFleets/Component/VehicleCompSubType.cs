using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.VehicleAndFleets.Component
{
    public class VehicleCompSubType
    {
        public VehicleCompSubType(int vehicleSubCompId, string vehicleSubCompName)
        {
            SubCompType = vehicleSubCompId;
            SubCompName = vehicleSubCompName;
        }

       public VehicleCompSubType(VehicleCompSubType vehicleCompSubType)
        {
            SubCompType = vehicleCompSubType.SubCompType;
            SubCompName = new string(vehicleCompSubType.SubCompName.ToCharArray());
            axleValidator = new AxleValidator(vehicleCompSubType.axleValidator);
            ImageName = vehicleCompSubType.ImageName;
        }

        //vehicle component sub type id
        public int SubCompType { get; set; }
        //subcomponent name
        public string SubCompName { get; set; }
        //AxleValidotor. This is specifi to a subcomponent
        public AxleValidator axleValidator {get; set;}
        //list of vehicleparameter class
        public List<IFXProperty> VehicleParameterList { get; set; }
        //Image Name of the component
        private string imageName;
        public string ImageName
        {
            get { return imageName; }
            set { imageName = new string(value.ToCharArray()); }
        }
    }
}

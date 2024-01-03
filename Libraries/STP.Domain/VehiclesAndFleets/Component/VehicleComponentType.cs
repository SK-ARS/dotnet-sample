using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.VehicleAndFleets.Component
{
    public class VehicleComponentType
    {

        public VehicleComponentType(int vechicleCompId, string vechicleCompName, bool isTractor, string imageName)
        {
            ComponentTypeId = vechicleCompId;
            ComponentName = vechicleCompName;
            IsTractor = isTractor;
            ImageName = imageName;
        }

        public VehicleComponentType(VehicleComponentType vehicleComponentType)
        {
            ComponentTypeId    = vehicleComponentType.ComponentTypeId;
            ComponentName      = new string (vehicleComponentType.ComponentName.ToCharArray());
            ImageName = vehicleComponentType.ImageName;
            IsTractor = vehicleComponentType.IsTractor;
        }

        public VehicleComponentType()
        {
            // TODO: Complete member initialization
        }

        //Component type Id
        public int ComponentTypeId { get; set; }
        //Component Name
        public string ComponentName { get; set; }
        //Image Name of the component
        private string imageName;
        public string ImageName{
            get{ return imageName;}
            set {imageName = new string(value.ToCharArray());}
        }
        //flag whether the sub component is a tractor or trailer
        public bool IsTractor { get; set; }

    }
}

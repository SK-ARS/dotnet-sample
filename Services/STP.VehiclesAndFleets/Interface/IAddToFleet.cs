using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace STP.VehiclesAndFleets.Interface
{
    public interface IAddToFleet
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentid"></param>
        /// <param name="organisationid"></param>
        /// <returns></returns>
        int AddComponentToFleet(int componentid, int organisationid);
        int AddVR1ComponentToFleet(int componentid, int organisationid, int flag, string userSchema);
        int AddApplicationComponentToFleet(int componentid, int organisationid, int flag, string userSchema);
        int AddMovementComponentToFleet(int componentid, int organisationid, string userSchema);
    }
}

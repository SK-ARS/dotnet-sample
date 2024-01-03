using STP.Domain;
using STP.Domain.VehiclesAndFleets.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.VehiclesAndFleets.Interface
{
  public  interface IVehiConfigGetConfigByOrgId
    {
        List<VehicleConfigurationGridList> GetConfigByOrganisationId(int organisationId, int movtype, int movetype1, string userSchema, int filterFavouritesVehConfig,int presetFilter=1,int ? sortOrder=1);
    }
}

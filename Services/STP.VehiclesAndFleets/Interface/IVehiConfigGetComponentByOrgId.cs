using STP.Domain;
using STP.Domain.VehiclesAndFleets.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.VehiclesAndFleets.Interface
{
   public  interface IVehiConfigGetComponentByOrgId
    {
        List<ComponentGridList> GetAllVR1ComponentByOrganisationId(long componentId, string userSchema);
        List<ComponentGridList> GetAllComponentByOrganisationId(int organisationId, long componentId);
        List<ComponentGridList> GetAllAppComponentByOrganisationId(long componentId, string userSchema);
        List<ComponentGridList> GetComponentByOrganisationId(int organisationId, int pageNumber, int pageSize, string componentName, string componentType, string vehicleIntent, int filterFavourites, string userSchema, int presetFilter, int? sortOrder = null);
        List<ComponentIdModel> GetListComponentId(int vehicleId);
        List<ComponentIdModel> GetVR1ListComponentId(int vehicleId, string userSchema);
        List<ComponentIdModel> GetAppListComponentId(int vehicleId, string userSchema);
    }
}


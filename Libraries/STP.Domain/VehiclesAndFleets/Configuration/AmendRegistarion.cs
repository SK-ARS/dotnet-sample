using System.Collections.Generic;

namespace STP.Domain.VehiclesAndFleets.Configuration
{
    public class AmendRegistarion
    {
        public int VehicleId { get; set; }
        public List<RegistrationDetail> RegistrationDetails { get; set; }
        public List<int> DeletedConfigIds { get; set; }

    }

    public class RegistrationDetail
    {
        public string RegId { get; set; }
        public string FleetId { get; set; }
        public int Id { get; set; }
    }
}

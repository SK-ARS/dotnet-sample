using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Movements
{
    public class HaulierMovementsListParams
    {
        public int OrganisationId { get; set; }

        public int PageNo { get; set; }

        public int PageSize { get; set; }

        public MovementsFilter MovementFilter { get; set; }

        public MovementsAdvancedFilter AdvancedMovementFilter { get; set; }

        public int PresetFilter { get; set; }

        public string UserSchema { get; set; }

        public int ShowPreviousSORTRoute { get; set; }

        public bool PrevMovImport { get; set; }
        public int VehicleClass { get; set; }
        public int MovementType { get; set; }
    }
}
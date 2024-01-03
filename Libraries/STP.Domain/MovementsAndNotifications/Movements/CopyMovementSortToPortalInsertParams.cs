using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.MovementsAndNotifications.Movements
{
    public class CopyMovementSortToPortalInsertParams
    {
        public MovementCopyDetails MovementCopyDetail { get; set; }
        public int MovementCloneStatus { get; set; }
        public int VersionID { get; set; }
        public string EsdalReference { get; set; }
        public byte[] HAContactBytes { get; set; }
        public int OrganizationID { get; set; }
        public string ModelUserSchema { get; set; }
    }
}

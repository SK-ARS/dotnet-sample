using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.VehicleAndFleets.Component
{
    public class MovementClassification
    {
        private int movementId;
        private string movementName;

        public MovementClassification(int movementId, string movementName)
        {
            // TODO: Complete member initialization
            this.ClassificationId = movementId;
            this.ClassificationName = movementName;
        }

        public MovementClassification()
        {
            // TODO: Complete member initialization
        }
        //Movement classification Id
        public int ClassificationId { get; set; }
        //Movement Classification Name
        public string ClassificationName { get; set; }
    }
}

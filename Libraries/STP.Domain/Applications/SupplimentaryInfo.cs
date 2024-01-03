using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.Applications
{
    public class SupplimentaryInfo
    {
        public string TotalDistanceOfRoad { get; set; }
        public string ApprValueOfLoad { get; set; }
        public string DateOfAuthority { get; set; }
        public int LoadDivision { get; set; }
        public string AdditionalCost { get; set; }
        public string RiskNature { get; set; }
        public int Shipment { get; set; }
        public string PortNames { get; set; }
        public string SeaQuotation { get; set; }
        public int Address { get; set; }
        public string ProposedMoveDetails { get; set; }
        public string ApprCostOfMovement { get; set; }
        public string AdditionalConsideration { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STP.Domain.Structures
{
    public class ConfigBandModel
    {
        /// <summary>
        /// Structure Id
        /// </summary>
        public int StructureId { get; set; }
        /// <summary>
        /// Is Default Banding Weight
        /// </summary>
        public bool? IsDefaultWeight { get; set; }
        /// <summary>
        /// Is Default Banding SV
        /// </summary>
        public bool? IsDefaultSV { get; set; }
        /// <summary>
        /// Minimum default weight
        /// </summary>

        public double DefaultMinWeight { get; set; }
        /// <summary>
        /// Maximum default weight
        /// </summary>
        public double DefaultMaxWeight { get; set; }
        /// <summary>
        /// Minimum Organisation weight
        /// </summary>        
        [RegularExpression(@"[0-9\.]+$", ErrorMessage = "Only numbers and . are allowed")]
        public double? OrgMinWeight { get; set; }
        /// <summary>
        /// Maximum Organisation weight
        /// </summary>
        [RegularExpression(@"[0-9\.]+$", ErrorMessage = "Only numbers and . are allowed")]
        public double? OrgMaxWeight { get; set; }
        /// <summary>
        /// Minimum default SV
        /// </summary>
        public double DefaultMinSV { get; set; }
        /// <summary>
        /// Maximum default SV
        /// </summary>
        public double DefaultMaxSV { get; set; }
        /// <summary>
        /// Minimum Organisation SV
        /// </summary>
        [RegularExpression(@"[0-9\.]+$", ErrorMessage = "Only numbers and . are allowed")]
        public double? OrgMinSV { get; set; }
        /// <summary>
        /// Maximum Organisation SV
        /// </summary>
        [RegularExpression(@"[0-9\.]+$", ErrorMessage = "Only numbers and . are allowed")]
        public double? OrgMaxSV { get; set; }
    }
}
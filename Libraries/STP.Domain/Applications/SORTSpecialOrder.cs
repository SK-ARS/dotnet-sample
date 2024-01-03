using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STP.Domain.Applications
{
    public class SORTSpecialOrder
    {
        /// <summary>
        /// Project Id
        /// </summary>
        public int ProjectID { get; set; }
        /// <summary>
        /// ESDAL Reference number
        /// </summary>
        public string ESDALNo { get; set; }
        /// <summary>
        /// Special order number
        /// </summary>
        [Display(Name = "Special order number")]
        public string SONumber { get; set; }
        /// <summary>
        /// SO creation date
        /// </summary>
        [Display(Name = "Creation date*")]
        public string SOCreateDate { get; set; }
        /// <summary>
        /// expiry date
        /// </summary>
        [Display(Name = "Expiry date*")]
        [Required(ErrorMessage = "Expiry date is required")]
        public string ExpiryDate { get; set; }
        /// <summary>
        /// template
        /// </summary>
        [Display(Name = "Template*")]
        [Required(ErrorMessage = "Template is required")]
        public string Template { get; set; }
        /// <summary>
        /// signatory
        /// </summary>
        [Display(Name = "Signatory*")]
        [Required(ErrorMessage = "Signatory is required")]
        public string Signatory { get; set; }
        /// <summary>
        /// Signatories role
        /// </summary>
        [Display(Name = "Signatories role*")]
        [Required(ErrorMessage = "Signatories role is required")]
        public string SignatoryRole { get; set; }
        /// <summary>
        /// Signed date
        /// </summary>
        [Display(Name = "Signature date*")]
        [Required(ErrorMessage = "Signature date is required")]
        public string SignDate { get; set; }
        /// <summary>
        /// state
        /// </summary>
        [Display(Name = "State*")]
        public string State { get; set; }
        public int Year { get; set; }
        public int VersionID { get; set; }
        public string Recommended { get; set; }
        public string Applicability { get; set; }
        public int CoverageStatus { get; set; }
        public int SpecialOrderCount { get; set; }
        public long PlannerID { get; set; }
        public string ProjectStatus { get; set; }
    }
	public class SOCoverageModel
    {
        public List<SORTRoutePart> RoutePart { get; set; }
        public List<SORTVehicleConfig> VehicleConfig { get; set; }
    }

    public class SORTRoutePart
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class SORTVehicleConfig
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }    
}
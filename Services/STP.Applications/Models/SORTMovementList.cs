using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Applications.Models
{
    public class SORTMovementList
    {
        /// <summary>
        /// SORT type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// SORT priority
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// ESDAL2 Reference string
        /// </summary>
        public string ESDALRef { get; set; }
        /// <summary>
        /// Movement from location
        /// </summary>
        public string MoveFrom { get; set; }
        /// <summary>
        /// Movement to location
        /// </summary>
        public string MoveTo { get; set; }
        /// <summary>
        /// Movement to location
        /// </summary>
        public int VehicleClassification { get; set; }
        /// <summary>
        /// Total record count
        /// </summary>
        public int TotalRecordCount { get; set; }
        public int WorkStatus { get; set; }        
        public int ProjectStatus { get; set; }       
        public string Owner { get; set; }        
        public string CheckerName { get; set; }        
        public string ApplicationDate { get; set; }        
        public string DueDate { get; set; }        
        public string FromDate { get; set; }        
        public string ToDate { get; set; }          
        public long MovementVersionId { get; set; }        
        public long MovementRevisionId { get; set; }        
        public int ProjectEsdalReference { get; set; }        
        public string HaulierMnemonic { get; set; }        
        public int ApplicationRevisionNum { get; set; }       
        public int MovementVersionNumber { get; set; }        
        public string MovementType { get; set; }        
        public int VersionStatus { get; set; }        
        public string ESDALReference { get; set; }        
        public string MyReference { get; set; }        
        public long AppRevID { get; set; }       
        public long ProjectID { get; set; }       
        public int RevisionNo { get; set; }       
        public string CommittedDate { get; set; }       
        public long VersionNumber { get; set; }        
        public long VersionID { get; set; }        
        public long RevisionID { get; set; }        
        public string OrderNumber { get; set; }          
        public string ExpiryDate { get; set; }        
        public int OrganisationID { get; set; }      
        public int AppStatus { get; set; }     
        public int EnterBySORT { get; set; }       
        public DateTime SOCreateDate { get; set; }
        public int IsWithdrawn { get; set; }
        public int IsDeclined { get; set; }
        public long PlannerID { get; set; }
        public long CheckerID { get; set; }       
        public string DistributionDate { get; set; }
        public int Days { get; set; }
        public int IsNotified { get; set; }
    }
}
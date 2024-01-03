using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace STP.Structures.Models
{
    public class DelegationList
    {
        //variables for delegation arrangementlist
        public long ArrangementId { get; set; }
        public string ArrangementName { get; set; }
        public string OrganisationName { get; set; }
        public string DelegatedOrganisationName { get; set; }
        public int AllowSubDelegation { get; set; }
        public bool SubDelegation { get; set; }
        public bool CopyNotification { get; set; }
        public bool NotifyIfFails { get; set; }       
        //variables for review delegation list
        public int TotalNoOfStructures { get; set; }
        public int RetainNotification { get; set; }
        public int AcceptFailure { get; set; }
        public string ContactName { get; set; }
        public int DelegateAll { get; set; }
        public long OrgToId { get; set; }    
        /// <summary>
        /// Below both properties use for search criteria
        /// author - Newweb
        /// Createation date  - 27 March 2014
        /// </summary>
        public string searchType { get; set; } //For search panel id , value pair
        public string searchValue { get; set; } //For search panel id , value pair
        /// <summary>
        /// Used to store total record count
        /// </summary>
        public decimal TOTAL_RECORD_COUNT { get; set; }
        /// <summary>
        /// Ogranisation Id
        /// </summary>
        public long OrganisationId { get; set; }
        /// <summary>
        /// List of StructureInDelegList
        /// </summary>
        public List<StructureInDelegList> StructureInDelegations { get; set; }
        public List<StructureContactsList> StructureInContactList { get; set; }
        /// <summary>
        /// Contact Type
        /// </summary>
        public string ContactType { get; set; }
        /// <summary>
        /// Contact Type List
        /// </summary>
        public long ContactId { get; set; }
        public short IS_ROAD_DELEGATION { get; set; }
        public short IS_AREA_DELEGATION { get; set; }
        public AcceptType SelectedType { get; set; }
        public string SelectedTypeName { get; set; }
        public decimal ALLOWING_SUBDELEGATE_STRUCTURES { get; set; }
    }
    public class StructureInDelegList
    {
        public long StructureId { get; set; }
        public string StructureName { get; set; }
        public string StructureReference { get; set; }
        public string StructureOwnedBy { get; set; }
        public bool Delete { get; set; }
        public double OWNER_Id { get; set; }
        /// <summary>
        /// Used to store total record count
        /// </summary>
        public decimal TOTAL_RECORD_COUNT { get; set; }
    }
    public class StructureContactsList
    {
        public long StructureId { get; set; }
        public long ChainNo { get; set; }
        public int Position { get; set; }
        public string StructureCode { get; set; }
        public long OwnerID { get; set; }
        public string OwnerName { get; set; }
        public long OrganisationID { get; set; }
        public string OrganisationName { get; set; }
        public long ContactId { get; set; }
        public string ContactName { get; set; }
        public int RoleType { get; set; }
        public long ArrangementId { get; set; }
        public int Last_Delegation { get; set; }
        public int ReceiveFailure { get; set; }
        public int StructureEnabled { get; set; }
        public int RetainNotification { get; set; }
        public int AllowSubDelegation { get; set; }
        public long OrgToID { get; set; }
        public decimal OrganisationCount { get; set; }
        public string DefaultContact { get; set; }
    }
    public class RoadDelegationList
    {   //Delegation arrangement ID 
        public long ArrangementId { get; set; }
        //Delegating Structure Owener Name
        public string DelegatingSOAName { get; set; }
        //Structure Owener Name
        public string SOAName { get; set; }
        //Delegated to contact name
        public string ContactName { get; set; }
        //Delegating Delegation Name
        public string Delegating_DelegationName { get; set; }
        //Delegation Name
        public string DelegationName { get; set; }
        //Retain Notificaiton (Yes/No)
        public string RetainNotification { get; set; }
        //Delegation Created Date
        public string CreatedDate { get; set; }
        // Used to store total record count
        public decimal TOTAL_RECORD_COUNT { get; set; }
    }
    public class DelegationStructuresList
    {
        public DelegationList CreateDelegationList { get; set; }
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public int StructurePerPageCount { get; set; }
        public long organisationId { get; set; }
        public int OrgFromId { get; set; }
        public long ArrangementID { get; set; }
        public List<StructureInDelegList> StructuresGridToShow { get; set; }
        public object StructuresStaticPageList { get; set; }
        public long ContactId { get; set; }
        public string SelectedTypeName { get; set; }
        public int AcceptFailure { get; set; }
        public AcceptType SelectedType { get; set; }
        public string ArrangementName { get; set; }
        public bool CopyNotification { get; set; }
        public bool SubDelegation { get; set; }
        public int DelegateAll { get; set; }
        public short IS_ROAD_DELEGATION { get; set; }
        public short IS_AREA_DELEGATION { get; set; }
        public int RetainNotification { get; set; }
        public List<StructureInDelegList> StructureInDelegations { get; set; }
        public List<StructureContactsList> StructureInContactList { get; set; }
    }
    public enum AcceptType
    {
        Yes,
        No
    }
}

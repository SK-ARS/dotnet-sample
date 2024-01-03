using STP.Structures.Interface;
using STP.Domain;
using STP.Structures.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using STP.Domain.Structures;
namespace STP.Structures.Providers
{
    public class DelegationArrangementProvider : IDelegationArrangementProvider
    {
        #region DelegationArrangementProvider Singleton

        private DelegationArrangementProvider()
        {
        }
        public static DelegationArrangementProvider Instance
        {
            [DebuggerStepThrough]
            get
            {
                return Nested.instance;
            }
        }

        /// <summary>
        /// Not to be called while using logic
        /// </summary>
        internal static class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
            internal static readonly DelegationArrangementProvider instance = new DelegationArrangementProvider();
        }

        #region Logger instance

        private const string PolicyName = "DelegationArrangementProvider";     

        #endregion


        #endregion

        /// <summary>
        /// Returns deletegation arrangment list
        /// </summary>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="searchType">Search type</param>
        /// <param name="searchValue">Search value</param>
        /// <returns></returns>
        public List<DelegationList> GetDelegArrangList(long organisationId, int pageNumber, int pageSize, string searchType, string searchValue, int presetFilter, int? sortOrder = null)
        {
            return StructureDAO.GetDelegArrangList(organisationId, pageNumber, pageSize, searchType, searchValue, presetFilter, sortOrder);
        }
        /// <summary>
        /// Returns deletegation arrangment 
        /// </summary>
        /// <param name="arrangid">Arrangement Id</param>
        /// <param name="orgId">Organization Id</param>       
        /// <returns></returns>
        public DelegationList GetArrangement(long arrangid, int orgId)
        {
            return StructureDAO.GetArrangementDetails(arrangid, orgId);
        }
        /// <summary>
        /// Returns organisation list
        /// </summary>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="Organisationname">Organisation name</param>
        /// <returns></returns>
        public List<DelegationList> GetOrganisationList(int pageNumber, int pageSize, string Organisationname)
        {
            return StructureDAO.GetOrganisationList(pageNumber, pageSize, Organisationname);
        }
        /// <summary>
        /// Returns deligation list
        /// </summary>
        /// <param name="organisationId">Organization Id</param>
        /// <param name="delegationName">Delegation Name</param>       
        /// <returns></returns>
        public List<DropDown> GetDelegationAutoFill(int organisationId, string delegationName)
        {
            return StructureDAO.GetDelegationNameList(organisationId, delegationName);
        }
        /// <summary>
        /// Returns deletegation arrangment list
        /// </summary>
        /// <param name="orgId">Organisation Id</param>
        /// <param name="pageNum">Page Number</param>
        /// <param name="arrangName">Arrangement Name type</param>        
        /// <returns></returns>
        public List<DelegationList> GetDelegateArrangList(int orgId, int pageNum, int pageSize, string arrangName)
        {           
            return StructureDAO.GetDelegArrangementList(orgId, pageNum, pageSize, arrangName);
        }
        /// <summary>
        /// Returns Structures in Delegation list 
        /// </summary>
        /// <param name="arrangid">Arrangement  Id</param>
        /// <param name="orgId">Organisation Id</param>
        /// <param name="pageNum">pageNum</param>  
        /// <param name="pageSize">pageSize</param>         
        /// <returns></returns>
        public List<StructureInDelegationList> GetStructuresInDeleg(long arrangid, long orgId, int? pageNum, int? pageSize)
        {
            return StructureDAO.GetStructuresInDelegation(arrangid, orgId, pageNum, pageSize);
        }
        /// <summary>
        /// Returns contact list
        /// </summary>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="Contactname">Contactname name</param>
        /// <param name="orgID">Organisation Id</param>
        /// <returns></returns>
        public List<DelegationList> GetContactList(int pageNumber, int pageSize, string Contactname, int orgID)
        {
            return StructureDAO.GetContactList(pageNumber, pageSize, Contactname, orgID);
        }
        /// <summary>
        /// Returns the List of Road Delegation Arrengement
        /// </summary>
        /// <param name="pageNum">page number</param>
        /// <param name="pageSize">page size</param>
        /// <returns>RoadDelegationList</returns>
        public List<RoadDelegationList> GetRoadDelegationList(int pageNum, int pageSize, long OrganisationId,int presetFilter, int? sortOrder = null)
        {
            return StructureDAO.GetRoadDelegationList(pageNum, pageSize, OrganisationId,presetFilter,sortOrder);
        }
        /// <summary>
        /// Returns Structure In Delegation list
        /// </summary>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Size of page</param>
        /// <param name="structurecodes">combine structure code</param>
        /// <param name="pageSize">Size of page</param>
        /// <param name="structurecodecount">Structure code count</param>
        /// <returns>StructureInDelegationList</returns>
        public List<StructureInDelegationList> GetStructureInDelegationList(int pageNumber, int? pageSize, string structurecodes, int OrganisationId, int structurecodecount)
        {
            return StructureDAO.GetStructureInDelegationList(pageNumber, pageSize, structurecodes, OrganisationId, structurecodecount);
        }
        /// <summary>
        /// overriding function to take input of array of structure codes that are delegated/checked by user
        /// </summary>
        /// <param name="structurecodes"></param>
        /// <param name="OrganisationId"></param>
        /// <returns></returns>
        public List<StructureInDelegationList> GetStructureInDelegationList(string[] structurecodes, int OrganisationId)
        {
            return StructureDAO.GetStructureInDelegationList(structurecodes, OrganisationId);
        }
        /// <summary>
        /// Save Structures
        /// </summary>
        /// <param name="delegationList">DelegationList model</param>
        /// <returns></returns>
        public bool ManageStructureDelegation(DelegationList delegationList)
        {
            return StructureDAO.ManageStructureDelegation(delegationList);
        }
        /// <summary>
        /// Saves structure contact details while adding delegation arrangements
        /// </summary>
        /// <param name="structureContact">structure contact list</param>
        /// <returns>boolean value indicating the success / failure of structure contact details</returns>
        public bool ManageDelegationStructureContact(DelegationList structureContact)
        {
            return StructureDAO.ManageDelegationStructureContact(structureContact);
        }
        /// <summary>
        /// Check sub delegation details before adding delegation arrangements
        /// </summary>
        /// <param name="structureID"></param>
        /// <param name="organisationID"></param>
        /// <returns></returns>
        public int CheckSubDelegationList(long structureID, long organisationID)
        {
            return StructureDAO.CheckSubDelegationList(structureID, organisationID);
        }
        /// <summary>
        /// delete structureedit in delegation arrangement
        /// </summary>        
        /// <param name="arrangId"></param>
        /// <returns></returns>
        public int DeleteStructureEdit(long arrangementId)
        {
            return StructureDAO.DeleteStructureEdit(arrangementId);
        }
        /// <summary>
        /// Get structure contact list based upon arrangement Id
        /// </summary>
        /// <param name="arrangementId">ArrangementId of the respective structure contact</param>
        /// <returns>List of structure contact</returns>
        public List<StructureContactsList> GetStructureContactList(long arrangementId)
        {
            return StructureDAO.GetStructureContactList(arrangementId);
        }
        /// <summary>
        /// delete structure in delegation arrangement
        /// </summary>
        /// <param name="structId"></param>
        /// <param name="arrangId"></param>
        /// <returns></returns>
        public int DeleteStructInDelegation(long structId, long arrangId)
        {
            return StructureDAO.DeleteStructInDelegation(structId, arrangId);
        }
        /// <summary>
        /// Save delegation arrangement
        /// </summary>
        /// <param name="savedelegation">DelegationList model</param>
        /// <returns></returns>
        public DelegationList ManageDelegationArrangement(DelegationList savedelegation, long organisationId)
        {
            return StructureDAO.ManageDelegationArrangement(savedelegation, organisationId);
        }
        /// <summary>
        /// DeleteStructureContact
        /// </summary>
        /// <param name="structContactList">Parameter passed through StructureContactsList model</param>
        /// <returns></returns> 
        public int DeleteStructureContact(long StructureId, string StructureCode, long ArrangementId, long OwnerID)
        {
            return StructureDAO.DeleteStructureContact( StructureId, StructureCode,  ArrangementId,  OwnerID);
        }
        /// <summary>
        /// delete delegation arrangement
        /// </summary>
        /// <param name="arrangId">arrangId</param>
        /// <returns></returns>
        public int DeleteDelegationArrangement(long arrangId)
        {
            return StructureDAO.DeleteDelegationArrangement(arrangId);
        }

        public List<StructureSummary> GetNotDelegatedStructureListSearch(int orgId, int pageNum, int pageSize, SearchStructures objSearchStruct,int sortOrder,int sortType)
        {
            return StructureDAO.GetNotDelegatedStructureListSearch(orgId, pageNum, pageSize, objSearchStruct,sortOrder,sortType);
        }

        public List<StructureDeleArrList> GetStructureDeleArrg(string StructureCode)
        {
            return StructureDAO.GetStructureDeleArrg( StructureCode);
        }
    }
}
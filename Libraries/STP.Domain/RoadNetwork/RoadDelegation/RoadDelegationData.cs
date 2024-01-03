using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using NetSdoGeometry;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace STP.Domain.RoadNetwork.RoadDelegation
{
    #region RoadContact
    [DataContract]
    public class RoadContact
    {
        [DataMember]
        public long ManagerOrgId { get; set; }
        [DataMember]
        public long ManagerContactId { get; set; }
        [DataMember]
        public long DelegatingOrgId { get; set; }
        [DataMember]
        public long DelegatingContactId { get; set; }

        [DataMember]
        public string ManagerOrgName { get; set; }
        [DataMember]
        public string ManagerContactName { get; set; }
        [DataMember]
        public string DelegatingOrgName { get; set; }
        [DataMember]
        public string DelegatingContactName { get; set; }
    }
    #endregion
    public class RoadDelegationOrgSummary
    {
        public long OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public sdogeometry OrganisationGeoRegion { get; set; }
        public int TotalRows { get; set; }
        public string ContactName { get; set; }
    }
    public class RoadDelegationContactSummary
    {
        public long ContactId { get; set; }
        public string ContactRole { get; set; }
        public string FullName { get; set; }
        public int TotalRows { get; set; }
        public RoadDelegationOrgSummary RoadDelegOrgDetails { get; set; }
        public RoadDelegationContactSummary()
        {
            RoadDelegOrgDetails = new RoadDelegationOrgSummary();
        }
    }
    
    #region RoadLink
    [DataContract]
    public class RoadLink
    {
        [DataMember]
        public long ArrangementId { get; set; }

        [DataMember]
        public int AcceptFailure { get; set; }
        [DataMember]
        public int RetainNotification { get; set; }
        [DataMember]
        public int GroupId { get; set; }
        [DataMember]
        public int CountryId { get; set; }
        [DataMember]
        public string RoadName { get; set; }

        [DataMember]
        public sdogeometry RoadGeometry { get; set; }

        [DataMember]
        public List<RoadContact> RoadContactInfo { get; set; }

        public LinkInfo RoadLinkInfo { get; set; }

        public RoadLink()
        {
            RoadContactInfo = new List<RoadContact>();
            RoadLinkInfo = new LinkInfo();
        }
    }
    #endregion

    #region LinkInfo
    [DataContract]
    public class LinkInfo
    {
        [DataMember]
        public long LinkId { get; set; }
        [DataMember]
        public Int32? ResponsibilityFrom { get; set; }
        [DataMember]
        public Int32? ResponsibilityTo { get; set; }
        [DataMember]
        public sdogeometry LinkGeometry { get; set; }
        [DataMember]
        public bool SubDelegationAllowed { get; set; }
        [DataMember]
        public long? ArrangementId { get; set; }
        [DataMember]
        public char LinkManageStatus { get; set; }
    }
    #endregion

    public class RoadDelegationDataMapperInput
    {
        public string CompressedRoadDelegationString { get; set; }
    }
    public class RoadDelegationDataMapper
    {
        [DataMember]
        public RoadDelegationData RoadDelegationData { get; set; }
    }

    #region RoadDelegationData
    [DataContract]
    public class RoadDelegationData
    {
        [DataMember]
        public long ArrangementId { get; set; }
        [DataMember]
        public long FromOrgId { get; set; }
        [DataMember]
        public long ToOrgId { get; set; }
        [DataMember]
        public long FromContactId { get; set; }
        [DataMember]
        public long ToContactId { get; set; }
        [DataMember]
        public long RoadGroupNo { get; set; }
        [DataMember]
        public string ArrangementName { get; set; }
        [DataMember]
        public string FromOrgName { get; set; }
        [DataMember]
        public string ToOrgName { get; set; }
        [DataMember]
        public string FromContactName { get; set; }
        [DataMember]
        public string ToContactName { get; set; }
        [DataMember]
        public string Comments { get; set; }
        [DataMember]
        public int RetainNotification { get; set; }
        [DataMember]
        public int AcceptFailure { get; set; }
        [DataMember]
        public int AllowSubdelegation { get; set; }
        [DataMember]
        public int SelectedRadio { get; set; }
        [DataMember]
        public int DelegateAll { get; set; }
        [DataMember]
        public long TotalRecordCount { get; set; }
        [DataMember]
        public List<long> LinkIdList { get; set; }
        [DataMember]
        public List<LinkInfo> LinkInfoList { get; set; }        
        [DataMember]
        public string FromContactType { get; set; }
        //[DataMember]
        //public decimal TOTAL_RECORD_COUNT { get; set; }
        public RoadDelegationData()
        {
            LinkInfoList = new List<LinkInfo>();
        }
    }
    #endregion

    #region RoadDelegationSearchParam
    [DataContract]
    public class RoadDelegationSearchParam
    {
        [DataMember]
        public string ArrangementName { get; set; }
        [DataMember]
        public string FromOrgName { get; set; }
        [DataMember]
        public string ToOrgName { get; set; }
        [DataMember]
        public string RoadName { get; set; }
        [DataMember]
        public string SearchText { get; set; }
        [DataMember]
        public long? ArrangementId { get; set; }
        /// <summary>
        /// from org id is the Manager organisation id when links are fetched for displaying on map.
        /// from org id is the delegating organisation id for fetching links based on delegation arrangements
        /// </summary>
        [DataMember]
        public long? FromOrgId { get; set; }
        /// <summary>
        /// to org id is the Delegating organisation id when links are fetched for displaying on map.
        /// to org id is the manager organisation id for fetching links based on delegation arrangements
        /// </summary>
        [DataMember]
        public long? ToOrgId { get; set; }
    }
    #endregion

    #region DataLinkContactsData
    [DataContract]
    public class DataLinkContactsData
    {
        [DataMember]
        public long LinkId { get; set; }

        [DataMember]
        public int? ResponsibilityFrom { get; set; }

        [DataMember]
        public int? ResponsibilityTo { get; set; }

        [DataMember]
        public long ManagerId { get; set; }

        [DataMember]
        public long DelegatingId { get; set; }

        [DataMember]
        public long HaMacId { get; set; }

        [DataMember]
        public long LocalAuthorityId { get; set; }

        [DataMember]
        public long PoliceForceId { get; set; }

        [DataMember]
        public long ArrangementId { get; set; }
    }
    #endregion

    public class DelegationArrangementDetails
    {
        public long ToOrgId { get; set; }
        public string ArrangementName { get; set; }
        public int DelegateAll { get; set; }
    }
    public class Delegation : INullable, IOracleCustomType
    {
        private bool m_IsNull;

        [OracleObjectMappingAttribute("LINK_ID")]
        public long LinkId { get; set; }

        [OracleObjectMappingAttribute("RESPONSIBILITY_FROM")]
        public long? ResponsibilityFrom { get; set; }

        [OracleObjectMappingAttribute("RESPONSIBILITY_TO")]
        public long? ResponsibilityTo { get; set; }

        [OracleObjectMappingAttribute("MANAGER_ID")]
        public long ManagerId { get; set; }

        [OracleObjectMappingAttribute("DELEGATING_ID")]
        public long DelegatingId { get; set; }

        [OracleObjectMappingAttribute("HA_MAC_ID")]
        public long HaMacId { get; set; }

        [OracleObjectMappingAttribute("LOCAL_AUTHORITY_ID")]
        public long LocalAuthorityId { get; set; }

        [OracleObjectMappingAttribute("POLICE_FORCE_ID")]
        public long PoliceForceId { get; set; }

        [OracleObjectMappingAttribute("ARRANGEMENT_ID")]
        public long? ArrangementId { get; set; }

        public virtual bool IsNull
        {
            get
            {
                return m_IsNull;
            }
        }

        // SUProject.Null is used to return a NULL SUProject object
        public static Delegation Null
        {
            get
            {
                Delegation delegObj = new Delegation();
                delegObj.m_IsNull = true;
                return delegObj;
            }
        }
        public void FromCustomObject(OracleConnection con, IntPtr pUdt)
        {
            OracleUdt.SetValue(con, pUdt, "LINK_ID", LinkId);
            OracleUdt.SetValue(con, pUdt, "RESPONSIBILITY_FROM", ResponsibilityFrom);
            OracleUdt.SetValue(con, pUdt, "RESPONSIBILITY_TO", ResponsibilityTo);
            OracleUdt.SetValue(con, pUdt, "MANAGER_ID", ManagerId);
            OracleUdt.SetValue(con, pUdt, "DELEGATING_ID", DelegatingId);
            OracleUdt.SetValue(con, pUdt, "HA_MAC_ID", HaMacId);
            OracleUdt.SetValue(con, pUdt, "LOCAL_AUTHORITY_ID", LocalAuthorityId);
            OracleUdt.SetValue(con, pUdt, "POLICE_FORCE_ID", PoliceForceId);
            OracleUdt.SetValue(con, pUdt, "ARRANGEMENT_ID", ArrangementId);
        }

        public void ToCustomObject(OracleConnection con, IntPtr pUdt)
        {
            LinkId = (long)OracleUdt.GetValue(con, pUdt, "LINK_ID");
            ResponsibilityFrom = (long?)OracleUdt.GetValue(con, pUdt, "RESPONSIBILITY_FROM");
            ResponsibilityTo = (long?)OracleUdt.GetValue(con, pUdt, "RESPONSIBILITY_TO");
            ManagerId = (long)OracleUdt.GetValue(con, pUdt, "MANAGER_ID");
            DelegatingId = (long)OracleUdt.GetValue(con, pUdt, "DELEGATING_ID");
            HaMacId = (long)OracleUdt.GetValue(con, pUdt, "HA_MAC_ID");
            LocalAuthorityId = (long)OracleUdt.GetValue(con, pUdt, "LOCAL_AUTHORITY_ID");
            PoliceForceId = (long)OracleUdt.GetValue(con, pUdt, "POLICE_FORCE_ID");
            ArrangementId = (long?)OracleUdt.GetValue(con, pUdt, "ARRANGEMENT_ID");
        }
    }

    [OracleCustomTypeMappingAttribute("PORTAL.DELEGATION")]
    public class DelegationFactory : IOracleCustomTypeFactory
    {
        // Implementation of IOracleCustomTypeFactory.CreateObject()
        public IOracleCustomType CreateObject()
        {
            // Return a new custom object
            return new Delegation();
        }
    }

    public class DelegationArray : INullable, IOracleCustomType
    {
        private bool m_IsNull;
        [OracleArrayMapping()]
        public Delegation[] DelegationArr { get; set; }
        public virtual bool IsNull
        {
            get
            {
                return m_IsNull;
            }
        }
        public static DelegationArray Null
        {
            get
            {
                DelegationArray delegArr = new DelegationArray();
                delegArr.m_IsNull = true;
                return delegArr;
            }
        }

        public void FromCustomObject(OracleConnection con, IntPtr pUdt)
        {
            OracleUdt.SetValue(con, pUdt, 0, DelegationArr);
        }

        public void ToCustomObject(OracleConnection con, IntPtr pUdt)
        {
            DelegationArr = (Delegation[])OracleUdt.GetValue(con, pUdt, 0);
        }
    }

    [OracleCustomTypeMapping("PORTAL.DELEGATIONARRAY")]
    public class DelegationArrayFactory : IOracleCustomTypeFactory, IOracleArrayTypeFactory
    {
        #region IOracleCustomTypeFactory Members
        public IOracleCustomType CreateObject()
        {
            return new DelegationArray();
        }
        #endregion

        #region IOracleArrayTypeFactory Members
        public Array CreateArray(int numElems)
        {
            return new Delegation[numElems];
        }

        public Array CreateStatusArray(int numElems)
        {
            return new Array[0];
        }
        #endregion
    }
}
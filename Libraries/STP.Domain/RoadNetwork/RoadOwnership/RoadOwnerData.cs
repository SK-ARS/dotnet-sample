using System;
using System.Collections.Generic;
using NetSdoGeometry;
using System.Runtime.Serialization;
using Oracle.DataAccess.Types;
using Oracle.DataAccess.Client;
using Newtonsoft.Json;

namespace STP.Domain.RoadNetwork.RoadOwnership
{
    public class LinkInfo
    {
        [DataMember]
        public long LinkId { get; set; }
        [DataMember]
        public string LinkStatus { get; set; }
        [DataMember]
        public long? ResponsibilityFrom { get; set; }
        [DataMember]
        public long? ResponsibilityTo { get; set; }
        [DataMember]
        public sdogeometry LinkGeometry { get; set; }
        [DataMember]
        public bool SubDelegationAllowed { get; set; }
        [DataMember]
        public long? ArrangementId { get; set; }
        [DataMember]
        public char LinkManageStatus { get; set; }
    }
    public class LinkInfoJson
    {
        [DataMember]
        [JsonProperty("LinkId")]
        public long linkId { get; set; }
        [DataMember]
        [JsonProperty("LinkStatus")]
        public string linkStatus { get; set; }
        [DataMember]
        [JsonProperty("ResponsibilityFrom")]
        public long? fromLinearRef { get; set; }
        [DataMember]
        [JsonProperty("ResponsibilityTo")]
        public long? toLinearRef { get; set; }
        [DataMember]
        [JsonProperty("LinkGeometry")]
        public sdogeometry linkGeom { get; set; }
        [DataMember]
        [JsonProperty("SubDelegationAllowed")]
        public bool subDelegationAllowed { get; set; }
        [DataMember]
        [JsonProperty("ArrangementId")]
        public long? arrangementId { get; set; }
        [DataMember]
        [JsonProperty("LinkManageStatus")]
        public char linkManageStatus { get; set; }
    }
    public class OwnerInfo
    {
        public string OwnerName { get; set; }
        public long OwnerId { get; set; }
        public int Type { get; set; }
    }
    public class RoadOwnershipData
    {
        public long LinkId { get; set; }
        public long ResponsibilityNo { get; set; }
        public long ResponsibilityFrom { get; set; }
        public long ResponsibilityTo { get; set; }
        public String ManagerName { get; set; }
        public long ManagerId { get; set; }
        public String LocalAuthorityName { get; set; }
        public long LaId { get; set; }
        public String HaMacName { get; set; }
        public long HaMacId { get; set; }
        public String PoliceName { get; set; }
        public long PoliceId { get; set; }
        public bool RetainDelegation { get; set; }
        public bool SelectAllRds { get; set; }
        public List<long> LinkIdList { get; set; }
        public List<LinkInfo> LinkInfoList { get; set; }
        public int TotalRows { get; set; }
        public bool ChangeRecordFlag { get; set; }
        public List<OwnerInfo> OwnerInfoList { get; set; }
        public String HdnManagerName { get; set; }
        public String HdnHaMacName { get; set; }
        public String HdnPoliceName { get; set; }
        public String HdnLocalAuthorityName { get; set; }
        public String HdnRetainDelegation { get; set; }
        public string ArrangementName { get; set; }
        public string DelegatorOrgName { get; set; }
        public string DelegatingOrgName { get; set; }
        public RoadContactModal RoadContact { get; set; }

        public RoadOwnershipData()
        {
            RoadContact = new RoadContactModal();
            LinkInfoList = new List<LinkInfo>();
        }
    }

    public class RoadOwnershipOrgSummary
    {
        public long OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public sdogeometry OrganisationGeoRegion { get; set; }
        public int TotalRows { get; set; }
        public int FetchFlag { get; set; }
    }
    public class ArrangementDetails
    {
        public long ToOrgId { get; set; }
        public string ArrangementName { get; set; }
    }

    public class RoadOwnerShipDetails
    {
        public List<OwnerInfo> NewOwnerList { get; set; }
        public List<LinkInfo> LinkInfoList { get; set; }
        public List<LinkInfo> AssignedLinkInfoList { get; set; }
        public List<LinkInfo> UnassignedLinkInfoList { get; set; }
        public List<ArrangementDetails> NewManagerDelegationDetailsList { get; set; }
    }
    #region RoadContact
    [DataContract]
    public class RoadContactModal
    {
        [DataMember]
        public string ContactName { get; set; }
        [DataMember]
        public int Position { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public long ContactId { get; set; }
        [DataMember]
        public short ContactNo { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string OrganisationName { get; set; }
        [DataMember]
        public string OrganisationType { get; set; }
        [DataMember]
        public string AddressLine1 { get; set; }
        [DataMember]
        public string AddressLine2 { get; set; }
        [DataMember]
        public string AddressLine3 { get; set; }
        [DataMember]
        public string AddressLine4 { get; set; }
        [DataMember]
        public string AddressLine5 { get; set; }
        [DataMember]
        public string PostCode { get; set; }
        [DataMember]
        public int CountryId { get; set; }
        [DataMember]
        public string CountryName { get; set; }
        [DataMember]
        public string Telephone { get; set; }
        [DataMember]
        public string Extension { get; set; }
        [DataMember]
        public string Mobile { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public string EMail { get; set; }
        [DataMember]
        public string EmailPreference { get; set; }
        [DataMember]
        public long CautionId { get; set; }
        [DataMember]
        public decimal TotalRecordCount { get; set; }
        [DataMember]
        public object OrganisationId { get; set; }
        [DataMember]
        public object RoleType { get; set; }
        [DataMember]
        public object IsAdHoc { get; set; }
        [DataMember]
        public object UserSchema { get; set; }
    }
    #endregion

    #region DataLinkContactsData
    [DataContract]
    public class DataLinkContactsData
    {
        [DataMember]
        public long LinkId { get; set; }

        [DataMember]
        public long? ResponsibilityFrom { get; set; }

        [DataMember]
        public long? ResponsibilityTo { get; set; }

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
        public long? ArrangementId { get; set; }
    }
    #endregion

    public class DataLink : INullable, IOracleCustomType
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
        public static DataLink Null
        {
            get
            {
                DataLink dataLinkObj = new DataLink();
                dataLinkObj.m_IsNull = true;
                return dataLinkObj;
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

    [OracleCustomTypeMappingAttribute("PORTAL.DATALINK")]
    public class DataLinksFactory : IOracleCustomTypeFactory
    {
        // Implementation of IOracleCustomTypeFactory.CreateObject()
        public IOracleCustomType CreateObject()
        {
            // Return a new custom object
            return new DataLink();
        }
    }

    public class DataLinkArray : INullable, IOracleCustomType
    {
        private bool m_IsNull;
        [OracleArrayMapping()]
        public DataLink[] DataLinkArr { get; set; }
        public virtual bool IsNull
        {
            get
            {
                return m_IsNull;
            }
        }
        public static DataLinkArray Null
        {
            get
            {
                DataLinkArray dataLinkArr = new DataLinkArray();
                dataLinkArr.m_IsNull = true;
                return dataLinkArr;
            }
        }


        public void FromCustomObject(OracleConnection con, IntPtr pUdt)
        {
            OracleUdt.SetValue(con, pUdt, 0, DataLinkArr);
        }

        public void ToCustomObject(OracleConnection con, IntPtr pUdt)
        {
            DataLinkArr = (DataLink[])OracleUdt.GetValue(con, pUdt, 0);
        }
    }

    [OracleCustomTypeMapping("PORTAL.DATALINKARRAY")]
    public class DataLinkArrayFactory : IOracleCustomTypeFactory, IOracleArrayTypeFactory
    {
        #region IOracleCustomTypeFactory Members
        public IOracleCustomType CreateObject()
        {
            return new DataLinkArray();
        }

        #endregion

        #region IOracleArrayTypeFactory Members
        public Array CreateArray(int numElems)
        {
            return new DataLink[numElems];
        }

        public Array CreateStatusArray(int numElems)
        {
            return null;
        }
        #endregion
    }
}
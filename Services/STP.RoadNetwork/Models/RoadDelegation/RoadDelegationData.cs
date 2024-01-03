using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using NetSdoGeometry;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace STP.RoadNetwork.Models.RoadDelegation
{
    #region RoadContact
    [DataContract]
    public class RoadContact
    {
        [DataMember]
        public long managerOrgId { get; set; }
        [DataMember]
        public long managerCntctId { get; set; }
        [DataMember]
        public long delegatingOrgId { get; set; }
        [DataMember]
        public long delegatingCntctId { get; set; }

        [DataMember]
        public string managerOrgName { get; set; }
        [DataMember]
        public string managerContactName { get; set; }
        [DataMember]
        public string delegatingOrgName { get; set; }
        [DataMember]
        public string delegatingContactName { get; set; }
    }
    #endregion
    public class RoadDelegationOrgSummary
    {
        public long organisationId { get; set; }
        public string organisationName { get; set; }
        public sdogeometry orgGeoRegion { get; set; }
        public int totalRows { get; set; }
        public string contactName { get; set; }
    }
    public class RoadDelegationContactSummary
    {
        public long contactId { get; set; }
        public string contactRole { get; set; }
        public string fullName { get; set; }
        public int totalRows { get; set; }
        public RoadDelegationOrgSummary roadDelegOrgDetails { get; set; }
        public RoadDelegationContactSummary()
        {
            roadDelegOrgDetails = new RoadDelegationOrgSummary();
        }
    }
    
    #region RoadLink
    [DataContract]
    public class RoadLink
    {
        [DataMember]
        public long arrangementId { get; set; }

        [DataMember]
        public int acceptFailure { get; set; }
        [DataMember]
        public int retainNotification { get; set; }
        [DataMember]
        public int groupId { get; set; }
        [DataMember]
        public int countryId { get; set; }
        [DataMember]
        public string roadName { get; set; }

        [DataMember]
        public sdogeometry roadGeometry { get; set; }

        [DataMember]
        public List<RoadContact> roadContactInfo { get; set; }

        public LinkInfo roadLinkInfo { get; set; }

        public RoadLink()
        {
            roadContactInfo = new List<RoadContact>();
            roadLinkInfo = new LinkInfo();
        }
    }
    #endregion

    #region LinkInfo
    [DataContract]
    public class LinkInfo
    {
        [DataMember]
        public long linkId { get; set; }
        [DataMember]
        public Int32? fromLinearRef { get; set; }
        [DataMember]
        public Int32? toLinearRef { get; set; }
        [DataMember]
        public sdogeometry linkGeom { get; set; }
        [DataMember]
        public bool subDelegationAllowed { get; set; }
        [DataMember]
        public long? arrangementId { get; set; }
        [DataMember]
        public char linkManageStatus { get; set; }
    }
    #endregion

    #region RoadDelegationData
    [DataContract]
    public class RoadDelegationData
    {
        [DataMember]
        public long arrangementId { get; set; }
        [DataMember]
        public long fromOrgId { get; set; }
        [DataMember]
        public long toOrgId { get; set; }
        [DataMember]
        public long fromContactId { get; set; }
        [DataMember]
        public long toContactId { get; set; }
        [DataMember]
        public long roadGrp { get; set; }

        [DataMember]
        public string arrangementName { get; set; }
        [DataMember]
        public string fromOrgName { get; set; }
        [DataMember]
        public string toOrgName { get; set; }
        [DataMember]
        public string fromContactName { get; set; }
        [DataMember]
        public string toContactName { get; set; }
        [DataMember]
        public string comments { get; set; }
        [DataMember]
        public int retainNotification { get; set; }
        [DataMember]
        public int acceptFailure { get; set; }
        [DataMember]
        public int allowSubdelegation { get; set; }
        [DataMember]
        public int selectedRadio { get; set; }
        [DataMember]
        public int delegateAll { get; set; }
        [DataMember]
        public long totalRecordCount { get; set; }
        [DataMember]
        public List<long> linkIdList { get; set; }
        [DataMember]
        public List<LinkInfo> linkIdInfo { get; set; }        
        [DataMember]
        public string fromContactType { get; set; }
        [DataMember]
        public decimal TOTAL_RECORD_COUNT { get; set; }
        public RoadDelegationData()
        {
            linkIdInfo = new List<LinkInfo>();
        }
    }
    #endregion

    #region RoadDelegationSearchParam
    [DataContract]
    public class RoadDelegationSearchParam
    {
        [DataMember]
        public string arrngmntName { get; set; }
        [DataMember]
        public string fromOrgName { get; set; }
        [DataMember]
        public string toOrgName { get; set; }
        [DataMember]
        public string roadName { get; set; }

        [DataMember]
        public string searchText { get; set; }
        [DataMember]
        public long? arrngmntId { get; set; }
        /// <summary>
        /// from org id is the Manager organisation id when links are fetched for displaying on map.
        /// from org id is the delegating organisation id for fetching links based on delegation arrangements
        /// </summary>
        [DataMember]
        public long? fromOrgId { get; set; }
        /// <summary>
        /// to org id is the Delegating organisation id when links are fetched for displaying on map.
        /// to org id is the manager organisation id for fetching links based on delegation arrangements
        /// </summary>
        [DataMember]
        public long? toOrgId { get; set; }
    }
    #endregion

    #region DataLinkContactsData
    [DataContract]
    public class DataLinkContactsData
    {
        [DataMember]
        public long linkId { get; set; }

        [DataMember]
        public int? responsibilityFrom { get; set; }

        [DataMember]
        public int? responsibilityTo { get; set; }

        [DataMember]
        public long manager_id { get; set; }

        [DataMember]
        public long delegating_id { get; set; }

        [DataMember]
        public long ha_mac_id { get; set; }

        [DataMember]
        public long local_authority_id { get; set; }

        [DataMember]
        public long police_force_id { get; set; }

        [DataMember]
        public long arrangement_id { get; set; }
    }
    #endregion

    public class DelegationArrangementDetails
    {
        public long orgToId { get; set; }
        public string arrangementName { get; set; }
        public int delegateAll { get; set; }
    }
    public class Delegation : INullable, IOracleCustomType
    {
        private bool m_bIsNull;

        [OracleObjectMappingAttribute("LINK_ID")]
        public long LINK_ID { get; set; }

        [OracleObjectMappingAttribute("RESPONSIBILITY_FROM")]
        public long? RESPONSIBILITY_FROM { get; set; }

        [OracleObjectMappingAttribute("RESPONSIBILITY_TO")]
        public long? RESPONSIBILITY_TO { get; set; }

        [OracleObjectMappingAttribute("MANAGER_ID")]
        public long MANAGER_ID { get; set; }

        [OracleObjectMappingAttribute("DELEGATING_ID")]
        public long DELEGATING_ID { get; set; }

        [OracleObjectMappingAttribute("HA_MAC_ID")]
        public long HA_MAC_ID { get; set; }

        [OracleObjectMappingAttribute("LOCAL_AUTHORITY_ID")]
        public long LOCAL_AUTHORITY_ID { get; set; }

        [OracleObjectMappingAttribute("POLICE_FORCE_ID")]
        public long POLICE_FORCE_ID { get; set; }

        [OracleObjectMappingAttribute("ARRANGEMENT_ID")]
        public long? ARRANGEMENT_ID { get; set; }

        public virtual bool IsNull
        {
            get
            {
                return m_bIsNull;
            }
        }

        // SUProject.Null is used to return a NULL SUProject object
        public static Delegation Null
        {
            get
            {
                Delegation p = new Delegation();
                p.m_bIsNull = true;
                return p;
            }
        }
        public void FromCustomObject(OracleConnection con, IntPtr pUdt)
        {
            OracleUdt.SetValue(con, pUdt, "LINK_ID", LINK_ID);
            OracleUdt.SetValue(con, pUdt, "RESPONSIBILITY_FROM", RESPONSIBILITY_FROM);
            OracleUdt.SetValue(con, pUdt, "RESPONSIBILITY_TO", RESPONSIBILITY_TO);
            OracleUdt.SetValue(con, pUdt, "MANAGER_ID", MANAGER_ID);
            OracleUdt.SetValue(con, pUdt, "DELEGATING_ID", DELEGATING_ID);
            OracleUdt.SetValue(con, pUdt, "HA_MAC_ID", HA_MAC_ID);
            OracleUdt.SetValue(con, pUdt, "LOCAL_AUTHORITY_ID", LOCAL_AUTHORITY_ID);
            OracleUdt.SetValue(con, pUdt, "POLICE_FORCE_ID", POLICE_FORCE_ID);
            OracleUdt.SetValue(con, pUdt, "ARRANGEMENT_ID", ARRANGEMENT_ID);
        }

        public void ToCustomObject(OracleConnection con, IntPtr pUdt)
        {
            LINK_ID = (long)OracleUdt.GetValue(con, pUdt, "LINK_ID");
            RESPONSIBILITY_FROM = (long?)OracleUdt.GetValue(con, pUdt, "RESPONSIBILITY_FROM");
            RESPONSIBILITY_TO = (long?)OracleUdt.GetValue(con, pUdt, "RESPONSIBILITY_TO");
            MANAGER_ID = (long)OracleUdt.GetValue(con, pUdt, "MANAGER_ID");
            DELEGATING_ID = (long)OracleUdt.GetValue(con, pUdt, "DELEGATING_ID");
            HA_MAC_ID = (long)OracleUdt.GetValue(con, pUdt, "HA_MAC_ID");
            LOCAL_AUTHORITY_ID = (long)OracleUdt.GetValue(con, pUdt, "LOCAL_AUTHORITY_ID");
            POLICE_FORCE_ID = (long)OracleUdt.GetValue(con, pUdt, "POLICE_FORCE_ID");
            ARRANGEMENT_ID = (long?)OracleUdt.GetValue(con, pUdt, "ARRANGEMENT_ID");
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
        [OracleArrayMapping()]
        public Delegation[] DELEGATIONObj { get; set; }

        private bool m_bIsNull;
        public virtual bool IsNull
        {
            get
            {
                return m_bIsNull;
            }
        }
        public static DelegationArray Null
        {
            get
            {
                DelegationArray p = new DelegationArray();
                p.m_bIsNull = true;
                return p;
            }
        }

        public void FromCustomObject(OracleConnection con, IntPtr pUdt)
        {
            OracleUdt.SetValue(con, pUdt, 0, DELEGATIONObj);
        }

        public void ToCustomObject(OracleConnection con, IntPtr pUdt)
        {
            DELEGATIONObj = (Delegation[])OracleUdt.GetValue(con, pUdt, 0);
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
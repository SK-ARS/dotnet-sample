using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSdoGeometry;
using System.Runtime.Serialization;
using Oracle.DataAccess.Types;
using Oracle.DataAccess.Client;

namespace STP.RoadNetwork.Models.RoadOwnership
{
    public class LinkInfo
    {
        [DataMember]
        public long linkId { get; set; }
        [DataMember]
        public string linkStatus { get; set; }
        [DataMember]
        public long? fromLinearRef { get; set; }
        [DataMember]
        public long? toLinearRef { get; set; }
        [DataMember]
        public sdogeometry linkGeom { get; set; }
        [DataMember]
        public bool subDelegationAllowed { get; set; }
        [DataMember]
        public long? arrangementId { get; set; }
        [DataMember]
        public char linkManageStatus { get; set; }
    }
    public class OwnerInfo
    {
        public string ownerName { get; set; }
        public long ownerId { get; set; }
        public int type { get; set; }
    }
    public class RoadOwnershipData
    {
        public long link_Id { get; set; }
        public long responsibility_No { get; set; }
        public long responsibility_From { get; set; }
        public long esponsibility_To { get; set; }
        public String managerName { get; set; }
        public long manager_Id { get; set; }
        public String local_Authorityname { get; set; }
        public long la_Id { get; set; }
        public String ha_Mac_Name { get; set; }
        public long ha_Mac_Id { get; set; }
        public String police_Name { get; set; }
        public long police_Id { get; set; }
        public bool retain_Delegation { get; set; }
        public bool selectAllRds { get; set; }
        public List<long> linkIdList { get; set; }
        public List<LinkInfo> linkIdInfo { get; set; }
        public int totalRows { get; set; }
        public bool changeRecordFlag { get; set; }
        public List<OwnerInfo> ownerInfoList { get; set; }
        public String hdnManagerName { get; set; }
        public String hdnHa_mac_Name { get; set; }
        public String hdnPolice_Name { get; set; }
        public String hdnLocal_authority_Name { get; set; }
        public String hdnretain_Delegation { get; set; }
        public string arrangementName { get; set; }
        public string delegatorOrgName { get; set; }
        public string delegatingOrgName { get; set; }
        public RoadContactModal roadContact { get; set; }

        public RoadOwnershipData()
        {
            roadContact = new RoadContactModal();
            linkIdInfo = new List<LinkInfo>();
        }
    }

    public class RoadOwnershipOrgSummary
    {
        public long organisationId { get; set; }
        public string organisationName { get; set; }
        public sdogeometry orgGeoRegion { get; set; }
        public int totalRows { get; set; }
        public int fetchFlag { get; set; }
    }
    public class ArrangementDetails
    {
        public long orgToId { get; set; }
        public string arrangementName { get; set; }
    }

    public class RoadOwnerShipDetails
    {
        public List<OwnerInfo> newOwnerList { get; set; }
        public List<LinkInfo> linkInfo { get; set; }
        public List<LinkInfo> assignedLinkInfo { get; set; }
        public List<LinkInfo> unassignedLinkInfo { get; set; }
        public List<ArrangementDetails> newManagerDelegationDetails { get; set; }
    }
    #region RoadContact
    [DataContract]
    public class RoadContactModal
    {
        [DataMember]
        public string CONTACT_NAME { get; set; }
        [DataMember]
        public int POSITION { get; set; }
        [DataMember]
        public string DESCRIPTION { get; set; }
        [DataMember]
        public long CONTACT_ID { get; set; }
        [DataMember]
        public short CONTACT_NO { get; set; }
        [DataMember]
        public string FIRST_NAME { get; set; }
        [DataMember]
        public string FULL_NAME { get; set; }
        [DataMember]
        public string ORGANISATION_NAME { get; set; }
        [DataMember]
        public string ORGANISATION_TYPE { get; set; }
        [DataMember]
        public string ADDRESS_LINE_1 { get; set; }
        [DataMember]
        public string ADDRESS_LINE_2 { get; set; }
        [DataMember]
        public string ADDRESS_LINE_3 { get; set; }
        [DataMember]
        public string ADDRESS_LINE_4 { get; set; }
        [DataMember]
        public string ADDRESS_LINE_5 { get; set; }
        [DataMember]
        public string POST_CODE { get; set; }
        [DataMember]
        public int CountryID { get; set; }
        [DataMember]
        public string CountryName { get; set; }
        [DataMember]
        public string TELEPHONE { get; set; }
        [DataMember]
        public string EXTENSION { get; set; }
        [DataMember]
        public string MOBILE { get; set; }
        [DataMember]
        public string FAX { get; set; }
        [DataMember]
        public string EMAIL { get; set; }
        [DataMember]
        public string EMAIL_PREFERENCE { get; set; }
        [DataMember]
        public long CAUTION_ID { get; set; }
        [DataMember]
        public decimal TotalRecordCount { get; set; }
        [DataMember]
        public object ORGANISATION_ID { get; set; }
        [DataMember]
        public object ROLE_TYPE { get; set; }
        [DataMember]
        public object IS_AD_HOC { get; set; }
        [DataMember]
        public object USER_SCHEMA { get; set; }
    }
    #endregion

    #region DataLinkContactsData
    [DataContract]
    public class DataLinkContactsData
    {
        [DataMember]
        public long linkId { get; set; }

        [DataMember]
        public long? responsibilityFrom { get; set; }

        [DataMember]
        public long? responsibilityTo { get; set; }

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
        public long? arrangement_id { get; set; }
    }
    #endregion

    public class DataLink : INullable, IOracleCustomType
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
        public static DataLink Null
        {
            get
            {
                DataLink p = new DataLink();
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
        [OracleArrayMapping()]
        public DataLink[] dataLinkObj { get; set; }

        private bool m_bIsNull;
        public virtual bool IsNull
        {
            get
            {
                return m_bIsNull;
            }
        }
        public static DataLinkArray Null
        {
            get
            {
                DataLinkArray p = new DataLinkArray();
                p.m_bIsNull = true;
                return p;
            }
        }


        public void FromCustomObject(OracleConnection con, IntPtr pUdt)
        {
            OracleUdt.SetValue(con, pUdt, 0, dataLinkObj);
        }

        public void ToCustomObject(OracleConnection con, IntPtr pUdt)
        {
            dataLinkObj = (DataLink[])OracleUdt.GetValue(con, pUdt, 0);
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
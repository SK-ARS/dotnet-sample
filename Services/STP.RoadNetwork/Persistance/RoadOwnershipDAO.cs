using System;
using System.Collections.Generic;
using Oracle.DataAccess.Client;
using STP.Domain.RoadNetwork.RoadOwnership;
using STP.DataAccess.SafeProcedure;
using STP.Common.Enums;
using STP.Common.Constants;
using NetSdoGeometry;
using STP.Common.Logger;

namespace STP.RoadNetwork.Persistance
{
    public static class RoadOwnershipDAO
    {
        internal static List<RoadOwnershipOrgSummary> GetRoadOwnershipOrganisations(string orgName, int pageNum, int pageSize, int searchFlag)
        {
            List<RoadOwnershipOrgSummary> rdOwnerOrgList = new List<RoadOwnershipOrgSummary>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                rdOwnerOrgList,
                UserSchema.Portal + ".STP_ROAD_OWNERSHIP.SP_RDOWNERSHIP_FETCH_SOA_ORG",
                parameter =>
                {

                    parameter.AddWithValue("P_ORG_NAME_SEARCH", orgName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SEARCH_FLAG", searchFlag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_PAGENUMBER", pageNum, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_PAGESIZE", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.OrganisationId = records.GetLongOrDefault("ORGANISATION_ID");
                    instance.OrganisationName = records.GetStringOrDefault("ORGNAME");
                    instance.TotalRows = Convert.ToInt32(records.GetDecimalOrDefault("TOTAL_ROWS"));
                }
                );
            return rdOwnerOrgList;
        }
        internal static List<long> GetUnassignedLinks(List<long> linkIdList)
        {                
            OracleParameter param = new OracleParameter();
            List<long> newLinkId = new List<long>();
            List<long> tmpLinkId = new List<long>();
            long link = 0;

            #region Portion to make an oracle parameter from link id list
            if (linkIdList != null)
            {
                param.OracleDbType = OracleDbType.Int32;
                param.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
                param.Value = linkIdList.ToArray(); // change when the testing is completed
                param.Size = linkIdList.ToArray().Length;
            }
            else
            {
                param.OracleDbType = OracleDbType.Int32;
                param.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
                param.Value = null;
                param.Size = 0;
            }
            #endregion

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                tmpLinkId,
               UserSchema.Portal + ".STP_ROAD_OWNERSHIP.SP_GET_UNASSIGNED_LINKS",
                parameter =>
                {
                    parameter.Add(param);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    link = records.GetLongOrDefault("LINK_ID");
                    newLinkId.Add(link);
                }
            );

            return newLinkId;           
        }
        internal static List<ArrangementDetails> GetDelegationArrangementDetails(int orgId)
        {
            List<ArrangementDetails> rddelegList = new List<ArrangementDetails>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                rddelegList,
                 UserSchema.Portal + ".STP_ROAD_OWNERSHIP.SP_GET_DELEGATION_ARRANGMENT",
                parameter =>
                {
                    parameter.AddWithValue("R_ORG_ID", orgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.ToOrgId = records.GetLongOrDefault("ORG_TO_ID");
                    instance.ArrangementName = records.GetStringOrDefault("NAME");
                }
            );
            return rddelegList;
        }
        public static List<RoadContactModal> GetRoadOwnerContactList(long linkID, long length, string pageType, string userSchema = UserSchema.Portal)
        {
            List<RoadContactModal> roadContactList = new List<RoadContactModal>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                roadContactList,
                userSchema + ".SP_GET_ROAD_OWN_CONTACTS",
                parameter =>
                {
                    parameter.AddWithValue("P_LINK_ID", linkID, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_LENGTH", length, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.ContactName = records.GetStringOrDefault("FULL_NAME");
                    instance.OrganisationName = records.GetStringOrDefault("NAME");
                    instance.OrganisationType = records.GetStringOrDefault("ORGTYPE");
                    instance.AddressLine1 = records.GetStringOrDefault("ADDRESS_1");
                    instance.AddressLine2 = records.GetStringOrDefault("ADDRESS_2");
                    instance.AddressLine3 = records.GetStringOrDefault("ADDRESS_3");
                    instance.AddressLine4 = records.GetStringOrDefault("ADDRESS_4");
                    instance.PostCode = records.GetStringOrDefault("POST_CODE");
                    instance.Telephone = records.GetStringOrDefault("TELEPHONE");
                    instance.Fax = records.GetStringOrDefault("FAX");
                    instance.EMail = records.GetStringOrDefault("EMAIL");

                }
            );
            return roadContactList;
        }
        internal static List<RoadOwnershipData> GetRoadOwnershipDetails(List<long> linkIds)
        {
            List<RoadOwnershipData> rdOwnerShipArray = new List<RoadOwnershipData>();
            foreach (long linkID in linkIds)
            {
                rdOwnerShipArray.Add(GetLinkDetails(linkID));
            }
            return rdOwnerShipArray;            
        }
        internal static RoadOwnershipData GetLinkDetails(long linkId)
        {
            try
            {
                RoadOwnershipData rdOwnerObjList = new RoadOwnershipData();
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    rdOwnerObjList,
                     UserSchema.Portal + ".STP_ROAD_OWNERSHIP.SP_SELECT_LINK_DETAILS",
                    parameter =>
                    {
                        parameter.AddWithValue("P_LINK_ID", linkId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    (records, instance) =>
                    {
                        instance.LinkId = records.GetLongOrDefault("LINK_ID");
                        instance.DelegatorOrgName = records.GetStringOrDefault("MANAGER_NAME");
                        instance.DelegatingOrgName = records.GetStringOrDefault("DELEGATING_NAME");
                        instance.HdnManagerName = records.GetStringOrDefault("MANAGER_NAME");
                        instance.HdnLocalAuthorityName = records.GetStringOrDefault("LA_NAME");
                        instance.HdnHaMacName = records.GetStringOrDefault("HA_MAC_NAME");
                        instance.HdnPoliceName = records.GetStringOrDefault("POLICE_FORCE_NAME");
                        string delegName = records.GetStringOrDefault("ARRANGEMENT_NAME");
                        if (delegName != null && delegName != "")
                        {
                            instance.ArrangementName = records.GetStringOrDefault("ARRANGEMENT_NAME");
                            instance.RoadContact.ContactId = (long)Convert.ToInt32(records.GetDecimalOrDefault("CONTACT_ID"));
                            instance.RoadContact.FirstName = records.GetStringOrDefault("FIRST_NAME");
                            instance.RoadContact.FullName = records.GetStringOrDefault("SUR_NAME");
                            instance.RoadContact.AddressLine1 = records.GetStringOrDefault("ADDRESSLINE_1");
                            instance.RoadContact.AddressLine2 = records.GetStringOrDefault("ADDRESSLINE_2");
                            instance.RoadContact.PostCode = records.GetStringOrDefault("POSTCODE");
                            instance.RoadContact.EMail = records.GetStringOrDefault("EMAIL");
                            instance.RoadContact.Fax = records.GetStringOrDefault("FAX");
                            instance.RoadContact.Telephone = records.GetStringOrDefault("MOBILE");
                        }
                    }
                );
                return rdOwnerObjList;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RoadOwnership/GetLinkDetails, Exception: " + ex​​​​);
                throw;
            }
        }
        internal static bool SaveRoadOwnership(RoadOwnerShipDetails roadOwnershipDetails)
        {
            long newOrgId = roadOwnershipDetails.NewOwnerList[0].OwnerId;
            int newOrgType = roadOwnershipDetails.NewOwnerList[0].Type;

            List<DataLink> dataLinkList = new List<DataLink>();
            DataLink dataLink = null;
            long result = 0;

            foreach (LinkInfo linkInfoObj in roadOwnershipDetails.AssignedLinkInfoList)
            {
                List<DataLinkContactsData> dcData = new List<DataLinkContactsData>();
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                dcData,
                 UserSchema.Portal + ".STP_ROAD_OWNERSHIP.GET_PARTIAL_OWN_RECORDS",
                parameter =>
                {
                    parameter.AddWithValue("P_LINK_ID", linkInfoObj.LinkId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.LinkId = records.GetLongOrDefault("LINK_ID");
                        instance.ResponsibilityFrom = records.GetInt32OrDefault("RESPONSIBILITY_FROM");
                        instance.ResponsibilityTo = records.GetInt32OrDefault("RESPONSIBILITY_TO");
                        instance.ManagerId = records.GetLongOrDefault("MANAGER_ID");
                        instance.DelegatingId = records.GetLongOrDefault("DELEGATING_ID");
                        instance.HaMacId = records.GetLongOrDefault("HA_MAC_ID");
                        instance.LocalAuthorityId = records.GetLongOrDefault("LOCAL_AUTHORITY_ID");
                        instance.PoliceForceId = records.GetLongOrDefault("POLICE_FORCE_ID");
                        instance.ArrangementId = records.GetLongOrDefault("ARRANGEMENT_ID");
                    }
                );

                List<DataLinkContactsData> processedData = ProcessLinkOwnershipRecordsAuto(dcData, linkInfoObj, newOrgId, newOrgType);
                foreach (DataLinkContactsData obj in processedData)
                {
                    dataLink = new DataLink();
                    dataLink.LinkId = obj.LinkId;
                    dataLink.ResponsibilityFrom = obj.ResponsibilityFrom;
                    dataLink.ResponsibilityTo = obj.ResponsibilityTo;
                    dataLink.ManagerId = obj.ManagerId;
                    dataLink.DelegatingId = obj.DelegatingId;
                    dataLink.HaMacId = obj.HaMacId;
                    dataLink.LocalAuthorityId = obj.LocalAuthorityId;
                    dataLink.PoliceForceId = obj.PoliceForceId;
                    dataLink.ArrangementId = obj.ArrangementId;
                    dataLinkList.Add(dataLink);
                }
            }

            DataLinkArray dataLinkDetail = new DataLinkArray();
            dataLinkDetail.DataLinkArr = dataLinkList.ToArray();

            OracleCommand cmd = new OracleCommand();
            OracleParameter dataLinks = cmd.CreateParameter();
            dataLinks.OracleDbType = OracleDbType.Object;
            dataLinks.UdtTypeName = "PORTAL.DATALINKARRAY";
            dataLinks.Value = dataLinkList.Count > 0 ? dataLinkDetail : null;
            dataLinks.ParameterName = "P_DATA_LINK_ARRAY";
            
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
            result,
             UserSchema.Portal + ".STP_ROAD_OWNERSHIP.SP_RD_SAVE_ROAD_OWN_PARTIAL",
            parameter =>
            {
                parameter.AddWithValue("P_RETAIN_NOTIF_FLAG", null, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_ACCPT_FAIL_FLAG", null, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.Add(dataLinks);
                parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
                (records, instance) =>
                {
                    result = Convert.ToInt32(records["FLAG"]);
                }
            );
            return result != 0;
        }
        internal static List<DataLinkContactsData> ProcessLinkOwnershipRecordsAuto(List<DataLinkContactsData> resultList, LinkInfo linkInfoObj, long newOrgId, int newOrgType)
        {
            try
            {
                List<DataLinkContactsData> processedData = new List<DataLinkContactsData>();
                long? from = linkInfoObj.ResponsibilityFrom;
                long? to = linkInfoObj.ResponsibilityTo;
                bool fromSetFlag = false;

                if (resultList.Count > 0)
                {
                    resultList = SortLinkOwnershipRecords(resultList);
                }

                if (from == null)
                    from = 0;
                if (to == null || to == 0)
                    to = 999999; //a large value

                for (int i = 0; i < resultList.Count; i++)
                {
                    DataLinkContactsData currentResult = resultList[i];
                    long? currentRespFrom = currentResult.ResponsibilityFrom;
                    if (currentRespFrom == null)
                        currentRespFrom = 0;

                    long? currentRespTo = currentResult.ResponsibilityTo;
                    if (currentRespTo == null || currentRespTo == 0)
                        currentRespTo = 999999; //a large value

                    if (!fromSetFlag)
                    {
                        if (from >= currentRespFrom && from <= currentRespTo)
                        {
                            if (from != 0)
                                processedData.Add(new DataLinkContactsData { LinkId = currentResult.LinkId, ResponsibilityFrom = currentRespFrom, ResponsibilityTo = from, ManagerId = currentResult.ManagerId, DelegatingId = currentResult.DelegatingId, HaMacId = currentResult.HaMacId, LocalAuthorityId = currentResult.LocalAuthorityId, PoliceForceId = currentResult.PoliceForceId, ArrangementId = currentResult.ArrangementId });
                            fromSetFlag = true;
                        }
                        else
                        {
                            processedData.Add(new DataLinkContactsData { LinkId = currentResult.LinkId, ResponsibilityFrom = currentRespFrom, ResponsibilityTo = currentRespTo, ManagerId = currentResult.ManagerId, DelegatingId = currentResult.DelegatingId, HaMacId = currentResult.HaMacId, LocalAuthorityId = currentResult.LocalAuthorityId, PoliceForceId = currentResult.PoliceForceId, ArrangementId = currentResult.ArrangementId });
                            continue;
                        }
                    }

                    if (to >= currentRespFrom && to <= currentRespTo)
                    {
                        long newManagerId = currentResult.ManagerId;
                        long newDelegatingId = currentResult.DelegatingId;
                        long newHaMacId = currentResult.HaMacId;
                        long newLocAuthId = currentResult.LocalAuthorityId;
                        long newPoliceForceId = currentResult.PoliceForceId;

                        if (newOrgType == 1)
                            newManagerId = newOrgId;
                        else if (newOrgType == 3)
                            newHaMacId = newOrgId;
                        else if (newOrgType == 2)
                            newLocAuthId = newOrgId;
                        else if (newOrgType == 4)
                            newPoliceForceId = newOrgId;

                        processedData.Add(new DataLinkContactsData { LinkId = currentResult.LinkId, ResponsibilityFrom = from, ResponsibilityTo = to, ManagerId = newManagerId, DelegatingId = newDelegatingId, HaMacId = newHaMacId, LocalAuthorityId = newLocAuthId, PoliceForceId = newPoliceForceId, ArrangementId = null });

                        if (to < currentRespTo)
                        {
                            processedData.Add(new DataLinkContactsData { LinkId = currentResult.LinkId, ResponsibilityFrom = to, ResponsibilityTo = currentRespTo, ManagerId = currentResult.ManagerId, DelegatingId = currentResult.DelegatingId, HaMacId = currentResult.HaMacId, LocalAuthorityId = currentResult.LocalAuthorityId, PoliceForceId = currentResult.PoliceForceId, ArrangementId = currentResult.ArrangementId });
                            for (int j = i + 1; j < resultList.Count; j++)
                            {
                                processedData.Add(new DataLinkContactsData { LinkId = resultList[j].LinkId, ResponsibilityFrom = resultList[j].ResponsibilityFrom, ResponsibilityTo = resultList[j].ResponsibilityTo, ManagerId = resultList[j].ManagerId, DelegatingId = resultList[j].DelegatingId, HaMacId = resultList[j].HaMacId, LocalAuthorityId = resultList[j].LocalAuthorityId, PoliceForceId = resultList[j].PoliceForceId, ArrangementId = currentResult.ArrangementId });
                            }
                            break;
                        }
                        else
                        {
                            if (i + 1 < resultList.Count)
                                processedData.Add(new DataLinkContactsData { LinkId = resultList[i + 1].LinkId, ResponsibilityFrom = to, ResponsibilityTo = resultList[i + 1].ResponsibilityTo, ManagerId = resultList[i + 1].ManagerId, DelegatingId = resultList[i + 1].DelegatingId, HaMacId = resultList[i + 1].HaMacId, LocalAuthorityId = resultList[i + 1].LocalAuthorityId, PoliceForceId = resultList[i + 1].PoliceForceId, ArrangementId = currentResult.ArrangementId });
                            for (int j = i + 2; j < resultList.Count; j++)
                            {
                                processedData.Add(new DataLinkContactsData { LinkId = resultList[j].LinkId, ResponsibilityFrom = resultList[j].ResponsibilityFrom, ResponsibilityTo = resultList[j].ResponsibilityTo, ManagerId = resultList[j].ManagerId, DelegatingId = resultList[j].DelegatingId, HaMacId = resultList[j].HaMacId, LocalAuthorityId = resultList[j].LocalAuthorityId, PoliceForceId = resultList[j].PoliceForceId, ArrangementId = currentResult.ArrangementId });
                            }
                            break;
                        }
                    }                    
                }

                foreach (DataLinkContactsData obj in processedData)
                {
                    if (obj.ResponsibilityFrom == 0 || obj.ResponsibilityFrom == 999999)
                        obj.ResponsibilityFrom = null;
                    if (obj.ResponsibilityTo == 0 || obj.ResponsibilityTo == 999999)
                        obj.ResponsibilityTo = null;
                }

                if (processedData.Count > 0)
                {
                    processedData = RefineProcessedDataRecords(processedData);
                }

                return processedData;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RoadOwnership/ProcessLinkOwnershipRecordsAuto, Exception: " + ex​​​​);
                throw;
            }            
        }
        internal static List<DataLinkContactsData> SortLinkOwnershipRecords(List<DataLinkContactsData> resultList)
        {
            try
            {
                for (int i = 0; i < resultList.Count - 1; i++)
                {
                    for (int j = 0; j < resultList.Count - 1; j++)
                    {
                        if (resultList[j].ResponsibilityFrom > resultList[j + 1].ResponsibilityFrom || resultList[j + 1].ResponsibilityFrom == null)
                        {
                            DataLinkContactsData temp = resultList[j + 1];
                            resultList[j + 1] = resultList[j];
                            resultList[j] = temp;
                        }
                    }
                }
                return resultList;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RoadOwnership/SortLinkOwnershipRecords, Exception: " + ex​​​​);
                throw;
            }            
        }
        internal static List<DataLinkContactsData> RefineProcessedDataRecords(List<DataLinkContactsData> processedData)
        {
            try
            {
                List<DataLinkContactsData> resultData = new List<DataLinkContactsData>();
                for (int i = 0; i < processedData.Count; i++)
                {
                    if (resultData.Count > 0)
                    {
                        if (processedData[i].ResponsibilityFrom != processedData[i].ResponsibilityTo ||
                            (processedData[i].ResponsibilityFrom == null))
                        {
                            for (int j = 0; j < resultData.Count; j++)
                            {
                                if (processedData[i].ResponsibilityFrom == resultData[j].ResponsibilityFrom ||
                                    processedData[i].ResponsibilityTo == resultData[j].ResponsibilityTo)
                                    break;
                                if (j == resultData.Count - 1)
                                    resultData.Add(processedData[i]);
                            }
                        }
                    }
                    else
                    {
                        resultData.Add(processedData[i]);
                    }
                }
                return resultData;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RoadOwnership/RefineProcessedDataRecords, Exception: " + ex​​​​);
                throw;
            }            
        }
        internal static List<LinkInfo> FetchRoadInfoToDisplayOnMap(int organisationId, int fetchFlag, sdogeometry areaGeomVal, int zoomLevel)
        {
            List<LinkInfo> linkIdList = new List<LinkInfo>();
            OracleCommand cmd = new OracleCommand();
            OracleParameter areaGeom = cmd.CreateParameter();
            areaGeom.OracleDbType = OracleDbType.Object;
            areaGeom.UdtTypeName = "MDSYS.SDO_GEOMETRY";
            areaGeom.Value = areaGeomVal;
            areaGeom.ParameterName = "P_ORG_GEOM";

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
            linkIdList,
             UserSchema.Portal + ".RD_OWNERSHIP_FETCH_LINKS",                    
                parameter =>
                {
                    parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ZOOM_LEVEL", zoomLevel, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.Add(areaGeom);
                    parameter.AddWithValue("P_SEARCH_FLAG", fetchFlag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.LinkId = (long)Convert.ToInt32(records["LINK_ID"]);

                    if (fetchFlag != 0 && fetchFlag != 2)
                        instance.LinkGeometry = records.GetGeometryOrNull("GEOM");
                    if (fetchFlag == 0 || fetchFlag == 1)
                        instance.LinkStatus = "owned";
                    else if (fetchFlag == 2 || fetchFlag == 3)
                        instance.LinkStatus = "unassigned";                        
                }
            );
            return linkIdList;
        }
    }
}
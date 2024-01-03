using System;
using System.Collections.Generic;
using Oracle.DataAccess.Client;
using STP.DataAccess.SafeProcedure;
using STP.Domain.RoadNetwork.RoadDelegation;
using STP.Common.Enums;
using STP.Common.Logger;
using NetSdoGeometry;
using STP.Common.Constants;

namespace STP.RoadNetwork.Persistance
{
    public static class RoadDelegationDAO
    {
        internal static List<RoadDelegationData> GetRoadDelegationList(RoadDelegationSearchParam searchParam, int pageSize, int pageNumber, int sortOrder, int presetFilter)
        {
            List<RoadDelegationData> roadDelegationList = new List<RoadDelegationData>();                
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                roadDelegationList,
                UserSchema.Portal + ".STP_ROAD_DELEGATION.SP_RD_SEARCH_AND_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_ARRANGEMENT_NAME", searchParam.ArrangementName != null ? searchParam.ArrangementName.ToUpper().Trim() : null, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_FROM", searchParam.FromOrgName != null ? searchParam.FromOrgName.ToUpper().Trim() : null, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ORG_TO", searchParam.ToOrgName != null ? searchParam.ToOrgName.ToUpper().Trim() : null, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SEARCH_TXT", searchParam.SearchText != null ? searchParam.SearchText.ToUpper().Trim() : null, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_FROM_ORG_ID", searchParam.FromOrgId == 0 ? null : searchParam.FromOrgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_TO_ORG_ID", searchParam.ToOrgId == 0 ? null : searchParam.ToOrgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_PAGENUMBER", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_PAGESIZE", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("SORT_ORDER", sortOrder, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("PRESET_FILTER", presetFilter, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.ArrangementId = records.GetLongOrDefault("ARRANGEMENT_ID");
                    instance.FromOrgId = records.GetLongOrDefault("ORG_FROM_ID");
                    instance.ToOrgId = records.GetLongOrDefault("ORG_TO_ID");
                    instance.TotalRecordCount = (long)Convert.ToInt32(records["TOTALRECORDCOUNT"]);
                    instance.ArrangementName = records.GetStringOrDefault("NAME");
                    instance.FromOrgName = records.GetStringOrDefault("DELGATING_ORG"); //From organisation (Delegator)
                    instance.ToOrgName = records.GetStringOrDefault("DELEGATED_ORG"); //To Organisation (Delegate)
                    instance.AcceptFailure = Convert.ToInt32(records["ACCEPT_FAILURES"]);
                    instance.AllowSubdelegation = Convert.ToInt32(records["ALLOW_SUBDELEGATION"]);
                    instance.RetainNotification = Convert.ToInt32(records["RETAIN_NOTIFICATION"]);
                }
            );            
            return roadDelegationList;
        }
        internal static List<RoadDelegationOrgSummary> GetRoadDelegationOrganisations(string orgName, int page, int pageSize, int searchFlag = 1)
        {
            List<RoadDelegationOrgSummary> rdDelegOrgList = new List<RoadDelegationOrgSummary>();                
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                rdDelegOrgList,
                UserSchema.Portal + ".STP_ROAD_DELEGATION.SP_RD_SEARCH_FETCH_SOA_ORG",
                parameter =>
                {

                    parameter.AddWithValue("P_ORG_NAME_SEARCH", orgName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SEARCH_FLAG", searchFlag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_PAGENUMBER", page, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_PAGESIZE", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.OrganisationId = records.GetLongOrDefault("ORGANISATION_ID");
                    instance.OrganisationName = records.GetStringOrDefault("ORGNAME");
                    instance.ContactName = records.GetStringOrDefault("FULL_NAME");
                    instance.TotalRows = Convert.ToInt32(records.GetDecimalOrDefault("TOTAL_ROWS"));
                }
             );            
            return rdDelegOrgList;
        }
        internal static bool IsDelegationAllowed(int orgId)
        {
            bool result = false;                
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + ".STP_ROAD_DELEGATION.SP_RD_FETCH_DELEGATE_ALL_DET",
                parameter =>
                {
                    parameter.AddWithValue("P_ORG_ID", orgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    result = record.GetStringOrDefault("STATUS") == "TRUE";
                });            
            return result;
        }
        internal static RoadDelegationData GetRoadDelegationDetailsWithLinkInfo(int arrangementId)
        {
            RoadDelegationData roadDelegationObj = new RoadDelegationData();
            long linkId = 0;
            int statusFlag = 0;                
            List<RoadDelegationData> tempRoadDelegListObj = new List<RoadDelegationData>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                tempRoadDelegListObj,
                UserSchema.Portal + ".STP_ROAD_DELEGATION.SP_RD_GET_ROAD_DELEG",
                parameter =>
                {
                    parameter.AddWithValue("P_ARRANGEMENT_ID", arrangementId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    if (statusFlag == 0)
                    {
                        statusFlag++;
                        roadDelegationObj.ArrangementId = records.GetLongOrDefault("ARRANGEMENT_ID");
                        roadDelegationObj.ArrangementName = records.GetStringOrDefault("NAME");
                        roadDelegationObj.FromOrgId = records.GetLongOrDefault("DELEGATING_ID");
                        roadDelegationObj.FromOrgName = records.GetStringOrDefault("DELEGATING_NAME");
                        var test=records.GetFieldType("DELEGATING_CONTACT_ID");
                        roadDelegationObj.FromContactId = Convert.ToInt64(records.GetDecimalOrDefault("DELEGATING_CONTACT_ID"));
                        roadDelegationObj.FromContactName = records.GetStringOrDefault("DELEGATING_FULL_NAME");
                        roadDelegationObj.ToOrgId = records.GetLongOrDefault("MANAGER_ID");
                        roadDelegationObj.ToOrgName = records.GetStringOrDefault("MANAGER_NAME");
                        roadDelegationObj.ToContactId = Convert.ToInt64(records.GetDecimalOrDefault("MANAGER_CONTACT_ID"));
                        roadDelegationObj.ToContactName = records.GetStringOrDefault("MANAGER_FULL_NAME");
                        roadDelegationObj.AcceptFailure = Convert.ToInt32(records["ACCEPT_FAILURES"]);
                        roadDelegationObj.AllowSubdelegation = Convert.ToInt32(records["ALLOW_SUBDELEGATION"]);
                        roadDelegationObj.DelegateAll = Convert.ToInt32(records["DELEGATE_ALL"]);
                        roadDelegationObj.RetainNotification = Convert.ToInt32(records["RETAIN_NOTIFICATION"]);
                        roadDelegationObj.Comments = records.GetStringOrDefault("COMMENTS");
                        roadDelegationObj.LinkIdList = new List<long>();
                    }
                    linkId = records.GetLongOrDefault("LINK_ID");
                    roadDelegationObj.LinkIdList.Add(linkId);
                }
            );
            return roadDelegationObj;
        }
        internal static RoadDelegationData GetRoadDelegationDetails(int arrangementId)
        {
            RoadDelegationData roadDelegationObj = new RoadDelegationData();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                roadDelegationObj,
                UserSchema.Portal + ".STP_ROAD_DELEGATION.SP_RD_GET_DELEG_DET",
                parameter =>
                {
                    parameter.AddWithValue("P_ARRANGEMENT_ID", arrangementId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records) =>
                {
                    roadDelegationObj.ArrangementId = records.GetLongOrDefault("ARRANGEMENT_ID");
                    roadDelegationObj.ArrangementName = records.GetStringOrDefault("NAME");
                    roadDelegationObj.FromOrgId = records.GetLongOrDefault("DELEGATING_ID");
                    roadDelegationObj.FromOrgName = records.GetStringOrDefault("DELEGATING_NAME");
                    roadDelegationObj.FromContactId = Convert.ToInt32(records["DELEGATING_CONTACT_ID"]);
                    roadDelegationObj.FromContactName = records.GetStringOrDefault("DELEGATING_FULL_NAME");
                    roadDelegationObj.ToOrgId = records.GetLongOrDefault("MANAGER_ID");
                    roadDelegationObj.ToOrgName = records.GetStringOrDefault("MANAGER_NAME");
                    roadDelegationObj.ToContactId = Convert.ToInt32(records["MANAGER_CONTACT_ID"]);
                    roadDelegationObj.ToContactName = records.GetStringOrDefault("MANAGER_FULL_NAME");
                    roadDelegationObj.AcceptFailure = Convert.ToInt32(records["ACCEPT_FAILURES"]);
                    roadDelegationObj.AllowSubdelegation = Convert.ToInt32(records["ALLOW_SUBDELEGATION"]);
                    roadDelegationObj.DelegateAll = Convert.ToInt32(records["DELEGATE_ALL"]);
                    roadDelegationObj.RetainNotification = Convert.ToInt32(records["RETAIN_NOTIFICATION"]);
                    roadDelegationObj.Comments = records.GetStringOrDefault("COMMENTS");
                }
            );
            return roadDelegationObj;
        }
        internal static int DeleteRoadDelegation(long arrangementId)
        {
            int affectedRows = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
               UserSchema.Portal + ".STP_ROAD_DELEGATION.SP_RD_DELETE_ROAD_DELEG",
                parameter =>
                {
                    parameter.AddWithValue("P_ARRANGEMENT_ID", arrangementId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    //parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    parameter.AddWithValue("P_AFFECTEDROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    affectedRows = record.GetInt32("P_AFFECTEDROWS");
                }
            );
            return affectedRows;
        }
        internal static List<DelegationArrangementDetails> GetArrangementDetails(int orgId)
        {
            List<DelegationArrangementDetails> rdDelegList = new List<DelegationArrangementDetails>();               
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                rdDelegList,
                UserSchema.Portal + ".STP_ROAD_DELEGATION.SP_RD_GET_DELEG_ARRANGMENT",
                parameter =>
                {
                    parameter.AddWithValue("P_ORG_ID", orgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.DelegateAll = Convert.ToInt32(records["DELEGATE_ALL"]);
                    instance.ArrangementName = records.GetStringOrDefault("NAME");
                });
            return rdDelegList;
        }
        internal static RoadDelegationOrgSummary GetOrganisationGeoRegion(int orgId)
        {
            RoadDelegationOrgSummary rdDelegOrg = new RoadDelegationOrgSummary();               
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                rdDelegOrg,
                UserSchema.Portal + ".STP_ROAD_DELEGATION.SP_RD_FETCH_ORG_GEO_REG",
                parameter =>
                {
                    parameter.AddWithValue("P_ORG_ID", orgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.OrganisationGeoRegion = records.GetGeometryOrNull("GEO_REGION");
                }
            );
            return rdDelegOrg;
        }
        internal static List<long> GetLinksAllowedForDelegation(List<long> linkIdList, int fromOrgId)
        {
            OracleParameter param = new OracleParameter();
            List<long> allowedLinkIdLst = new List<long>();
            List<long> tmpLinkIdLst = new List<long>();
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
            tmpLinkIdLst,
            UserSchema.Portal + ".STP_ROAD_DELEGATION.SP_RD_CHECK_DELEG_ALLWD",
            parameter =>
            {
                parameter.Add(param);
                parameter.AddWithValue("P_FROM_ORG_ID", fromOrgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
                (records, instance) =>
                {
                    link = records.GetLongOrDefault("LINK_ID");
                    allowedLinkIdLst.Add(link);
                }
            );                       
            return allowedLinkIdLst;
        }
        internal static bool CreateRoadDelegation(RoadDelegationData roadDelegationObj)
        {
            long result = 0;
            List<LinkInfo> linkIdWithDetails = new List<LinkInfo>();
            List<long> linkIdWithoutDetails = new List<long>();
            OracleCommand cmd = new OracleCommand();
            OracleParameter param = new OracleParameter();
               
            long orgId = roadDelegationObj.FromOrgId;
            if (roadDelegationObj.LinkIdList == null)
            {
                roadDelegationObj.LinkIdList = new List<long>();
            }

            foreach (LinkInfo linkInfoObj in roadDelegationObj.LinkInfoList)
            {
                if (linkInfoObj.ResponsibilityTo == null && linkInfoObj.ResponsibilityFrom == null)
                {
                    linkIdWithoutDetails.Add(linkInfoObj.LinkId); // creating a list of link id's that dont have any LRS value's
                }
                else
                {//saving link information that has LRS values in them.
                    linkIdWithDetails.Add(new LinkInfo { LinkId = linkInfoObj.LinkId, ResponsibilityFrom = linkInfoObj.ResponsibilityFrom, ResponsibilityTo = linkInfoObj.ResponsibilityTo });
                }
            }

            #region Portion to make an oracle parameter from link id list                
            if (linkIdWithoutDetails.ToArray().Length == 0 || (linkIdWithoutDetails.ToArray().Length == 0 && roadDelegationObj.DelegateAll == 1))
            {
                param.OracleDbType = OracleDbType.Int32;
                param.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
                param.Value = new long[] { 10000 }; //Just assigning a temporary array for avoiding exception
                param.Size = 1;
            }
            else
            {
                param.OracleDbType = OracleDbType.Int32;
                param.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
                param.Value = linkIdWithoutDetails.ToArray(); // change when the testing is completed
                param.Size = linkIdWithoutDetails.ToArray().Length;
            }

            #region saving road delegation information that has links without LRS information.
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
            result,
            UserSchema.Portal + ".STP_ROAD_DELEGATION.SP_RD_SAVE_ROAD_DELEG",
            parameter =>
            {
                parameter.AddWithValue("P_ARRANGEMENT_NAME", roadDelegationObj.ArrangementName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_ORG_FROM", roadDelegationObj.FromOrgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);//delegating
                parameter.AddWithValue("P_FROM_ORGNAME", roadDelegationObj.FromOrgName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);//delegating
                parameter.AddWithValue("P_CONT_FROM_ID", roadDelegationObj.FromContactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);//delegating
                parameter.AddWithValue("P_ORG_TO", roadDelegationObj.ToOrgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);//new manager
                parameter.AddWithValue("P_TO_ORGNAME", roadDelegationObj.ToOrgName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);//new manager
                parameter.AddWithValue("P_CONT_TO_ID", roadDelegationObj.ToContactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);//new manager
                parameter.AddWithValue("P_RETAIN_NOTIF_FLAG", roadDelegationObj.RetainNotification, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_ALLW_SUB_DELG_FLAG", roadDelegationObj.AllowSubdelegation, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_ACCPT_FAIL_FLAG", roadDelegationObj.AcceptFailure, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_COMMENTS", roadDelegationObj.Comments, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_DELEGATE_ALL", roadDelegationObj.DelegateAll, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.Add(param);
                parameter.AddWithValue("P_ROAD_GRP", roadDelegationObj.RoadGroupNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
                (records, instance) =>
                {
                    result = Convert.ToInt32(records["STATUS"]);
                }
            );
            long arrID = result;
            #endregion

            if (linkIdWithDetails.Count > 0)
            {
                List<Delegation> delegationList = new List<Delegation>();
                Delegation delegList = null;

                foreach (LinkInfo linkInfoObj in linkIdWithDetails)
                {
                    List<DataLinkContactsData> dcData = new List<DataLinkContactsData>();

                    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                        dcData,
                        UserSchema.Portal + ".STP_ROAD_DELEGATION.GET_PARTIAL_DELEG_RECORDS",
                        parameter =>
                        {
                            parameter.AddWithValue("P_LINK_ID", linkInfoObj.LinkId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                            parameter.AddWithValue("P_MANAGER_ID", orgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767); //Manager Organisation
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

                    List<DataLinkContactsData> processedData = ProcessLinkDelegationRecordsAuto(dcData, linkInfoObj, roadDelegationObj.ToOrgId, roadDelegationObj.FromOrgId, arrID);
                    foreach (DataLinkContactsData obj in processedData)
                    {
                        delegList = new Delegation();
                        delegList.LinkId = obj.LinkId;
                        delegList.ResponsibilityFrom = obj.ResponsibilityFrom;
                        delegList.ResponsibilityTo = obj.ResponsibilityTo;
                        delegList.ManagerId = obj.ManagerId;
                        delegList.DelegatingId = obj.DelegatingId;
                        delegList.HaMacId = obj.HaMacId;
                        delegList.LocalAuthorityId = obj.LocalAuthorityId;
                        delegList.PoliceForceId = obj.PoliceForceId;
                        delegList.ArrangementId = obj.ArrangementId;
                        delegationList.Add(delegList);
                    }
                }

                DelegationArray delegDetail = new DelegationArray();
                delegDetail.DelegationArr = delegationList.ToArray();

                OracleParameter delegations = cmd.CreateParameter();
                delegations.OracleDbType = OracleDbType.Object;
                delegations.UdtTypeName = "PORTAL.DELEGATIONARRAY";
                delegations.Value = delegationList.Count > 0 ? delegDetail : null;
                delegations.ParameterName = "P_DELEG_ARRAY";

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + ".STP_ROAD_DELEGATION.SP_RD_SAVE_ROAD_DELEG_PARTIAL",
                parameter =>
                {
                    parameter.AddWithValue("P_RETAIN_NOTIF_FLAG", roadDelegationObj.RetainNotification, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ACCPT_FAIL_FLAG", roadDelegationObj.AcceptFailure, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.Add(delegations);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        result = Convert.ToInt32(records["FLAG"]);
                    }
                );
            }                
            #endregion

            #region saving road delegation information that has links with LRS information.
            // to be implemented
            #endregion

            return result != 0;

        }
        internal static List<DataLinkContactsData> ProcessLinkDelegationRecordsAuto(List<DataLinkContactsData> resultList, LinkInfo linkInfoObj, long newManagerId, long newDelegatingId, long newArrangementId)
        {
            try
            {
                List<DataLinkContactsData> processedData = new List<DataLinkContactsData>();
                int? from = linkInfoObj.ResponsibilityFrom;
                int? to = linkInfoObj.ResponsibilityTo;

                bool fromSetFlag = false;

                if (resultList.Count > 0)
                {
                    resultList = SortLinkDelegationRecords(resultList);
                }

                if (from == null)
                    from = 0;
                if (to == null || to == 0)
                    to = 999999; //a large value

                for (int i = 0; i < resultList.Count; i++)
                {
                    DataLinkContactsData currentResult = resultList[i];
                    int? currentRespFrom = currentResult.ResponsibilityFrom;
                    if (currentRespFrom == null)
                        currentRespFrom = 0;

                    int? currentRespTo = currentResult.ResponsibilityTo;
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
                        processedData.Add(new DataLinkContactsData { LinkId = currentResult.LinkId, ResponsibilityFrom = from, ResponsibilityTo = to, ManagerId = newManagerId, DelegatingId = newDelegatingId, HaMacId = currentResult.HaMacId, LocalAuthorityId = currentResult.LocalAuthorityId, PoliceForceId = currentResult.PoliceForceId, ArrangementId = newArrangementId });
                        
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
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RoadDelegation/ProcessLinkDelegationRecordsAuto, Exception: " + ex​​​​);               
                throw;
            }            
        }
        internal static List<DataLinkContactsData> SortLinkDelegationRecords(List<DataLinkContactsData> resultList)
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
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RoadDelegation/SortLinkDelegationRecords, Exception: " + ex​​​​);
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
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - RoadDelegation/RefineProcessedDataRecords, Exception: " + ex​​​​);
                throw;
            }                        
        }

        /// <summary>
        /// function to fetch road links. This function can fetch links within an area through geom, owned or managed by organisation( through organisation id or name) 
        /// </summary>
        /// <param name="arrangementId"></param>
        /// <param name="searchFlag">
        /// 0 : Fetch roads of an arrangement. 
        /// 1 : Fetch roads of Delegator or Delegate organisation id 
        /// 2 : Fetch roads of Delegator or Delegate Oraganisation name 
        /// 3 : fetch roads within a geometry to select roads for delegation these are owned or managed by a road manager
        /// 4 : fetch roads within a geometry to select roads of a delegating organisation
        /// 5 : fetch roads of an area based on various zoom level's owned by a organisation.
        /// 6 : fetch roads for an arrangement id based on various zoom level's and geometry
        /// </param>
        /// <param name="areaGeomVal">geometry of area</param>
        /// <param name="searchParam">search input's</param>
        /// <returns></returns>
        internal static List<LinkInfo> FetchRoadInfoToDisplayOnMap(int arrangementId, int zoomLevel, int searchFlag, sdogeometry areaGeomVal, RoadDelegationSearchParam searchParam)
        {
            List<LinkInfo> linkInfoList = new List<LinkInfo>();
            OracleCommand cmd = new OracleCommand();
            OracleParameter areaGeom = cmd.CreateParameter();
            areaGeom.OracleDbType = OracleDbType.Object;
            areaGeom.UdtTypeName = "MDSYS.SDO_GEOMETRY";
            areaGeom.Value = areaGeomVal;
            areaGeom.ParameterName = "P_ORG_GEOM";

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
            linkInfoList,
            UserSchema.Portal + ".STP_ROAD_DELEGATION.SP_RD_FETCH_ROAD_LINKS",
            parameter =>
            {
                parameter.AddWithValue("P_ARRANGEMENT_ID", arrangementId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_FROM_ORG_ID", searchParam.FromOrgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767); //Manager for searchflag 3 and Delegating for other flags
                parameter.AddWithValue("P_TO_ORG_ID", searchParam.ToOrgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767); //Delegating for searchflag 3 and Manager for other flags
                parameter.AddWithValue("P_FROM_ORG", searchParam.FromOrgName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767); //Manager for searchflag 3 and Delegating for other flags
                parameter.AddWithValue("P_TO_ORG", searchParam.ToOrgName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);  //Delegating for searchflag 3 and Manager for other flags
                parameter.Add(areaGeom);
                parameter.AddWithValue("P_ZOOM_LEVEL", zoomLevel, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_SEARCH_FLAG", searchFlag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
                (records, instance) =>
                {                            
                    instance.ResponsibilityFrom = records.GetInt32Nullable("RESPONSIBILITY_FROM");
                    instance.ResponsibilityTo = records.GetInt32Nullable("RESPONSIBILITY_TO");
                    instance.LinkId = records.GetLongOrDefault("LINK_ID");

                    if (searchFlag == 3 || searchFlag == 5 || searchFlag == 6 || searchFlag == 7)
                    {
                        instance.LinkGeometry = records.GetGeometryOrNull("GEOM");
                        if (searchFlag != 6 && searchFlag != 7)
                        {
                                    
                            instance.SubDelegationAllowed = records.GetStringOrDefault("ALLOW_SUBDELEGATION") == "TRUE";
                            instance.ArrangementId = records.GetLongOrNullable("ARRANGEMENT_ID");
                            if (instance.ArrangementId != null && instance.ArrangementId != 0)
                            {
                                if (instance.SubDelegationAllowed)
                                {
                                    instance.LinkManageStatus = 'b'; // sub delegation allowed
                                }
                                else
                                {
                                    instance.LinkManageStatus = 'c'; // sub delegation not allowed
                                }
                            }
                            else
                            {
                                instance.LinkManageStatus = 'a';
                            }                                                                        
                        }
                    }
                }
            );                
            return linkInfoList;
        }

        internal static bool EditRoadDelegation(RoadDelegationData roadDelegationObject)
        {
            long result = 0;
            List<LinkInfo> linkIdWithDetails = new List<LinkInfo>();
            List<long> linkIdWithoutDetails = new List<long>();
            OracleCommand cmd = new OracleCommand();                
            OracleParameter param = new OracleParameter();
            Logger.GetInstance().LogMessage(Log_Priority.WARNING, Logger.LogInstance + @" - RoadDelegationDAO/EditRoadDelegation,Info : " + roadDelegationObject.ArrangementName + " " + roadDelegationObject.LinkInfoList.Count);
            if (roadDelegationObject.LinkIdList == null)
            {
                roadDelegationObject.LinkIdList = new List<long>();
            }
            foreach (LinkInfo linkInfoObj in roadDelegationObject.LinkInfoList)
            {
                if (linkInfoObj.ResponsibilityTo == null && linkInfoObj.ResponsibilityFrom == null)
                {
                    linkIdWithoutDetails.Add(linkInfoObj.LinkId); // creating a list of link id's that dont have any LRS value's
                }
                else
                {//saving link information that has LRS values in them.
                    linkIdWithDetails.Add(new LinkInfo { LinkId = linkInfoObj.LinkId, ResponsibilityFrom = linkInfoObj.ResponsibilityFrom, ResponsibilityTo = linkInfoObj.ResponsibilityTo });
                }
            }

            #region Portion to make an oracle parameter from link id list                {
            if (linkIdWithoutDetails.ToArray().Length == 0 || (linkIdWithoutDetails.ToArray().Length == 0 && roadDelegationObject.DelegateAll == 1))
            {
                param.OracleDbType = OracleDbType.Int32;
                param.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
                param.Value = null;
            }
            else
            {
                param.OracleDbType = OracleDbType.Int32;
                param.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
                param.Value = linkIdWithoutDetails.ToArray(); // change when the testing is completed
                param.Size = linkIdWithoutDetails.ToArray().Length;
            }

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
            result,
            UserSchema.Portal + ".STP_ROAD_DELEGATION.SP_RD_EDIT_ROAD_DELEG",
            parameter =>
            {
                parameter.AddWithValue("P_ARRANGEMENT_ID", roadDelegationObject.ArrangementId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_ARRANGEMENT_NAME", roadDelegationObject.ArrangementName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_ORG_FROM", roadDelegationObject.FromOrgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);//Delegating organisation
                parameter.AddWithValue("P_FROM_ORGNAME", roadDelegationObject.FromOrgName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_CONT_FROM_ID", roadDelegationObject.FromContactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_ORG_TO", roadDelegationObject.ToOrgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767); //Manager Organisation
                parameter.AddWithValue("P_TO_ORGNAME", roadDelegationObject.ToOrgName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_CONT_TO_ID", roadDelegationObject.ToContactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_RETAIN_NOTIF_FLAG", roadDelegationObject.RetainNotification, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_ALLW_SUB_DELG_FLAG", roadDelegationObject.AllowSubdelegation, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_ACCPT_FAIL_FLAG", roadDelegationObject.AcceptFailure, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_COMMENTS", roadDelegationObject.Comments, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_DELEGATE_ALL", roadDelegationObject.DelegateAll, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.Add(param);
                parameter.AddWithValue("P_ROAD_GRP", roadDelegationObject.RoadGroupNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
                (records, instance) =>
                {
                    result = Convert.ToInt32(records["STATUS"]);

                }
            );
            # region linkwithdetails
            if (linkIdWithDetails.Count > 0)
            {
                List<Delegation> delegationList = new List<Delegation>();
                Delegation delegList = null;

                foreach (LinkInfo linkInfoObj in linkIdWithDetails)
                {
                    List<DataLinkContactsData> dcData = new List<DataLinkContactsData>();

                    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    dcData,
                    UserSchema.Portal + ".STP_ROAD_DELEGATION.GET_PARTIAL_DELEG_RECORDS",
                    parameter =>
                    {
                        parameter.AddWithValue("P_LINK_ID", linkInfoObj.LinkId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_MANAGER_ID", null, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
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

                    List<DataLinkContactsData> processedData = ProcessLinkDelegationRecordsAuto(dcData, linkInfoObj, roadDelegationObject.ToOrgId, roadDelegationObject.FromOrgId, roadDelegationObject.ArrangementId);
                    foreach (DataLinkContactsData obj in processedData)
                    {
                        delegList = new Delegation();
                        delegList.LinkId = obj.LinkId;
                        delegList.ResponsibilityFrom = obj.ResponsibilityFrom;
                        delegList.ResponsibilityTo = obj.ResponsibilityTo;
                        delegList.ManagerId = obj.ManagerId;
                        delegList.DelegatingId = obj.DelegatingId;
                        delegList.HaMacId = obj.HaMacId;
                        delegList.LocalAuthorityId = obj.LocalAuthorityId;
                        delegList.PoliceForceId = obj.PoliceForceId;
                        delegList.ArrangementId = obj.ArrangementId;
                        delegationList.Add(delegList);
                    }
                }

                DelegationArray delegDetail = new DelegationArray();
                delegDetail.DelegationArr = delegationList.ToArray();

                OracleParameter delegations = cmd.CreateParameter();
                delegations.OracleDbType = OracleDbType.Object;
                delegations.UdtTypeName = "PORTAL.DELEGATIONARRAY";
                delegations.Value = delegationList.Count > 0 ? delegDetail : null;
                delegations.ParameterName = "P_DELEG_ARRAY";

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + ".STP_ROAD_DELEGATION.SP_RD_SAVE_ROAD_DELEG_PARTIAL",//SP_RD_SAVE_ROAD_DELEG_TEST",
                parameter =>
                {
                    parameter.AddWithValue("P_RETAIN_NOTIF_FLAG", roadDelegationObject.RetainNotification, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ACCPT_FAIL_FLAG", roadDelegationObject.AcceptFailure, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.Add(delegations);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        result = Convert.ToInt32(records["FLAG"]);
                    }
                );
            }              
            #endregion
            #endregion

            #region saving road delegation information that has links with LRS information.
            // to be implemented
            #endregion

            return result != 0;
        }
    }
}
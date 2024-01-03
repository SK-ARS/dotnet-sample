using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.DataAccess.SafeProcedure;
using STP.Domain.MovementsAndNotifications.HaulierMovementsAPI;
using STP.Domain.MovementsAndNotifications.SOAPoliceMovementsAPI;
using STP.Domain.MovementsAndNotifications.SORTMovementsAPI;
using System;
using System.Collections.Generic;
using System.Globalization;
using static STP.Common.Enums.ExternalApiEnums;
using static STP.Domain.VehiclesAndFleets.VehicleEnums;

namespace STP.MovementsAndNotifications.Persistance
{
    public static class MovementExternalApiDao
    {
        #region Get Haulier Movement List
        public static HaulierMovementDetails GetHaulierMovementList(int organisationId, int historicData, int movementType, int pageNo, int pageSize)
        {
            List<Domain.MovementsAndNotifications.HaulierMovementsAPI.Movement> movementsList = new List<Domain.MovementsAndNotifications.HaulierMovementsAPI.Movement>();
            HaulierMovementDetails movementDetails = new HaulierMovementDetails();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                 movementsList, UserSchema.Portal + ".STP_MOVEMENT_EXTERNAL.SP_GET_HAULIER_MOVEMENT_LIST",
                parameter =>
                {
                    parameter.AddWithValue("p_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SHOW_HISTORIC", historicData, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_FLAG", movementType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_PAGENUMBER ", pageNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_PAGESIZE", pageSize, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.ESDALReferenceNumber = records.GetStringOrDefault("ESDAL");
                        var vehicleType = records.GetFieldType("VEHICLE_CLASSIFICATION") != null && records.GetFieldType("VEHICLE_CLASSIFICATION").Name == "Decimal" ? records.GetDecimalOrDefault("VEHICLE_CLASSIFICATION") : records.GetInt32OrDefault("VEHICLE_CLASSIFICATION");
                        ExternalApiGeneralClassificationType ExternalApiGeneralClassificationType = (ExternalApiGeneralClassificationType)vehicleType;
                        try
                        {
                            instance.MovementType = ExternalApiGeneralClassificationType.GetEnumDescription();
                        }
                        catch
                        {
                            instance.MovementType = (VehicleClassificationType.NoVehicleClassification).GetEnumDescription();
                        }
                        var MovementCategory = records.GetStringOrDefault("MOVEMENT_TYPE");
                        instance.MovementCategory = MovementCategory;

                        decimal IsVR1 = records.GetFieldType("IS_VR1") != null && records.GetFieldType("IS_VR1").Name == "Decimal" ? records.GetDecimalOrDefault("IS_VR1") : (records.GetFieldType("IS_VR1").Name == "Int16"? records.GetInt16OrDefault("IS_VR1"): records.GetInt32OrDefault("IS_VR1"));
                        ExternalApiBitSystem bitSystem = (ExternalApiBitSystem)IsVR1;
                        instance.IsVR1 = bitSystem.GetEnumDescription();

                        if (MovementCategory.ToLower() == "application")
                        {
                            var ApplicationStatus = records.GetFieldType("APPLICATION_STATUS") != null && records.GetFieldType("APPLICATION_STATUS").Name == "Decimal" ? records.GetDecimalOrDefault("APPLICATION_STATUS") : records.GetInt32OrDefault("APPLICATION_STATUS");

                            if (instance.IsVR1 == ExternalApiBitSystem.Yes.ToString())
                                instance.Status = ((ExternalAPIStatus)ApplicationStatus).GetEnumDescription();
                            else
                            {
                                var VersionStatus = records.GetFieldType("VERSION_STATUS")!=null && records.GetFieldType("VERSION_STATUS").Name == "Decimal" ? records.GetDecimalOrDefault("VERSION_STATUS") : records.GetInt32OrDefault("VERSION_STATUS");
                                if (VersionStatus > 0)
                                    instance.Status = ((ExternalAPIStatus)VersionStatus).GetEnumDescription();
                                else
                                    instance.Status = ((ExternalAPIStatus)ApplicationStatus).GetEnumDescription();
                            }
                        }
                        else
                        {
                            //Notification
                            var WorkInProgress = records.GetFieldType("WORK_IN_PROGRESS") != null && records.GetFieldType("WORK_IN_PROGRESS").Name == "Decimal" ? records.GetDecimalOrDefault("WORK_IN_PROGRESS") : (records.GetFieldType("WORK_IN_PROGRESS").Name == "Int16"? records.GetInt16OrDefault("WORK_IN_PROGRESS"): records.GetInt32OrDefault("WORK_IN_PROGRESS"));
                            if (WorkInProgress > 0)
                            {
                                var ExternalAPIStatusType = ExternalAPIStatus.workinprogress;
                                instance.Status = ExternalAPIStatusType.GetEnumDescription();
                            }
                            else
                            {
                                var ExternalAPIStatusType = ExternalAPIStatus.submitted;
                                instance.Status = ExternalAPIStatusType.GetEnumDescription();
                            }
                        }

                        instance.FromSummary = records.GetStringOrDefault("FROM_DESCR");
                        instance.ToSummary = records.GetStringOrDefault("TO_DESCR");
                        instance.FromDate = records.GetDateTimeOrDefault("START_DATE").ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        instance.ToDate = records.GetDateTimeOrDefault("END_DATE").ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        instance.HaulierReference = records.GetStringOrDefault("HAULIERS_REF");

                        movementDetails.TotalRecords = Convert.ToInt32(records.GetDecimalOrDefault("ROW_COUNT"));

                    }
            );
            //To get the Page Count
            if (movementDetails.TotalRecords > 0)
            {
                int Pages = movementDetails.TotalRecords / (pageSize);
                int RemainingRecords = movementDetails.TotalRecords % (pageSize);

                if (RemainingRecords >= 1)
                    movementDetails.NumberOfPages = Pages + 1;
                else
                    movementDetails.NumberOfPages = Pages;
            }
            movementDetails.Movements = movementsList;
            movementDetails.PageSize = movementsList.Count;
            movementDetails.PageNumber = pageNo;
            return movementDetails;
        }
        #endregion

        #region Get SOA/Police Movement List
        public static SoaPoliceDetails GetSOAPoliceMovementList(int organisationId, int historicData, int pageNo, int pageSize, bool isPolice)
        {
            List<Domain.MovementsAndNotifications.SOAPoliceMovementsAPI.Movement> movementsList = new List<Domain.MovementsAndNotifications.SOAPoliceMovementsAPI.Movement>();
            SoaPoliceDetails movementDetails = new SoaPoliceDetails();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                movementsList,
                UserSchema.Portal + ".STP_MOVEMENT_EXTERNAL.SP_GET_MOVEMENT_INBOX_SOA_POLICE",
                parameter =>
                {
                    parameter.AddWithValue("p_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_SHOW_HISTORIC", historicData, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_PAGENUMBER ", pageNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_PAGESIZE", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);

                },
                (records, instance) =>
                {
                    instance.ESDALReferenceNumber = records.GetStringOrDefault("ESDAL_REFERENCE");

                    var itemStatus = records.GetInt32OrDefault("ITEM_STATUS");
                    ExternalApiInboxStatus inboxStatus = (ExternalApiInboxStatus)itemStatus;
                    instance.InboxStatus = inboxStatus.GetEnumDescription();

                    instance.MovementDate = records.GetDateTimeOrDefault("START_DATE").ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " to " + records.GetDateTimeOrDefault("END_DATE").ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    instance.ReceivedDate = records.GetDateTimeOrDefault("RECEIVED_DATE").ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    var itemType = records.GetInt32OrDefault("ITEM_TYPE");
                    ExternalApiMessageTypes messageType = (ExternalApiMessageTypes)itemType;
                    instance.MessageType = messageType.GetEnumDescription();

                    var moveType = records.GetInt32OrDefault("MOVE_TYPE");
                    ExternalApiGeneralClassificationType movementType = (ExternalApiGeneralClassificationType)moveType;
                    instance.MovementType = movementType.GetEnumDescription();

                    if (!isPolice)
                    {
                        var icaStaus = records.GetInt32OrDefault("ICA_STATUS");
                        ExternalApiSuitability suitability = (ExternalApiSuitability)icaStaus;
                        if(suitability==0)
                        { suitability = STP.Common.Enums.ExternalApiEnums.ExternalApiSuitability.unknown; }
                        instance.Suitablity = suitability.GetEnumDescription();
                    }
                    var isVr1 = records.GetInt16OrDefault("requires_vr1");
                    ExternalApiBitSystem bitSystem = (ExternalApiBitSystem)isVr1;
                    instance.IsVR1 = bitSystem.GetEnumDescription();
                    movementDetails.TotalRecords = Convert.ToInt32(records.GetDecimalOrDefault("totalrecordcount"));
                }
            );
            //To get the Page Count
            if (movementDetails.TotalRecords > 0)
            {
                int Pages = movementDetails.TotalRecords / pageSize;
                int RemainingRecords = movementDetails.TotalRecords % pageSize;
                if (RemainingRecords >= 1)
                    movementDetails.NumberOfPages = Pages + 1;
                else
                    movementDetails.NumberOfPages = Pages;
            }
            movementDetails.Movements = movementsList;
            movementDetails.PageSize = movementsList.Count;
            movementDetails.PageNumber = pageNo;
            return movementDetails;
        }
        #endregion

        #region Get SORT Movement List
        public static SORTMovementDetails GetSORTMovementList(int organisationId, int historicData, int pageNo, int pageSize)
        {
            List<Domain.MovementsAndNotifications.SORTMovementsAPI.Movement> movementsList = new List<Domain.MovementsAndNotifications.SORTMovementsAPI.Movement>();
            SORTMovementDetails movementDetails = new SORTMovementDetails();
            try
            {
                ExternalApiGeneralClassificationType classificationType = new ExternalApiGeneralClassificationType();
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                    movementsList,
                    UserSchema.Sort + ".STP_MOVEMENT_EXTERNAL.SP_GET_SORT_MOV_LIST_FILTER_SRT ",
                    parameter =>
                    {
                        parameter.AddWithValue("p_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_SHOW_HISTORIC", historicData, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_PAGENUMBER ", pageNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_PAGESIZE", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            instance.ESDALReferenceNumber = (records.GetStringOrDefault("ESDAL_REF_NUMBER"));
                            
                            if (records.GetInt32OrDefault("VEHICLE_CLASSIFICATION") > 0)                            
                                instance.MovementType = ((ExternalApiGeneralClassificationType)records.GetInt32OrDefault("VEHICLE_CLASSIFICATION")).GetEnumDescription();                            
                            else
                                instance.MovementType = string.Empty;

                            if (records.GetInt32OrDefault("CHECKING_STATUS") > 0)
                                instance.CheckingStatus = ((ExternalApiCheckStatus)records.GetInt32OrDefault("CHECKING_STATUS")).GetEnumDescription();                        
                            else
                                instance.CheckingStatus = string.Empty;

                            if (records.GetInt32OrDefault("PROJECT_STATUS") > 0)
                                instance.ProjectStatus = ((ExternalApiProjectStatus)records.GetInt32OrDefault("PROJECT_STATUS")).GetEnumDescription();
                            else
                                instance.ProjectStatus = string.Empty;

                            instance.Owner = records.GetStringOrDefault("NAME");
                            instance.FromSummary = records.GetStringOrDefault("FROM_DESCR");
                            instance.ToSummary = records.GetStringOrDefault("TO_DESCR");
                            instance.ApplicationDate = records.GetDateTimeOrDefault("APPLICATION_DATE").ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                            instance.DueDate = records.GetDateTimeOrDefault("DUE_DATE").ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) == "01/01/0001" ? "" : records.GetDateTimeOrDefault("DUE_DATE").ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                            movementDetails.TotalRecords = Convert.ToInt32(records.GetDecimalOrDefault("ROW_COUNT"));
                        }
                );
                //To get the Page Count
                if (movementDetails.TotalRecords > 0)
                {
                    int Pages = movementDetails.TotalRecords / (pageSize);
                    int RemainingRecords = movementDetails.TotalRecords % (pageSize);

                    if (RemainingRecords >= 1)
                        movementDetails.NumberOfPages = Pages + 1;
                    else
                        movementDetails.NumberOfPages = Pages;
                }
                movementDetails.SORTMovements = movementsList;
                movementDetails.PageSize = movementsList.Count;
                movementDetails.PageNumber = pageNo;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return movementDetails;
        }

        #endregion
    }
}
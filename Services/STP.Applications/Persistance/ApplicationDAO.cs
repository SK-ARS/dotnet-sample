using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using System;
using System.Collections.Generic;
using System.Globalization;
using STP.Domain.Structures;
using STP.Domain.SecurityAndUsers;
using STP.Domain.Applications;
using STP.Domain.VehiclesAndFleets.Configuration;
using STP.Domain.MovementsAndNotifications.Movements;
using static STP.Common.Enums.ExternalApiEnums;

namespace STP.Applications.Persistance
{
    public static class ApplicationDAO
    {
        #region Get VR1 General
        public static ApplyForVR1 GetVR1General(string userSchema, long revisionId, long versionId, long organisationId, int historic)
        {
            ApplyForVR1 applyForVR1 = new ApplyForVR1();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                applyForVR1,
                userSchema + ".SP_VR1GENERAL_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("p_revision_id ", revisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_version_id ", versionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_organisation_id ", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HISTORIC", historic, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.MyReference = records.GetStringOrDefault("HAULIERS_REF");
                    instance.LoadDescription = records.GetStringOrDefault("LOAD_DESCR");
                    instance.VR1ApplicationStatus = records.GetInt32OrDefault("APPLICATION_STATUS");
                    instance.VR1ProjectStatus = records.GetInt32OrDefault("PROJECT_STATUS");
                    instance.AnalysisId = records.GetLongOrDefault("analysis_id");
                    instance.ESDALReference = records.GetStringOrDefault("ESDAL_REF");
                    instance.SubMovementClass = records.GetInt32OrDefault("VEHICLE_CLASSIFICATION");
                    instance.ClientName = records.GetStringOrDefault("CLIENT_DESCR");
                    instance.FromSummary = records.GetStringOrDefault("FROM_DESCR");
                    instance.ToSummary = records.GetStringOrDefault("TO_DESCR");
                    instance.MovementDateFrom = ConvertDate(records.GetStringOrDefault("MOVEMENT_START_DATE"));
                    instance.MovementDateTo = ConvertDate(records.GetStringOrDefault("MOVEMENT_END_DATE"));
                    instance.ApplicationNotes = records.GetStringOrDefault("APPLICATION_NOTES");
                    instance.NoOfMovements = records.GetInt16Nullable("TOTAL_MOVES");
                    instance.MaxPiecesPerLoad = records.GetInt16Nullable("MAX_PARTS_PER_MOVE");
                    instance.DescriptionWithApplication = records.GetStringOrDefault("application_name");
                    instance.ReducedDetails = records.GetInt16Nullable("REDUCED_DETAILS");
                    instance.VehicleDescription = records.GetStringOrDefault("reduced_vehicle_descr");
                    instance.ApplicationDate = ConvertDate(records.GetStringOrDefault("application_date"));
                    instance.ContactName = records.GetStringOrDefault("ON_BEHALF_OF_CONTACT");
                    instance.HaulierOrgName = records.GetStringOrDefault("ON_BEHALF_OF_NAME");
                    instance.HaulierAddress1 = records.GetStringOrDefault("ON_BEHALF_OF_ADDRESS_1");
                    instance.HaulierAddress2 = records.GetStringOrDefault("ON_BEHALF_OF_ADDRESS_2");
                    instance.HaulierAddress3 = records.GetStringOrDefault("ON_BEHALF_OF_ADDRESS_3");
                    instance.HaulierAddress4 = records.GetStringOrDefault("ON_BEHALF_OF_ADDRESS_4");
                    instance.HaulierAddress5 = records.GetStringOrDefault("ON_BEHALF_OF_ADDRESS_5");
                    instance.CountryId = Convert.ToString(records.GetInt32OrDefault("ON_BEHALF_OF_COUNTRY"));
                    instance.Country = records.GetStringOrDefault("V_ON_BEHALF_OF_COUNTRY");
                    instance.OnBehalfOfPostCode = records.GetStringOrDefault("ON_BEHALF_OF_POST_CODE");
                    instance.HaulierTelephoneNo = records.GetStringOrDefault("ON_BEHALF_OF_TEL_NO");
                    instance.OnBehalfOfEmailId = records.GetStringOrDefault("ON_BEHALF_OF_EMAIL");
                    instance.HaulierFaxNo = records.GetStringOrDefault("ON_BEHALF_OF_FAX_NO");
                    instance.HaulierOperatorLicence = records.GetStringOrDefault("ON_BEHALF_OF_LICENCE_NO");
                    instance.OverallWidth = records.GetDoubleOrDefault("WIDTH_MAX_MTR");
                    instance.OverallLength = records.GetDoubleOrDefault("LEN_MAX_MTR");
                    instance.OverallHeight = records.GetDoubleOrDefault("MAX_HEIGHT_MAX_MTR");
                    instance.GrossWeight = Convert.ToDouble(records.GetInt32OrDefault("GROSS_WEIGHT_MAX_KG"));
                    instance.VR1ContentRefNo = records.GetStringOrDefault("content_ref_no");
                    instance.LatestVersion = (int)records.GetDecimalOrDefault("MAX_STAT");
                    instance.FolderId = records.GetFieldType("FOLDER_ID")!=null && 
                    records.GetFieldType("FOLDER_ID").Name == "Decimal" ? Convert.ToInt64(records.GetDecimalOrDefault("FOLDER_ID"))
                                    : records.GetLongOrDefault("FOLDER_ID"); 
                    instance.ProjectId = records.GetLongOrDefault("PROJECT_ID");
                    instance.VersionId = (int)records.GetLongOrDefault("version_id");
                    instance.VApprDate = records.GetStringOrDefault("V_APPROVAL_DATE");
                    instance.IsNotified = records.GetInt16OrDefault("IS_NOTIFIED");
                    instance.VR1Number = records.GetStringOrDefault("VR1_NUMBER");
                    instance.HaulierContactName = records.GetStringOrDefault("haulier_contact");
                    instance.HaulierApplicantName = records.GetStringOrDefault("haulier_name");
                    instance.HaulierApplicantAddress1 = records.GetStringOrDefault("haulier_address_1");
                    instance.HaulierApplicantAddress2 = records.GetStringOrDefault("haulier_address_2");
                    instance.HaulierApplicantAddress3 = records.GetStringOrDefault("haulier_address_3");
                    instance.HaulierApplicantAddress4 = records.GetStringOrDefault("haulier_address_4");
                    instance.HaulierApplicantAddress5 = records.GetStringOrDefault("haulier_address_5");
                    instance.HaulierPostCode = records.GetStringOrDefault("haulier_post_code");
                    instance.HaulierCountry = records.GetStringOrDefault("haulier_country");
                    instance.HaulierTelephone = records.GetStringOrDefault("haulier_tel_no");
                    instance.HaulierFaxNumber = records.GetStringOrDefault("haulier_fax_no");
                    instance.HaulierEmailId = records.GetStringOrDefault("haulier_email"); 
                });
            return applyForVR1;
        }
        #endregion

        #region private methods
        private static string ConvertDate(string date)
        {
            string newDate = string.Empty;
            if (!string.IsNullOrEmpty(date))
            {
                newDate = date;
            }
            return newDate;
        }
        #endregion

        #region Get SO Application Details
        public static SOApplicationTabs GetSOApplTabDetails(long revisionId, long versionId, string userSchema, int historic)
        {
            SOApplicationTabs ObjSOApplication = new SOApplicationTabs();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                ObjSOApplication,
                userSchema + ".SP_SO_APP_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("p_revision_id ", revisionId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_version_id ", versionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HISTORIC", historic, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.AnalysisId = records.GetLongOrDefault("ANALYSIS_ID");
                    instance.VersionId = records.GetLongOrDefault("VERSION_ID");
                    instance.ApplicationStatus = records.GetInt32OrDefault("APPLICATION_STATUS");
                    instance.ApplicationRevisionId = records.GetLongOrDefault("REVISION_ID");
                    instance.VersionStatus = records.GetInt32OrDefault("VERSION_STATUS");
                    instance.ESDALReference = records.GetStringOrDefault("ESDAL_REF");
                    instance.IsDistributed = (int)records.GetDecimalOrDefault("IS_DISTRIBUTED");
                });
            return ObjSOApplication;
        }
        #endregion

        #region Reset Need Attention
        public static int ResetNeedAttention(long projectID, long revisionID, long versionID)
        {
            int cnt = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                cnt,
                 UserSchema.Portal + ".SP_RESET_NEED_ATTENTION",
                parameter =>
                {
                    parameter.AddWithValue("P_PROJECT_ID", projectID, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REVISION_ID", revisionID, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VERSION_ID", versionID, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        cnt = (int)records.GetDecimalOrDefault("P_ROWCNT");
                    }
                );
            return cnt;
        }
        #endregion

        #region Update Needs Attention
        public static int UpdateNeedsAttention(int notificationID = 0, int revisionID = 0, int naFlag = 0)
        {
            int result = 0;

            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                      UserSchema.Portal + ".STP_NOTIFICATION_ROUTE.SP_UPDATE_NEEDS_ATTENTION",
                    parameter =>
                    {
                        parameter.AddWithValue("NA_NOTIFICATION_ID", notificationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("NA_REVISION_ID", revisionID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("NA_FLAG", naFlag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                    },
                    records =>
                    {
                        result = records.GetInt32("P_AFFECTED_ROWS");
                    }
                );

            return result;
        }
        #endregion

        #region Get Project Folder Details
        public static ProjectFolderModel GetProjectFolderDetails(ProjectFolderModelParams objProjectFolderModelParams)
        {

            ProjectFolderModel objProjectFolderModel = new ProjectFolderModel();

            Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("GetProjectFolderDetails , NotificationId: {0}, ProjectId: {1}, RevisionId: {2}, Flag: {3}, FolderID: {4}", objProjectFolderModelParams.NotificationId, objProjectFolderModelParams.ProjectId, objProjectFolderModelParams.RevisionId, objProjectFolderModelParams.Flag, objProjectFolderModelParams.FolderId));
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
            objProjectFolderModel,
                UserSchema.Portal + ".SP_COMMON_TOGET_FOLDERS",
            parameter =>
            {
                parameter.AddWithValue("P_FLAG_FOLDER", objProjectFolderModelParams.Flag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_FOLDER_ID", objProjectFolderModelParams.FolderId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_PROJ_ID", objProjectFolderModelParams.ProjectId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_HaulierNemonic", objProjectFolderModelParams.HaulierMnemonic, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_ESDALREFNO", objProjectFolderModelParams.ESDALReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_NOTIF_ID", objProjectFolderModelParams.NotificationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_REVISION_ID", objProjectFolderModelParams.RevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
                (records, instance) =>
                {
                    instance.FolderId = records.GetLongOrDefault("FOLDER_ID");
                    instance.FolderName = records.GetStringOrDefault("FOLDER_NAME");
                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("ProjectFolderDetails , FOLDER_ID: {0}, FOLDER_NAME: {1}", instance.FolderId, instance.FolderName));
                }
           );

            return objProjectFolderModel;
        }
        #endregion

        #region Get Project Folder List
        public static List<ProjectFolderModel> GetProjectFolderList(int organisationId)
        {
            List<ProjectFolderModel> objProjectFolderModel = new List<ProjectFolderModel>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
           objProjectFolderModel,
               UserSchema.Portal + ".SP_LIST_PROJECTFOLDER",
           parameter =>
           {
               parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
               parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
           },
               (records, instance) =>
               {
                   instance.FolderId = records.GetLongOrDefault("FOLDER_ID");
                   instance.FolderName = records.GetStringOrDefault("FOLDER_NAME");

               }
          );
            return objProjectFolderModel;
        }

        #endregion

        #region GetStructureGeneralDetails

        public static List<AffStructureGeneralDetails> GetStructureGeneralDetailList(string structureCode, int sectionId)
        {

            List<AffStructureGeneralDetails> objstructures = new List<AffStructureGeneralDetails>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
               objstructures,
                   UserSchema.Portal + ".SP_AFFECTED_STRUCT_DETAILS",
               parameter =>
               {

                   parameter.AddWithValue("P_STRUCT_CODE", structureCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("P_SECTION_ID", sectionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("p_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
                   (records, instance) =>
                   {
                       instance.StructureChainNo = records.GetLongOrDefault("CHAIN_NO");
                       instance.StructurePosition = records.GetInt16OrDefault("POSITION");
                       instance.ESRN = records.GetStringOrDefault("STRUCTURE_CODE");
                       instance.StructureDescription = records.GetStringOrDefault("DESCRIPTION");
                       instance.StructureClass = records.GetStringOrDefault("STRUCTURE_CLASS");
                       instance.OSGR = records.GetLongOrDefault("NORTHING");
                       instance.Easting = records.GetLongOrDefault("EASTING");
                       instance.StructureType = records.GetStringOrDefault("STRUCT_TYPE");
                       instance.StructureType1 = records.GetStringOrDefault("STRUCT_TYPE1");
                       instance.StructureType2 = records.GetStringOrDefault("STRUCT_TYPE2");
                       instance.OwnerName = records.GetStringOrDefault("OWNER");
                       instance.AlternativeName = records.GetStringOrDefault("ALTERNATIVENAME");
                       instance.Notes = records.GetStringOrDefault("NOTES");
                       instance.ContactId = Convert.ToInt64(records["CONTACT_ID"]); //this function will convert the record object to Int64 #7197
                       instance.StructureId = records.GetLongOrDefault("STRUCTURE_ID");
                       instance.StructureName = records.GetStringOrDefault("STRUCTURE_NAME");
                       instance.OrganisationId = records.GetLongOrDefault("ORGANISATION_ID");
                       instance.StructureCategory = records.GetStringOrDefault("STRUCT_CATEGORY");
                       instance.StructureLength = records.GetDoubleOrDefault("STRUCTURE_LENGTH");
                       if (sectionId != 0)
                       {
                           //Structure Imposed Constraints detail
                           instance.SignedAxleGroupLength = records.GetDoubleOrDefault("SIGN_AXLE_GROUP_LEN");
                           instance.SignedAxleGroupWeight = records.GetInt32OrDefault("SIGN_AXLE_GROUP_WEIGHT");
                           instance.SignedDoubleAxleWeight = records.GetInt32OrDefault("SIGN_DOUBLE_AXLE_WEIGHT");
                           instance.SignedGrossWeight = records.GetInt32OrDefault("SIGN_GROSS_WEIGHT");
                           instance.SignedHeightInFeet = records.GetDoubleOrDefault("SIGN_HEIGHT_FEET");
                           instance.SignedHeightInMetres = records.GetDoubleOrDefault("SIGN_HEIGHT_METRES");
                           instance.SignedLengthInFeet = records.GetDoubleOrDefault("SIGN_LEN_FEET");
                           instance.SignedLengthInMetres = records.GetDoubleOrDefault("SIGN_LEN_METRES");
                           instance.SignedSingleAxleWeight = records.GetInt32OrDefault("SIGN_SINGLE_AXLE_WEIGHT");
                           instance.SignedTripleAxleWeight = records.GetInt32OrDefault("SIGN_TRIPLE_AXLE_WEIGHT");
                           instance.SignedWidthInFeet = records.GetDoubleOrDefault("SIGN_WIDTH_FEET");
                           instance.SignedWidthInMetres = records.GetDoubleOrDefault("SIGN_WIDTH_METRES");
                       }

                   }
              );
            return objstructures;
        }


        #endregion

        #region GetHAContactDetails
        public static HAContact GetHAContactDetails(decimal ContactId)
        {

            HAContact hAContactObj = new HAContact();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                hAContactObj,
                    UserSchema.Portal + ".SP_R_CONTACT_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("P_CONTACT_ID", ContactId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.ContactName = records.GetStringOrDefault("CONTACT_NAME");
                        instance.HAAddress1 = records.GetStringOrDefault("ADDRESSLINE_1");
                        instance.HAAddress2 = records.GetStringOrDefault("ADDRESSLINE_2");
                        instance.HAAddress3 = records.GetStringOrDefault("ADDRESSLINE_3");
                        instance.HAAddress4 = records.GetStringOrDefault("ADDRESSLINE_4");
                        instance.HAAddress5 = records.GetStringOrDefault("ADDRESSLINE_5");
                        instance.PostCode = records.GetStringOrDefault("POSTCODE");
                        instance.Country = records.GetStringOrDefault("COUNTRY");
                        instance.Telephone = records.GetStringOrDefault("PHONENUMBER");
                        instance.Fax = records.GetStringOrDefault("FAX");
                        instance.Email = records.GetStringOrDefault("EMAIL");
                        instance.OrganisationName = records.GetStringOrDefault("OrgName");
                        instance.Title = records.GetStringOrDefault("title");
                    }
            );
            return hAContactObj;
        }
        #endregion

        #region UpdatePartId
        public static int UpdatePartId(UpdatePartIdInputParams updatePartIdInputParams)
        {
            int result = 0;
            int? routepartid = null;
            int? applpartid = null;

            if (updatePartIdInputParams.Notif)
            {
                routepartid = updatePartIdInputParams.PartId;
            }
            if (!updatePartIdInputParams.VR1Appl && !updatePartIdInputParams.Iscand)
            {
                applpartid = updatePartIdInputParams.PartId;
            }
            else
            {
                routepartid = updatePartIdInputParams.PartId;
            }
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                 updatePartIdInputParams.userSchema + ".SP_UPDATE_PARTID",
                parameter =>
                {
                    parameter.AddWithValue("p_VEHICLE_ID", updatePartIdInputParams.VehicleId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_PART_ID", applpartid, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ROUTEPART_ID", routepartid, OracleDbType.Int32, ParameterDirectionWrap.Input, 99999);
                    parameter.AddWithValue("p_ROUTETYPE", updatePartIdInputParams.RType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     result = (int)record.GetDecimalOrDefault("STATUS_1");
                 }
            );

            return result;
        }
        #endregion

        #region viewAffStructureSections
        public static List<AffStructureSectionList> ViewAffStructureSections(string structureCode)
        {
            List<AffStructureSectionList> structureSectionsObj = new List<AffStructureSectionList>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
            structureSectionsObj,
           UserSchema.Portal + ".SP_AFFECTED_STRUCT_SECTION",
            parameter =>
            {
                parameter.AddWithValue("P_STRUCT_CODE", structureCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
                (records, instance) =>
                {
                    instance.StructureId = records.GetLongOrDefault("STRUCTURE_ID");
                    instance.SectionId = records.GetLongOrDefault("SECTION_ID");
                    instance.StructureSections = records.GetStringOrDefault("STRUCTURE_SECTION");
                }
             );
            return structureSectionsObj;
        }
        #endregion

        #region UpdateVR1Application
        public static bool UpdateVR1Application(ApplyForVR1 vr1application, int organisationId, int userId, long apprevid)
        {
            bool result = false;
            long? folderId = null;
            if (vr1application.FolderId != 0)
            {
                folderId = vr1application.FolderId;
            }
            try
            {

                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    result,
                   UserSchema.Portal + ".SP_EDIT_APPLICATIONS",
                    parameter =>
                    {
                        parameter.AddWithValue("A_VEHICLE_CLASSIFICATION", vr1application.SubMovementClass, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("HAUL_ORG", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("REV_ID", vr1application.ApplicationRevisionId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("L_DESC ", vr1application.LoadDescription, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                        parameter.AddWithValue("F_DESCR", vr1application.FromSummary, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("T_DESCR ", vr1application.ToSummary, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("CLIENT", vr1application.ClientName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("DESCR", vr1application.DescriptionWithApplication, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                        parameter.AddWithValue("MOV_START", vr1application.MovementDateFrom, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("MOV_END", vr1application.MovementDateTo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("NOTES", vr1application.ApplicationNotes, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                        parameter.AddWithValue("NO_OF_MOV", vr1application.NoOfMovements, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("No_OF_PIECES", vr1application.MaxPiecesPerLoad, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_REF", vr1application.MyReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("USERID", userId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("NOTES_ON_ESCORT", vr1application.ApplicationNotes, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                        parameter.AddWithValue("VEHICLE_DESC", vr1application.VehicleDescription, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                        parameter.AddWithValue("ONBEHALF_CONT", vr1application.ContactName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("ONBEHALF_NAME", vr1application.HaulierOrgName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("ONBEHALF_ADD_1", vr1application.HaulierAddress1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("ONBEHALF_ADD_2", vr1application.HaulierAddress2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("ONBEHALF_ADD_3", vr1application.HaulierAddress3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("ONBEHALF_ADD_4", vr1application.HaulierAddress4, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("ONBEHALF_ADD_5", vr1application.HaulierAddress5, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("ONBEHALF_POST", vr1application.OnBehalfOfPostCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("ONBEHALF_COUNTRY", vr1application.CountryId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("ONBEHALF_TEL", vr1application.HaulierTelephoneNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("ONBEHALF_EMAIL", vr1application.OnBehalfOfEmailId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("ONBEHALF_FAX_NO", vr1application.HaulierFaxNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("ONBEHALF_LICENCE_NO", vr1application.HaulierOperatorLicence, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                        parameter.AddWithValue("OverallLength", vr1application.OverallLength, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("GrossWeight", vr1application.GrossWeight, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("OverallHeight", vr1application.OverallHeight, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("OverallWidth", vr1application.OverallWidth, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_FOLDER_ID", folderId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);

                    },
                     record =>
                     {
                         if (record.GetDecimalOrDefault("PROJ_COUNT") == 0)
                             result = false;
                         else
                             result = true;

                     }
                );
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"ApplicationDAO/UpdateVR1Application , Exception: ", ex);
                throw;
            }
            return result;
        }
        #endregion

        #region SaveVR1Application
        public static ApplyForVR1 SaveVR1Application(ApplyForVR1 vr1application, int organisationId, int userId)
        {
            try
            {
                long? folderId = null;
                if (vr1application.FolderId != 0)
                {
                    folderId = vr1application.FolderId;
                }
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    vr1application,
                   UserSchema.Portal + ".SP_INSERT_APPLICATIONS",
                    parameter =>
                    {
                        parameter.AddWithValue("HAUL_ORG", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("L_DESC ", vr1application.LoadDescription, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("F_DESCR", vr1application.FromSummary, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("T_DESCR ", vr1application.ToSummary, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("CLIENT", vr1application.ClientName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                        parameter.AddWithValue("DESCR", vr1application.DescriptionWithApplication, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("MOV_START", vr1application.MovementDateFrom, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("MOV_END", vr1application.MovementDateTo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("NOTES", vr1application.ApplicationNotes, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                        parameter.AddWithValue("NO_OF_MOV", vr1application.NoOfMovements, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("No_OF_PIECES", vr1application.MaxPiecesPerLoad, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_REF", vr1application.MyReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                        parameter.AddWithValue("USERID", userId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("NOTES_ON_ESCORT", vr1application.ApplicationNotes, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                        parameter.AddWithValue("AGENTNAME", vr1application.ContactName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                        parameter.AddWithValue("VEHICLE_CLASSIFICATION", vr1application.SubMovementClass, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("VEHICLE_DESC", vr1application.VehicleDescription, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("REDUCED_DETAILS", vr1application.ReducedDetails, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);

                        parameter.AddWithValue("ONBEHALF_CONT", vr1application.ContactName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("ONBEHALF_NAME", vr1application.HaulierOrgName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("ONBEHALF_ADD_1", vr1application.HaulierAddress1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("ONBEHALF_ADD_2", vr1application.HaulierAddress2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("ONBEHALF_ADD_3", vr1application.HaulierAddress3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("ONBEHALF_ADD_4", vr1application.HaulierAddress4, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("ONBEHALF_ADD_5", vr1application.HaulierAddress5, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("ONBEHALF_POST", vr1application.OnBehalfOfPostCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("ONBEHALF_COUNTRY", vr1application.CountryId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("ONBEHALF_TEL", vr1application.HaulierTelephoneNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("ONBEHALF_EMAIL", vr1application.OnBehalfOfEmailId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("ONBEHALF_FAX_NO", vr1application.HaulierFaxNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("ONBEHALF_LICENCE_NO", vr1application.HaulierOperatorLicence, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);


                        parameter.AddWithValue("OverallWidth", vr1application.OverallWidth, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("OverallLength", vr1application.OverallLength, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("OverallHeight", vr1application.OverallHeight, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("GrossWeight", vr1application.GrossWeight, OracleDbType.Decimal, ParameterDirectionWrap.Input, 32767);

                        parameter.AddWithValue("P_Folder_ID", folderId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                     record =>
                     {

                         vr1application.ApplicationRevisionId = record.GetLongOrDefault("REVISION_ID");
                         vr1application.AnalysisId = (int)record.GetDecimalOrDefault("ANALYSIS_ID");
                         vr1application.VR1ContentRefNo = record.GetStringOrDefault("CONTENT_REF_NO");
                         vr1application.VersionId = (int)record.GetDecimalOrDefault("VERSION_ID");

                     }
                );
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @"ApplicationDAO/SaveVR1Application , Exception: ", ex);
                throw;
            }
            return vr1application;
        }
        #endregion

        #region SaveAppGeneral
        public static bool SaveAppGeneral(SOApplication soApplication, int organisationId, int userId, string userSchema, long applicationRevId = 0)
        {
            DateTime? appDate = null;
            DateTime? appDueDate = null;
            string procedure = ".STP_MOVEMENT.SP_SAVE_APP_GENERAL";

            DateTime fromDate = DateTime.ParseExact(soApplication.MovementDateFrom, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(soApplication.MovementDateTo, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
            if (soApplication.ApplicationDate != null)
            {
                appDate = DateTime.ParseExact(soApplication.ApplicationDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                appDueDate = DateTime.ParseExact(soApplication.ApplicationDueDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
            }
            bool result = false;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
               userSchema + procedure,
                parameter =>
                {
                    parameter.AddWithValue("HAUL_ORG", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("REV_ID", applicationRevId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("L_DESC ", soApplication.Load, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                    parameter.AddWithValue("F_DESCR", soApplication.FromAddress, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("T_DESCR ", soApplication.ToAddress, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("CLIENT", soApplication.Description, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("DESCR", soApplication.HaulierDescription, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                    parameter.AddWithValue("MOV_START", fromDate, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("MOV_END", toDate, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("NOTES", soApplication.ApplicationNotesFromHA, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                    parameter.AddWithValue("NO_OF_MOV", soApplication.NumberOfMovements, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("No_OF_PIECES", soApplication.NumberofPieces, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_REF", soApplication.HaulierReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("USERID", userId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("NOTES_ON_ESCORT", soApplication.NotesOnEscort, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                    parameter.AddWithValue("AGENTNAME", soApplication.AgentName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_HAULIER_CONT", soApplication.OnBehalOfContactName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_NAME", soApplication.OnBehalOfHaulierOrgName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_ADD_1", soApplication.OnBehalOfHaulierAddress1, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_ADD_2", soApplication.OnBehalOfHaulierAddress2, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_ADD_3", soApplication.OnBehalOfHaulierAddress3, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_ADD_4", soApplication.OnBehalOfHaulierAddress4, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_ADD_5", soApplication.OnBehalOfHaulierAddress5, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_POST", soApplication.OnBehalOfHaulPostCode, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_COUNTRY", soApplication.OnBehalOfCountryID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_TEL", soApplication.OnBehalOfHaulTelephoneNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_EMAIL", soApplication.OnBehalOfHaulEmailID, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_FAX_NO", soApplication.OnBehalOfHaulFaxNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAULIER_LICENCE_NO", soApplication.OnBehalOfHaulOperatorLicens, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    if (userSchema == UserSchema.Sort)
                    {
                        if(appDate.HasValue)
                        { 
                            parameter.AddWithValue("APP_START", TimeZoneInfo.ConvertTimeToUtc(appDate.Value), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767); 
                        }
                        else
                        {
                            parameter.AddWithValue("APP_START", appDate.Value, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                        }

                        if (appDueDate.HasValue)
                        {
                            parameter.AddWithValue("APP_DUE", TimeZoneInfo.ConvertTimeToUtc(appDueDate.Value), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                        }
                        else
                        {
                            parameter.AddWithValue("APP_DUE", appDueDate, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                        }
                        parameter.AddWithValue("HAUL_CONT_NAME", soApplication.OrgHaulierContactName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("ORG_EMAIL", soApplication.OrgEmailId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("ORG_FAX", soApplication.OrgFax, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    }
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     if (record.GetDecimalOrDefault("PROJ_COUNT") == 0)
                         result = false;
                     else
                         result = true;
                 }
            );


            return result;
        }
        #endregion

        #region SaveSOApplication
        public static long SaveSOApplication(SOApplication soApplication, int organisationId, int userId)
        {
            DateTime fromDate = Convert.ToDateTime(soApplication.MovementDateFrom);
            DateTime toDate = Convert.ToDateTime(soApplication.MovementDateTo);

            long revId = 0;
            long? folderId = null;

            if (soApplication.FolderId != 0)
            {
                folderId = soApplication.FolderId;
            }

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                revId,
               UserSchema.Portal + ".SP_INSERT_SPEC_ORDER",
                parameter =>
                {
                    parameter.AddWithValue("HAUL_ORG", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("L_DESC ", soApplication.Load, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("F_DESCR", soApplication.FromAddress, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("T_DESCR ", soApplication.ToAddress, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("CLIENT", soApplication.Description, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("DESCR", soApplication.HaulierDescription, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("MOV_START", TimeZoneInfo.ConvertTimeToUtc(fromDate), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("MOV_END", TimeZoneInfo.ConvertTimeToUtc(toDate), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("NOTES", soApplication.ApplicationNotesFromHA, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                    parameter.AddWithValue("NO_OF_MOV", soApplication.NumberOfMovements, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("No_OF_PIECES", soApplication.NumberofPieces, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REF", soApplication.HaulierReference, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("USERID", userId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("NOTES_ON_ESCORT", soApplication.NotesOnEscort, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 99999);
                    parameter.AddWithValue("AGENTNAME", soApplication.AgentName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("FolderID", folderId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     revId = record.GetLongOrDefault("REVISION_ID");
                 }
            );


            return revId;
        }

        #endregion

        #region GetSOGeneralWorkInProcessByRevisionId
        public static SOApplication GetSOGeneralWorkInProcessByRevisionId(string userSchema = UserSchema.Portal, long revisionId = 0, long versionId = 0, int orgId = 0)
        {
            SOApplication soApplication = new SOApplication();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                soApplication,
                    userSchema + ".SP_SO_GENERAL_APP_REVISIONID",
                parameter =>
                {
                    parameter.AddWithValue("p_revision_id", revisionId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_version_id", versionId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_org_id", orgId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.HaulierReference = records.GetStringOrDefault("HAULIERS_REF");
                    instance.HaulierName = records.GetStringOrDefault("HAULIER_NAME");
                    instance.AnalysisId = records.GetLongOrDefault("analysis_id");
                    instance.Description = records.GetStringOrDefault("CLIENT_DESCR");
                    instance.AgentName = records.GetStringOrDefault("AGENT_NAME");
                    instance.Load = records.GetStringOrDefault("LOAD_DESCR");
                    instance.FromAddress = records.GetStringOrDefault("FROM_DESCR");
                    instance.ToAddress = records.GetStringOrDefault("TO_DESCR");
                    instance.ApplicationStatus = records.GetInt32OrDefault("APPLICATION_STATUS");
                    instance.VehicleClassification = records.GetInt32OrDefault("VEHICLE_CLASSIFICATION");
                    instance.MovementDateFrom = records.GetStringOrDefault("MOVEMENT_START_DATE");
                    instance.MovementDateTo = records.GetStringOrDefault("MOVEMENT_END_DATE");
                    instance.NumberOfMovements = records.GetInt16Nullable("TOTAL_MOVES");
                    instance.NumberofPieces = records.GetInt16Nullable("MAX_PARTS_PER_MOVE");
                    instance.ESDALReference = records.GetStringOrDefault("ESDAL_REF");
                    instance.HaulierDescription = records.GetStringOrDefault("APPLICATION_NAME");
                    instance.NotesOnEscort = records.GetStringOrDefault("notesonescort");
                    instance.ApplicationNotesFromHA = records.GetStringOrDefault("APPLICATION_NOTES");
                    instance.ApplicationRevId = records.GetLongOrDefault("REVISION_ID");
                    instance.HaulierFaxNo = records.GetStringOrDefault("HAULIER_FAX_NO");
                    instance.HaulierEmailId = records.GetStringOrDefault("HAULIER_EMAIL");
                    instance.ApplicationDate = records.GetStringOrDefault("APP_DATE");
                    instance.ApplicationDueDate = records.GetStringOrDefault("APP_DUE_DATE");
                    instance.OrgId = (int)records.GetLongOrDefault("ORGANISATION_ID");
                    instance.ProjectStatus = records.GetStringOrDefault("PROJECT_STATUS_NAME");
                    instance.MovementStatus = records.GetStringOrDefault("MVERSION_STATUS");
                    instance.MovementName = records.GetStringOrDefault("FROM_DESCR") + " to " + records.GetStringOrDefault("TO_DESCR");
                    instance.ProjectId = records.GetLongOrDefault("PROJECT_ID");
                    if (userSchema == UserSchema.Sort && versionId != 0)
                        instance.MovementName = records.GetStringOrDefault("PROJECT_NAME");
                    string date = records.GetStringOrDefault("Mov_AGREED_DATE");
                    instance.MovementAgreedDate = date == "" ? null : date;
                    if (userSchema == UserSchema.Portal && versionId == 0)
                    {
                        instance.HAReference = records.GetStringOrDefault("HA_JOB_FILE_REF");
                        instance.FolderId = Convert.ToInt32(records.GetDecimalOrDefault("FOLDER_ID"));
                    }
                    if (versionId != 0)
                    {
                        instance.MovementFromDescription = records.GetStringOrDefault("MFROM_DESCR");
                        instance.MovementToDescription = records.GetStringOrDefault("MTO_DESCR");
                        instance.MovementLoadDescription = records.GetStringOrDefault("MLOAD_DESCR");
                        instance.VersionNo = records.GetInt16OrDefault("VERSION_NO");
                    }

                    //VR1
                    instance.OnBehalOfContactName = records.GetStringOrDefault("ON_BEHALF_OF_CONTACT");
                    instance.OnBehalOfHaulierOrgName = records.GetStringOrDefault("ON_BEHALF_OF_NAME");
                    instance.OnBehalOfHaulierAddress1 = records.GetStringOrDefault("ON_BEHALF_OF_ADDRESS_1");
                    instance.OnBehalOfHaulierAddress2 = records.GetStringOrDefault("ON_BEHALF_OF_ADDRESS_2");
                    instance.OnBehalOfHaulierAddress3 = records.GetStringOrDefault("ON_BEHALF_OF_ADDRESS_3");
                    instance.OnBehalOfHaulierAddress4 = records.GetStringOrDefault("ON_BEHALF_OF_ADDRESS_4");
                    instance.OnBehalOfHaulierAddress5 = records.GetStringOrDefault("ON_BEHALF_OF_ADDRESS_5");
                    instance.OnBehalOfHaulPostCode = records.GetStringOrDefault("ON_BEHALF_OF_POST_CODE");
                    instance.OnBehalOfCountryID = records.GetInt32OrDefault("ON_BEHALF_OF_COUNTRY");
                    instance.OnBehalOfHaulTelephoneNo = records.GetStringOrDefault("ON_BEHALF_OF_TEL_NO");
                    instance.OnBehalOfHaulFaxNo = records.GetStringOrDefault("ON_BEHALF_OF_FAX_NO");
                    instance.OnBehalOfHaulEmailID = records.GetStringOrDefault("ON_BEHALF_OF_EMAIL");
                    instance.OnBehalOfHaulOperatorLicens = records.GetStringOrDefault("ON_BEHALF_OF_LICENCE_NO");
                });
            return soApplication;
        }
        #endregion

        #region GetSOGeneralDetails
        public static SOApplication GetSOGeneralDetails(long revisionId, long versionId, string userSchema, int historic)
        {
            SOApplication soApplication = new SOApplication();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                soApplication,
                userSchema + ".SP_R_REV_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("p_revision_id ", revisionId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_version_id ", versionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HISTORIC", historic, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.ProjectId = records.GetLongOrDefault("PROJECT_ID");
                    instance.AnalysisId = records.GetLongOrDefault("ANALYSIS_ID");
                    instance.VersionId = records.GetLongOrDefault("VERSION_ID");
                    instance.VersionNo = records.GetShortOrDefault("VERSION_NO");
                    instance.Load = records.GetStringOrDefault("LOAD_DESCR");
                    instance.ApplicationStatus = records.GetInt32OrDefault("APPLICATION_STATUS");
                    instance.VehicleClassification = records.GetInt32OrDefault("VEHICLE_CLASSIFICATION");
                    instance.VersionStatus = records.GetInt32OrDefault("VERSION_STATUS");
                    instance.Description = records.GetStringOrDefault("MOVEMENT_LOAD");
                    instance.FromAddress = records.GetStringOrDefault("FROM_DESCR");
                    instance.ToAddress = records.GetStringOrDefault("TO_DESCR");
                    instance.ESDALReference = records.GetStringOrDefault("ESDAL_REF");
                    instance.ESDALReferenceSORT = records.GetStringOrDefault("ESDAL_REF_SORT");
                    instance.ApplicationNotesFromHA = records.GetStringOrDefault("APPLICATION_NOTES");
                    instance.AgentName = records.GetStringOrDefault("AGENT_NAME");
                    instance.NumberOfMovements = records.GetInt16Nullable("TOTAL_MOVES"); 
                    instance.NumberofPieces = records.GetInt16Nullable("MAX_PARTS_PER_MOVE");
                    instance.ApplicationNotesToHA = records.GetByteArrayOrNull("NOTES_TO");
                    instance.HaulierReference = records.GetStringOrDefault("HAULIERS_REF");
                    instance.HAContactId = records.GetDecimalOrDefault("CONTACT_ID");
                    instance.HAContactName = records.GetStringOrDefault("HAULIER_CONTACT");
                    instance.Haulier = records.GetStringOrDefault("HAULIER_NAME");
                    instance.HaulierContactAddress1 = records.GetStringOrDefault("HAULIER_ADDRESS_1");
                    instance.HaulierContactAddress2 = records.GetStringOrDefault("HAULIER_ADDRESS_2");
                    instance.HaulierContactAddress3 = records.GetStringOrDefault("HAULIER_ADDRESS_3");
                    instance.HaulierContactAddress4 = records.GetStringOrDefault("HAULIER_ADDRESS_4");
                    instance.HaulierContactAddress5 = records.GetStringOrDefault("HAULIER_ADDRESS_5");
                    instance.PostCode = records.GetStringOrDefault("HAULIER_POST_CODE");
                    instance.Country = records.GetStringOrDefault("HAULIER_COUNTRY");
                    instance.Telephone = records.GetStringOrDefault("HAULIER_TEL_NO");
                    instance.HaulierFaxNo = records.GetStringOrDefault("HAULIER_FAX_NO");
                    instance.HaulierEmailId = records.GetStringOrDefault("HAULIER_EMAIL");
                    instance.OperatorLicence = records.GetStringOrDefault("HAULIER_LICENCE_NO"); //cross check
                    instance.HAReference = records.GetStringOrDefault("HA_JOB_FILE_REF");
                    instance.HaulierDescription = records.GetStringOrDefault("APPLICATION_NAME");
                    instance.ApplicationRevId = records.GetLongOrDefault("REVISION_ID");
                    instance.MovementDateFrom = records.GetStringOrDefault("MOVEMENT_START_DATE");
                    instance.MovementDateTo = records.GetStringOrDefault("MOVEMENT_END_DATE");
                    instance.ProjectStatus = records.GetStringOrDefault("Proj_Status");
                    instance.CheckingStatus = records.GetStringOrDefault("Checking_Status");
                    instance.ApplicationDate = records.GetStringOrDefault("APPLICATION_DATE");
                    instance.HaulierName = records.GetStringOrDefault("haulier_name");
                    instance.CheckingStatusCode = records.GetInt32OrDefault("cstatus");
                    instance.CheckerUserId = records.GetLongOrDefault("CUSERID");
                    instance.IsNotified = records.GetShortOrDefault("IS_NOTIFIED");
                    instance.MovementName = records.GetStringOrDefault("FROM_DESCR") + " to " + records.GetStringOrDefault("TO_DESCR");
                    instance.HAJobFileReference = records.GetStringOrDefault("HA_JOB_FILE_REF");
                    string date = records.GetStringOrDefault("APP_DUE_DATE");
                    instance.PlannerUserId = records.GetLongOrDefault("PLANNER_USERID");
                    instance.ApplicationStatusCode = records.GetInt32OrDefault("PROJECT_STATUS");
                    instance.LastCandidateRouteId = (long)records.GetDecimalOrDefault("MAX_ROUTE_ID");
                    instance.RouteAnalysisId = (long)records.GetDecimalOrDefault("ranalysis_id");
                    instance.LastRevisionId = (long)records.GetDecimalOrDefault("max_revision_id");
                    instance.LastRevisionNo = (int)records.GetDecimalOrDefault("max_revision_no");
                    instance.LastSpecialOrderNo = records.GetStringOrDefault("SORDER_NO");
                    instance.VR1Number = records.GetStringOrDefault("VR1NO");
                    instance.SONumber = records.GetStringOrDefault("SO_NUMBER");
                    instance.EnteredBySORT = records.GetInt16OrDefault("ENTERED_BY_SORT");
                    if (userSchema.ToUpper() == UserSchema.Portal.ToUpper())
                    {
                        instance.MovementFromDescription = records.GetStringOrDefault("MOVEMENT_FROMDESCR");
                        instance.MovementToDescription = records.GetStringOrDefault("MOVEMENT_TODESCR");
                    }
                    else
                    {
                        instance.PreviousVersionDistributed = records.GetDecimalOrDefault("PREV_VERSDISTRTD");
                        instance.DistributedMovAnalysisId = (long)records.GetDecimalOrDefault("DITR_MOV_ANALYID");
                    }
                    if (date == "")
                        instance.ApplicationDueDate = null;
                    else
                        instance.ApplicationDueDate = date;
                    instance.IsMovDistributed = Convert.ToInt16(records["IsMovDistrbted"]);
                });
            return soApplication;
        }
        #endregion

        #region GetSONumberStatus
        public static string GetSONumberStatus(int project_id, string userSchema = UserSchema.Portal)
        {
            string soNumber = string.Empty;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                soNumber,
                userSchema + ".STP_MOVEMENT_TRANS_DISTR.SP_GET_SONUMBER_STATUS",
                parameter =>
                {
                    parameter.AddWithValue("P_PROJECT_ID", project_id, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    soNumber = record.GetStringOrDefault("V_ORDERNO");
                }
                );
            return soNumber;
        }
        #endregion

        #region VR1SupplementaryInfo
        public static bool VR1SupplementaryInfo(SupplimentaryInfoParams objSupplimentaryInfoParams)
        {
            bool result = false;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    result,
                  objSupplimentaryInfoParams.UserSchema + ".SP_SUPPLEMENTARYINFOSAVEEDIT",
                    parameter =>
                    {
                        parameter.AddWithValue("P_REVISION_ID", objSupplimentaryInfoParams.ApplicationRevisionId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_DISTANCE_BY_ROAD", objSupplimentaryInfoParams.SupplimentaryInfo.TotalDistanceOfRoad, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_VALUE_OF_LOAD", objSupplimentaryInfoParams.SupplimentaryInfo.ApprValueOfLoad, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_SIMILAR_MOVEMENT", objSupplimentaryInfoParams.SupplimentaryInfo.DateOfAuthority, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_LOAD_DIVISIBLE", objSupplimentaryInfoParams.SupplimentaryInfo.LoadDivision, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                        parameter.AddWithValue("P_COST_OF_ROAD_MOVE", objSupplimentaryInfoParams.SupplimentaryInfo.ApprCostOfMovement, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_COST_OF_DIVISION", objSupplimentaryInfoParams.SupplimentaryInfo.AdditionalCost, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RISK_OF_DIVISION", objSupplimentaryInfoParams.SupplimentaryInfo.RiskNature, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_OTHER_CONSIDERATIONS", objSupplimentaryInfoParams.SupplimentaryInfo.AdditionalConsideration, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_IS_FINAL_ADDRESS", objSupplimentaryInfoParams.SupplimentaryInfo.Address, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);

                        parameter.AddWithValue("P_FURTHER_MOVES", objSupplimentaryInfoParams.SupplimentaryInfo.ProposedMoveDetails, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_SEA_QUOTATION", objSupplimentaryInfoParams.SupplimentaryInfo.SeaQuotation, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_BETWEEN_WHICH_PORTS", objSupplimentaryInfoParams.SupplimentaryInfo.PortNames, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_SEA_CONSIDERED", objSupplimentaryInfoParams.SupplimentaryInfo.Shipment, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                     record =>
                     {

                     }
                );
            result = true;
            return result;
        }
        #endregion

        #region Delete Application
        public static int DeleteApplication(long apprevisionID, string userSchema)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                   userSchema + ".SP_DELETE_APPLICATION",
                    parameter =>
                    {
                        parameter.AddWithValue("P_REVISION_ID", apprevisionID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                    },
                     record =>
                     {
                         result = Convert.ToInt32("p_AFFECTED_ROWS");
                     }
                );
            return result;
        }
        #endregion

        #region CheckLatestAppStatus
        public static SOApplication CheckLatestAppStatus(long projectId)
        {
            SOApplication sOApplicationObj = new SOApplication();
            try
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                sOApplicationObj,
               UserSchema.Portal + ".SP_CHK_LAT_APP_STATUS",
                parameter =>
                {
                    parameter.AddWithValue("P_PROJECT_ID", projectId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 (records, instance) =>
                 {
                     instance.ApplicationStatus = records.GetInt32OrDefault("APPLICATION_STATUS");
                     instance.ApplicationRevId = records.GetLongOrDefault("REVISION_ID");
                 }
            );
            }
            catch (Exception ex)
            {
                sOApplicationObj.ApplicationRevId = 0;
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Application/CheckLatestAppStatus, Exception: " + ex​​​​);
            }
            return sOApplicationObj;

        }
        #endregion

        #region WithdrawApplication
        public static ApplicationWithdraw WithdrawApplication(long projectId, long appRevId)
        {
            ApplicationWithdraw withDrawApp = new ApplicationWithdraw();
            try
            {
                decimal res = 0;
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    withDrawApp,
                   UserSchema.Portal + ".SP_WITHDRAW_APPLICATION",
                    parameter =>
                    {
                        parameter.AddWithValue("P_PROJECT_ID", projectId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_APP_REV_ID", appRevId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                     (records, instance) =>
                     {
                         res = Convert.ToInt32(records.GetDecimalOrDefault("CNT"));
                         instance.ProjectStatus = records.GetStringOrDefault("PROJECT_STATUS");
                         instance.CheckingStatus = records.GetStringOrDefault("CHECKING_STATUS");
                         if (res == 1)
                             instance.Result = true;
                         else
                             instance.Result = false;
                     }
                );
            }
            catch (Exception ex)
            {
                withDrawApp.Result = false;
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Application/WithdrawApplication, Exception: " + ex​​​​);
            }
            return withDrawApp;
        }
        #endregion

        #region Revise SO Application
        public static SOApplication ReviseSOApplication(long apprevId, string userSchema)
        {
            SOApplication sOApplication = new SOApplication();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                sOApplication,
                userSchema + ".SP_APPL_REVISE",
                parameter =>
                {
                    parameter.AddWithValue("P_REV_ID", apprevId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    sOApplication.ApplicationRevId = record.GetLongOrDefault("REVISION_ID");
                    sOApplication.VehicleClassification = record.GetInt32OrDefault("VEHICLE_CLASSIFICATION");
                    sOApplication.MovementId = (long)record.GetDecimalOrDefault("MOVEMENT_ID");
                    sOApplication.LastRevisionNo = record.GetInt16OrDefault("revision_no");
                    sOApplication.ProjectId= record.GetLongOrDefault("project_id");
                    //var a = record.GetFieldType("VERSION_NO");
                    //sOApplication.VersionNo = record.GetInt16OrDefault("VERSION_NO");
                }
            );

            return sOApplication;
        }
        #endregion

        #region Clone SO Application
        public static SOApplication CloneSOApplication(long apprevId, int organisationId, int userid)
        {
            SOApplication sOApplication = new SOApplication();
            try
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    sOApplication,
                   UserSchema.Portal + ".SP_APPL_REVISE_CLONE",
                    parameter =>
                    {
                        parameter.AddWithValue("P_REV_ID", apprevId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_USER_ID", userid, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    record =>
                    {
                        sOApplication.ApplicationRevId = record.GetLongOrDefault("REVISION_ID");
                        sOApplication.VehicleClassification = record.GetInt32OrDefault("VEHICLE_CLASSIFICATION");
                        sOApplication.MovementId = (long)record.GetDecimalOrDefault("MOVEMENT_ID");
                        sOApplication.LastRevisionNo = record.GetInt16OrDefault("REVISION_NO");
                        sOApplication.ProjectId = record.GetLongOrDefault("PROJECT_ID");
                    }
                );
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Application/CloneSOApplication, Exception: " + ex​​​​);
            }
            return sOApplication;
        }
        #endregion

        #region Clone SO History Application
        public static SOApplication CloneSOHistoryApplication(long apprevId, int organisationId, int userid, string userSchema)
        {
            SOApplication sOApplication = new SOApplication();
            try
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    sOApplication,
                   userSchema + ".STP_HISTORIC_MOVEMENT.SP_CLONE_SO_APPL",
                    parameter =>
                    {
                        parameter.AddWithValue("P_REVISION_ID", apprevId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_ORG_ID", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_USER_ID", userid, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    record =>
                    {
                        sOApplication.ApplicationRevId = record.GetLongOrDefault("REVISION_ID");
                        sOApplication.VehicleClassification = record.GetInt32OrDefault("VEHICLE_CLASSIFICATION");
                        sOApplication.MovementId = (long)record.GetDecimalOrDefault("MOVEMENT_ID");
                        sOApplication.LastRevisionNo = record.GetInt16OrDefault("REVISION_NO");
                        sOApplication.ProjectId = record.GetLongOrDefault("PROJECT_ID");

                    }
                );
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Application/CloneSOHistoryApplication, Exception: " + ex​​​​);
            }
            return sOApplication;
        }
        #endregion

        #region Revise VR1 Application
        public static ApplyForVR1 ReviseVR1Application(long apprevId, int reducedDet, int cloneApp, int versionId, string userSchema)
        {
            ApplyForVR1 obj = new ApplyForVR1();
            
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    obj,
                    userSchema + ".SP_REVISE_VR1APPL",
                    parameter =>
                    {
                        parameter.AddWithValue("P_REV_ID", apprevId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_REDUCED_DET", reducedDet, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_CLONE_APP", cloneApp, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_VERSION_ID", versionId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                    record =>
                    {
                        obj.ApplicationRevisionId = record.GetLongOrDefault("REVISION_ID");
                        obj.VersionId = (int)record.GetDecimalOrDefault("VERSION_ID");
                        obj.SubMovementClass = record.GetInt32OrDefault("VEHICLE_CLASSIFICATION");
                        obj.MovementId = (long)record.GetDecimalOrDefault("MOVEMENT_ID");
                        obj.ProjectId= record.GetLongOrDefault("PROJECT_ID");
                        obj.RevisionNumber= record.GetInt16OrDefault("REVISION_NO");
                    }
                );
             
            return obj;
        }
        #endregion

        #region clone historic VR1 Application
        public static ApplyForVR1 CloneHistoryVR1Application(long apprevId, int reducedDet, int cloneApp, int versionId, string userSchema)
        {
            ApplyForVR1 obj = new ApplyForVR1();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                obj,
                userSchema + ".STP_HISTORIC_MOVEMENT.SP_CLONE_VR1_APPL",
                parameter =>
                {
                    parameter.AddWithValue("P_REV_ID", apprevId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REDUCED_DET", reducedDet, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CLONE_APP", cloneApp, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VERSION_ID", versionId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    obj.ApplicationRevisionId = record.GetLongOrDefault("REVISION_ID");
                    obj.VersionId = (int)record.GetDecimalOrDefault("VERSION_ID");
                    obj.SubMovementClass = record.GetInt32OrDefault("VEHICLE_CLASSIFICATION");
                    obj.MovementId = (long)record.GetDecimalOrDefault("MOVEMENT_ID");
                    obj.ProjectId = record.GetLongOrDefault("PROJECT_ID");
                    obj.RevisionNumber= record.GetInt16OrDefault("REVISION_NO");
                }
            );

            return obj;
        }
        #endregion

        #region CheckSOValidation
        public static ApplyForVR1 CheckSOValidation(int apprevisionId, string userSchema = UserSchema.Portal)
        {

            ApplyForVR1 objso = new ApplyForVR1();
            //Setup Procedure LIST_STRUCTURE
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                objso,
                userSchema + ".SP_CHECK_SO_VALIDATION",
                parameter =>
                {
                    parameter.AddWithValue("P_REVISIONID ", apprevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.CheckRoute = records.GetInt16OrDefault("CHECK_ROUTE");
                        instance.CheckVehicle = records.GetInt16OrDefault("CHECK_VEHICLE");
                        instance.CheckVehicleConfig = records.GetInt16OrDefault("CHECK_VEHICLECONFIG");
                        instance.CheckSuppliInfo = records.GetInt16OrDefault("CHECK_SUPPLINFO");
                        instance.CheckRouteConfig = records.GetInt16OrDefault("CHECK_ROUTECONFIG");
                    }
           );
            return objso;
        }
        #endregion

        #region GetApplStatus
        public static int GetApplStatus(int VersionNo, int RevisionNo, long ProjectId, string userSchema, int historic)
        {
            int AppStatus = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                AppStatus,
                userSchema + ".SP_GET_LATEST_VERSION",
                parameter =>
                {
                    parameter.AddWithValue("P_VER_NO", VersionNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REV_NO", RevisionNo, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_PROJ_ID", ProjectId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HISTORIC", historic, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records) =>
                {
                    AppStatus = (int)records.GetDecimalOrDefault("MAX_STAT");
                });
            return AppStatus;
        }
        #endregion

        #region GetHAContDetFromInboundDoc
        public static HAContact GetHAContDetFromInboundDoc(string EsdalRefNo)
        {
            HAContact ObjContactDet = new HAContact();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                  ObjContactDet,
                 UserSchema.Portal + ".GET_HA_CONTACT_DETAILS",
                  parameter =>
                  {
                      parameter.AddWithValue("P_ESDALREFNO", EsdalRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                      parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);

                  },
                  record =>
                  {
                      ObjContactDet.ContactDetails = record.GetByteArrayOrNull("document");

                  });
            return ObjContactDet;
        }
        #endregion

        #region CheckVR1Validation
        public static ApplyForVR1 CheckVR1Validation(int versionId, int ShowVehicle, string contentref, int apprevisionId, string userSchema = UserSchema.Portal)
        {
            ApplyForVR1 objCheckValidation = new ApplyForVR1();
            //Setup Procedure LIST_STRUCTURE
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                objCheckValidation,
                userSchema + ".SP_CHECK_VR1_VALIDATION",
                parameter =>
                {
                    parameter.AddWithValue("P_VERSIONID ", versionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SHOWVEHICLE", ShowVehicle, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONTENTREFNO ", contentref, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REVISIONID ", apprevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.CheckRoute = records.GetInt16OrDefault("CHECK_ROUTE");
                        instance.CheckVehicle = records.GetInt16OrDefault("CHECK_VEHICLE");
                        instance.CheckVehicleConfig = records.GetInt16OrDefault("CHECK_VEHICLECONFIG");
                        instance.CheckSuppliInfo = records.GetInt16OrDefault("CHECK_SUPPLINFO");
                        instance.CheckRouteConfig = records.GetInt16OrDefault("CHECK_ROUTECONFIG");
                    }
           );
            return objCheckValidation;
        }
        #endregion

        #region ListVR1RouteDetails
        public static List<VR1RouteImport> ListVR1RouteDetails(string ContentRefNo)
        {
            List<VR1RouteImport> objlistrt = new List<VR1RouteImport>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                   objlistrt,
                     UserSchema.Portal + ".STP_NOTIFICATION_ROUTE.SP_CHECK_ROUTE_IMPORT",
                   parameter =>
                   {
                       parameter.AddWithValue("RI_CONTENT_REF_NO ", ContentRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input);
                       parameter.AddWithValue("RI_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                   },
                       (records, instance) =>
                       {
                           instance.RouteName = records.GetStringOrDefault("PART_NAME");
                       }
               );
            return objlistrt;
        }
        #endregion

        #region SubmitSoApplication
        public static SOApplication SubmitSoApplication(int apprevisionId, int userId)
        {
            bool result = false;
            SOApplication soGeneralDetails = new SOApplication();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
               UserSchema.Portal + ".STP_SUBMIT_SO_APPL.SP_SUBMIT_SPEC_ORDER",
                parameter =>
                {
                    parameter.AddWithValue("P_REV_ID", apprevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_USERID", userId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     soGeneralDetails.ApplicationStatus = Convert.ToInt32(record.GetDecimalOrDefault("APP_ROW1"));
                     soGeneralDetails.ESDALReference = record.GetStringOrDefault("ESDALREFNO1");
                     soGeneralDetails.ProjectId = record.GetLongOrDefault("PROJECT_ID");
                     soGeneralDetails.LastRevisionNo = record.GetInt16OrDefault("REVISION_NO");
                     var s = record.GetFieldType("VERSION_NO");
                     soGeneralDetails.VersionNo= (int)record.GetDecimalOrDefault("VERSION_NO");
                 }
            );
            return soGeneralDetails;
        }
        #endregion

        #region SubmitVR1Application
        public static ApplyForVR1 SubmitVR1Application(int apprevisionId, int reducedDetails)
        {
            ApplyForVR1 applyForVR1 = new ApplyForVR1();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                applyForVR1,
               UserSchema.Portal + ".STP_SUBMIT_VR1_APPL.SP_SUBMIT_VR1",
                parameter =>
                {
                    parameter.AddWithValue("P_REV_ID", apprevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REDUCED_DET", reducedDetails, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 record =>
                 {
                     applyForVR1.Status = Convert.ToInt32(record.GetDecimalOrDefault("APP_COUNT"));
                     applyForVR1.ESDALReference = record.GetStringOrDefault("ESDALREFNO");
                     applyForVR1.ProjectId= record.GetLongOrDefault("PROJECT_ID");
                     applyForVR1.RevisionNumber = record.GetInt16OrDefault("REVISION_NO");
                     applyForVR1.VersionNumber = (int)record.GetDecimalOrDefault("VERSION_NO");
                 }
            );
            return applyForVR1;
        }
        #endregion

        #region GetVR1VehicleDEtails
        public static ApplyForVR1 GetVR1VehicleDEtails(VR1VehicleDetailsParams vr1VehicleDetailsParams)
        {
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                vr1VehicleDetailsParams.VR1Application,
                vr1VehicleDetailsParams.UserSchema + ".SP_VR1GENERAL_VEHICLE_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("p_CONTENT_REF_NO", vr1VehicleDetailsParams.ContentNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HISTORIC", vr1VehicleDetailsParams.Historic, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.OverallWidth = records.GetDoubleOrDefault("WIDTH");
                    instance.OverallLength = records.GetDoubleOrDefault("LEN");
                    instance.OverallHeight = records.GetDoubleOrDefault("MAX_HEIGHT");
                    instance.GrossWeight = records.GetDoubleOrDefault("gross_weight");
                });
            return vr1VehicleDetailsParams.VR1Application;
        }
        #endregion

        #region GetSOHaulApplDetails
        public static SOHaulierApplication GetSOHaulApplDetails(long revisionId, long versionId, int historic)
        {
            SOHaulierApplication sOApplicationObj = new SOHaulierApplication();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                sOApplicationObj,
                UserSchema.Portal + ".SP_HAULIER_APP_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("P_REVISION_ID", revisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VERSION_ID ", versionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HISTORIC", historic, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.RevisionId = records.GetLongOrDefault("REVISION_ID");
                    instance.HaulierESDALReference = records.GetStringOrDefault("ESDAL_REF");
                    instance.HaulierReference = records.GetStringOrDefault("HAULIERS_REF");
                    instance.HaulierFromSummary = records.GetStringOrDefault("FROM_DESCR");
                    instance.HaulierToSummary = records.GetStringOrDefault("TO_DESCR");
                    instance.HaulierClientName = records.GetStringOrDefault("CLIENT_DESCR");
                    instance.HaulierDescription = records.GetStringOrDefault("APPLICATION_NAME");
                    instance.HaulierLoad = records.GetStringOrDefault("LOAD_DESCR");
                    instance.HaulierApplicationDate = records.GetDateTimeOrDefault("APPLICATION_DATE");
                    instance.HaulierMovementDateFrom = records.GetDateTimeOrDefault("MOVEMENT_START_DATE");
                    instance.HaulierMovementDateTo = records.GetDateTimeOrDefault("MOVEMENT_END_DATE");
                    instance.HaulierContactName = records.GetStringOrDefault("HAULIER_CONTACT");
                    instance.HaulierApplicantName = records.GetStringOrDefault("HAULIER_NAME");
                    instance.HaulierApplicantAddress1 = records.GetStringOrDefault("HAULIER_ADDRESS_1");
                    instance.HaulierApplicantAddress2 = records.GetStringOrDefault("HAULIER_ADDRESS_2");
                    instance.HaulierApplicantAddress3 = records.GetStringOrDefault("HAULIER_ADDRESS_3");
                    instance.HaulierApplicantAddress4 = records.GetStringOrDefault("HAULIER_ADDRESS_4");
                    instance.HaulierApplicantAddress5 = records.GetStringOrDefault("HAULIER_ADDRESS_5");
                    instance.HaulierPostCode = records.GetStringOrDefault("HAULIER_POST_CODE"); 
                    instance.HaulierCountry = records.GetStringOrDefault("HAULIER_COUNTRY");
                    instance.HaulierTelephone = records.GetStringOrDefault("HAULIER_TEL_NO");
                    instance.HaulierFaxNumber = records.GetStringOrDefault("HAULIER_FAX_NO");
                    instance.HaulierEmailId = records.GetStringOrDefault("HAULIER_EMAIL");
                    instance.HaulierOperatorLicence = records.GetStringOrDefault("HAULIER_LICENCE_NO"); 
                    instance.HaulierNumberOfMovements = records.GetInt16OrDefault("TOTAL_MOVES");
                    instance.HaulierNumberOfPieces = records.GetInt16OrDefault("MAX_PARTS_PER_MOVE");
                    instance.HaulierApplicationNotes = records.GetStringOrDefault("APPLICATION_NOTES");
                    instance.AgentName = records.GetStringOrDefault("agent_name");
                    instance.NotesOnEscort = records.GetStringOrDefault("notesonescort");
                    instance.VehicleDescription = records.GetStringOrDefault("REDUCED_VEHICLE_DESCR");
                    instance.SubMovementClass = records.GetInt32OrDefault("vehicle_classification");
                    instance.Status = records.GetStringOrDefault("status");

                    instance.OnBehalOfContactName = records.GetStringOrDefault("ON_BEHALF_OF_CONTACT");
                    instance.OnBehalOfHaulierOrgName = records.GetStringOrDefault("ON_BEHALF_OF_NAME");
                    instance.OnBehalOfHaulierAddress1 = records.GetStringOrDefault("ON_BEHALF_OF_ADDRESS_1");
                    instance.OnBehalOfHaulierAddress2 = records.GetStringOrDefault("ON_BEHALF_OF_ADDRESS_2");
                    instance.OnBehalOfHaulierAddress3 = records.GetStringOrDefault("ON_BEHALF_OF_ADDRESS_3");
                    instance.OnBehalOfHaulierAddress4 = records.GetStringOrDefault("ON_BEHALF_OF_ADDRESS_4");
                    instance.OnBehalOfHaulierAddress5 = records.GetStringOrDefault("ON_BEHALF_OF_ADDRESS_5");
                    instance.OnBehalOfHaulPostCode = records.GetStringOrDefault("ON_BEHALF_OF_POST_CODE");
                    instance.OnBehalOfCountryID = records.GetInt32OrDefault("ON_BEHALF_OF_COUNTRY");
                    instance.OnBehalOfCountryName = records.GetStringOrDefault("ON_BEHALF_OF_COUNTRYNAME");
                    instance.OnBehalOfHaulTelephoneNo = records.GetStringOrDefault("ON_BEHALF_OF_TEL_NO");
                    instance.OnBehalOfHaulFaxNo = records.GetStringOrDefault("ON_BEHALF_OF_FAX_NO");
                    instance.OnBehalOfHaulEmailID = records.GetStringOrDefault("ON_BEHALF_OF_EMAIL");
                    instance.OnBehalOfHaulOperatorLicens = records.GetStringOrDefault("ON_BEHALF_OF_LICENCE_NO");
                });
            return sOApplicationObj;
        }
        #endregion

        #region Get SORT VR1 GeneralDetails
        public static ApplyForVR1 GetSORTVR1GeneralDetails(int ProjectID, string userSchema = UserSchema.Portal)
        {
            ApplyForVR1 GetSortVR1Genral = new ApplyForVR1();
            try
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    GetSortVR1Genral,
                       userSchema + ".SP_SORT_GET_VR1GENRAL_DETAILS",
                    parameter =>
                    {
                        parameter.AddWithValue("p_PROJECT_ID ", ProjectID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            instance.LoadDescription = records.GetStringOrDefault("LOAD_DESCR");
                            instance.SubMovementClass = records.GetInt32OrDefault("VEHICLE_CLASSIFICATION");
                            instance.ClientName = records.GetStringOrDefault("CLIENT_DESCR");
                            instance.FromSummary = records.GetStringOrDefault("FROM_DESCR");
                            instance.ToSummary = records.GetStringOrDefault("TO_DESCR");
                            instance.MovementDateFrom = ConvertDate(records.GetStringOrDefault("MOVEMENT_START_DATE"));
                            instance.MovementDateTo = ConvertDate(records.GetStringOrDefault("MOVEMENT_END_DATE"));
                            instance.ApplicationNotes = records.GetStringOrDefault("APPLICATION_NOTES");
                            instance.NoOfMovements = records.GetInt16Nullable("TOTAL_MOVES");
                            instance.MaxPiecesPerLoad = records.GetInt16Nullable("MAX_PARTS_PER_MOVE");
                            instance.DescriptionWithApplication = records.GetStringOrDefault("APPLICATION_NAME");
                            instance.ReducedDetails = records.GetInt16Nullable("REDUCED_DETAILS");
                            instance.VehicleDescription = records.GetStringOrDefault("reduced_vehicle_descr");
                            instance.AllocateToName = records.GetStringOrDefault("USERNAME");
                            instance.AllocateTo = Convert.ToString(records.GetDecimalOrDefault("user_id"));

                            instance.ApplicantName = records.GetStringOrDefault("HAULIER_NAME");
                            instance.ApplicantAddress1 = records.GetStringOrDefault("HAULIER_ADDRESS_1");
                            instance.ApplicantAddress2 = records.GetStringOrDefault("HAULIER_ADDRESS_2");
                            instance.ApplicantAddress3 = records.GetStringOrDefault("HAULIER_ADDRESS_3");
                            instance.ApplicantAddress4 = records.GetStringOrDefault("HAULIER_ADDRESS_4");
                            instance.ApplicantAddress5 = records.GetStringOrDefault("HAULIER_ADDRESS_5");
                            instance.ApplicantCountryId = Convert.ToString(records.GetInt32OrDefault("HAULIER_COUNTRY"));
                            instance.ApplicantCountryName = records.GetStringOrDefault("HAULIER_COUNT_NAME");
                            instance.ApplicationPostCode = records.GetStringOrDefault("HAULIER_POST_CODE");
                            instance.HaulierOrgName = records.GetStringOrDefault("ON_BEHALF_OF_NAME");
                            instance.HaulierAddress1 = records.GetStringOrDefault("ON_BEHALF_OF_ADDRESS_1");
                            instance.HaulierAddress2 = records.GetStringOrDefault("ON_BEHALF_OF_ADDRESS_2");
                            instance.HaulierAddress3 = records.GetStringOrDefault("ON_BEHALF_OF_ADDRESS_3");
                            instance.HaulierAddress4 = records.GetStringOrDefault("ON_BEHALF_OF_ADDRESS_4");
                            instance.HaulierAddress5 = records.GetStringOrDefault("ON_BEHALF_OF_ADDRESS_5");
                            instance.CountryId = Convert.ToString(records.GetInt32OrDefault("ON_BEHALF_OF_COUNTRY"));
                            instance.OnBehalfOfPostCode = records.GetStringOrDefault("ON_BEHALF_OF_POST_CODE");
                            instance.ApplicantTelephoneNo = records.GetStringOrDefault("HAULIER_TEL_NO");
                            instance.ApplicantEmailId = records.GetStringOrDefault("HAULIER_EMAIL");
                            instance.ApplicantFaxNo = records.GetStringOrDefault("HAULIER_FAX_NO");
                            instance.OnBehalfOfEmailId = records.GetStringOrDefault("ON_BEHALF_OF_EMAIL");
                            instance.HaulierFaxNo = records.GetStringOrDefault("ON_BEHALF_OF_FAX_NO");
                            instance.HaulierTelephoneNo = records.GetStringOrDefault("ON_BEHALF_OF_TEL_NO");
                            instance.HaulierOperatorLicence = records.GetStringOrDefault("ON_BEHALF_OF_LICENCE_NO");

                            instance.OverallWidth = records.GetDoubleOrDefault("WIDTH_MAX_MTR");
                            instance.OverallLength = records.GetDoubleOrDefault("LEN_MAX_MTR");
                            instance.OverallHeight = records.GetDoubleOrDefault("MAX_HEIGHT_MAX_MTR");
                            instance.GrossWeight = Convert.ToDouble(records.GetInt32OrDefault("GROSS_WEIGHT_MAX_KG"));



                        }
                );
            }

            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Application/ReviseVR1Application, Exception: " + ex​​​​);
            }
            return GetSortVR1Genral;
        }
        #endregion

        #region GetSORTHaulierAppRouteParts
        public static List<AffectedStructures> GetSORTHaulierAppRouteParts(int versionID, string vr1ContentRefNo, string userSchema = UserSchema.Portal)
        {
            List<AffectedStructures> objAffectedAtructures = new List<AffectedStructures>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
            objAffectedAtructures,
                userSchema + ".SP_GET_ROUTES_FROM_ROUTE_PART",
            parameter =>
            {
                parameter.AddWithValue("P_VER_ID", versionID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_CONTENT_REFNO", vr1ContentRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
            (records, instance) =>
            {
                instance.PartId = records.GetLongOrDefault("ROUTE_PART_ID");
                instance.PartNo = records.GetInt16OrDefault("ROUTE_PART_NO");
                instance.PartName = records.GetStringOrDefault("PART_NAME");
                instance.PartNarrative = records.GetStringOrDefault("PART_DESCR");
                instance.RouteType = "planned";
            });
            return objAffectedAtructures;
        }
        #endregion

        #region GetHaulierApplRouteParts
        public static List<AffectedStructures> GetHaulierApplRouteParts(int revisionID, int appFlag, int sortRouteVehicleFlag, string userSchema)
        {
            List<AffectedStructures> objAffectedAtructures = new List<AffectedStructures>();
            try
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
               objAffectedAtructures,
                  userSchema + ".SP_GET_HAULIER_APP",
               parameter =>
               {
                   parameter.AddWithValue("REVISION_ID", revisionID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("APP_FLAG", appFlag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   if (userSchema == UserSchema.Sort)
                       parameter.AddWithValue("SORT_FLAG", sortRouteVehicleFlag, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                   parameter.AddWithValue("PRESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
               },
                   (records, instance) =>
                   {
                       if (appFlag == 1 || appFlag == 2)
                       {
                           instance.PartId = records.GetLongOrDefault("ROUTE_PART_ID");
                           instance.PartNo = records.GetInt16OrDefault("ROUTE_PART_NO");
                           instance.PartName = records.GetStringOrDefault("PART_NAME");
                           instance.PartNarrative = records.GetStringOrDefault("PART_DESCR");
                           if (appFlag == 2)
                           {
                               instance.RouteType = records.GetStringOrDefault("ROUTE_TYPE");
                           }
                           else
                           {
                               instance.RouteType = "planned";
                           }
                       }
                       else if (appFlag == 5)
                       {
                           instance.PartId = records.GetLongOrDefault("ROUTE_PART_ID");
                           instance.PartNo = records.GetInt16OrDefault("ROUTE_PART_NO");
                           instance.PartName = records.GetStringOrDefault("PART_NAME");
                           instance.PartNarrative = records.GetStringOrDefault("PART_DESCR");
                           instance.RouteType = "planned";
                       }
                       else
                       {
                           instance.PartId = records.GetLongOrDefault("PART_ID");
                           instance.PartNo = records.GetInt16OrDefault("PART_NO");
                           instance.PartName = records.GetStringOrDefault("PART_NAME");
                           instance.PartNarrative = records.GetStringOrDefault("PART_DESCR_1");
                           instance.RouteType = records.GetStringOrDefault("ROUTE_TYPE");
                       }
                   }
              );
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Application/GetHaulierApplRouteParts, Exception: " + ex​​​​);
            }
            return objAffectedAtructures;
        }
        #endregion

        #region EsdalRefNum
        public static string EsdalRefNum(int SOVersionID)
        {
            string result = "";
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
            result,
            UserSchema.Portal + ".SP_GET_ESDAL_REF_NUM",
            parameter =>
            {
                parameter.AddWithValue("P_VERSION_ID", SOVersionID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
            records =>
            {
                result = records.GetStringOrDefault("VAR_ESDAL_REFERENCE_NO");
            });
            return result;
        }
        #endregion

        #region Insert Plan Movemnt App
        public static AppGeneralDetails InsertApplicationType(PlanMovementType saveAppType)
        {
            AppGeneralDetails appGeneral = new AppGeneralDetails();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                appGeneral,
                saveAppType.UserSchema + ".STP_MOVEMENT.SP_INSERT_APPLICATION",
                parameter =>
                {
                    parameter.AddWithValue("P_ORG_ID", saveAppType.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONTACT_ID ", saveAppType.ContactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MOVEMENT_ID", saveAppType.MovementId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VEHICLE_CLASS ", saveAppType.VehicleClass, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_FROM_DESC ", saveAppType.FromDesc, OracleDbType.Varchar2,ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_TO_DESC ", saveAppType.ToDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_START_DATE ", TimeZoneInfo.ConvertTimeToUtc(saveAppType.MovementStart), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_END_DATE ", TimeZoneInfo.ConvertTimeToUtc(saveAppType.MovementEnd), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_REF ", saveAppType.HaulierRef, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NOTIF_ID", saveAppType.NotificationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REV_ID", saveAppType.RevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    if (saveAppType.UserSchema == UserSchema.Sort) {
                        parameter.AddWithValue("P_ALLOCATE_USER_ID", saveAppType.AllocateUserId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    }
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (record,instance) =>
                    {
                        instance.RevisionId = record.GetLongOrDefault("REVISION_ID");
                        if (saveAppType.MoveType == (int)MovementType.vr_1)
                        {
                            instance.IsVr1 = true;
                            instance.VersionId= (long)record.GetDecimalOrDefault("VERSION_ID");
                        }
                    }
            );
            return appGeneral;
        }
        public static AppGeneralDetails UpdateApplicationType(PlanMovementType updateAppType)
        {
            AppGeneralDetails appGeneral = new AppGeneralDetails();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                appGeneral,
                updateAppType.UserSchema + ".STP_MOVEMENT.SP_UPDATE_APPLICATION",
                parameter =>
                {
                    parameter.AddWithValue("P_ORG_ID", updateAppType.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONTACT_ID ", updateAppType.ContactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_MOVEMENT_ID", updateAppType.MovementId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VEHICLE_CLASS ", updateAppType.VehicleClass, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_FROM_DESC ", updateAppType.FromDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_TO_DESC ", updateAppType.ToDesc, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_START_DATE ", TimeZoneInfo.ConvertTimeToUtc(updateAppType.MovementStart), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_END_DATE ", TimeZoneInfo.ConvertTimeToUtc(updateAppType.MovementEnd), OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HAUL_REF ", updateAppType.HaulierRef, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_REVISION_ID", updateAppType.RevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_VEHICLE_EDIT ", updateAppType.IsVehicleEdit, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    if (updateAppType.UserSchema == UserSchema.Sort)
                    {
                        parameter.AddWithValue("P_ALLOCATE_USER_ID", updateAppType.AllocateUserId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    }
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (record, instance) =>
                    {
                        instance.RevisionId = record.GetLongOrDefault("REVISION_ID");
                        if (updateAppType.MoveType == (int)MovementType.vr_1)
                        {
                            instance.IsVr1 = true;
                            instance.VersionId = (long)record.GetDecimalOrDefault("VERSION_ID");
                        }
                    }
            );
            return appGeneral;
        }
        #endregion

        #region VR1GetSupplementaryInfo
        public static SupplimentaryInfo VR1GetSupplementaryInfo(int apprevid=0, string userSchema=UserSchema.Portal, int historic = 0)
        {

            SupplimentaryInfo supplimentaryInfo = new SupplimentaryInfo();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                supplimentaryInfo,
                userSchema + ".SP_GET_SUPPLEMENTARYINFO",
                parameter =>
                {
                    parameter.AddWithValue("P_REVISION_ID ", apprevid, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_HISTORIC", historic, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    supplimentaryInfo.TotalDistanceOfRoad = records.GetStringOrDefault("DISTANCE_BY_ROAD");
                    supplimentaryInfo.ApprValueOfLoad = records.GetStringOrDefault("VALUE_OF_LOAD");
                    supplimentaryInfo.DateOfAuthority = records.GetStringOrDefault("SIMILAR_MOVEMENT");
                    supplimentaryInfo.LoadDivision = records.GetShortOrDefault("LOAD_DIVISIBLE");
                    supplimentaryInfo.AdditionalCost = records.GetStringOrDefault("COST_OF_DIVISION");
                    supplimentaryInfo.RiskNature = records.GetStringOrDefault("RISK_OF_DIVISION");
                    supplimentaryInfo.Shipment = records.GetShortOrDefault("SEA_CONSIDERED");
                    supplimentaryInfo.PortNames = records.GetStringOrDefault("BETWEEN_WHICH_PORTS");
                    supplimentaryInfo.SeaQuotation = records.GetStringOrDefault("SEA_QUOTATION");
                    supplimentaryInfo.Address = records.GetShortOrDefault("IS_FINAL_ADDRESS");
                    supplimentaryInfo.ProposedMoveDetails = records.GetStringOrDefault("FURTHER_MOVES");
                    supplimentaryInfo.ApprCostOfMovement = records.GetStringOrDefault("COST_OF_ROAD_MOVE");
                    supplimentaryInfo.AdditionalConsideration = records.GetStringOrDefault("OTHER_CONSIDERATIONS");
                });
            return supplimentaryInfo;
        }
        #endregion

        #region GetAgreedRoutePart
        public static List<AffectedStructures> GetAgreedRoutePart(int VersionId, int revisionid, string userSchema = UserSchema.Portal, string ContentRefNo = "")
        {
            List<AffectedStructures> objAffectedAtructures = new List<AffectedStructures>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
            objAffectedAtructures,
                userSchema + ".SP_ROUTE_PART_DETAILS",
            parameter =>
            {
                parameter.AddWithValue("P_VER_ID", VersionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_REV_ID", revisionid, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_CONT_REF_NUM", ContentRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("p_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
            (records, instance) =>
            {
                instance.RoutePartID = records.GetLongOrDefault("ROUTE_PART_ID");
                instance.RoutePartNo = records.GetInt16OrDefault("ROUTE_PART_NO");
                instance.RoutePartName = records.GetStringOrDefault("PART_NAME");
            });
            return objAffectedAtructures;
        }

        #endregion

        #region GetNotifRoutePart
        public static List<AffectedStructures> GetNotifRoutePart(int NotifId, int rp_flag)
        {
            List<AffectedStructures> objAffectedAtructures = new List<AffectedStructures>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
            objAffectedAtructures,
                UserSchema.Portal + ".SP_NOTIF_ROUTE_PARTS",
            parameter =>
            {
                parameter.AddWithValue("P_NOTIFICATIONID", NotifId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("p_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
            (records, instance) =>
            {
                if (rp_flag == 1)
                {
                    instance.RoutePartID = records.GetLongOrDefault("ROUTE_PART_ID");
                    instance.RoutePartNo = records.GetInt16OrDefault("ROUTE_PART_NO");
                    instance.RoutePartName = records.GetStringOrDefault("PART_NAME");
                    instance.PartNarrative = records.GetStringOrDefault("PART_DESCR");
                }
                else
                {
                    instance.PartId = records.GetLongOrDefault("ROUTE_PART_ID");
                    instance.PartNo = records.GetInt16OrDefault("ROUTE_PART_NO");
                    instance.PartName = records.GetStringOrDefault("PART_NAME");
                    instance.PartNarrative = records.GetStringOrDefault("PART_DESCR");
                    instance.RouteType = "planned";
                }
            });
            return objAffectedAtructures;
        }
        #endregion

        #region IMP_CondidateRoue
        public static long IMP_CondidateRoue(int routepartId, int AppRevId, int versionid, string contentref, string userSchema = UserSchema.Sort)
        {
            long result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
            result,
            userSchema + ".STP_ROUTE_IMPORT.SP_IMP_CANDIDATE_ROUTE",
            parameter =>
            {
                parameter.AddWithValue("P_RPART_ID", routepartId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_REVISION_ID ", AppRevId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_VERSION_ID ", versionid, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_CONTENTREF", contentref, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                parameter.AddWithValue("P_RESULT_SET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
            },
            record =>
            {
                result = record.GetLongOrDefault("ROUTE_PART_ID");
            });
            return result;
        }
        #endregion SaveRouteInRouteParts

        #region GetApplicationDetails
        public static PlanMovementType GetApplicationDetails(long revisionId, string userSchema)
        {
            PlanMovementType planMovement = new PlanMovementType();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                planMovement,
                    userSchema + ".SP_APPLICATION_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("p_revision_id", revisionId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767); 
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.HaulierRef = records.GetStringOrDefault("HAULIERS_REF");                    
                    instance.FromDesc = records.GetStringOrDefault("FROM_DESCR");
                    instance.ToDesc = records.GetStringOrDefault("TO_DESCR");                    
                    instance.VehicleClass = records.GetInt32OrDefault("VEHICLE_CLASSIFICATION");
                    instance.MovementStart = records.GetDateTimeOrEmpty("MOVEMENT_START_DATE");
                    instance.MovementEnd = records.GetDateTimeOrEmpty("MOVEMENT_END_DATE"); 
                    if (userSchema == UserSchema.Sort)
                        instance.AllocateUserId = records.GetLongOrDefault("planner_user_id");
                   
                });
            return planMovement;
        }

        public static PlanMovementType GetNotificationDetails(long notificationId, string userSchema)
        {
            PlanMovementType planMovement = new PlanMovementType();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                planMovement,
                    userSchema + ".SP_NOTIFICATION_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("P_NOTIF_ID", notificationId, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                (records, instance) =>
                {
                    instance.HaulierRef = records.GetStringOrDefault("HAULIERS_REF");
                    instance.FromDesc = records.GetStringOrDefault("FROM_DESCR");
                    instance.ToDesc = records.GetStringOrDefault("TO_DESCR");
                    instance.VehicleClass = records.GetInt32OrDefault("VEHICLE_CLASSIFICATION");
                    instance.MovementStart = records.GetDateTimeOrEmpty("move_start_date");
                    instance.MovementEnd = records.GetDateTimeOrEmpty("move_end_date");
                    instance.HaulierName = records.GetStringOrDefault("haulier_name");
                    instance.HaulierContact = records.GetStringOrDefault("haulier_contact");
                    instance.HaulierOnBehalfOf = records.GetStringOrDefault("On_Behalf_Of");
                    instance.NotificationDate = records.GetDateTimeOrEmpty("NOTIFICATION_DATE");
                });
            return planMovement;
        }

        #endregion

        #region ExportApplicationData
        public static Domain.ExternalAPI.ExportAppGeneralDetails ExportApplicationData(string ESDALReferenceNumber, string userSchema)
        {
            Domain.ExternalAPI.ExportAppGeneralDetails ObjNEAppGeneralDetails = new Domain.ExternalAPI.ExportAppGeneralDetails();
            string appJson = string.Empty;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                appJson,
               userSchema + ".STP_EXPORT_DETAILS.SP_GET_APP_GEN_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("P_ESDALREFNO", ESDALReferenceNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                 (records, instance) =>
                 {
                     appJson = records.GetStringOrDefault("APP_JSON");
                 });
                if (!string.IsNullOrWhiteSpace(appJson))
                {
                    ObjNEAppGeneralDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<Domain.ExternalAPI.ExportAppGeneralDetails>(appJson);
                    ObjNEAppGeneralDetails.IsVR1 = ObjNEAppGeneralDetails.Classification != "GC002";
                }
            return ObjNEAppGeneralDetails;
        }
        #endregion
    }
}
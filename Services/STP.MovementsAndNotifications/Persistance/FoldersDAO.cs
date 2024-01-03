using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.DataAccess.SafeProcedure;
using STP.Domain.MovementsAndNotifications.Folder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.MovementsAndNotifications.Persistance
{
    public static class FoldersDAO
    {
        public static List<FoldersDomain> GetFoldersSearchInfo(int pageNumber, int pageSize, int organisationId, string searchString)//string folderName,
        {
            List<FoldersDomain> folderDaoObj = new List<FoldersDomain>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                folderDaoObj,
                UserSchema.Portal + ".SP_R_LIST_FOLDERS",
                 parameter =>
                 {
                     parameter.AddWithValue("pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("OrgId", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("P_SEARCH_STRING", searchString, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                 },
                (records, instance) =>
                {
                    instance.FolderId = (int)records.GetLongOrDefault("FOLDER_ID");
                    instance.OrganisationId = (int)records.GetLongOrDefault("ORGANISATION_ID");
                    instance.FolderName = records.GetStringOrDefault("FOLDER_NAME");
                    instance.Listcount = (int)records.GetDecimalOrDefault("TOTALRECORDCOUNT");
                    instance.RecordCount = (int)records.GetDecimalOrDefault("FOLDER_COUNT");
                }
             );

            return folderDaoObj;
        }

        public static int InsertFolderInfo(int organisationId, string folderName, int parentId = 0)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + ".SP_INSERT_FOLDER",
                 parameter =>
                 {
                     parameter.AddWithValue("p_orgId", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     if (parentId > 0)
                         parameter.AddWithValue("p_parentId", parentId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     else
                         parameter.AddWithValue("p_parentId", null, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("p_folderName", folderName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                 },
                 records =>
                 {
                     result = (int)records.GetDecimalOrDefault("CNT");
                 }
              );
            return result;
        }

        public static int DeleteFolderInfo(int folderId)
        {
            int rowsAffected = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                UserSchema.Portal + ".SP_DELETE_FOLDER",
                 parameter =>
                 {
                     parameter.AddWithValue("folderId", folderId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("p_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                 },
                 records =>
                 {
                     rowsAffected = records.GetInt32("p_AFFECTED_ROWS");
                 }
              );
            return rowsAffected;
        }

        public static int EditFolderInfo(int folderId, int organisationId, string folderName)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + ".SP_EDIT_FOLDER",
                 parameter =>
                 {
                     parameter.AddWithValue("folderId", folderId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("OrgId", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("folderName", folderName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                 },
                 records =>
                 {
                     result = (int)records.GetDecimalOrDefault("CNT");
                 }
              );
            return result;
        }

        public static List<FolderTreeModel> GetFolders(int organisationId)
        {
            List<FolderTreeModel> folderDaoObj = new List<FolderTreeModel>();
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                folderDaoObj,
                UserSchema.Portal + ".SP_R_GET_FOLDERS",
                 parameter =>
                 {
                     parameter.AddWithValue("OrgId", organisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                 },
                (records, instance) =>
                {
                    instance.FolderId = (int)records.GetLongOrDefault("FOLDER_ID");
                    instance.OrganisationId = (int)records.GetLongOrDefault("ORGANISATION_ID");
                    instance.FolderName = records.GetStringOrDefault("FOLDER_NAME");
                    instance.ParentFolderId = (int)records.GetLongOrDefault("PARENT_FOLDER_ID");
                }
             );

            return folderDaoObj;
        }

        public static int AddItemToFolder(AddItemFolderModel model)
        {
            int rowsAffected = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                rowsAffected,
                UserSchema.Portal + ".SP_INSERT_PROJECT_FOLDER",
                 parameter =>
                 {
                     parameter.AddWithValue("P_FOLDER_ID", model.FolderId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     //parameter.AddWithValue("P_PROJECT_ID", model.ProjectId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     if (model.NotificationId > 0)
                     {
                         parameter.AddWithValue("P_MOVEMENT_ID", model.NotificationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("P_MOVEMENT_TYPE", "NOTIFICATION", OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                     }
                     else
                     {
                         parameter.AddWithValue("P_MOVEMENT_ID", model.MovementRevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                         parameter.AddWithValue("P_MOVEMENT_TYPE", "APPLICATION", OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                     }
                     parameter.AddWithValue("P_SORT", (model.UserSchema==UserSchema.Sort?1:0), OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("P_VERSION_ID", model.MovementVersionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("p_Resultset", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                 },
                 records =>
                 {
                     rowsAffected = (int)records.GetDecimalOrDefault("CNT");
                 }
              );
            return rowsAffected;
        }

        public static int RemoveItemsFromFolder(AddItemFolderModel model)
        {
            int rowsAffected = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
                UserSchema.Portal + ".SP_DEL_FROM_PROJECT_FOLDER",
                 parameter =>
                 {
                     parameter.AddWithValue("P_FOLDER_ID", model.FolderId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     if (model.NotificationId > 0)
                         parameter.AddWithValue("P_MOVEMENT_ID", model.NotificationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     else
                         parameter.AddWithValue("P_MOVEMENT_ID", model.MovementRevisionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("P_VERSION_ID", model.MovementVersionId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("p_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                 },
                 records =>
                 {
                     rowsAffected = records.GetInt32("p_AFFECTED_ROWS");
                 }
              );
            return rowsAffected;
        }

        public static int MoveFolderToFolder(FolderTreeModel model)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
                UserSchema.Portal + ".SP_EDIT_FOLDER_PARENTID",
                 parameter =>
                 {
                     parameter.AddWithValue("P_FOLDERID", model.FolderId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("P_ORG_ID", model.OrganisationId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     if (model.ParentFolderId > 0)
                         parameter.AddWithValue("P_PARENT_FOLDERID", model.ParentFolderId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     else
                         parameter.AddWithValue("P_PARENT_FOLDERID", null, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                     parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                 },
                 records =>
                 {
                     result = (int)records.GetDecimalOrDefault("CNT");
                 }
              );
            return result;
        }
    }
}
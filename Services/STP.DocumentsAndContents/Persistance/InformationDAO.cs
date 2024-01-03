using Oracle.DataAccess.Client;
using STP.Common.Constants;
using STP.Common.Enums;
using STP.Common.Logger;
using STP.DataAccess.SafeProcedure;
using STP.DocumentsAndContents.Providers;
using System;
using System.Collections.Generic;
using STP.Domain;
using STP.Domain.DocumentsAndContents;
using System.Globalization;

namespace STP.DocumentsAndContents.Persistance
{
    public static class InformationDAO
    {
        /// <summary>
        /// Get Haulier Contact List
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<InformationModel> GetInformationList(int pageNumber, int pageSize, string pageName, string contentType, int userType, int sortOrder, int presetFilter)
        {

            List<InformationModel> informationListObject = new List<InformationModel>();
            //Setup Procedure LIST_Infromation
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                informationListObject,
                UserSchema.Portal + ".GET_INFORMATION_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_pageNumber", pageNumber, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_PageName", pageName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ContentType", contentType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_UserTypeId", userType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("SORT_ORDER", sortOrder, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("PRESET_FILTER", presetFilter, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.ContentId = records.GetLongOrDefault("CONTENT_ID");
                        instance.Name = records.GetStringOrDefault("NAME");
                        instance.Priority = records.GetLongOrDefault("PRIORITY");
                        instance.Title = records.GetStringOrDefault("TITLE");
                        instance.ContentType = records.GetInt32OrDefault("CONTENT_TYPE");
                        instance.PriorityName = records.GetStringOrDefault("PRIORITY_NAME");
                        instance.TotalRecordCount = records.GetDecimalOrDefault("TotalRecordCount");
                        instance.PublishedDate = records.GetDateTimeOrDefault("uploaded_date");
                        instance.Suppressed = records.GetShortOrDefault("suppressed");
                    }
            );

            return informationListObject;
        }


        /// <summary>
        /// Delete Information by Content Id
        /// </summary>
        /// <param name="deletedContactId"></param>
        /// <returns></returns>
        internal static int DeleteInformation(int deletedContactId)
        {
            int rowsAffected = 0;
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(

                UserSchema.Portal + ".DELETE_INFORMATION",
                parameter =>
                {
                    parameter.AddWithValue("P_CONTENT_ID", deletedContactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                record =>
                {
                    rowsAffected = record.GetInt32("p_AFFECTED_ROWS");
                }
            );
            result = rowsAffected;
            return result;
        }
        /// <summary>
        /// Get Enum list by EnumType
        /// </summary>
        /// <param name="EnumTypeName"></param>
        /// <returns></returns>
        internal static List<InformationModel> GetEnumValsListByEnumType(string EnumTypeName)
        {

            List<InformationModel> EnumNames = new List<InformationModel>();

            //Setup Procedure LIST_Infromation
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                EnumNames,
               UserSchema.Portal + ".GET_ENUM_VAL_BY_ENUM_TYPE",
                parameter =>
                {
                    parameter.AddWithValue("P_NAME", EnumTypeName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.EnumValuesName = records.GetStringOrDefault("NAME");
                        instance.Code = records.GetInt32OrDefault("CODE");
                    }
            );

            return EnumNames;
        }

        /// <summary>
        /// Get Associated files by ContentId
        /// </summary>
        /// <param name="CONTENT_ID"></param>
        /// <returns></returns>
        internal static List<WebContentFile> GetAssociatedFilesByContentId(int CONTENT_ID)
        {

            List<WebContentFile> AssociatedFiles = new List<WebContentFile>();

            //Setup Procedure LIST_Infromation
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                AssociatedFiles,
               UserSchema.Portal + ".GET_INFORMATION_FILE_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_CONTENT_ID", CONTENT_ID, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.ContentID = records.GetLongOrDefault("CONTENT_ID");
                        instance.FileContentUpload = records.GetByteArrayOrNull("FILE_CONTENT");
                        instance.FileID = records.GetLongOrDefault("FILE_ID");
                        instance.FileName = records.GetStringOrDefault("FILE_NAME");
                        instance.MimeTypeUpload = records.GetStringOrDefault("MIME_TYPE");
                        instance.VersionID = records.GetLongOrDefault("VERSION_ID");
                    }
            );

            return AssociatedFiles;
        }
        /// <summary>
        /// Get portal by content Id
        /// </summary>
        /// <param name="CONTENT_ID"></param>
        /// <returns></returns>
        internal static List<InformationModel> GetPortalContentById(int CONTENT_ID)
        {

            List<InformationModel> PortalList = new List<InformationModel>();

            //Setup Procedure LIST_Infromation
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                PortalList,
               UserSchema.Portal + ".GET_PORTAL_CONTENT_BY_ID",
                parameter =>
                {
                    parameter.AddWithValue("P_CONTENT_ID", CONTENT_ID, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.EnumValuesName = records.GetStringOrDefault("NAME");
                        instance.Code = records.GetInt32OrDefault("CODE");
                    }
            );

            return PortalList;
        }
        internal static InformationModel GetInformationById(int managedContentId)
        {
            InformationModel InfoModelObj = new InformationModel();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                InfoModelObj,
               UserSchema.Portal + ".GET_INFORMATION_DETAILS",
                parameter =>
                {
                    parameter.AddWithValue("P_CONTENT_ID", managedContentId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.ContentId = records.GetLongOrDefault("CONTENT_ID");
                        instance.Name = records.GetStringOrDefault("NAME");
                        instance.Title = records.GetStringOrDefault("TITLE");
                        instance.Description = records.GetStringOrDefault("DESCRIPTION");
                        instance.Priority = records.GetLongOrDefault("PRIORITY");
                        instance.PriorityName = records.GetStringOrDefault("PRIORITY_NAME");
                        instance.ContentType = records.GetInt32OrDefault("CONTENT_TYPE");
                        instance.Suppressed = records.GetInt16OrDefault("SUPPRESSED");
                        instance.Type = records.GetInt16OrDefault("TYPE");
                        instance.EnumValuesName = records.GetStringOrDefault("ENUM_VALS_NAME");
                        instance.DownloadType = records.GetInt32OrDefault("DOWNLOAD_TYPE");
                        instance.LinkUrl = records.GetStringOrDefault("LINK_URL");
                        string str = records.GetDataTypeName("END_DATE");
                        instance.EndDate = records.GetDateTimeOrDefault("END_DATE");
                        instance.UploadedDate = records.GetDateTimeOrDefault("UPLOADED_DATE");
                    }
            );

            return InfoModelObj;
        }
        /// <summary>
        /// Mange information . Create and update operation.
        /// </summary>
        /// <param name="informationModel"></param>
        /// <returns></returns>
        internal static InformationModel ManageInformation(InformationModel informationModel)
        {
            InformationModel infoModelObj = new InformationModel();
            //DateTime currentDateTime = DateTime.Now;
            //DateTime yesterdayDateTime = DateTime.Now.AddDays(-1);
            informationModel.StartDate = DateTime.Now;
            informationModel.EndDate = Convert.ToDateTime(informationModel.EndDate);

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                infoModelObj,
               UserSchema.Portal + ".MANAGE_INFORMATION",
                parameter =>
                {
                    parameter.AddWithValue("P_CONTENT_ID", informationModel.ContentId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONTENT_TYPE_NAME", informationModel.ContentTypeName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CONTENT_TYPE_NAME_ENUM", informationModel.ContentTypeNameEnum, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_NAME", informationModel.Name, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_SUPPRESSED", informationModel.Suppressed, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_PRIORITY", informationModel.Priority, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_TITLE", informationModel.Title, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_DESCRIPTION", informationModel.Description, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_PORTAL_ID", informationModel.PortalId, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_LINK_URL", informationModel.LinkUrl, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_DOWNLOAD_TYPE", informationModel.DownloadType, OracleDbType.Double, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_START_DATE", informationModel.StartDate, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_END_DATE", informationModel.EndDate, OracleDbType.TimeStamp, ParameterDirectionWrap.Input, 32767);



                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.ContentId = records.GetLongOrDefault("CONTENT_ID");
                        instance.CurrentVerssionID = records.GetLongOrDefault("CURRENT_VERSION_ID");
                        instance.FileName = records.GetStringOrDefault("FILE_NAME");
                    }
            );
            return infoModelObj;
        }

        /// <summary>
        /// Manage Information related files
        /// </summary>
        /// <param name="informationModel"></param>
        /// <returns></returns>
        internal static bool ManageInformationFiles(InformationModel informationModel)
        {
            bool result = false;
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                result,
               UserSchema.Portal + ".Manage_information_files",
                (DataAccess.Delegates.OracleParameterMapper)(parameter =>
{
    parameter.AddWithValue("P_CONTENT_ID", informationModel.ContentId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
    parameter.AddWithValue("P_VERSION_ID", informationModel.VersionId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
    parameter.AddWithValue("P_FILE_ID", informationModel.FileId, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
    parameter.AddWithValue("P_FILE_NAME", informationModel.FileName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
    parameter.AddWithValue("P_FILE_CONTENT", (object)informationModel.FileContent, OracleDbType.Blob, ParameterDirectionWrap.Input);
    parameter.AddWithValue("P_MIME_TYPE", informationModel.MimeType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
}),
                record =>
                {
                    result = true;
                }
            );
            return result;
        }


        /// <summary>
        /// Get download files list.
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="PageName"></param>
        /// <param name="ContentType"></param>
        /// <param name="UserType"></param>
        /// <param name="IsAdmin"></param>
        /// <returns></returns>
        public static List<InformationModel> GetDownloadList(int pageNum, int pageSize, string pageName, string contentType, int userType, int isAdmin, string downloadType)
        {

            List<InformationModel> InformationListObj = new List<InformationModel>();

            //Setup Procedure LIST_Infromation
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                InformationListObj,
               UserSchema.Portal + ".GET_DOWNLOAD_LIST",
                parameter =>
                {
                    parameter.AddWithValue("P_pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_pageNumber", pageNum, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_PageName", pageName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_ContentType", contentType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_UserType", userType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_IsAdmin", isAdmin, OracleDbType.Int16, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_DownloadType", downloadType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.ContentId = records.GetLongOrDefault("CONTENT_ID");
                        instance.Name = records.GetStringOrDefault("NAME");
                        instance.Priority = records.GetLongOrDefault("PRIORITY");
                        instance.Title = records.GetStringOrDefault("TITLE");
                        instance.ContentType = records.GetInt32OrDefault("CONTENT_TYPE");
                        instance.PriorityName = records.GetStringOrDefault("PRIORITY_NAME");
                        instance.TotalRecordCount = records.GetDecimalOrDefault("TotalRecordCount");
                        instance.PublishedDate = records.GetDateTimeOrDefault("uploaded_date");
                        instance.Suppressed = records.GetShortOrDefault("suppressed");
                        instance.DownloadType = records.GetInt32OrDefault("download_type");
                        instance.Description = records.GetStringOrDefault("description");
                        instance.FileName = records.GetStringOrDefault("FileInfo");
                        instance.LinkUrl = records.GetStringOrDefault("LINK_URL");
                        instance.VideoFileName = records.GetStringOrDefault("VIDEO_FILE_NAME");
                        instance.CurrentVerssionID = records.GetLongOrDefault("CURRENT_VERSION_ID");
                    }
            );

            return InformationListObj;
        }

        /// <summary>
        /// To fetch hot news
        /// </summary>
        /// <returns></returns>
        public static InformatinDetail GetHotNews(long userTypeId)
        {

            InformatinDetail infoDetal = new InformatinDetail();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                infoDetal,
               UserSchema.Portal + ".SP_FETCH_HOT_NEWS",
                parameter =>
                {
                    parameter.AddWithValue("p_portal_type", userTypeId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.HotNewsName = records.GetStringOrDefault("NAME");
                        instance.HotNewsContentId = records.GetLongOrDefault("CONTENT_ID");
                    }
            );

            return infoDetal;
        }

        /// <summary>
        /// Get information list
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="portalid"></param>
        /// <param name="SearchType"></param>
        /// <returns></returns>
        public static List<InformationModel> GetUniqueInfoList(int pageNum, int pageSize, int portalid, string SearchType)
        {
            List<InformationModel> InformationListObj = new List<InformationModel>();

            //Setup Procedure LIST_Infromation
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                InformationListObj,
               UserSchema.Portal + ".GET_UNIQUE_INFO_LIST_BY_PORTAL",
                parameter =>
                {
                    parameter.AddWithValue("pageNumber", pageNum, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_Portalid", portalid, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("SearchType", SearchType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (DataAccess.Delegates.RecordMapper<InformationModel>)((records, instance) =>
                    {
                        instance.ContentId = records.GetLongOrDefault("CONTENT_ID");
                        instance.Name = records.GetStringOrDefault("NAME");
                        instance.PriorityName = records.GetStringOrDefault("PRIORITY_NAME");
                        instance.TotalRecordCount = records.GetDecimalOrDefault("TotalRecordCount");
                        instance.Description = records.GetStringOrDefault("description");
                        instance.MimeType = records.GetStringOrDefault("mime_type");
                        instance.FileContent = records.GetByteArrayOrNull("file_content");
                        instance.FileCount = records.GetDecimalOrDefault("FileCount");
                        //instance.FromDate = (string.IsNullOrEmpty(Convert.ToString(records.GetDateTimeOrEmpty("START_DATE")))) ? string.Empty : Convert.ToDateTime(records.GetDateTimeOrEmpty("START_DATE")).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        instance.UploadedDate = records.GetDateTimeOrDefault("uploaded_date");
                    })
            );

            return InformationListObj;
        }
        /// <summary>
        /// Get Haulier Contact List
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<InformationModel> GetInformationListPortal(int pageNum, int pageSize, string contentType, long userType)
        {

            List<InformationModel> informationListObject = new List<InformationModel>();
            //Setup Procedure LIST_Infromation
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                informationListObject,
                 //"portal.GET_INFORMATION_LIST",
                 //parameter =>
                 //{
                 //    parameter.AddWithValue("P_pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                 //    parameter.AddWithValue("P_pageNumber", pageNum, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                 //    parameter.AddWithValue("P_PageName", pageName, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                 //    parameter.AddWithValue("P_ContentType", contentType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                 //    parameter.AddWithValue("P_UserTypeId", userType, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                 //    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                 //},
                 //    (records, instance) =>
                 //    {
                 //        instance.ContentId = records.GetLongOrDefault("CONTENT_ID");
                 //        instance.Name = records.GetStringOrDefault("NAME");
                 //        instance.Priority = records.GetLongOrDefault("PRIORITY");
                 //        instance.Title = records.GetStringOrDefault("TITLE");
                 //        instance.ContentType = records.GetInt32OrDefault("CONTENT_TYPE");
                 //        instance.PriorityName = records.GetStringOrDefault("PRIORITY_NAME");
                 //        instance.TotalRecordCount = records.GetDecimalOrDefault("TotalRecordCount");
                 //        instance.PublishedDate = records.GetDateTimeOrDefault("uploaded_date");
                 //        instance.Suppressed = records.GetShortOrDefault("suppressed");
                 //    }
                 UserSchema.Portal + ".GET_INFORMATION_LIST_BY_PORTAL",
                    parameter =>
                    {
                        parameter.AddWithValue("pageNumber", pageNum, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("pageSize", pageSize, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_Portalid", userType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("SearchType", contentType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                        (records, instance) =>
                        {
                            instance.ContentId = records.GetLongOrDefault("CONTENT_ID");
                            instance.Name = records.GetStringOrDefault("NAME");
                            instance.TotalRecordCount = records.GetDecimalOrDefault("TotalRecordCount");
                            instance.Description = records.GetStringOrDefault("description");
                            instance.MimeType = records.GetStringOrDefault("mime_type");
                            instance.FileContent = records.GetByteArrayOrNull("file_content");
                        }
            );

            return informationListObject;
        }
        #region Manage Favourites Haulier for vehicle and routes
        public static int ManageFavourites(int categoryId, int categoryType, int isFavourites)
        {
            int result = 0;
            SafeProcedure.DBProvider.Oracle.ExecuteNonQuery(
               UserSchema.Portal + ".SP_MANAGE_FAVOURITES",
                parameter =>
                {
                    parameter.AddWithValue("P_CATEGORY_ID", categoryId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_CATEGORY_TYPE", categoryType, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_IS_FAVOURITE", isFavourites, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("P_AFFECTED_ROWS", null, OracleDbType.Int32, ParameterDirectionWrap.Output);
                },
                records =>
                {
                    result = (records.GetInt32("P_AFFECTED_ROWS"));
                }
                );
            return result;
        }
        #endregion

        internal static List<InformatinDetail> GetHotNewsForAdmin(string SearchType)
        {
            List<InformatinDetail> InfoList = new List<InformatinDetail>();

            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                InfoList,
                UserSchema.Portal+".GET_HOT_NEWS_FOR_ADMIN",
                parameter =>
                {
                    parameter.AddWithValue("SearchType", SearchType, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("P_RESULTSET", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.PortalName = records.GetStringOrDefault("COMBINED_NEWS");
                        instance.HotNewsContentId = records.GetLongOrDefault("CONTENT_ID");
                    }
            );
            return InfoList;
        }
        /// <summary>
        /// Get Latest News list
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="portalid"></param>
        /// <param name="SearchType"></param>
        /// <returns></returns>
        public static List<LatestNews> GetLatestNews(int portalId, int timeInterval)
        {
            List<LatestNews> LatestNewsListObj = new List<LatestNews>();

            //Setup Procedure LIST_Infromation
            SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
                LatestNewsListObj,
               UserSchema.Portal + ".GET_LATEST_NEWS",
                parameter =>
                {
                    parameter.AddWithValue("P_Portalid", portalId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
                    parameter.AddWithValue("TimeInterval", timeInterval, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);

                    parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                },
                    (records, instance) =>
                    {
                        instance.ContentId = records.GetLongOrDefault("CONTENT_ID");
                        instance.UploadedDateTime = records.GetDateTimeOrDefault("uploaded_date");
                        instance.Title = records.GetStringOrDefault("TITLE");
                    }
            );

            return LatestNewsListObj;
        }
    }

}
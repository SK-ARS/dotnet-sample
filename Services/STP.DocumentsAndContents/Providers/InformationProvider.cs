using System;
using System.Collections.Generic;
using STP.DocumentsAndContents.Persistance;
using System.Diagnostics;
using STP.DocumentsAndContents.Interface;
using STP.Domain;
using STP.Domain.DocumentsAndContents;

namespace STP.DocumentsAndContents.Providers
{
    public sealed class InformationProvider : IInformation
    {
        #region ListHaulierContact Singleton

        public static InformationProvider Instance
        {
            [DebuggerStepThrough]
            get
            {
                return Nested.instance;
            }
        }

        /// <summary>
        /// Not to be called while using logic
        /// </summary>
        internal static class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
            internal static readonly InformationProvider instance = new InformationProvider();
        }

        #region Logger instance

        private const string PolicyName = "ListHaulierContact";

        #endregion

        #endregion

        /// <summary>
        /// Get Information List
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<InformationModel> GetInformationList(int pageNumber, int pageSize, string pageName, string contentType, int userType, int sortOrder, int presetFilter)
        {
            return InformationDAO.GetInformationList(pageNumber, pageSize, pageName, contentType, userType, sortOrder, presetFilter);
        }
        /// <summary>
        /// Manage information .insert and update operation.
        /// </summary>
        /// <param name="informationModel"></param>
        /// <returns></returns>
        public InformationModel ManageInformation(InformationModel informationModel)
        {
            return InformationDAO.ManageInformation(informationModel);
        }

        public List<InformationModel> GetInformationListPortal(int pageNumber, int pageSize, string contentType,long userType)
        {
            return InformationDAO.GetInformationListPortal(pageNumber, pageSize, contentType, userType);
        }
        /// <summary>
        /// Manage uploaded files for information
        /// </summary>
        /// <param name="informationModel"></param>
        /// <returns></returns>
        public bool ManageInformationFiles(InformationModel informationModel)
        {
            return InformationDAO.ManageInformationFiles(informationModel);
        }

        /// <summary>
        /// Delete Information by ContentId
        /// </summary>
        /// <param name="deletedContactId"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public int DeleteInformation(int deletedContentId)
        {
            return InformationDAO.DeleteInformation(deletedContentId);
        }
        /// <summary>
        ///Get Enum vals list by Enum Name
        /// </summary>
        /// <param name="ContentId"></param>
        /// <returns></returns>

        public List<InformationModel> GetEnumValsListByEnumType(string EnumTypeName)
        {
            return InformationDAO.GetEnumValsListByEnumType(EnumTypeName);
        }

        /// <summary>
        /// Get Associated files by Content Id
        /// </summary>
        /// <param name="ContentId"></param>
        /// <returns></returns>

        public List<WebContentFile> GetAssociatedFilesByContentId(int CONTENT_ID)
        {
            return InformationDAO.GetAssociatedFilesByContentId(CONTENT_ID);
        }
        /// <summary>
        /// Get portal by content id
        /// </summary>
        /// <param name="CONTENT_ID"></param>
        /// <returns></returns>
        public List<InformationModel> GetPortalContentById(int CONTENT_ID)
        {
            return InformationDAO.GetPortalContentById(CONTENT_ID);
        }

        /// <summary>
        /// Get Information by ContentId
        /// </summary>
        /// <param name="deletedContactId"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public InformationModel GetInformationById(int managedContentId)
        {
            return InformationDAO.GetInformationById(managedContentId);
        }

        public List<InformationModel> GetDownloadList(int pageNum, int pageSize, string pageName, string contentType, int userType, int isAdmin, string downloadType)
        {
            return InformationDAO.GetDownloadList(pageNum, pageSize, pageName, contentType, userType, isAdmin, downloadType);
        }
        /// <summary>
        /// Get all hot news
        /// </summary>
        /// <returns></returns>
        public InformatinDetail GetAllHotNews(long userTypeId)
        {
            return InformationDAO.GetHotNews(userTypeId);
        }
        /// <summary>
        /// Get Information List
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<InformationModel> GetUniqueInfoList(int pageNum, int pageSize, int portalid, string SearchType)
        {
            return InformationDAO.GetUniqueInfoList(pageNum, pageSize, portalid, SearchType);
        }

        public int ManageFavourites(int categoryId, int categoryType,int isFavourites)
        {
            return InformationDAO.ManageFavourites(categoryId, categoryType,isFavourites);
        }

        public List<InformatinDetail> GetHotNewsForAdmin(string SearchType)
        {
            return InformationDAO.GetHotNewsForAdmin(SearchType);
        }

        public List<LatestNews> GetLatestNews(int portalId, int timeInterval)
        {
            return InformationDAO.GetLatestNews(portalId, timeInterval);
        }
    }
}
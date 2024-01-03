using STP.Common.Constants;
using STP.Structures.Interface;
using STP.Domain;
using STP.Structures.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using STP.Domain.Structures;

namespace STP.Structures.Providers
{
    public class StructureProvider : IStructure
    {
        #region  StructureProvider Singleton
        public static StructureProvider Instance
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
            internal static readonly StructureProvider instance = new StructureProvider();
        }
        #endregion

        /// <summary>
        /// Get Caution list
        /// </summary>
        /// <param name="pageNumber">Page</param>
        /// <param name="pageSize"> size of page</param>
        /// <param name="StructureID">Structure Id </param>
        /// <returns>Return list of caution list</returns>
        ///

        public List<StructureModel> GetCautionList(int pageNumber, int pageSize, long structureID, long SectionID)
        {
            return StructureDAO.GetCautionList(pageNumber, pageSize, structureID, SectionID);
        }
        public StructureModel GetCautionDetails(long cautionID)
        {
            return StructureDAO.GetCautionDetails(cautionID);
        }
        public List<RoadDelegationList> GetRoadDelegationList(int pageNum, int pageSize, long organisationId)
        {
            return StructureDAO.GetRoadDelegationList(pageNum, pageSize, organisationId);
        }
        public bool SaveCautions(StructureModel Structuremodelalter)
        {
            return StructureDAO.SaveCautions(Structuremodelalter);
        }
        public bool UpdateStructureLog(List<StructureLogModel> structureLogsModel)
        {
            return StructureDAO.UpdateStructureLog(structureLogsModel);
        }
        public int DeleteCaution(long cautionId, string userName)
        {
            return StructureDAO.DeleteCaution(cautionId, userName);
        }
        public List<StructureModel> GetStructureHistory(int pageNumber, int pageSize, long StructureID)
        {
            return StructureDAO.GetStructureHistory(pageNumber, pageSize, StructureID);
        }
        public List<StructureHistoryList> GetStructHistoryById(int pageNumber, int pageSize, long StructureID)
        {
            return StructureDAO.GetStructHistoryById(StructureID,pageNumber, pageSize);
        }

        public int GetStructHistoryCount(long StructureID)
        {
            return StructureDAO.GetStructureHistoryCnt(StructureID);
        }
        //-------------Susmitha------------------------------------------

        /// <summary>
        /// Review contacts list
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Size of page</param>
        /// <param name="cautionId">Caution id</param>
        /// <param name="contactNo">Contact no</param>
        /// <returns>Get contact list</returns>
        public List<StructureContactModel> GetStructureContactList(int pageNumber, int pageSize, long cautionId, short contactNo = 0)
        {
            return StructureDAO.GetStructureContactList(pageNumber, pageSize, cautionId, contactNo);
        }
        public bool SaveStructureContact(StructureContactModel structureContact)
        {
            return StructureDAO.SaveStructureContact(structureContact);
        }
        public int DeleteStructureContact(short CONTACT_NO, long CautionId)
        {
            return StructureDAO.DeleteStructureContact(CONTACT_NO, CautionId);
        }
        public List<StructureNotification> GetAllStructureNotification(string structureId, int pageNum, int pageSize)
        {
            return StructureDAO.GetAllStructureNotification(structureId, pageNum, pageSize);
        }
        public List<StructureNotification> GetAllStructureOnerousVehicles(string structureId, int pageNumber, int pageSize, string searchCriteria, string searchStatus, DateTime? startDate, DateTime? endDate, int statusCount, int sort, long organisationId)
        {
            return StructureDAO.GetAllStructureOnerousVehicles(structureId, pageNumber, pageSize, searchCriteria, searchStatus, startDate, endDate, statusCount, sort, organisationId);
        }
    }
}
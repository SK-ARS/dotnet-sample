using STP.DocumentsAndContents.Persistance;
using STP.Domain;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace STP.DocumentsAndContents.Providers
{
    public class HaulierAddressProvider
    {
        #region ListHaulierContact Singleton

        public static HaulierAddressProvider Instance
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
            internal static readonly HaulierAddressProvider instance = new HaulierAddressProvider();
        }

    

        #endregion

        #region GetHaulierContactList implementation
        /// <summary>
        /// Get Haulier Contact List
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<HaulierContactModel> GetHaulierContactList(int organizationId, int pageNumber, int pageSize, string searchCriteria, string searchValue,int presetFilter, int? sortOrder = null)
        {
            return HaulierAddressDAO.GetHaulierContactList(organizationId, pageNumber, pageSize, searchCriteria, searchValue, presetFilter, sortOrder);
        }

        /// <summary>
        /// Get Haulier contact detail by HaulierContactId
        /// </summary>
        /// <param name="HAULIER_CONTACT_ID"></param>
        /// <returns></returns>
        public HaulierContactModel GetHaulierContactById(double haulierContactId)
        {
            return HaulierAddressDAO.GetHaulierContactById(haulierContactId);
        }
        public int DeleteHaulierContact(double haulierContactId)
        {
            return HaulierAddressDAO.DeleteHaulierContact(haulierContactId);
        }

        public bool ManageHaulierContact(HaulierContactModel haulierContactModel)
        {
            return HaulierAddressDAO.ManageHaulierContact(haulierContactModel);
        }
        #endregion
    }
}
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
    public class ContactProvider
    {
        protected ContactProvider()
        {

        }
        #region ListContact Singleton

        public static ContactProvider Instance
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
            internal static readonly ContactProvider instance = new ContactProvider();
        }

        public List<ContactListModel> GetContactListSearch(int organizationId, int pageNumber, int pageSize, int searchCriteria, string searchValue, int sortFlag, int presetFilter, int? sortOrder = null)
        {
            return ContactDAO.GetContactListSearch(organizationId, pageNumber, pageSize, searchCriteria, searchValue, sortFlag,presetFilter,sortOrder);
        }
        #endregion

    }
}
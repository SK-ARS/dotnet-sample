using STP.Domain;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.ServiceAccess.DocumentsAndContents
{
   public interface IContactService
    {
        List<ContactListModel> GetContactListSearch(int orgId, int pageNum, int pageSize, int searchCriteria, string searchValue, int sortFlag, int presetFilter, int? sortOrder = null);
        List<HaulierContactModel> GetHaulierContactList(int orgId, int pageNum, int pageSize, string searchCriteria, string searchValue, int presetFilter, int? sortOrder = null);
        HaulierContactModel GetHaulierContactById(double haulierContactId);
        bool ManageHaulierContact(HaulierContactModel haulierContactModel);
        int DeleteHaulierContact(double haulierContactId);
    }
}

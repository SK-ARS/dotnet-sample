using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.DocumentsAndContents.ServiceAccess
{
    public interface ICommunicationService
    {
        int SendMail(NotificationContacts objContact, UserInfo userInfo, long TransmId, byte[] content, byte[] attachment, string EsdalReference, int xmlAttach = 0, bool isImminent = false, string docTypeName = null, string replyToEmail = "noreply@esdal2.com");
        int SendFax(NotificationContacts objContact, UserInfo userInfo, long TransmId, byte[] content);
    }
}

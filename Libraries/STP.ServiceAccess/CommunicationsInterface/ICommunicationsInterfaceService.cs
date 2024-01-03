using STP.Domain.Communications;
using STP.Domain.SecurityAndUsers;

namespace STP.ServiceAccess.CommunicationsInterface
{
    public interface ICommunicationsInterfaceService
    {
        bool SendFax(NotificationContacts objContact, UserInfo userInfo, long TransmId, byte[] content);
        bool SendGeneralmail(CommunicationParams communicationParams);
        bool SendAutoResponseMail(string EsdalReference, string HaulierEmailAddress, string organisationName, MailResponse responseMailDetails);
    }
}

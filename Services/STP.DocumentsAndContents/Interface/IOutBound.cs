using STP.Common.Constants;
using STP.Domain.LoggingAndReporting;
using STP.Domain.SecurityAndUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.DocumentsAndContents.Interface
{
    public interface IOutBound
    {
        long SaveInboxItems(int NotificationID, long documentId, int OrganisationID, string esDAlRefNo, string userSchema = UserSchema.Portal, int icaStatus = 277001, bool ImminentMovestatus = false);
        //bool GenerateMovementAction(UserInfo UserSessionValue, string EsdalRef, MovementActionIdentifiers movActionItem, long projectId = 0, int revisionNo = 0, int versionNo = 0, int movFlagVar = 0, NotificationContacts objContact = null);
        //long SaveMovementActionForDistTrans(MovementActionIdentifiers movactiontype, string MovDescrp, string userSchema);
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.MovementsAndNotifications.Notification
{
    public class Notification
    {
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Please enter a valid name")]
        public string Name { get; set; }

        public string MovementClass { get; set; }

        public string SubMovementClass { get; set; }
        public string SubMovementClass1 { get; set; }
        public string SubMovementClass2 { get; set; }
        public string SubMovementClass3 { get; set; }
    }

    public class GeneralDetails
    {
        public long AnalysisId { get; set; }
        public long VersionId { get; set; }
        public string MovementName { get; set; }
        public string ESDALReference { get; set; }
        public string MyReference { get; set; }
        public string ClientName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Classification { get; set; }
        public byte[] NotesFromHaulier { get; set; }
        public string MovemntDateFrom { get; set; }
        public string MovemntDateTo { get; set; }
        public int NoOfMovements { get; set; }
        public int MaxPiecesPerLoad { get; set; }

        public string OrderString(string str, int ln)
        {
            int len = str.Length;

            if (len < ln)
            {
                int len1 = ln - len;
                for (int i = 1; i <= len1; i++)
                {
                    str = "0" + str;
                }
            }

            return (str);
        }
    }

    public class DetailedNotification
    {
        public string VR1ReferenceNo { get; set; }
        public string SOReferenceNo { get; set; }
        public string VSONo { get; set; }
    }

    #region code added by netweb
    public class NotificationHistoryModel
    {

        public string ESDALReference { get; set; }
        public DateTime NotificationDate { get; set; }
        public long NotificationID { get; set; }
        public long RevisionID { get; set; }
        public long VersionID { get; set; }
    }

    /// <summary>
    /// Notification Status Model list
    /// </summary>
    public class NotificationStatusModel
    {
        public string userSchema { get; set; }
        public string NOTIFICATION_CODE { get; set; }
        public long DOCUMENT_ID { get; set; }
        public long VERSION_ID { get; set; }
        public string FIRST_NAME { get; set; }
        public string SUR_NAME { get; set; }
        public string FAX { get; set; }
        public string EMAIL { get; set; }
        public long ORGANISATION_ID { get; set; }
        public string OrganisationName { get; set; }
        public string COMMUNICATION_METHOD { get; set; }
        public DateTime WHEN { get; set; }
        public int COLLABORATION_NO { get; set; }
        public long CONTACT_ID { get; set; }
        public long STATUS { get; set; }
        public string NOTES { get; set; }
        public string NotificationStatus { get; set; }
        public string StatusName { get; set; }
        public string InternalStatusName { get; set; }
        public decimal TotalRecordCount { get; set; }
        public int SeenBySort { get; set; }

        //Added by ajit
        public string InternalNotes { get; set; }
    }

    public class NotificationPrintModel
    {
        public long NotificationId { get; set; }

        public long ContactId { get; set; }
    }
    #endregion
}

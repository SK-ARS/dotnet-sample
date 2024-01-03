#region

using System;
using System.Diagnostics;

#endregion

namespace STP.Common.Mail
{
    public class ApplicationEmailInfo
    {
        public ApplicationEmailInfo()
        {
        }

        public int UserNameID { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

        public string MessageBody { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

        public string MessageType { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

        public string MessageFormat { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

        public string MessageFrom { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

        public string MessageTo { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

        public string MessageCc { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

        public string MessageBcc { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

        public string MessageSubject { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

        public string MessageSendMethod { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

        public int ReturnValue { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

        public bool SendImage { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

        public string ImageFormat { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

        public string ImageName { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

        public byte[] Image { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

        public string CountryTo { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

        public DateTime SendOn { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

        public bool ValidateIfDoNotSend { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

        public bool ValidateIfActivated { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

        public int CredentialsID { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

        public string ExtInfo { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

        public int AppID { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }
    }
}
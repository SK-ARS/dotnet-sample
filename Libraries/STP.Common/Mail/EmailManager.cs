#region

using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using STP.Common.Extensions;

#endregion

namespace STP.Common.Mail
{
    /// <summary>
    ///     Manages sending of a email
    /// </summary>
    public sealed class EmailManager
    {
        private readonly bool _defaultCredentials;
        private readonly string _domain;
        private readonly bool _enableDBEmail;
        private readonly bool _enableSSL;
        private readonly string _from;
        private readonly bool _isSMTPEnabled;
        private readonly string _password;
        private readonly string _smtpServerHost;
        private readonly int _smtpServerPort;
        private readonly string _username;

        #region Singleton

        private static volatile EmailManager instance;
        // Lock synchronization object
        private static readonly object SyncLock = new object();

        /// <summary>
        ///     Construct a email manager but load from configuration files.
        /// </summary>
        public EmailManager()
        {
            var config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            var settings = (MailSettingsSectionGroup) config.GetSectionGroup("system.net/mailSettings");

            if (settings != null)
            {
                _smtpServerHost = settings.Smtp.Network.Host;
                _smtpServerPort = settings.Smtp.Network.Port;
                _username = settings.Smtp.Network.UserName;
                _password = settings.Smtp.Network.Password;
                _from = settings.Smtp.From;
                _domain = settings.Smtp.Network.ClientDomain;
                _defaultCredentials = settings.Smtp.Network.DefaultCredentials;
            }
            _enableSSL = STPConfigurationManager.ConfigProvider.EmailConfigProvider.GetValue("EnableSSL").ToBool();
            _enableDBEmail = STPConfigurationManager.ConfigProvider.EmailConfigProvider.GetValue("EnableEmail").ToBool();
            _isSMTPEnabled = STPConfigurationManager.ConfigProvider.EmailConfigProvider.GetValue("SMTPEnabled").ToBool();
        }


        internal static EmailManager Instance
        {
            [DebuggerStepThrough]
            get
            {
                // Support multithreaded applications through 'Double checked locking' pattern which (once
                // the instance exists) avoids locking each time the method is invoked
                if (instance == null)
                {
                    lock (SyncLock)
                    {
                        if (instance == null)
                        {
                            instance = new EmailManager();
                        }
                    }
                }
                return instance;
            }
        }

        #region Logger instance

        private const string PolicyName = "EmailManagerProvider";
        //        private static readonly LogWrapper log = new LogWrapper();

        #endregion

        #endregion

        /// <summary>
        ///     Create a Email Manager that is configuration dependency free.
        /// </summary>
        /// <param name="smtpServerHost"></param>
        /// <param name="smtpServerPort"></param>
        public EmailManager(string smtpServerHost, int smtpServerPort)
        {
            var config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            var settings = (MailSettingsSectionGroup) config.GetSectionGroup("system.net/mailSettings");

            if (settings != null)
            {
                _smtpServerHost = settings.Smtp.Network.Host;
                _smtpServerPort = settings.Smtp.Network.Port;
                _username = settings.Smtp.Network.UserName;
                _password = settings.Smtp.Network.Password;
                _from = settings.Smtp.From;
                _domain = settings.Smtp.Network.ClientDomain;
                _defaultCredentials = settings.Smtp.Network.DefaultCredentials;
            }
            _enableSSL = STPConfigurationManager.ConfigProvider.EmailConfigProvider.GetValue("EnableSSL").ToBool();
            _enableDBEmail = STPConfigurationManager.ConfigProvider.EmailConfigProvider.GetValue("DBEnabled").ToBool();
            _isSMTPEnabled = STPConfigurationManager.ConfigProvider.EmailConfigProvider.GetValue("SMTPEnabled").ToBool();
        }

        public void SendErrorEmail(string message)
        {
            var mailMessage = new MailMessage
            {
                Body = message,
                From =
                    new MailAddress(
                        STPConfigurationManager.ConfigProvider.EmailConfigProvider.GetValue("SenderAddress"))
            };

            foreach (
                string address in
                    STPConfigurationManager.ConfigProvider.CommonSettingsProvider.GetValue(
                        "ApplicationErrorEmailTo").Split(';'))
                if (!string.IsNullOrEmpty(address)) mailMessage.To.Add(address);
            foreach (
                string address in
                    STPConfigurationManager.ConfigProvider.CommonSettingsProvider.GetValue(
                        "ApplicationErrorEmailCC").Split(';'))
                if (!string.IsNullOrEmpty(address)) mailMessage.CC.Add(address);
            foreach (
                string address in
                    STPConfigurationManager.ConfigProvider.CommonSettingsProvider.GetValue("BCC").Split(';'))
                if (!string.IsNullOrEmpty(address)) mailMessage.Bcc.Add(address);


            mailMessage.Subject = "Fx Client Online Error";
            mailMessage.IsBodyHtml = true;
            if (_isSMTPEnabled)
                Send(mailMessage);
        }

        public void Send(string message, string subject, string to, string cc, string bcc, char delimiter)
        {
            var mailMessage = new MailMessage
            {
                Body = message,
                From =
                    new MailAddress(STPConfigurationManager.ConfigProvider.EmailConfigProvider.GetValue("SenderAddress"))
            };

            if (!string.IsNullOrEmpty(to))
            {
                mailMessage.CC.Add(to);
            }

            if (!string.IsNullOrEmpty(cc))
            {
                mailMessage.Bcc.Add(cc);
            }
            mailMessage.Subject = subject;
            if (_isSMTPEnabled)
                Send(mailMessage);
        }

        /// <summary>
        ///     Synchronously send an email.  Keep in mind, you are at mercy of the smtp server.
        /// </summary>
        /// <param name="mailMessage"></param>
        public void Send(MailMessage mailMessage)
        {
            try
            {
                if (_isSMTPEnabled)
                {
                    using (var smtpClient = new SmtpClient(_smtpServerHost, _smtpServerPort))
                    {
                        /*
                        smtpClient.Credentials = new NetworkCredential {Domain = _domain, Password = _password, UserName = _username};
                        smtpClient.UseDefaultCredentials = _defaultCredentials;
                        */
                        smtpClient.Send(mailMessage);
                    }
                }
            }
            catch
            {
                if (_enableDBEmail)
                    SendDBMail(mailMessage, "STPOnline", 0);
            }
        }

        /// <summary>
        ///     Synchronously send an email.  Keep in mind, you are at mercy of the smtp server.
        /// </summary>
        /// <param name="mailMessage"></param>
        /// <param name="exception"> </param>
        public void Send(MailMessage mailMessage, Exception exception)
        {
            try
            {
                if (_isSMTPEnabled)
                {
                    using (var smtpClient = new SmtpClient(_smtpServerHost, _smtpServerPort))
                    {
                        string body = PopulateBody(mailMessage, exception);
                        mailMessage.Body = body;
                        smtpClient.Send(mailMessage);
                    }
                }
            }
            catch
            {
                if (_enableDBEmail)
                    SendDBMail(mailMessage, "STPOnline", 0);
            }
        }

        /// <summary>
        ///     Send email through database in release version
        /// </summary>
        /// <param name="mailMessage"></param>
        /// <param name="mailType"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int SendDBMail(MailMessage mailMessage, string mailType, int userId)
        {
            int retVal = 0;

            if (_enableDBEmail)
            {
                var mail = new ApplicationEmailInfo
                {
                    MessageBody = mailMessage.Body,
                    MessageType = mailType,
                    MessageFormat = "Text/HTML",
                    MessageFrom = mailMessage.From.ToString(),
                    MessageTo = mailMessage.To.ToString(),
                    MessageCc = mailMessage.CC.ToString(),
                    MessageBcc = mailMessage.Bcc.ToString(),
                    MessageSubject = mailMessage.Subject,
                    MessageSendMethod = "Email",
                    UserNameID = userId,
                    SendImage = false,
                    ImageFormat = string.Empty,
                    ImageName = string.Empty,
                    Image = null,
                    CountryTo = string.Empty,
                    SendOn = DateTime.Now,
                    ValidateIfDoNotSend = false,
                    ValidateIfActivated = false,
                    CredentialsID = 0,
                    ExtInfo = string.Empty
                };

                string connectionString =
                    STPConfigurationManager.ConfigProvider.DatabaseConfigProvider.DatabaseGroupCollectionItems[
                        "SQL"].Databases["mainserver1"].ConnectionString;
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // Create a command and prepare it for execution
                    var command = new SqlCommand
                    {
                        CommandText = "dbo.sys_sp_CommServer_Message_ToSend_Setup",
                        // Set the command text (stored procedure name or SQL statement)
                        CommandType = CommandType.StoredProcedure, // Set the command type
                        Connection = connection // Associate the connection with the command
                    };
                    var outParam = new SqlParameter("@fRetVal", mail.ReturnValue)
                    {Direction = ParameterDirection.Output};
                    command.Parameters.Add(outParam);
                    command.Parameters.AddWithValue("@fMessage", mail.MessageBody);
                    command.Parameters.AddWithValue("@fMessageType", mail.MessageType);
                    command.Parameters.AddWithValue("@fMessageFormat", mail.MessageFormat);
                    command.Parameters.AddWithValue("@fMessageFrom", mail.MessageFrom);
                    command.Parameters.AddWithValue("@fMessageTo",
                        string.IsNullOrEmpty(mail.MessageTo) ? " " : mail.MessageTo);
                    command.Parameters.AddWithValue("@fMessageCc",
                        string.IsNullOrEmpty(mail.MessageCc) ? " " : mail.MessageCc);
                    command.Parameters.AddWithValue("@fMessageBcc",
                        string.IsNullOrEmpty(mail.MessageBcc) ? " " : mail.MessageBcc);
                    command.Parameters.AddWithValue("@fMessageSubject",
                        string.IsNullOrEmpty(mail.MessageSubject) ? DBNull.Value : (object) mail.MessageSubject);
                    command.Parameters.AddWithValue("@fMessageSendMethod", mail.MessageSendMethod);
                    command.Parameters.AddWithValue("@fUserNameID", mail.UserNameID);
                    command.Parameters.AddWithValue("@bSendImage", mail.SendImage);
                    command.Parameters.AddWithValue("@fImageFormat",
                        string.IsNullOrEmpty(mail.ImageFormat) ? DBNull.Value : (object) mail.ImageFormat);
                    command.Parameters.AddWithValue("@fImageName",
                        string.IsNullOrEmpty(mail.ImageName) ? DBNull.Value : (object) mail.ImageName);
                    var imgParam = new SqlParameter("@Image", (mail.Image != null) ? (object) mail.Image : DBNull.Value)
                    {
                        Direction = ParameterDirection.Input,
                        DbType = DbType.Binary
                    };
                    command.Parameters.Add(imgParam);
                    command.Parameters.AddWithValue("@sCountryTo",
                        string.IsNullOrEmpty(mail.CountryTo) ? DBNull.Value : (object) mail.CountryTo);
                    command.Parameters.AddWithValue("@dSendOn", mail.SendOn);
                    command.Parameters.AddWithValue("@bValidateIfDoNotSend", mail.ValidateIfDoNotSend);
                    command.Parameters.AddWithValue("@bValidateIfActivated", mail.ValidateIfActivated);
                    command.Parameters.AddWithValue("@lCredentialsID", mail.CredentialsID);
                    command.Parameters.AddWithValue("@sExtInfo",
                        string.IsNullOrEmpty(mail.ExtInfo) ? DBNull.Value : (object) mail.ExtInfo);

                    // Finally, execute the command
                    int recsAffected = command.ExecuteNonQuery();
                    retVal = command.Parameters["@fRetVal"].Value.ToInt();

                    // Detach the SqlParameters from the command object, so they can be used again
                    command.Parameters.Clear();
                    connection.Close();
                }
            }
            return retVal;
        }

        #region Helper Methods

        private string PopulateBody(MailMessage mailMessage, Exception exception)
        {
            string version = STPConfigurationManager.ConfigProvider.ServiceProvider.BuildProvider.BuildVersion;
            string body;
            using (
                var reader = new StreamReader(HttpContext.Current.Server.MapPath("~/External/Error/EmailTemplate.html"))
                )
            {
                body = reader.ReadToEnd();
            }
            if (!string.IsNullOrEmpty(body) && exception != null)
            {
                body = body.Replace("{Title}", mailMessage.Subject);
                body = body.Replace("{Url}", HttpContext.Current.Request.CurrentExecutionFilePath);
                body = body.Replace("{StackTrace}", exception.StackTrace);
                body = body.Replace("{Version}", version);
            }
            return body;
        }

        #endregion
    }
}
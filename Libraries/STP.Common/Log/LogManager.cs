#region

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using STP.Common.Configuration;
using STP.Common.Mail;

#endregion

namespace STP.Common.Log
{
    public sealed class LogManager
    {
        #region Singleton

        private static volatile LogManager instance;
        // Lock synchronization object
        private static readonly object SyncLock = new object();

        public static LogManager Instance
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
                            instance = new LogManager();
                        }
                    }
                }
                return instance;
            }
        }

        #region Logger instance

        private const string PolicyName = "LogManagerProvider";
        //        private static readonly LogWrapper log = new LogWrapper();

        #endregion

        #endregion

        #region Members

        public void Reset()
        {
            var loggingSettings = (LoggingSettings) ConfigurationManager.GetSection("loggingConfiguration");
            var objRollingFlatFileTraceListenerData =
                loggingSettings.TraceListeners.Get("Rolling Flat File Trace Listener") as
                    RollingFlatFileTraceListenerData;
            if (objRollingFlatFileTraceListenerData != null)
            {
                string sLogFile = objRollingFlatFileTraceListenerData.FileName;
                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Writer.Dispose();
                File.Delete(sLogFile);
                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Reset();
            }
        }

        public void Log(LogEntry log)
        {
            if (STPConfig.Instance.EnableLog)
                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(log);
        }

        public void Log(object message, ICollection<string> categories, int priority, int eventId,
            TraceEventType severity, string title, IDictionary<string, object> properties)
        {
            if (STPConfig.Instance.EnableLog)
                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, categories, priority, eventId, severity, title, properties);
        }

        public void Log(object message, ICollection<string> categories, int priority,
            IDictionary<string, object> properties)
        {
            if (STPConfig.Instance.EnableLog)
                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, categories, priority, properties);
        }

        public void Log(object message, ICollection<string> categories, IDictionary<string, object> properties)
        {
            if (STPConfig.Instance.EnableLog)
                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, categories, properties);
        }

        public void Log(object message, ICollection<string> categories, int priority, int eventId,
            TraceEventType severity, string title)
        {
            if (STPConfig.Instance.EnableLog)
                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, categories, priority, eventId, severity, title);
        }

        public void Log(object message, ICollection<string> categories, int priority, int eventId,
            TraceEventType severity)
        {
            if (STPConfig.Instance.EnableLog)
                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, categories, priority, eventId, severity);
        }

        public void Log(object message, ICollection<string> categories, int priority, int eventId)
        {
            if (STPConfig.Instance.EnableLog)
                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, categories, priority, eventId);
        }

        public void Log(object message, ICollection<string> categories, int priority)
        {
            if (STPConfig.Instance.EnableLog)
                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, categories, priority);
        }

        public void Log(object message, ICollection<string> categories)
        {
            if (STPConfig.Instance.EnableLog)
                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, categories);
        }

        public void Log(object message, string category, int priority, int eventId, TraceEventType severity,
            string title, IDictionary<string, object> properties)
        {
            if (STPConfig.Instance.EnableLog)
                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, category, priority, eventId, severity, title, properties);
        }

        public void Log(object message, string category, int priority, IDictionary<string, object> properties)
        {
            if (STPConfig.Instance.EnableLog)
               Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, category, priority, properties);
        }

        public void Log(object message, string category, IDictionary<string, object> properties)
        {
            if (STPConfig.Instance.EnableLog)
               Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, category, properties);
        }

        public void Log(object message, IDictionary<string, object> properties)
        {
            if (STPConfig.Instance.EnableLog)
               Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, properties);
        }

        public void Log(object message, string category, int priority, int eventId, TraceEventType severity,
            string title)
        {
            if (STPConfig.Instance.EnableLog)
               Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, category, priority, eventId, severity, title);
        }

        public void Log(object message, string category, int priority, int eventId, TraceEventType severity)
        {
            if (STPConfig.Instance.EnableLog)
               Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, category, priority, eventId, severity);
        }

        public void Log(object message, string category, int priority, int eventId)
        {
            if (STPConfig.Instance.EnableLog)
               Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, category, priority, eventId);
        }

        public void Log(object message, string category, int priority)
        {
            if (STPConfig.Instance.EnableLog)
               Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, category, priority);
        }

        public void Log(object message, string category)
        {
            if (STPConfig.Instance.EnableLog)
               Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, category);
        }

        public void Log(object message)
        {
            if (STPConfig.Instance.EnableLog)
               Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message);
        }

        public void LogAndEmail(object message, string category, int priority, int eventId,
            TraceEventType severity, int userId)
        {
            if (STPConfig.Instance.EnableLog)
            {
               Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, category, priority, eventId, severity);
            }

            var exception = (Exception) message;
            var sendFrom = STPConfigurationManager.ConfigProvider.EmailConfigProvider.GetValue("MailFrom");
            var sendTo = STPConfigurationManager.ConfigProvider.EmailConfigProvider.GetValue("MailTo");
            var sendCC = STPConfigurationManager.ConfigProvider.EmailConfigProvider.GetValue("MailCC");
            var sendBCC = STPConfigurationManager.ConfigProvider.EmailConfigProvider.GetValue("MailBCC");
            var mailMessage = new MailMessage
            {
                Body =
                    string.Format("{0} :: {1} : Trace:: {2}::Message:::{3}", category, severity,
                        exception.StackTrace, exception.Message),
                From = new MailAddress(sendFrom),
                Subject =
                    string.Format("STPOnline Application Error : {0}.....",
                        exception.Message.Length > 100
                            ? exception.Message.Substring(0, 100)
                            : exception.Message)
            };


            mailMessage.To.Add(sendTo);
            if (!string.IsNullOrEmpty(sendCC))
            {
                mailMessage.CC.Add(sendCC);
            }

            if (!string.IsNullOrEmpty(sendBCC))
            {
                mailMessage.Bcc.Add(sendBCC);
            }
            mailMessage.IsBodyHtml = true;
            EmailManager.Instance.SendDBMail(mailMessage, "HTML", userId);
        }

        public void LogAndSmtpEmail(object message, string category, int priority, int eventId,
            TraceEventType severity, int userId)
        {
            if (STPConfig.Instance.EnableLog)
            {
               Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, category, priority, eventId, severity);
            }

            var exception = (Exception) message;
            var sendFrom = STPConfigurationManager.ConfigProvider.EmailConfigProvider.GetValue("MailFrom");
            var sendTo = STPConfigurationManager.ConfigProvider.EmailConfigProvider.GetValue("MailTo");
            var sendCC = STPConfigurationManager.ConfigProvider.EmailConfigProvider.GetValue("MailCC");
            var sendBCC = STPConfigurationManager.ConfigProvider.EmailConfigProvider.GetValue("MailBCC");
            var mailMessage = new MailMessage
            {
                Body = string.Format("{0} :: {1} : {2}", category, severity.ToString(), exception.StackTrace),
                Subject = string.Format("STPOnline Application Error : {0}", exception.Message),
                From = new MailAddress(sendFrom)
            };
            mailMessage.To.Add(sendTo);
            if (!string.IsNullOrEmpty(sendCC))
            {
                mailMessage.CC.Add(sendCC);
            }

            if (!string.IsNullOrEmpty(sendBCC))
            {
                mailMessage.Bcc.Add(sendBCC);
            }
            mailMessage.IsBodyHtml = true;
            EmailManager.Instance.Send(mailMessage, exception);
        }

        //public  MailAddressCollection GetCollection(string col)
        //{
        //    var mailCol = new MailAddressCollection();
        //    if (!string.IsNullOrEmpty(col))
        //    {
        //        string[] mailAdds = col.Split(",".ToCharArray());
        //        if (mailAdds.Length > 0)
        //        {
        //            foreach (var mailAdd in mailAdds)
        //            {
        //                mailCol.Add(new MailAddress(mailAdd));
        //            }
        //        }
        //    }
        //    return mailCol;
        //}

        public void LogAndEmail(object message)
        {
            try
            {
                var sendFrom = STPConfigurationManager.ConfigProvider.EmailConfigProvider.GetValue("MailFrom");
                var sendTo = STPConfigurationManager.ConfigProvider.EmailConfigProvider.GetValue("MailTo");
                var sendCC = STPConfigurationManager.ConfigProvider.EmailConfigProvider.GetValue("MailCC");
                var sendBCC = STPConfigurationManager.ConfigProvider.EmailConfigProvider.GetValue("MailBCC");

                var ex = (Exception) message;
                var mailMessage = new MailMessage
                {
                    Body = string.Format("Error : {0}", ex.StackTrace),
                    Subject = string.Format("STPOnline Application Error: {0}", ex.Message),
                    From = new MailAddress(sendFrom)
                };
                mailMessage.To.Add(sendTo);
                mailMessage.CC.Add(sendCC);
                mailMessage.Bcc.Add(sendBCC);
                EmailManager.Instance.SendDBMail(mailMessage, "HTML", 0);
            }
            catch
            {
                var mailMessage = new MailMessage("STPonlinesupport@riafinancial.com", "pnayak@riafinancial.com")
                {
                    Body = string.Format("Error : {0}", message),
                    Subject = "STPOnline Application Error"
                };
                EmailManager.Instance.SendDBMail(mailMessage, "HTML", 0);
            }
        }

        #endregion

        //public  void Log(string message)
        //{
        //    var logEntry = new LogEntry
        //                       {
        //                           Message = message
        //                       };

        //    if (STPConfig.Instance.EnableLog)
        //       Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(logEntry);
        //}

        //public static void Log(Exception exception)
        //{
        //    var logEntry = new LogEntry
        //                       {
        //                           Title = exception.Message,
        //                           Message =
        //                               string.Format("File:: {0}{2}\tExcepion {1}",
        //                                             HttpContext.Current.Request.CurrentExecutionFilePath,
        //                                             exception.StackTrace, Environment.NewLine)
        //                       };

        //    if (STPConfig.Instance.EnableLog)
        //       Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(logEntry);
        //}
    }
}
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace STP.Common.Configuration.Interface
{
    public interface ILogWrapper
    {
        void Reset();

        void Log(LogEntry log);

        void Log(object message, ICollection<string> categories, int priority, int eventId,
            TraceEventType severity, string title, IDictionary<string, object> properties);

        void Log(object message, ICollection<string> categories, int priority,
            IDictionary<string, object> properties);

        void Log(object message, ICollection<string> categories, IDictionary<string, object> properties);

        void Log(object message, ICollection<string> categories, int priority, int eventId,
            TraceEventType severity, string title);

        void Log(object message, ICollection<string> categories, int priority, int eventId,
            TraceEventType severity);

        void Log(object message, ICollection<string> categories, int priority, int eventId);

        void Log(object message, ICollection<string> categories, int priority);

        void Log(object message, ICollection<string> categories);

        void Log(object message, string category, int priority, int eventId, TraceEventType severity,
            string title, IDictionary<string, object> properties);

        void Log(object message, string category, int priority, IDictionary<string, object> properties);

        void Log(object message, string category, IDictionary<string, object> properties);

        void Log(object message, IDictionary<string, object> properties);

        void Log(object message, string category, int priority, int eventId, TraceEventType severity,
            string title);

        void Log(object message, string category, int priority, int eventId, TraceEventType severity);

        void Log(object message, string category, int priority, int eventId);

        void Log(object message, string category, int priority);

        void Log(object message, string category);

        void Log(object message);

        //void LogAndEmail(object message, string category, int priority, int eventId,
        //   TraceEventType severity, int userId);

        //void LogAndSmtpEmail(object message, string category, int priority, int eventId,
        //  TraceEventType severity, int userId)
    }
}
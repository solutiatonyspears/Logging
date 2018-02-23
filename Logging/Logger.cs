using System;
using System.Diagnostics.Tracing;

namespace Solutia.Logging
{
    [EventSource(Name = "Logger")]      //You'll need to change this if you need to name your event source something different.
    public partial class Logger : EventSource
    {   
        public static class Keywords
        {
            public const EventKeywords Performance = (EventKeywords)1;
            public const EventKeywords Diagnostic = (EventKeywords)2;
            public const EventKeywords Security = (EventKeywords)4;
            public const EventKeywords Generic = (EventKeywords)8;
        }

        public static class Tasks
        {
            public const EventTask Initialize = (EventTask)1;
            public const EventTask Tracing = (EventTask)2;
        }

        public static class Opcodes
        {
            public const EventOpcode Started = (EventOpcode)20;
            public const EventOpcode Finish = (EventOpcode)21;
            public const EventOpcode Error = (EventOpcode)22;
            public const EventOpcode Starting = (EventOpcode)23;
        }

        private static readonly Lazy<Logger> Instance = new Lazy<Logger>(() => new Logger());

        private Logger() { }

        public static Logger Log
        {
            get { return Instance.Value; }
        }

        [Event(101, Message = "Application starting.", Level = EventLevel.Informational, Keywords = Keywords.Performance, Task = Tasks.Tracing, Opcode = Opcodes.Starting)]
        public void AppStarting()
        {
            if (IsEnabled()) this.WriteEvent(101);
        }

        [Event(102, Message = "Application started.", Level = EventLevel.Informational, Keywords = Keywords.Performance, Task = Tasks.Tracing, Opcode = Opcodes.Started)]
        public void AppStarted()
        {
            if (IsEnabled()) this.WriteEvent(102);
        }


        [Event(201, Message = "Application shutdown.", Level = EventLevel.Informational, Keywords = Keywords.Performance)]
        public void AppShutdown()
        {
            if (IsEnabled()) this.WriteEvent(201);
        }

        [Event(301, Message = "User {0} logged in.", Level = EventLevel.Informational, Keywords = Keywords.Security)]
        public void UserLoggedIn(string userName)
        {
            if (IsEnabled()) this.WriteEvent(301, userName);
        }

        [Event(302, Message = "User {0} logged out.", Level = EventLevel.Informational, Keywords = Keywords.Security)]
        public void UserLoggedOut(string userName)
        {
            if (IsEnabled()) this.WriteEvent(302, userName);
        }

        [Event(303, Message = "Login rejected. Username {0}, password {1}.", Level = EventLevel.Informational, Keywords = Keywords.Security)]
        public void UserLoginRejected(string userName, string password)
        {
            if (IsEnabled()) this.WriteEvent(303, userName, password);
        }

        //We need these non-event methods because the HTTPResponse methods only take basic types, not custom classes.
        [NonEvent]
        public void HttpResponse(HttpMessageLogEntry logEntry)
        {
            HttpResponse(logEntry.TaskId, logEntry.RequestInfo, logEntry.Message);
        }

        [Event(401, Message = "RequestId {0}\r\nRequest: {1}\r\n{2}", Level = EventLevel.Verbose, Keywords = Keywords.Diagnostic)]
        protected void HttpResponse(string requestId, string requestInfo, string message)
        {
            if (IsEnabled()) this.WriteEvent(401, requestId, requestInfo, message);
        }

        [NonEvent]
        public void HttpRequest(HttpMessageLogEntry logEntry)
        {
            HttpRequest(logEntry.TaskId, logEntry.RequestInfo, logEntry.Message);
        }

        [Event(402, Message = "ResponseId {0}\r\nResponse: {1}\r\n{2}", Level = EventLevel.Verbose, Keywords = Keywords.Diagnostic)]
        protected void HttpRequest(string responseId, string responseInfo, string message)
        {
            if (IsEnabled()) this.WriteEvent(402, responseId, responseInfo, message);
        }

        //The following are catch-all log entries and should only be used if none of the existing methods are suitable.
        [Event(900, Message = "{0}", Level = EventLevel.Critical, Keywords = Keywords.Generic)]
        public void Critical(string message)
        {
            if (IsEnabled()) WriteEvent(900, message);
        }

        [Event(901, Message = "{0}", Level = EventLevel.Error, Keywords = Keywords.Generic)]
        public void Error(string message)
        {
            if (IsEnabled()) WriteEvent(901, message);
        }

        [Event(902, Message = "{0}", Level = EventLevel.Warning, Keywords = Keywords.Generic)]
        public void Warning(string message)
        {
            if (IsEnabled()) WriteEvent(902, message);
        }

        [Event(903, Message = "{0}", Level = EventLevel.Informational, Keywords = Keywords.Generic)]
        public void Information(string message)
        {
            if (IsEnabled()) WriteEvent(903, message);
        }

        [NonEvent]
        public void Exception(Exception ex)
        {
            Exception(ex.Message, ex.StackTrace);
        }

        [Event(904, Message = "Message: '{0}', Stack Trace: '{1}'", Level = EventLevel.Error, Keywords = Keywords.Diagnostic)]
        protected void Exception(string message, string stackTrace)
        {
            if (IsEnabled()) WriteEvent(904, message, stackTrace);
        }
    }
}


using Solutia.Logging.Interfaces;
using System;

namespace Logging.Classes
{
    public class LogMessage : ILogMessage
    {
        public LogMessage(string message, EventLevel eventLevel)
        {
            Message = message;
            EventLevel = eventLevel;
        }

        public EventLevel EventLevel { get; private set; }
        public string Message {get; private set;}
    }

    public class ExceptionLogMessage:LogMessage
    {
        public ExceptionLogMessage(Exception ex, EventLevel eventLevel = EventLevel.Error):base(ex?.Message, eventLevel)
        {
            this.Exception = ex;
        }

        public Exception Exception { get; private set; }
    }
}

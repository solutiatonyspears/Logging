using Solutia.Logging.Interfaces;

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
}

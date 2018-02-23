using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solutia.Logging.Interfaces
{
    //Each log entry component represents one "field" in a log entry. Configure a log sink to take any combination of these components.
    public enum LogEntryComponent { LogLevel, LoggerName, Message, Date, LongDate, MachineName, UserName, Time, StackTrace, LogId }

    //Each log level represents the minimum log severity the sink will accept.
    public enum EventLevel { Always, Debug, Trace, Information, Warning, Error, Fatal, Off };

    //A Log Sink is a destination for log entries.
    public interface ILogSink
    {
        string Name { get; }
        EventLevel MinimumEventLevel { get; }
        IList<LogEntryComponent> LogEntryConfiguration { get; }
    }
}

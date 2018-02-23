using Solutia.Logging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Classes
{
    public class EventLogSink : ILogSink
    {
        public string Name { get; set; }
        public EventLevel MinimumEventLevel { get; set; }
        public string Source { get; set; }
        public string Log { get; set; }
        public IList<LogEntryComponent> LogEntryConfiguration { get; set; }
    }
}

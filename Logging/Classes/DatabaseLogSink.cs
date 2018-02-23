using Solutia.Logging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Classes
{
    public class DbLogSinkCommandParameter
    {
        public string Name { get; set; }
        public LogEntryComponent LogEntryComponent { get; set; }
    }

    public class DatabaseLogSink:ILogSink
    {
        public string ConnectionString { get; set; }
        public string CommandText { get; set;}

        public IList<DbLogSinkCommandParameter> Parameters { get; set; }

        public string Name { get; set; }

        public EventLevel MinimumEventLevel { get; set; }

        public IList<LogEntryComponent> LogEntryConfiguration => throw new NotImplementedException();
    }
}

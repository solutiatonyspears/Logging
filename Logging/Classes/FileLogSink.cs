using Solutia.Logging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Classes
{
    public class FileLogSink : ILogSink
    {
        public string Name { get; set; }
        public EventLevel MinimumEventLevel { get; set; }
        public string FileName { get; set; }
        public IList<LogEntryComponent> LogEntryConfiguration { get; set; }
        public bool ReplaceFileContents { get; set; }
    }
}

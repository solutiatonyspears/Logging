using Solutia.Logging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Implementation
{
    public abstract class LogWriterBase : ILogWriter
    {
        public EventHandler<ILogMessage> OnLogEntryCreated = delegate { };
        public abstract ILogWriter Configure(IEnumerable<ILogSink> configurations);
        public virtual ILogWriter Log(ILogMessage message)
        {
            OnLogEntryCreated(this, message);
            return this;
        }
    }
}

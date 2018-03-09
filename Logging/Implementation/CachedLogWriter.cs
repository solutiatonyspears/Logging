using Logging.Implementation;
using Logging.Interfaces;
using Solutia.Logging.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Solutia.Logging.Nlog.Implementation
{
    public abstract class CachedLogWriter : LogWriterBase, ICachedLogWriter
    {
        private ConcurrentBag<ILogMessage> cachedMessages = new ConcurrentBag<ILogMessage>();

        public int NumberOfLogsToCache { get; set; }

        public IEnumerable<ILogMessage> Logs
        {
            get
            {
                return cachedMessages.ToList();
            }
        }

        public void Flush()
        {
            var lockObject = new Object();

            lock (lockObject)
            {
                cachedMessages = new ConcurrentBag<ILogMessage>();
            }
        }

        public override ILogWriter Log(ILogMessage message)
        {
            cachedMessages.Add(message);
            return base.Log(message);
        }
    }
}

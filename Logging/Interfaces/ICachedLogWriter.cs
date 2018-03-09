using Solutia.Logging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Interfaces
{
    public interface ICachedLogWriter
    {
        int NumberOfLogsToCache { get; set; }
        IEnumerable<ILogMessage> Logs { get; }
        void Flush();
    }
}

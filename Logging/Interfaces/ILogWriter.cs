using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solutia.Logging.Interfaces
{
    public interface ILogWriter
    {
        ILogWriter Log(ILogMessage message);
        ILogWriter Configure(IEnumerable<ILogSink>configurations);
    }
}

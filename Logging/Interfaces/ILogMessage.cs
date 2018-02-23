using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solutia.Logging.Interfaces
{
    public interface ILogMessage
    {
        string Message { get; }
        EventLevel EventLevel { get; }
    }
}

using Logging.Classes;
using Solutia.Logging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.WindowsEventViewerTests
{
    public class ApplicationLogMessage:LogMessage
    {
        public string ApplicationName { get; private set; }

        public ApplicationLogMessage(string applicationName, string message, EventLevel eventLevel):base( message, eventLevel)
        {
            ApplicationName = applicationName;
        }
    }
}

using NLog.Layouts;
using Solutia.Logging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solutia.Logging.Nlog.Implementation
{
    internal class ParameterLayoutBuilder
    {
        public Layout BuildSimpleLayout(IEnumerable<LogEntryComponent> componentTypes)
        {
            StringBuilder sb = new StringBuilder();
            foreach(var componentType in componentTypes)
            {
                sb.Append(BuildSimpleLayout(componentType));
                sb.Append(" ");
            }

            return sb.ToString().TrimEnd();
        }

        public Layout BuildSimpleLayout(LogEntryComponent componentType)
        {
            switch(componentType)
            {
                case LogEntryComponent.Date: return "${date}";
                case LogEntryComponent.LoggerName: return "${name}";
                case LogEntryComponent.LogLevel: return "${level}";
                case LogEntryComponent.LongDate: return "${longdate}";
                case LogEntryComponent.MachineName: return "${machinename}";
                case LogEntryComponent.Message: return "${message}";
                case LogEntryComponent.StackTrace: return "${stacktrace}";
                case LogEntryComponent.Time: return "${time}";
                case LogEntryComponent.UserName: return "${windows-identity}";
                case LogEntryComponent.ApplicationName: return "${processname}";
                default: return "${literal}";
            }
        }
    }
}

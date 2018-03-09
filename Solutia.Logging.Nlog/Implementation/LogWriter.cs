using Logging.Classes;
using Logging.Implementation;
using NLog;
using NLog.Config;
using NLog.Targets;
using Solutia.Logging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solutia.Logging.Nlog.Implementation
{
    public class LogWriter : LogWriterBase
    {
        private static NLog.Logger logger = LogManager.GetCurrentClassLogger();

        public LogWriter(string name)
        {
            logger = LogManager.GetLogger(name);
        }

        private static readonly ParameterLayoutBuilder simpleParamLayoutBuilder = new ParameterLayoutBuilder();

        private void SetTargetConfiguration(Target target, LoggingConfiguration config)
        {
            NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(target, LogLevel.Trace);
            config.AddTarget(target);

            var rule = new LoggingRule("*", LogLevel.Trace, target);
            config.LoggingRules.Add(rule);
            config.AddTarget(target);
        }

        public override ILogWriter Configure(IEnumerable<ILogSink> configs)
        {
            var configuration = new LoggingConfiguration();

            foreach(var config in configs)
            {
                if(config is DatabaseLogSink dbConfig)
                {
                    var dbParams = new List<DatabaseParameterInfo>();

                    var dbTarget = new DatabaseTarget()
                    {
                        Name = dbConfig.Name,
                        ConnectionString = dbConfig.ConnectionString,
                        CommandText = dbConfig.CommandText,
                        CommandType = MapCommandType(dbConfig.CommandType)
                    };

                    //Map parameters. 
                    var layoutBuilder = new ParameterLayoutBuilder();
                    foreach (var parameter in dbConfig.Parameters)
                    {
                        dbTarget.Parameters.Add(new DatabaseParameterInfo()
                        {
                            Name = parameter.Name.StartsWith("@")?parameter.Name:"@" + parameter.Name,              //!!!This assumes we're using SQL Server. Probably need to create a converter for other supported database types.
                            Layout = simpleParamLayoutBuilder.BuildSimpleLayout(parameter.LogEntryComponent)
                        });
                    }

                    SetTargetConfiguration(dbTarget, configuration);
                }
                else if(config is EventLogSink elConfig)
                {
                    var layoutBuilder = new ParameterLayoutBuilder();

                    var elTarget = new EventLogTarget()
                    {
                        Name = elConfig.Source,
                        Log = elConfig.Source,
                        Layout = simpleParamLayoutBuilder.BuildSimpleLayout(elConfig.LogEntryConfiguration),
                        Source = elConfig.Source
                    };

                    SetTargetConfiguration(elTarget, configuration);
                }
                else if(config is FileLogSink flConfig)
                {
                    var fileTarget = new FileTarget()
                    {
                        Name = flConfig.Name,
                        FileName = flConfig.FileName,
                        CreateDirs = true,
                        Layout = simpleParamLayoutBuilder.BuildSimpleLayout(flConfig.LogEntryConfiguration),
                    };

                    SetTargetConfiguration(fileTarget, configuration);
                }

                LogManager.Configuration = configuration;
            }

            return this;
        }

       private System.Data.CommandType MapCommandType(CommandType commandType)
        {
            if(commandType == CommandType.StoredProcedure)
            {
                return System.Data.CommandType.StoredProcedure;
            }
            else if(commandType == CommandType.Table)
            {
                return System.Data.CommandType.Text;
            }
            else
            {
                throw new Exception("Unknown command type.");
            }
        }

        private LogLevel MapEventLevel(Solutia.Logging.Interfaces.EventLevel eventLevel)
        {
            switch(eventLevel)
            {
                case EventLevel.Debug: return LogLevel.Debug;
                case EventLevel.Error: return LogLevel.Error;
                case EventLevel.Fatal: return LogLevel.Fatal;
                case EventLevel.Information: return LogLevel.Info;
                case EventLevel.Warning: return LogLevel.Warn;
                case EventLevel.Trace: return LogLevel.Trace;
                case EventLevel.Off: return LogLevel.Off;
                default: throw new Exception("Unable to determine NLog logging level for Solutia logging level " + eventLevel);
            }
        }

        public override ILogWriter Log(ILogMessage message)
        {
            if (message is ExceptionLogMessage)
            {
                logger.Log(MapEventLevel(message.EventLevel), ((ExceptionLogMessage)message).Exception);
            }
            else
            {
                logger.Log(MapEventLevel(message.EventLevel), message.Message);
            }
            
            return base.Log(message);
        }
    }
}

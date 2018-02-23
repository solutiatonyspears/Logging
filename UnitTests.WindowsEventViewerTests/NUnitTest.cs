using System;
using System.Collections.Generic;
using Logging.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using NLog.Common;
using NLog.Config;
using NLog.Targets;
using Solutia.Logging.Interfaces;
using Solutia.Logging.Nlog.Implementation;

namespace UnitTests.WindowsEventViewerTests
{
    [TestClass]
    public class NLogTests
    {
        [TestMethod]
        public void TestSolutiaLogToDatabaseFileAndEventLogSucceeds()
        {
            var entryConfig = new List<LogEntryComponent>()
                {
                    LogEntryComponent.Date,
                    LogEntryComponent.MachineName,
                    LogEntryComponent.Message,
                    LogEntryComponent.LogLevel
                };

            var fileSink = new FileLogSink()
            {
                Name = "TestLogger",
                MinimumEventLevel = EventLevel.Trace,
                FileName = "c:\\solutiatest\\testlog.txt",
                LogEntryConfiguration = entryConfig
            };

            var elSink = new EventLogSink()
            {
                Name = "TestLogger",
                Source = "TestLogger",
                Log = "TestLogger",
                MinimumEventLevel = EventLevel.Trace,
                LogEntryConfiguration = entryConfig
            };

            var dbSink = new DatabaseLogSink()
            {
                ConnectionString = "Data Source=LAPTOP-6EA15661\\SQLEXPRESS;Initial Catalog=Logging;Integrated Security=True",
                CommandText = "WriteLog",
                Name = "TestLogger",
                Parameters = new List<DbLogSinkCommandParameter>()
                {
                    new DbLogSinkCommandParameter()
                    {
                        Name = "SeverityLevel",
                        LogEntryComponent = LogEntryComponent.LogLevel
                    },

                    new DbLogSinkCommandParameter()
                    {
                        Name = "Source",
                        LogEntryComponent = LogEntryComponent.MachineName
                    },

                    new DbLogSinkCommandParameter()
                    {
                        Name = "Message",
                        LogEntryComponent = LogEntryComponent.Message
                    },

                    new DbLogSinkCommandParameter()
                    {
                        Name = "LogId",
                        LogEntryComponent = LogEntryComponent.LogId
                    }
                }
            };

           ILogWriter writer = new LogWriter("d")
               .Configure(new List<ILogSink>() { fileSink, elSink, dbSink })
               .Log(new LogMessage("Hello from Solutia Logger", EventLevel.Debug));
        }


        [TestMethod]
        public void TestSolutiaLogToFileSucceeds()
        {
            var fileSink = new FileLogSink()
            {
                Name = "TestLogger",
                MinimumEventLevel = EventLevel.Trace,
                FileName = "c:\\solutiatest\\testlog.txt",
                LogEntryConfiguration = new List<LogEntryComponent>()
                {
                    LogEntryComponent.Date,
                    LogEntryComponent.MachineName,
                    LogEntryComponent.Message,
                    LogEntryComponent.LogLevel
                }
            };

            new Solutia.Logging.Nlog.Implementation.LogWriter("d")
            .Configure(new List<ILogSink>() { fileSink })
            .Log(new LogMessage("Hello from Solutia Logger", EventLevel.Debug));
        }

        [TestMethod]
        public void TestSolutiaLogToEventLogSucceeds()
        {
            var elSink = new EventLogSink()
            {
                Name = "TestLogger",
                Source = "TestLogger",
                Log = "TestLogger",
                MinimumEventLevel = EventLevel.Trace,
                LogEntryConfiguration = new List<LogEntryComponent>()
                {
                    LogEntryComponent.Date,
                    LogEntryComponent.MachineName,
                    LogEntryComponent.Message,
                    LogEntryComponent.LogLevel
                }
            };

            new Solutia.Logging.Nlog.Implementation.LogWriter("d")
            .Configure(new List<ILogSink>() { elSink })
            .Log(new LogMessage("Hello from Solutia Logger", EventLevel.Debug));
        }

        [TestMethod]
        public void TestSolutiaLogToDatabaseSucceeds()
        {
            var dbSink = new DatabaseLogSink()
            {
                ConnectionString = "Data Source=LAPTOP-6EA15661\\SQLEXPRESS;Initial Catalog=Logging;Integrated Security=True",
                CommandText ="WriteLog",
                Name = "TestLogger",
                Parameters = new List<DbLogSinkCommandParameter>()
                {
                    new DbLogSinkCommandParameter()
                    {
                        Name = "SeverityLevel",
                        LogEntryComponent = LogEntryComponent.LogLevel
                    },

                    new DbLogSinkCommandParameter()
                    {
                        Name = "Source",
                        LogEntryComponent = LogEntryComponent.MachineName
                    },

                    new DbLogSinkCommandParameter()
                    {
                        Name = "Message",
                        LogEntryComponent = LogEntryComponent.Message
                    },

                    new DbLogSinkCommandParameter()
                    {
                        Name = "LogId",
                        LogEntryComponent = LogEntryComponent.LogId
                    }
                }
            };

            new Solutia.Logging.Nlog.Implementation.LogWriter("d")
                .Configure(new List<DatabaseLogSink>() { dbSink })
                .Log(new LogMessage("Hello from Solutia Logger", EventLevel.Debug));
        }

        [TestMethod]
        public void TestNLogToFileLogSucceeds()
        {
            var target = new FileTarget()
            {
                Name = "f",
                FileName = "c:\\temp\\test.txt",
                ReplaceFileContentsOnEachWrite = false,
                CreateDirs = true
            };

            var config = new LoggingConfiguration();

            NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(target, LogLevel.Trace);
            config.AddTarget(target);

            var rule = new LoggingRule("*", LogLevel.Trace, target);
            config.LoggingRules.Add(rule);

            LogManager.Configuration = config;
            var logger = LogManager.GetCurrentClassLogger();

            logger.Log(LogLevel.Info, "Hello there");
            logger.Log(LogLevel.Error, new Exception("Hi, I'm an exception"));
        }

        [TestMethod]
        public void TestNLogToEventLogSucceeds()
        {
            var target = new EventLogTarget()
            {
                Name = "e",
                Source = "TestLogger",
                Layout = "${longdate} ${level:uppercase=true} ${logger} ${message}",
                Log = "TestLogger",
            };

            var config = new LoggingConfiguration();

            NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(target, LogLevel.Trace);
            config.AddTarget(target);

            var rule = new LoggingRule("*", LogLevel.Trace, target);
            config.LoggingRules.Add(rule);

            LogManager.Configuration = config;
            var logger = LogManager.GetCurrentClassLogger();

            logger.Log(LogLevel.Info, "Hello there");
            logger.Log(LogLevel.Error, new Exception("Hi, I'm an exception"));
        }

        [TestMethod]
        public void TestNLogToDatabaseSucceeds()
        {
            InternalLogger.LogToConsole = true;
            InternalLogger.LogToTrace = true;
            InternalLogger.LogLevel = LogLevel.Trace;
            var config = new LoggingConfiguration();
            
            var target = new DatabaseTarget()
            {
                Name = "d",
                ConnectionString = "Data Source=LAPTOP-6EA15661\\SQLEXPRESS;Initial Catalog=Logging;Integrated Security=True",
                CommandText = "WriteLog",
                CommandType = System.Data.CommandType.StoredProcedure
            };

            target.Parameters.Add(new DatabaseParameterInfo()
            {
                Name = "@SeverityLevel",
                Layout = new NLog.Layouts.SimpleLayout("${level}")
            });

            target.Parameters.Add(new DatabaseParameterInfo()
            {
                Name = "@Source",
                Layout = new NLog.Layouts.SimpleLayout("${logger}")
            });

            target.Parameters.Add(new DatabaseParameterInfo()
            {
                Name = "@Message",
                Layout = new NLog.Layouts.SimpleLayout("${message}")
            });

            target.Parameters.Add(new DatabaseParameterInfo()
            {
                Name = "@LogId",
                Layout = new NLog.Layouts.SimpleLayout("${..}")
            });

            NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(target, LogLevel.Trace);
            config.AddTarget(target);

            var rule = new LoggingRule("*", LogLevel.Trace, target);
            config.LoggingRules.Add(rule);

            LogManager.Configuration = config;
            var logger = LogManager.GetCurrentClassLogger();

            logger.Log(LogLevel.Info, "Hello there");
            logger.Log(LogLevel.Error, new Exception("Hi, I'm an exception") );
        }
    }
}

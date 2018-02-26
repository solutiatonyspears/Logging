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

namespace UnitTests.SolutiaTests
{
    [TestClass]
    public class SolutiaTests
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
               .Log(new LogMessage("Hello from Solutia File Logger", EventLevel.Debug));
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
            .Log(new LogMessage("Hello from Solutia File Logger", EventLevel.Debug));
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
            .Log(new LogMessage("Hello from Solutia Event Logger", EventLevel.Debug));
        }

        [TestMethod]
        public void TestSolutiaLogToDatabaseTableSucceeds()
        {
            var dbSink = new DatabaseLogSink()
            {
                ConnectionString = "Data Source=LAPTOP-6EA15661\\SQLEXPRESS;Initial Catalog=Logging;Integrated Security=True",

                CommandText = @"
                    INSERT INTO[dbo].[Log]
                               ([SeverityLevel]
                               ,
                                    [Message]
                               ,
                                    [Source], DateCreated)
                         VALUES
                               (@SeverityLevel
                               ,@Message
                               ,@Source, GETDATE());",

                CommandType = CommandType.Table,
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
                        Name = "Message",
                        LogEntryComponent = LogEntryComponent.Message
                    },

                    new DbLogSinkCommandParameter()
                    {
                        Name = "Source",
                        LogEntryComponent = LogEntryComponent.MachineName
                    },
                }
            };

            new Solutia.Logging.Nlog.Implementation.LogWriter("d")
                .Configure(new List<DatabaseLogSink>() { dbSink })
                .Log(new LogMessage("Hello from Solutia Database Table Logger", EventLevel.Debug));
        }

        [TestMethod]
        public void TestSolutiaLogToDatabaseSucceeds()
        {
            var dbSink = new DatabaseLogSink()
            {
                ConnectionString = "Data Source=LAPTOP-6EA15661\\SQLEXPRESS;Initial Catalog=Logging;Integrated Security=True",
                CommandText ="WriteLog",
                Name = "TestLogger",
                CommandType = CommandType.StoredProcedure,
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
                .Log(new LogMessage("Hello from Solutia Sproc Logger", EventLevel.Debug));
        }
    }
}

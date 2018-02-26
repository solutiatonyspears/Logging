using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using NLog.Common;
using NLog.Config;
using NLog.Targets;

namespace UnitTests.WindowsEventViewerTests
{
    [TestClass]
    public class NLogTests
    {
        

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
        public void TestNLogToDatabaseTableSucceeds()
        {
            InternalLogger.LogToConsole = true;
            InternalLogger.LogToTrace = true;
            InternalLogger.LogLevel = LogLevel.Trace;
            var config = new LoggingConfiguration();

            var target = new DatabaseTarget()
            {
                Name = "d",
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
                CommandType = System.Data.CommandType.Text
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

            logger.Log(LogLevel.Info, "Hello there, this is a table log");
            logger.Log(LogLevel.Error, new Exception("Hi, I'm a table-logged exception"));
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

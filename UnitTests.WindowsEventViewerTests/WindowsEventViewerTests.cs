using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solutia.Logging;

namespace UnitTests.WindowsEventViewerTests
{
    [TestClass]
    public class WindowsEventViewerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            Cleanup();

            try
            {
                EventLog.CreateEventSource("Logger", "TestLogger");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

        }

        [TestCleanup]
        public void Cleanup()
        {
            try
            {
                EventLog.Delete("TestLogger");
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        [TestMethod]
        public void TestCreateWindowsEventListenerSucceeds()
        {
            var eventLogParams = new WindowsEventLogParams()
            {
                EventLevel = System.Diagnostics.Tracing.EventLevel.LogAlways,
                EventSource = Logger.Log,
                InstanceName = "TestLogger",
                Keywords = System.Diagnostics.Tracing.EventKeywords.All,
                SourceName = "TestLogger"
            };

            var listener = new EventLogListenerFactory().CreateEventListener(eventLogParams);
            Assert.IsInstanceOfType(listener, typeof(WindowsEventLogListener));
        }

        [TestMethod]
        public void TestWriteWindowsEventListenerSucceeds()
        {
            var eventSource = Logger.Log;

            var analyzer = new EventSourceAnalyzer();
            analyzer.Inspect(eventSource);

            var eventLogParams = new WindowsEventLogParams()
            {
                EventLevel = System.Diagnostics.Tracing.EventLevel.LogAlways,
                EventSource = eventSource,
                InstanceName = "TestLogger",
                Keywords = System.Diagnostics.Tracing.EventKeywords.All,
                SourceName = "TestLogger"
            };

            var listener = new EventLogListenerFactory().CreateEventListener(eventLogParams);
            Assert.IsInstanceOfType(listener, typeof(WindowsEventLogListener));

        
            Logger.Log.AppStarted();

            var testLog = new EventLog();
            testLog.Log = "TestLogger";
            var entries = testLog.Entries;
            Assert.IsNotNull(entries);
            Assert.AreEqual(1, entries.Count);
        }

        [TestMethod]
        public void TestCreateMultipleLogListenersSucceeds()
        {
            var eventSource = Logger.Log;

            var analyzer = new EventSourceAnalyzer();
            analyzer.Inspect(eventSource);

            var windowsEventLogParams = new WindowsEventLogParams()
            {
                EventLevel = System.Diagnostics.Tracing.EventLevel.LogAlways,
                EventSource = eventSource,
                InstanceName = "TestLogger",
                Keywords = System.Diagnostics.Tracing.EventKeywords.All,
                SourceName = "TestLogger"
            };

            
            var databaseEventLogParams = new DatabaseEventLogParams()
            {
                EventLevel = System.Diagnostics.Tracing.EventLevel.LogAlways,
                EventSource = eventSource,
                InstanceName = "Logger",
                ConnectionString = "data source=blah;initial catalog=Logging;User ID='CarePointDMEBetaAdmin';Password='VpYvP7FY';MultipleActiveResultSets=True;App=EntityFramework",
                Keywords = System.Diagnostics.Tracing.EventKeywords.All,
                TableName = "dbo.Traces"
            };

            var factory = new EventLogListenerFactory();
            //var windowsEventListener = factory.CreateEventListener(windowsEventLogParams);
            var databaseEventListener = factory.CreateEventListener(databaseEventLogParams);

            Logger.Log.AppStarted();

            /*
            var testLog = new EventLog();
            testLog.Log = "TestLogger";
            var entries = testLog.Entries;
            Assert.IsNotNull(entries);
            Assert.AreEqual(1, entries.Count);
            */

        }
    }
}

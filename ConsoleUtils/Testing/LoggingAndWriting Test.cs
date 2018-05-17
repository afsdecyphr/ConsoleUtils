using System;
using ConsoleUtils.Logging;
using ConsoleUtils.Writing;

namespace ConsoleUtils.Testing
{
    public class LoggingAndWritingTest
    {
        // Initialize the Writer and Logger with logging to a file enabled to the path './consoleutils.log'
        private static InitWriterLogger __INIT = new InitWriterLogger(true, "./consoleutils-" + Environment.MachineName + ".log");

        private static Writer writer = __INIT.Writer;
        private static Logger logger = __INIT.Logger;

        public void Test()
        {
            TestLogger();
            TestWriter();
            writer.Custom("Writing and logging testing complete.", Color.Cyan);
            writer.Custom("Is Logging to File: " + __INIT.LogToFile, Color.Cyan);
            writer.Custom("Log File Location: " + __INIT.LogFilePath, Color.Cyan);
            writer.Wait();
        }

        public void TestLogger()
        {
            object obj = "";
            
            logger.Log("Logger Log(string)");
            obj = "Logger Log(object)";
            logger.Log(obj);

            logger.LogWarning("Logger Warn(string)");
            obj = "Logger Warn(object)";
            logger.LogWarning(obj);

            logger.LogError("Logger Error(string)");
            obj = "Logger Error(object)";
            logger.LogError(obj);

            logger.LogCustom("Logger Custom(string)");
            obj = "Logger Custom(object)";
            logger.LogCustom(obj);
        }

        public void TestWriter()
        {
            writer.CustomColorFormat(">y^bBlue>b^yYellow^hTest^y>lTestttttt");
            logger.Log(">y^bBlue>b^yYellow^hTest^y>lTestttttt");
            writer.CustomColorFormat("^cCyan");
            logger.Log("^cCyan");
        }
    }
}
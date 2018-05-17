using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace ConsoleUtils.Logging
{
    public static class StringExtension
    {
        public static bool isNull(this Object obj)
        {
            if (obj.Equals(null))
                return true;
            else
                return false;
        }
    }

    public class Logger
    {
        private bool logToFile = false;
        private string logFilePath = "consoleutils.log";

        public Logger(bool logToFile = false, string logFilePath = "")
        {
            if (logFilePath == "")
            {
                logFilePath = "./consoleutils-" + Environment.MachineName + ".log";
            }
            this.logToFile = logToFile;
            this.logFilePath = logFilePath;

            if (logToFile)
                if (File.Exists(logFilePath))
                    File.WriteAllText(logFilePath, String.Empty);
                else
                    File.Create(logFilePath);
        }

        public void WriteToFile(string text)
        {
            if (logToFile)
            {
                if (File.Exists(logFilePath))
                {
                    while (true)
                    {
                        try
                        {
                            using (FileStream Fs = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.None, 100))
                            {
                                using (StreamWriter sw = new StreamWriter(Fs))
                                {
                                    String timeStamp = "[" + string.Format("{0:MM-dd-yyyy @ hh:mm:ss}", DateTime.Now) + "]";
                                    sw.WriteLine(timeStamp + " " + text);
                                    sw.Close();
                                }
                                Fs.Close();
                                break;
                            }
                        }
                        catch (IOException)
                        {
                            LogError("unable");
                            Thread.Sleep(1000);
                        }
                    }
                }
            }
        }

        private enum LogType
        {
            Custom = 0,
            Log = 1,
            Warn = 2,
            Error = 3
        }

        private void _WRITELOG(LogType type, object message)
        {
            string prefix = "";

            switch (type)
            {
                case (LogType.Custom):
                    prefix = "";
                    break;

                case (LogType.Log):
                    prefix = "[LOG] ";
                    break;

                case (LogType.Warn):
                    prefix = "[WARN] ";
                    break;

                case (LogType.Error):
                    prefix = "[ERROR] ";
                    break;

                default:
                    prefix = "";
                    break;
            }

            string _CHAR = (prefix + message.ToString());
            WriteToFile(_CHAR);
            Debug.WriteLine(_CHAR);
        }

        /// <summary>
        /// Log a message from a string with the prefix '[LOG]'
        /// </summary>
        /// <param name="message">The text to write to the log</param>
        public void Log(String message)
        {
            if (!message.isNull())
                _WRITELOG(LogType.Log, message);
        }

        /// <summary>
        /// Log a message from an object with the prefix '[LOG]'
        /// </summary>
        /// <param name="message">The text to write to the log</param>
        public void Log(object message)
        {
            if (!message.isNull())
                _WRITELOG(LogType.Log, message);
        }

        /// <summary>
        /// Log a message from a string with the prefix '[WARN]'
        /// </summary>
        /// <param name="message">The text to write to the log</param>
        public void LogWarning(String message)
        {
            _WRITELOG(LogType.Warn, message);
        }

        /// <summary>
        /// Log a message from an object with the prefix '[WARN]'
        /// </summary>
        /// <param name="message">The text to write to the log</param>
        public void LogWarning(object message)
        {
            if (!message.isNull())
                _WRITELOG(LogType.Warn, message);
        }

        /// <summary>
        /// Log a message from a string with the prefix '[ERROR]'
        /// </summary>
        /// <param name="message">The text to write to the log</param>
        public void LogError(String message)
        {
            if (!message.isNull())
                _WRITELOG(LogType.Error, message);
        }

        /// <summary>
        /// Log a message from an object with the prefix '[ERROR]'
        /// </summary>
        /// <param name="message">The text to write to the log</param>
        public void LogError(object message)
        {
            if (!message.isNull())
                _WRITELOG(LogType.Error, message);
        }

        /// <summary>
        /// Log a message from a string
        /// </summary>
        /// <param name="message">The text to write to the log</param>
        public void LogCustom(String message)
        {
            if (!message.isNull())
                _WRITELOG(LogType.Custom, message);
        }

        /// <summary>
        /// Log a message from a string
        /// </summary>
        /// <param name="message">The text to write to the log</param>
        public void LogCustom(object message)
        {
            if (!message.isNull())
                _WRITELOG(LogType.Custom, message);
        }

        /// <summary>
        /// Log a message if the condition is true
        /// </summary>
        /// <param name="condition">Boolean which determines whether to log the message</param>
        /// <param name="message">The text to write to the log</param>
        public void LogIf(bool condition, String message)
        {
            if (condition && !message.isNull())
                _WRITELOG(LogType.Custom, message);
        }

        /// <summary>
        /// Log a message if the condition is true
        /// </summary>
        /// <param name="condition">Boolean which determines whether to log the message</param>
        /// <param name="message">The text to log as anobject</param>
        public void LogIf(bool condition, object message)
        {
            if (condition && !message.isNull())
                _WRITELOG(LogType.Custom, message);
        }
    }
}
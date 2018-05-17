using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

using ConsoleUtils.Logging;

namespace ConsoleUtils.Writing
{
    public enum Color
    {
        Black = ConsoleColor.Black,
        Blue = ConsoleColor.Blue,
        Cyan = ConsoleColor.Cyan,
        DarkBlue = ConsoleColor.DarkBlue,
        DarkCyan = ConsoleColor.DarkCyan,
        DarkGray = ConsoleColor.DarkGray,
        DarkGreen = ConsoleColor.DarkGreen,
        DarkMagenta = ConsoleColor.DarkMagenta,
        DarkRed = ConsoleColor.DarkRed,
        DarkYellow = ConsoleColor.DarkYellow,
        Gray = ConsoleColor.Gray,
        Green = ConsoleColor.Green,
        Magenta = ConsoleColor.Magenta,
        Red = ConsoleColor.Red,
        White = ConsoleColor.White,
        Yellow = ConsoleColor.Yellow,
        Default = ConsoleColor.Gray
    }

    /// <summary>
    /// Initializer for global writers and loggers
    /// </summary>
    public class InitWriterLogger
    {
        private Writer _writer = new Writer();
        private Logger _logger = new Logger();
        private bool _LogToFile = false;
        private string _LogFilePath = "./consoleutils.log";

        /// <summary>
        /// Initializer for a global logger and writer
        /// </summary>
        /// <param name="logToFile">Whether or not to log to a file</param>
        /// <param name="logFilePath">The file path of the log file</param>
        public InitWriterLogger(bool logToFile = false, string logFilePath = "")
        {
            if (logFilePath == "")
            {
                logFilePath = "./consoleutils-" + Environment.MachineName + ".log";
            }
            this.LogToFile = logToFile;
            this.LogFilePath = logFilePath;

            Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = "v" + fvi.FileVersion;

            Logger = new Logger(logToFile, logFilePath);
            Writer.Custom("[LOGGER] ConsoleUtils " + version + " by Josh Duncan", Color.Blue);
            Writer.Custom("[LOGGER] Initialized logger", Color.Blue);
            Writer = new Writer();
            Writer.Custom("[WRITER] ConsoleUtils " + version + " by Josh Duncan", Color.Blue);
            Writer.Custom("[WRITER] Initialized writer", Color.Blue);
        }

        public string LogFilePath
        {
            get { return this._LogFilePath; }
            set { this._LogFilePath = value; }
        }
        public bool LogToFile
        {
            get { return this._LogToFile; }
            set { this._LogToFile = value; }
        }
        public Writer Writer
        {
            get { return this._writer; }
            set { this._writer = value; }
        }
        public Logger Logger
        {
            get { return this._logger; }
            set { this._logger = value; }
        }
    }

    public class Writer
    {
        private ConsoleColor defaultFore = Console.ForegroundColor;
        private ConsoleColor defaultBack = Console.BackgroundColor;

        private enum MessageType
        {
            Custom = 0,
            Log = 1,
            Warn = 2,
            Error = 3
        }

        private void _WRITE(MessageType type, object message, Color color = Color.Default, bool isCentered = false)
        {
            string prefix = "";

            switch (type)
            {
                case (MessageType.Custom):
                    Console.ForegroundColor = (ConsoleColor)color;
                    break;

                case (MessageType.Log):
                    Console.ForegroundColor = ConsoleColor.Gray;
                    prefix = "[LOG] ";
                    break;

                case (MessageType.Warn):
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    prefix = "[WARN] ";
                    break;

                case (MessageType.Error):
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    prefix = "[ERROR] ";
                    break;

                default:
                    Console.ForegroundColor = (ConsoleColor)color;
                    break;
            }

            char[] _CHAR = (prefix + message).ToCharArray();

            if (!isCentered)
                Console.WriteLine(_CHAR);
            else if (isCentered)
            {
                int _WINDOWWIDTH = Console.BufferWidth;
                int _MESSAGELENGTH = _CHAR.Length;
                int _INDENT = (_WINDOWWIDTH / 2) - (_MESSAGELENGTH / 2);
                Console.CursorLeft = _INDENT;
                Console.WriteLine(_CHAR);
            }
            Console.ForegroundColor = defaultFore;
            Console.BackgroundColor = defaultBack;
            Console.CursorLeft = 0;
        }

        /// <summary>
        /// Write a message to the console with the prefix '[LOG]'
        /// </summary>
        /// <param name="message">The text to write to the console</param>
        /// <param name="isCentered">Boolean which determines whether or not to center the message</param>
        public void Log(String message, bool isCentered = false)
        {
            if (!message.isNull())
                _WRITE(MessageType.Log, message, Color.Default, isCentered);
        }

        /// <summary>
        /// Write a message in gray text to the console with the prefix '[LOG]'
        /// </summary>
        /// <param name="message">The text to write to the console</param>
        /// <param name="isCentered">Boolean which determines whether or not to center the message</param>
        public void Log(object message, bool isCentered = false)
        {
            if (!message.isNull())
                _WRITE(MessageType.Log, message, Color.Default, isCentered);
        }

        /// <summary>
        /// Write a message in yellow text to the console with the prefix '[WARN]'
        /// </summary>
        /// <param name="message">The text to write to the console</param>
        /// <param name="isCentered">Boolean which determines whether or not to center the message</param>
        public void Warning(String message, bool isCentered = false)
        {
            if (!message.isNull())
                _WRITE(MessageType.Warn, message, Color.Default, isCentered);
        }

        /// <summary>
        /// Write a message in yellow text to the console with the prefix '[WARN]'
        /// </summary>
        /// <param name="message">The text to write to the console</param>
        /// <param name="isCentered">Boolean which determines whether or not to center the message</param>
        public void Warning(object message, bool isCentered = false)
        {
            if (!message.isNull())
                _WRITE(MessageType.Warn, message, Color.Default, isCentered);
        }

        /// <summary>
        /// Write a message in red text to the console with the prefix '[ERROR]'
        /// </summary>
        /// <param name="message">The text to write to the console</param>
        /// <param name="isCentered">Boolean which determines whether or not to center the message</param>
        public void Error(String message, bool isCentered = false)
        {
            if (!message.isNull())
                _WRITE(MessageType.Error, message, Color.Default, isCentered);
        }

        /// <summary>
        /// Write a message in red text to the console with the prefix '[ERROR]'
        /// </summary>
        /// <param name="message">The text to write to the console</param>
        /// <param name="isCentered">Boolean which determines whether or not to center the message</param>
        public void Error(object message, bool isCentered = false)
        {
            if (!message.isNull())
                _WRITE(MessageType.Error, message, Color.Default, isCentered);
        }

        /// <summary>
        /// Write a message in the specified color to the console
        /// </summary>
        /// <param name="message">The text to write to the console</param>
        /// <param name="color">The color of the text</param>
        /// <param name="isCentered">Boolean which determines whether or not to center the message</param>
        public void Custom(String message, Color color, bool isCentered = false)
        {
            if (!message.isNull())
                _WRITE(MessageType.Custom, message, color, isCentered);
        }

        /// <summary>
        /// Write a message in the specified color to the console
        /// </summary>
        /// <param name="message">The text to write to the console</param>
        /// <param name="color">The color of the text</param>
        /// <param name="isCentered">Boolean which determines whether or not to center the message</param>
        public void Custom(object message, Color color, bool isCentered = false)
        {
            if (!message.isNull())
                _WRITE(MessageType.Custom, message, color, isCentered);
        }

        /// <summary>
        /// Write a message with custom color formatting. Use ^ to set forecolor and > to set backcolor.
        /// Example: '^b>vHI' would print HI in blue text with a dark cyan background.
        /// Color Codes:
        /// o = Black ; b = Blue ; c = Cyan ; n = Dark Blue ; v = Dark Cyan ; k = Dark Gray ; 
        /// h = Dark Green ; l = Dark Magenta ; e = Dark Red ; u = Dark Yellow ; g = Gray ; 
        /// w = White ; y = Yellow ; r = Red ; m = Magenta ; f = Green
        /// </summary>
        /// <param name="message">The text to write to the console</param>
        public void CustomColorFormat(object message)
        {
            Dictionary<int, ConsoleColor> colorAtIndex = new Dictionary<int, ConsoleColor>();
            Dictionary<int, ConsoleColor> backColorAtIndex = new Dictionary<int, ConsoleColor>();
            string mess = message.ToString();
            var foundColorIndexes = new List<int>();
            var foundBackIndexes = new List<int>();
            var startIndexes = new List<int>();
            var lengthIndexes = new List<int>();
            for (int i = 0; i < mess.Length; i++)
            {
                if (mess[i] == '^')
                    foundColorIndexes.Add(i);
                if (mess[i] == '>')
                    foundBackIndexes.Add(i);
            }

            if (foundColorIndexes[0] != 0)
            {
                int length;

                if (foundColorIndexes.Count > 1)
                    length = foundColorIndexes[0] - 3 - 0 + 2;
                else
                    length = mess.Length - (foundColorIndexes[0] + 2);

                startIndexes.Add(0);
                lengthIndexes.Add(length);
            }

            for (int i = 0; i < foundColorIndexes.Count; i++)
            {
                char shortCodeChar = mess[foundColorIndexes[i] + 1];
                int length;

                if (foundColorIndexes.Count > 1 && i + 1 < foundColorIndexes.Count)
                    length = foundColorIndexes[i + 1] - 4 - foundColorIndexes[i] + 2;
                else
                    length = mess.Length - (foundColorIndexes[i] + 2);

                startIndexes.Add(foundColorIndexes[i] + 2);

                if (mess[((foundColorIndexes[i]) + (length))] == '^' || mess[((foundColorIndexes[i]) + (length))] == '>')
                    lengthIndexes.Add(length - 2);
                else
                    lengthIndexes.Add(length);

                colorAtIndex.Add(foundColorIndexes[i] + 2, ApplyFormat(shortCodeChar));
            }

            for (int i = 0; i < foundBackIndexes.Count; i++)
            {
                char shortCodeChar = mess[foundBackIndexes[i] + 1];
                int length;

                if (foundBackIndexes.Count > 1 && i + 1 < foundBackIndexes.Count)
                    length = foundBackIndexes[i + 1] - 4 - foundBackIndexes[i] + 2;
                else
                    length = mess.Length - (foundBackIndexes[i] + 2);

                startIndexes.Add(foundBackIndexes[i] + 2);

                if (mess[((foundBackIndexes[i] + 2) + (length - 2))] == '^' || mess[((foundBackIndexes[i] + 2) + (length - 2))] == '>')
                    lengthIndexes.Add(length - 2);
                else
                    lengthIndexes.Add(length);

                backColorAtIndex.Add(foundBackIndexes[i] + 2, ApplyFormat(shortCodeChar));
            }

            var doneIndexes = new List<int>();
            int indexOn = 0;
            for (int i = 0; i < startIndexes.Count; i++)
            {
                if (mess[startIndexes[i]] != '>')
                {
                    int start = startIndexes[i];

                    if (mess[start] == '^')
                        start = start + 2;

                    if (!doneIndexes.Contains(start))
                    {
                        string sub = mess.Substring(start, lengthIndexes[i]);

                        if (colorAtIndex.ContainsKey(start - 2))
                            Console.ForegroundColor = colorAtIndex[start - 2];
                        else if (colorAtIndex.ContainsKey(start))
                            Console.ForegroundColor = colorAtIndex[start];

                        if (backColorAtIndex.ContainsKey(start - 2))
                            Console.BackgroundColor = backColorAtIndex[start - 2];
                        else if (backColorAtIndex.ContainsKey(start))
                            Console.BackgroundColor = backColorAtIndex[start];

                        if (sub.Contains(">"))
                        {
                            char scc = sub[sub.IndexOf('>') + 1];
                            sub = sub.Substring(0, sub.Length - sub.IndexOf('>') - 3);
                            startIndexes.Add((mess.Length - 1 - sub.Length));
                            backColorAtIndex[(mess.Length - 1 - sub.Length)] = ApplyFormat(scc);
                        }
                        Console.Write(sub);
                        indexOn = indexOn + sub.Length - 1;
                        doneIndexes.Add(start);
                    }
                }
            }
            Console.WriteLine("");
            Console.ForegroundColor = defaultFore;
            Console.BackgroundColor = defaultBack;
        }

        /// <summary>
        /// Write a message with custom color formatting. Use ^ to set forecolor and > to set backcolor.
        /// Example: '^b>vHI' would print HI in blue text with a dark cyan background.
        /// Color Codes:
        /// o = Black ; b = Blue ; c = Cyan ; n = Dark Blue ; v = Dark Cyan ; k = Dark Gray ; 
        /// h = Dark Green ; l = Dark Magenta ; e = Dark Red ; u = Dark Yellow ; g = Gray ; 
        /// w = White ; y = Yellow ; r = Red ; m = Magenta ; f = Green
        /// </summary>
        /// <param name="message">The text to write to the console</param>
        public void CustomColorFormat(String message)
        {
            Dictionary<int, ConsoleColor> colorAtIndex = new Dictionary<int, ConsoleColor>();
            Dictionary<int, ConsoleColor> backColorAtIndex = new Dictionary<int, ConsoleColor>();
            string mess = message.ToString();
            var foundColorIndexes = new List<int>();
            var foundBackIndexes = new List<int>();
            var startIndexes = new List<int>();
            var lengthIndexes = new List<int>();
            for (int i = 0; i < mess.Length; i++)
            {
                if (mess[i] == '^')
                    foundColorIndexes.Add(i);
                if (mess[i] == '>')
                    foundBackIndexes.Add(i);
            }

            if (foundColorIndexes[0] != 0)
            {
                int length;

                if (foundColorIndexes.Count > 1)
                    length = foundColorIndexes[0] - 3 - 0 + 2;
                else
                    length = mess.Length - (foundColorIndexes[0] + 2);

                startIndexes.Add(0);
                lengthIndexes.Add(length);
            }

            for (int i = 0; i < foundColorIndexes.Count; i++)
            {
                char shortCodeChar = mess[foundColorIndexes[i] + 1];
                int length;

                if (foundColorIndexes.Count > 1 && i + 1 < foundColorIndexes.Count)
                    length = foundColorIndexes[i + 1] - 4 - foundColorIndexes[i] + 2;
                else
                    length = mess.Length - (foundColorIndexes[i] + 2);

                startIndexes.Add(foundColorIndexes[i] + 2);

                if (mess[((foundColorIndexes[i]) + (length))] == '^' || mess[((foundColorIndexes[i]) + (length))] == '>')
                    lengthIndexes.Add(length - 2);
                else
                    lengthIndexes.Add(length);

                colorAtIndex.Add(foundColorIndexes[i] + 2, ApplyFormat(shortCodeChar));
            }

            for (int i = 0; i < foundBackIndexes.Count; i++)
            {
                char shortCodeChar = mess[foundBackIndexes[i] + 1];
                int length;

                if (foundBackIndexes.Count > 1 && i + 1 < foundBackIndexes.Count)
                    length = foundBackIndexes[i + 1] - 4 - foundBackIndexes[i] + 2;
                else
                    length = mess.Length - (foundBackIndexes[i] + 2);

                startIndexes.Add(foundBackIndexes[i] + 2);

                if (mess[((foundBackIndexes[i] + 2) + (length - 2))] == '^' || mess[((foundBackIndexes[i] + 2) + (length - 2))] == '>')
                    lengthIndexes.Add(length - 2);
                else
                    lengthIndexes.Add(length);

                backColorAtIndex.Add(foundBackIndexes[i] + 2, ApplyFormat(shortCodeChar));
            }

            var doneIndexes = new List<int>();
            int indexOn = 0;
            for (int i = 0; i < startIndexes.Count; i++)
            {
                if (mess[startIndexes[i]] != '>')
                {
                    int start = startIndexes[i];

                    if (mess[start] == '^')
                        start = start + 2;

                    if (!doneIndexes.Contains(start))
                    {
                        string sub = mess.Substring(start, lengthIndexes[i]);

                        if (colorAtIndex.ContainsKey(start - 2))
                            Console.ForegroundColor = colorAtIndex[start - 2];
                        else if (colorAtIndex.ContainsKey(start))
                            Console.ForegroundColor = colorAtIndex[start];

                        if (backColorAtIndex.ContainsKey(start - 2))
                            Console.BackgroundColor = backColorAtIndex[start - 2];
                        else if (backColorAtIndex.ContainsKey(start))
                            Console.BackgroundColor = backColorAtIndex[start];

                        if (sub.Contains(">"))
                        {
                            char scc = sub[sub.IndexOf('>') + 1];
                            sub = sub.Substring(0, sub.Length - sub.IndexOf('>') - 3);
                            startIndexes.Add((mess.Length - 1 - sub.Length));
                            backColorAtIndex[(mess.Length - 1 - sub.Length)] = ApplyFormat(scc);
                        }
                        Console.Write(sub);
                        indexOn = indexOn + sub.Length - 1;
                        doneIndexes.Add(start);
                    }
                }
            }
            Console.WriteLine("");
            Console.ForegroundColor = defaultFore;
            Console.BackgroundColor = defaultBack;
        }

        private ConsoleColor ApplyFormat(char shortCodeChar)
        {
            if (String.Equals(shortCodeChar.ToString(), 'o'.ToString(), StringComparison.OrdinalIgnoreCase))
                return ConsoleColor.Black;
            else if (String.Equals(shortCodeChar.ToString(), 'b'.ToString(), StringComparison.OrdinalIgnoreCase))
                return ConsoleColor.Blue;
            else if (String.Equals(shortCodeChar.ToString(), 'c'.ToString(), StringComparison.OrdinalIgnoreCase))
                return ConsoleColor.Cyan;
            else if (String.Equals(shortCodeChar.ToString(), 'n'.ToString(), StringComparison.OrdinalIgnoreCase))
                return ConsoleColor.DarkBlue;
            else if (String.Equals(shortCodeChar.ToString(), 'v'.ToString(), StringComparison.OrdinalIgnoreCase))
                return ConsoleColor.DarkCyan;
            else if (String.Equals(shortCodeChar.ToString(), 'k'.ToString(), StringComparison.OrdinalIgnoreCase))
                return ConsoleColor.DarkGray;
            else if (String.Equals(shortCodeChar.ToString(), 'h'.ToString(), StringComparison.OrdinalIgnoreCase))
                return ConsoleColor.DarkGreen;
            else if (String.Equals(shortCodeChar.ToString(), 'l'.ToString(), StringComparison.OrdinalIgnoreCase))
                return ConsoleColor.DarkMagenta;
            else if (String.Equals(shortCodeChar.ToString(), 'e'.ToString(), StringComparison.OrdinalIgnoreCase))
                return ConsoleColor.DarkRed;
            else if (String.Equals(shortCodeChar.ToString(), 'u'.ToString(), StringComparison.OrdinalIgnoreCase))
                return ConsoleColor.DarkYellow;
            else if (String.Equals(shortCodeChar.ToString(), 'g'.ToString(), StringComparison.OrdinalIgnoreCase))
                return ConsoleColor.Gray;
            else if (String.Equals(shortCodeChar.ToString(), 'f'.ToString(), StringComparison.OrdinalIgnoreCase))
                return ConsoleColor.Green;
            else if (String.Equals(shortCodeChar.ToString(), 'm'.ToString(), StringComparison.OrdinalIgnoreCase))
                return ConsoleColor.Magenta;
            else if (String.Equals(shortCodeChar.ToString(), 'r'.ToString(), StringComparison.OrdinalIgnoreCase))
                return ConsoleColor.Red;
            else if (String.Equals(shortCodeChar.ToString(), 'w'.ToString(), StringComparison.OrdinalIgnoreCase))
                return ConsoleColor.White;
            else if (String.Equals(shortCodeChar.ToString(), 'y'.ToString(), StringComparison.OrdinalIgnoreCase))
                return ConsoleColor.Yellow;
            else
                return ConsoleColor.Gray;
        }

        /// <summary>
        /// Waits for the user to press enter before continuing
        /// </summary>
        /// <param name="isCentered">Boolean which determines whether or not to center the message</param>
        public void Wait(bool isCentered = false)
        {
            string _MESSAGE = "Press enter to continue...";
            _WRITE(MessageType.Custom, _MESSAGE, Color.Default, isCentered);
            Console.ReadLine();
        }

        /// <summary>
        /// Write a message in the specified color to the console if the condition is true
        /// </summary>
        /// <param name="condition">Boolean which determines whether to write the message</param>
        /// <param name="message">The text to write to the console</param>
        /// <param name="color">The color of the text</param>
        /// <param name="isCentered">Boolean which determines whether or not to center the message</param>
        public void WriteIf(bool condition, String message, Color color = Color.Gray, bool isCentered = false)
        {
            if (condition && !message.isNull())
                _WRITE(MessageType.Custom, message, color, isCentered);
        }

        /// <summary>
        /// Write a message in the specified color to the console if the condition is true
        /// </summary>
        /// <param name="condition">Boolean which determines whether to write the message</param>
        /// <param name="message">The text to write to the console</param>
        /// <param name="color">The color of the text</param>
        /// <param name="isCentered">Boolean which determines whether or not to center the message</param>
        public void WriteIf(bool condition, object message, Color color = Color.Gray, bool isCentered = false)
        {
            if (condition && !message.isNull())
                _WRITE(MessageType.Custom, message, color, isCentered);
        }
    }
}
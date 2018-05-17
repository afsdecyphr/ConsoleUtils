using System;
using ConsoleUtils.Logging;
using ConsoleUtils.Servers.UPD;
using ConsoleUtils.Writing;

namespace ConsoleUtils.Testing
{
    public class SyncUDPTest
    {
        private static InitWriterLogger __INIT = new InitWriterLogger(true, "./consoleutils.log");
        public static Writer writer = __INIT.Writer;
        public static Logger logger = __INIT.Logger;

        private AsyncUDP udp;

        public void Test()
        {
            udp = new AsyncUDP(1313, true, true, __INIT);
            udp.OnLog += new AsyncUDP.LogHandler(LogHandler);
            udp.OnCommand += new AsyncUDP.CommandHandler(KeyHandler);
            udp.Start();
        }

        public void LogHandler(object a, LogArgs e)
        {
            writer.Custom(e.Message, e.MessageColor);
            logger.LogCustom(e.Message);
        }

        public void KeyHandler(object a, CommandArgs e)
        {
            switch (e.Message)
            {
                case "info":
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    writer.Custom("UDP Server Information:", Color.DarkMagenta);
                    StringFormatter("Port: ", udp.Port.ToString());
                    StringFormatter("Status: ", udp.Status);
                    StringFormatter("Is Handling Commands: ", udp.IsHandlingKeys.ToString());
                    StringFormatter("Is Outputting to Log: ", udp.IsOutputing.ToString());
                    logger.LogCustom(e.Message);
                    break;

                case "start":
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    udp.Start();
                    break;

                case "stop":
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    udp.Stop();
                    break;

                case "clear":
                    Console.Clear();
                    writer.Custom("Console cleared.", Color.Blue);
                    break;

                case "":
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    break;

                default:
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    writer.Custom("Unknown command: " + e.Message, Color.DarkMagenta);
                    logger.LogCustom(e.Message);
                    break;
            }
        }

        private static void StringFormatter(string key, string value)
        {
            int line = Console.CursorTop;
            int initLength = key.Length;

            Console.SetCursorPosition(0, line);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(key);
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(initLength, line);
            Console.Write(value);
            Console.SetCursorPosition(0, line + 1);
        }
    }
}
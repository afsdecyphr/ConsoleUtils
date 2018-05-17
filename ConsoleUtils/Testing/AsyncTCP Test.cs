using System;
using System.Diagnostics;
using System.Net.Sockets;
using ConsoleUtils.Logging;
using ConsoleUtils.Servers.TCP;
using ConsoleUtils.Writing;
using Serialization;

namespace ConsoleUtils.Testing
{
    public class AsyncTCPTest
    {
        private static int player1Y = 5;
        private static int player1X = 3;
        private static int player2Y = 5;
        private static int player2X = 3;

        private static bool isServerRunning = false;

        private static bool newMap1 = false;
        private static bool newMap2 = false;
        public static char[,] mapArray = new char[15, 35];
        private static string mapName;
        private static int mapNumber;
        private static MapLoader loader = new MapLoader();

        private static int loc = 0;
        private static int mapw = 35;
        private static int maph = 15;
        private static InitWriterLogger __INIT = new InitWriterLogger(true, "./consoleutils-" + Environment.MachineName + ".log");
        public static Writer writer = __INIT.Writer;
        public static Logger logger = __INIT.Logger;

        private static AsyncTCPServer server = new AsyncTCPServer("0.0.0.0", 1313, true, true, __INIT);

        public void Test()
        {
            server.OnCommand += new AsyncTCPServer.CommandHandler(KeyHandler);
            server.OnStream += new AsyncTCPServer.StreamHandler(StreamHandler);
            server.OnLog += new AsyncTCPServer.LogHandler(LogHandler);
            writer.Wait();

            try
            {
                server.Start();
            }
            catch (SocketException e)
            {
                logger.LogError(e.ToString());
                writer.Error(e.ToString());
                writer.Wait();
            }
        }

        public static void LogHandler(object a, LogArgs e)
        {
            writer.Custom(e.Message, e.MessageColor);
            logger.LogCustom(e.Message);
        }

        public static void KeyHandler(object a, CommandArgs e)
        {
            string input = e.Message;
            if (String.Equals(input, "stop", StringComparison.CurrentCultureIgnoreCase))
            {
                server.Stop();
            }
            else if (String.Equals(input, "clear", StringComparison.CurrentCultureIgnoreCase))
            {
                Console.Clear();
                server.Start();
            }
            else
            {
                try
                {
                    mapArray = loader.GetMapCharArray(input);
                    newMap1 = true;
                    newMap2 = true;
                    mapName = input;
                    mapNumber = Convert.ToInt32(input.Replace("level", ""));
                    writer.Custom("[SERVER] Sending map '" + input + "' to clients.", Color.Green);
                    server.Start();
                }
                catch
                {
                    writer.Custom("[SERVER] [ERROR] Could not find map '" + input + "'.", Color.DarkRed);
                    server.Start();
                }
            }
        }

        public static void StreamHandler(object a, StreamArgs e)
        {
            NetworkStream stream = e.Stream;
            try
            {
                GameRequest sq = stream.DeSerialize<GameRequest>();
                if (sq.MapNumber > mapNumber)
                {
                    Debug.WriteLine("yes: " + mapNumber + " ; " + sq.MapNumber);
                    mapNumber = sq.MapNumber;
                    mapName = "level" + mapNumber;
                    mapArray = loader.GetMapCharArray(mapName);
                }
                if (sq.Player.PlayerID == PlayerID.Player1)
                {
                    player1Y = sq.y;
                    player1X = sq.x;
                    GameRequest gR = new GameRequest { Player = new Player { PlayerID = PlayerID.Player2 }, x = player2X, y = player2Y, Running = true, MapName = mapName, Map = mapArray, MapNumber = mapNumber };
                    Debug.WriteLine(sq.MapNumber);
                    gR.Serialize<GameRequest>(stream);
                }
                if (sq.Player.PlayerID == PlayerID.Player2)
                {
                    player2Y = sq.y;
                    player2X = sq.x;
                    GameRequest gR = new GameRequest { Player = new Player { PlayerID = PlayerID.Player1 }, x = player1X, y = player1Y, Running = true, MapName = mapName, Map = mapArray, MapNumber = mapNumber };
                    Debug.WriteLine(sq.MapNumber);
                    gR.Serialize<GameRequest>(stream);
                }
                e.Canceled = false;
            }
            catch (Exception ex)
            {
                if (!e.Canceled)
                {
                    writer.Error(e.Player + " : Caught error deserializing stream or reading SpecialRequest object.");
                }
                e.Canceled = true;
            }
        }
    }
}
using System;
using System.Net;
using System.Net.Sockets;

using ConsoleUtils.Logging;
using ConsoleUtils.Writing;
using Serialization;

namespace ConsoleUtils.Servers.TCP
{
    public class CommandArgs : EventArgs
    {
        private string message;

        public CommandArgs(string message)
        {
            this.message = message;
        }

        public string Message
        {
            get
            {
                return message;
            }
        }
    }

    public class LogArgs : EventArgs
    {
        private string message;
        private Color messageColor;

        public LogArgs(string message, Color messageColor)
        {
            this.message = message;
            this.messageColor = messageColor;
        }

        public string Message
        {
            get
            {
                return message;
            }
        }

        public Color MessageColor
        {
            get
            {
                return messageColor;
            }
        }
    }

    public class StreamArgs : EventArgs
    {
        private NetworkStream stream;
        private bool closed;

        public StreamArgs(NetworkStream stream, bool closed)
        {
            Stream = stream;
            Canceled = false;
            ErrorThrown = false;
        }

        public NetworkStream Stream { get; private set; }

        public bool Canceled { get; set; }

        public bool ErrorThrown { get; set; }

        public String Player { get; set; }
    }

    public class AsyncTCPServer
    {
        private bool _ENABLED = false;
        private IPAddress _IPADDRESS;
        private int _PORT;
        private bool _HANDLEKEYS = false;
        private bool _RAN = false;
        private bool _OUTPUTLOG = true;
        private TcpListener _LISTENER;
        private InitWriterLogger __INIT;
        private Writer writer;
        private Logger logger;

        public delegate void CommandHandler(object myObject, CommandArgs myArgs);

        public event CommandHandler OnCommand;

        public delegate void LogHandler(object myObject, LogArgs myArgs);

        public event LogHandler OnLog;

        public delegate void StreamHandler(object myObject, StreamArgs myArgs);

        public event StreamHandler OnStream;

        /// <summary>
        /// An asynchronous TCP server
        /// </summary>
        /// <param name="ip">The IP address to host the server on</param>
        /// <param name="port">The port to host the server on</param>
        /// <param name="handleKeys">Boolean which determines whether to pass keys to the handleKeyMethod</param>
        /// <param name="outputLog">Boolean which determines whether or not to log</param>
        /// <param name="writer">Allows you to define a Writer to use from another instance</param>
        /// <param name="logger">Allows you to define a Logger to use from another instance to use its parameters</param>
        public AsyncTCPServer(string ip = "127.0.0.1",
            int port = 1313,
            bool handleKeys = false,
            bool outputLog = true,
            InitWriterLogger INIT = null)
        {
            if (INIT != null)
            {
                __INIT = INIT;
                writer = INIT.Writer;
                logger = INIT.Logger;
            }
            else
            {
                __INIT = new InitWriterLogger(false);
                writer = __INIT.Writer;
                logger = __INIT.Logger;
            }
            _HANDLEKEYS = handleKeys;
            _OUTPUTLOG = outputLog;
            if (ip != null)
                _IPADDRESS = IPAddress.Parse(ip);
            if (port != 0)
                _PORT = port;
        }

        /// <summary>
        /// Start the TCP server
        /// </summary>
        public void Start()
        {
            if (!_ENABLED)
            {
                _LISTENER = new TcpListener(_IPADDRESS, _PORT);
                _LISTENER.Start();
                if (_OUTPUTLOG)
                {
                    Log("[SERVER] Waiting for a connection...", Color.Magenta);
                }
                _ENABLED = true;
                StartAccept();
                if (_HANDLEKEYS)
                {
                    HandleKey(Console.ReadLine());
                }
                else
                {
                    Console.ReadLine();
                    HandleKey(null);
                }
            }
            else
            {
                if (_HANDLEKEYS)
                {
                    HandleKey(Console.ReadLine());
                }
                else
                {
                    Console.ReadLine();
                    HandleKey(null);
                }
            }
        }

        /// <summary>
        /// Stop the TCP server
        /// </summary>
        public void Stop()
        {
            _ENABLED = false;
        }

        private void StartAccept()
        {
            if (_ENABLED)
                _LISTENER.BeginAcceptTcpClient(HandleAsyncClient, _LISTENER);
            else
        }

        private void HandleAsyncClient(IAsyncResult res)
        {
            _RAN = true;
            StartAccept();
            TcpClient client = _LISTENER.EndAcceptTcpClient(res);
            NetworkStream stream = client.GetStream();

            Log("[SERVER] New connection obtained from " + client.Client.RemoteEndPoint, Color.Magenta);
            try
            {
                if (stream.DeSerialize<GameRequest>().Player.PlayerID == PlayerID.Player1)
                    Log("[SERVER] Stream successfully retrieved from client. Assigned Player ID: Player1", Color.Magenta);
                else if (stream.DeSerialize<GameRequest>().Player.PlayerID == PlayerID.Player2)
                    Log("[SERVER] Stream successfully retrieved from client. Assigned Player ID: Player2", Color.Magenta);
                else
                    Log("[SERVER] Stream successfully retrieved from client.", Color.Magenta);
            }
            catch (System.IO.IOException ex) { }

            while (true)
            {
                StreamArgs myArgs = new StreamArgs(stream, false);
                try
                {
                    if (stream.DeSerialize<GameRequest>().Player.PlayerID == PlayerID.Player1)
                        myArgs.Player = "Player1";
                    if (stream.DeSerialize<GameRequest>().Player.PlayerID == PlayerID.Player2)
                        myArgs.Player = "Player2";
                } catch (System.IO.IOException ex) { }
                OnStream(this, myArgs);
                if (myArgs.Canceled)
                {
                    Log("[SERVER] Closing connection to: " + client.Client.RemoteEndPoint, Color.Magenta);
                    stream.Close();
                    client.Close();
                    break;
                }
            }
        }

        private void HandleKey(string input)
        {
            if (_HANDLEKEYS)
            {
                CommandArgs myArgs = new CommandArgs(input);
                OnCommand(this, myArgs);
            }
            Start();
        }

        private void Log(string message, Color messageColor)
        {
            LogArgs myArgs = new LogArgs(message, messageColor);
            OnLog(this, myArgs);
        }
    }
}
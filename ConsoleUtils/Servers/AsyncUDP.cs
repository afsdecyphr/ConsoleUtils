using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ConsoleUtils.Logging;
using ConsoleUtils.Writing;

namespace ConsoleUtils.Servers.UPD
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

    public class AsyncUDP
    {
        private string _IP;
        private int _PORT;
        private bool _HANDLEKEYS;
        private bool _OUTPUTLOG;
        private bool _RUNNING = false;
        private bool _STOP = false;

        private UdpClient _LISTENER;

        private InitWriterLogger __INIT;
        private Writer writer;
        private Logger logger;

        public delegate void LogHandler(object myObject, LogArgs myArgs);

        public event LogHandler OnLog;

        public delegate void CommandHandler(object myObject, CommandArgs myArgs);

        public event CommandHandler OnCommand;

        public AsyncUDP(int port = 1313,
            bool handleKeys = false,
            bool outputLog = true,
            InitWriterLogger INIT = null)
        {
            _PORT = port;
            _HANDLEKEYS = handleKeys;
            _OUTPUTLOG = outputLog;
            __INIT = INIT;
            _LISTENER = new UdpClient(port);
        }

        public void Start()
        {
            if (!_RUNNING)
            {
                Log("[SERVER] Server started.", Color.Green);
                _RUNNING = false;
                _STOP = false;
                Init();
            }
            else
            {
                Log("[SERVER] Server already started.", Color.Green);
            }
        }

        public void Stop()
        {
            if (_RUNNING)
            {
                _RUNNING = false;
                _STOP = true;
                Log("[SERVER] Server stopped.", Color.Red);
            }
            else
            {
                Log("[SERVER] Server already stopped.", Color.Red);
            }
        }

        private void Init()
        {
            if (!_RUNNING && !_STOP)
            {
                _RUNNING = true;
                StartAccept();
                HandleKey(Console.ReadLine());
            }
            else
            {
                HandleKey(Console.ReadLine());
            }
        }

        private void StartAccept()
        {
            if (_RUNNING && !_STOP)
            {
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                UdpState s = new UdpState();
                s.e = RemoteIpEndPoint;
                s.u = _LISTENER;
                _LISTENER.BeginReceive(new AsyncCallback(HandleClient), s);
            }
        }

        private void HandleClient(IAsyncResult res)
        {
            StartAccept();
            if (_RUNNING && !_STOP)
            {
                UdpClient u = (UdpClient)((UdpState)(res.AsyncState)).u;
                IPEndPoint e = (IPEndPoint)((UdpState)(res.AsyncState)).e;

                Byte[] receiveBytes = u.EndReceive(res, ref e);
                string receiveString = Encoding.ASCII.GetString(receiveBytes);

                Log("[SERVER] Received: " + receiveString, Color.Magenta);
            }
        }

        private void HandleKey(string input)
        {
            if (_HANDLEKEYS)
            {
                CommandArgs myArgs = new CommandArgs(input);
                OnCommand(this, myArgs);
            }
            Init();
        }

        private void Log(object message, Color messageColor)
        {
            if (_OUTPUTLOG)
            {
                LogArgs myArgs = new LogArgs(message.ToString(), messageColor);
                OnLog(this, myArgs);
            }
        }

        public int Port
        {
            get
            {
                return _PORT;
            }
        }

        public bool IsHandlingKeys
        {
            get
            {
                return _HANDLEKEYS;
            }
        }

        public bool IsOutputing
        {
            get
            {
                return _OUTPUTLOG;
            }
        }

        public string Status
        {
            get
            {
                if (_RUNNING == true)
                    return "Server running.";
                else
                    return "Server not running.";
            }
        }
    }

    internal class UdpState
    {
        public IPEndPoint e;
        public UdpClient u;

        public UdpState()
        {
        }
    }
}
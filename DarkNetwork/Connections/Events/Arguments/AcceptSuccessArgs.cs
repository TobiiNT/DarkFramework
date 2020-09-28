using System;
using System.Net.Sockets;

namespace DarkNetwork.Connections.Events.Arguments
{
    public class AcceptSuccessArgs : EventArgs
    {
        public AcceptSuccessArgs(Socket Socket) => this.Socket = Socket;
        public Socket Socket { private set; get; }
    }
}

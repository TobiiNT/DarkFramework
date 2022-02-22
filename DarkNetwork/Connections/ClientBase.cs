using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using DarkNetwork.Connections.Events;
using DarkNetwork.Connections.Events.Arguments;
using DarkNetwork.Enums;
using DarkNetwork.Structures;
using DarkPacket.Readers;
using DarkPacket.Writer;

namespace DarkNetwork.Connections
{
    public class ClientBase : ClientEventHandler
    {
        private AsyncStates AsyncState { set; get; }
        private Socket Socket { set; get; }
        private SendQueue SendQueue { set; get; }
        private ReceiveQueue ReceiveQueue { get; set; }
        private AsyncCallback OnConnected { set; get; }
        private AsyncCallback OnReceived { set; get; }
        private AsyncCallback OnSended { set; get; }
        private byte[] DataReceived { set; get; }
        private IPEndPoint IPEndPoint { set; get; }
        private bool KeepAliveOn { set; get; } = true;
        private int KeepAliveTime { set; get; } = 20000;
        private int KeepAliveInterval { set; get; } = 20000;
        private int KeepAliveRetryCount { set; get; } = 10;
        private bool SocketIsRunning { set; get; }
        private bool IsDisposed { set; get; }
        public IPEndPoint GetIPEndpoint() => this.IPEndPoint;
        public bool IsRunning() => this.SocketIsRunning;

        public ClientBase()
        {
            this.OnConnected = new AsyncCallback(this.OnConnectedSocket);
            this.OnReceived = new AsyncCallback(this.OnReceiveData);
            this.OnSended = new AsyncCallback(this.OnSendData);

            this.SocketIsRunning = false;
            this.IsDisposed = false;

            this.SendQueue = new SendQueue();
            this.ReceiveQueue = new ReceiveQueue();
            this.DataReceived = new byte[102400];
        }
        protected void Start(string ServerIPAddress, int Port, bool KeepAliveOn = true, int KeepAliveTime = 20000, int KeepAliveInterval = 20000)
        {
            try
            {
                this.IPEndPoint = new IPEndPoint(IPAddress.Parse(ServerIPAddress), Port);

                this.KeepAliveOn = KeepAliveOn;
                this.KeepAliveTime = KeepAliveTime;
                this.KeepAliveInterval = KeepAliveInterval;

                this.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                this.Socket.BeginConnect(this.IPEndPoint, this.OnConnected, this.Socket);

                OnStartSuccess(this, new StartSuccessArgs());
            }
            catch (Exception Exception)
            {
                OnStartException(this, new StartExceptionArgs(Exception));
            }
        }
        protected void Start(Socket Socket)
        {
            try
            {
                this.Socket = Socket;

                this.IPEndPoint = (IPEndPoint)Socket.RemoteEndPoint;

                this.StartReceiving();

                OnStartSuccess(this, new StartSuccessArgs());
            }
            catch (Exception Exception)
            {
                OnStartException(this, new StartExceptionArgs(Exception));
            }
        }

        private void OnConnectedSocket(IAsyncResult AsyncResult)
        {
            try
            {
                if (this.Socket.Connected)
                {
                    this.Socket.EndConnect(AsyncResult);

                    this.StartReceiving();

                    OnConnectSuccess(this, new ConnectSuccessArgs());
                }
            }
            catch (Exception Exception)
            {
                OnConnectException(this, new ConnectExceptionArgs(Exception));
                this.Dispose();
            }
        }
        private void OnSendData(IAsyncResult AsyncResult)
        {
            if (this.Socket == null)
            {
                OnSendException(this, new SendExceptionArgs(new NullReferenceException("Socket is null")));
                this.Dispose();
            }
            else
            {
                try
                {
                    if (this.Socket.Connected && this.SocketIsRunning)
                    {
                        var DataLength = this.Socket.EndSend(AsyncResult, out SocketError Result);

                        if (Result == SocketError.Success && DataLength > 0)
                        {
                            OnSendSuccess(this, new SendSuccessArgs(DataLength));

                            SendQueue.Gram DataPending;
                            lock (this.SendQueue)
                            {
                                DataPending = this.SendQueue.Dequeue();
                            }
                            if (DataPending != null)
                            {
                                this.Socket.BeginSend(DataPending.Buffer, 0, DataPending.Length, SocketFlags.None, this.OnSended, this.Socket);
                            }
                        }
                        else
                        {
                            OnSendException(this, new SendExceptionArgs(new SocketException()));
                            this.Dispose();
                        }
                    }
                }
                catch (Exception Exception)
                {
                    OnSendException(this, new SendExceptionArgs(Exception));
                    this.Dispose();
                }
            }
        }
        private void OnReceiveData(IAsyncResult AsyncResult)
        {
            if (this.Socket == null)
            {
                OnReceiveException(this, new ReceiveExceptionArgs(new NullReferenceException("Socket is null")));
                this.Dispose();
            }
            else
            {
                try
                {
                    if (this.Socket.Connected && this.SocketIsRunning)
                    {
                        var DataLength = this.Socket.EndReceive(AsyncResult, out var Result);

                        if (Result == SocketError.Success && DataLength > 0)
                        {
                            var NewDataReceived = this.DataReceived;
                            lock (this.ReceiveQueue)
                            {
                                this.ReceiveQueue.Enqueue(NewDataReceived, 0, DataLength);
                            }
                            this.HandleReceive();

                            this.AsyncState &= ~AsyncStates.Pending;
                            if ((this.AsyncState & AsyncStates.Paused) == AsyncStates.Empty)
                            {
                                try
                                {
                                    this.InternalBeginReceive();
                                }
                                catch (Exception Exception)
                                {
                                    OnReceiveException(this, new ReceiveExceptionArgs(Exception));
                                    this.Dispose();
                                }
                            }
                        }
                        else
                        {
                            OnReceiveException(this, new ReceiveExceptionArgs(new SocketException()));
                            this.Dispose();
                        }
                    }
                    else
                    {
                        OnDisconnectSuccess(this, new DisconnectSuccessArgs());
                        this.Dispose();
                    }
                }
                catch (Exception Exception)
                {
                    OnReceiveException(this, new ReceiveExceptionArgs(Exception));
                    this.Dispose();
                }
            }
        }

        private void StartReceiving()
        {
            this.SocketIsRunning = true;

            if (this.AsyncState == AsyncStates.Empty)
            {
                this.InternalBeginReceive();
            }
        }

        private void InternalBeginReceive()
        {
            try
            {
                this.AsyncState |= AsyncStates.Pending;
                this.Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, this.KeepAliveOn);
                this.Socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, this.KeepAliveTime);
                this.Socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, this.KeepAliveInterval);
                this.Socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveRetryCount, this.KeepAliveRetryCount);

                this.Socket.BeginReceive(this.DataReceived, 0, this.DataReceived.Length, SocketFlags.None, out var Result, this.OnReceived, this.Socket);
            }
            catch (Exception Exception)
            {
                OnReceiveException(this, new ReceiveExceptionArgs(Exception));
                this.Dispose();
            }
        }
        protected void Send(byte[] Data)
        {
            try
            {
                if (this.Socket != null && this.Socket.Connected && this.SocketIsRunning)
                {
                    using (var Packet = new PacketWriter())
                    {
                        Packet.WriteByte(170);
                        Packet.WriteByte(85);
                        Packet.WriteBytes(Data);
                        Packet.WriteByte(85);
                        Packet.WriteByte(170);

                        Data = Packet.GetPacketData();
                    }

                    SendQueue.Gram DataSend;
                    lock (this.SendQueue)
                    {
                        DataSend = this.SendQueue.Enqueue(Data, Data.Length);
                    }
                    if (DataSend != null)
                    {
                        this.Socket.BeginSend(DataSend.Buffer, 0, DataSend.Length, SocketFlags.None, this.OnSended, this.Socket);
                    }
                }
            }
            catch (Exception Exception)
            {
                OnSendException(this, new SendExceptionArgs(Exception));
            }
        }
        private void HandleReceive()
        {
            try
            {
                if (this.ReceiveQueue != null && this.ReceiveQueue.Length > 0)
                {
                    lock (this.ReceiveQueue)
                    {
                        var BufferLength = this.ReceiveQueue.Length;
                        while (BufferLength > 0 && this.SocketIsRunning)
                        {
                            if (BufferLength > 4)
                            {
                                int CurrentPacketLength = this.ReceiveQueue.GetCurrentPacketSize();
                                if (CurrentPacketLength <= 0)
                                {
                                    this.ReceiveQueue.Clear();
                                }
                                else
                                {
                                    CurrentPacketLength += 8;
                                    if (BufferLength >= CurrentPacketLength)
                                    {
                                        var CurrentPacketData = new byte[CurrentPacketLength];
                                        this.ReceiveQueue.Dequeue(CurrentPacketData, 0, CurrentPacketLength);
                                        BufferLength = this.ReceiveQueue.Length;

                                        if (CurrentPacketData[0] == 170 &&
                                            CurrentPacketData[1] == 85 &&
                                            CurrentPacketData[CurrentPacketLength - 1] == 170 &&
                                            CurrentPacketData[CurrentPacketLength - 2] == 85)
                                        {
                                            using var Packet = new NetworkPacketReader(CurrentPacketData);
                                            var MainData = Packet.ReadBytes();
                                            OnReceiveSuccess(this, new ReceiveSuccessArgs(CurrentPacketLength, MainData));
                                            continue;
                                        }
                                        this.ReceiveQueue.Clear();
                                    }
                                    else if (this.ReceiveQueue.BufferData[0] != 170 && this.ReceiveQueue.BufferData[1] != 85)
                                    {
                                        this.ReceiveQueue.Clear();
                                    }
                                }
                            }
                            return;
                        }
                    }
                }
            }
            catch (Exception Exception)
            {
                OnReceiveException(this, new ReceiveExceptionArgs(Exception));
            }
        }

        protected void StopConnection()
        {
            var Caller = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name;

            try
            {
                if (this.Socket != null)
                {
                    this.Socket.Shutdown(SocketShutdown.Both);
                }


                if (this.Socket != null)
                {
                    this.Socket.Close();
                }

                OnStopSuccess(this, new StopSuccessArgs(Caller));
            }
            catch (Exception Exception)
            {
                OnStopException(this, new StopExceptionArgs(Caller, Exception));
            }
            finally
            {
                this.Socket = null;
            }
        }

        public void Dispose()
        {
            if (!this.IsDisposed)
            {
                var Caller = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name;

                try
                {
                    this.SocketIsRunning = false;

                    if (this.SendQueue != null)
                    {
                        if (!this.SendQueue.IsEmpty)
                        {
                            lock (this.SendQueue)
                            {
                                this.SendQueue.Clear();
                            }
                        }
                        this.SendQueue.Dispose();
                        this.SendQueue = null;
                    }

                    if (this.ReceiveQueue != null)
                    {
                        this.ReceiveQueue.Dispose();
                        this.ReceiveQueue = null;
                    }

                    this.StopConnection();

                    this.DataReceived = null;
                    this.OnConnected = null;
                    this.OnReceived = null;
                    this.OnSended = null;

                    OnDisposeSuccess(this, new DisposeSuccessArgs(Caller));
                }
                catch (Exception Exception)
                {
                    OnDisposeException(this, new DisposeExceptionArgs(Caller, Exception));
                }
                finally
                {
                    this.IsDisposed = true;
                }
            }
        }
    }
}

using DarkNetwork.Networks.Connections.Events;
using DarkNetwork.Networks.Connections.Events.Arguments;
using DarkNetwork.Networks.Enums;
using DarkNetwork.Networks.Structures;
using DarkPacket.Packets;
using DarkPacket.Readers;
using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace DarkNetwork.Networks.Connections
{
    public class ConnectionBase : ConnectionEventHandler
    {
        protected AsyncStates AsyncState { set; get; }
        protected Socket Socket { set; get; }
        protected SendQueue SendQueue { set; get; }
        protected ReceiveQueue ReceiveQueue { get; set; }
        protected AsyncCallback OnConnected{ set; get; }
        protected AsyncCallback OnReceived { set; get; }
        protected AsyncCallback OnSended { set; get; }
        protected byte[] DataReceived { set; get; }
        public IPEndPoint IPEndPoint { protected set; get; }
        public bool IsRunning { protected set; get; }
        public bool IsDisposed { protected set; get; }

        public ConnectionBase()
        {
            this.OnConnected = new AsyncCallback(this.OnConnectedSocket);
            this.OnReceived = new AsyncCallback(this.OnReceiveData);
            this.OnSended = new AsyncCallback(this.OnSendData);

            this.IsRunning = false;
            this.IsDisposed = false;
            this.SendQueue = new SendQueue();
            this.ReceiveQueue = new ReceiveQueue();
            this.DataReceived = new byte[102400];
        }
        protected void Start(string ServerIPAddress, int Port)
        {
            try
            {
                this.IPEndPoint = new IPEndPoint(IPAddress.Parse(ServerIPAddress), Port);

                this.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                
                this.Socket.UseOnlyOverlappedIO = true;
                this.Socket.BeginConnect(IPEndPoint, OnConnected, this.Socket);

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
                this.IPEndPoint = (IPEndPoint)Socket.RemoteEndPoint;

                this.Socket = Socket;

                this.StartReceiving();

                OnStartSuccess(this, new StartSuccessArgs());
            }
            catch (Exception Exception)
            {
                OnStartException(this, new StartExceptionArgs(Exception));
            }
        }

        protected void OnConnectedSocket(IAsyncResult AsyncResult)
        {
            try
            {
                Socket CurrentSocket = (Socket)AsyncResult.AsyncState;

                if (CurrentSocket.Connected)
                {
                    CurrentSocket.EndConnect(AsyncResult);

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
        protected void OnSendData(IAsyncResult AsyncResult)
        {
            if (this.Socket == null)
            {
                this.Dispose();
            }
            try
            {
                if (this.Socket.Connected && this.IsRunning)
                {
                    Socket AsyncState = (Socket)AsyncResult.AsyncState;
                    int num = AsyncState.EndSend(AsyncResult);
                    if (num <= 0)
                    {
                        this.Dispose();
                    }
                    else
                    {
                        SendQueue.Gram DataSend;
                        lock (this.SendQueue)
                        {
                            DataSend = this.SendQueue.Dequeue();
                        }
                        if (DataSend != null)
                        {
                            AsyncState.BeginSend(DataSend.Buffer, 0, DataSend.Length, SocketFlags.None, this.OnSended, AsyncState);
                            OnSendSuccess(this, new SendSuccessArgs(DataSend.Length));
                        }
                    }
                }
            }
            catch (Exception)
            {
                this.Dispose();
            }
        }
        protected void OnReceiveData(IAsyncResult AsyncResult)
        {
            if (this.Socket == null)
            {
                this.Dispose();
            }
            else
            {
                try
                {
                    Socket AsyncState = (Socket)AsyncResult.AsyncState;
                    int DataLength = AsyncState.EndReceive(AsyncResult, out SocketError Result);
                    if (Result == SocketError.Success && DataLength > 0)
                    {
                        byte[] recvBuffer = this.DataReceived;
                        lock (this.ReceiveQueue)
                        {
                            this.ReceiveQueue.Enqueue(recvBuffer, 0, DataLength);
                        }
                        this.HandleReceive();
                        if (!this.IsDisposed)
                        {
                            this.AsyncState &= ~AsyncStates.Pending;
                            if ((this.AsyncState & AsyncStates.Paused) == 0)
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
                    }
                    else
                    {
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

        protected void StartReceiving()
        {
            if ((this.AsyncState & (AsyncStates.Paused | AsyncStates.Pending)) == 0)
            {
                this.IsRunning = true;

                this.InternalBeginReceive();
            }
        }
        protected void InternalBeginReceive()
        {
            try
            {
                byte[] InOptionValues = new byte[Marshal.SizeOf(0) * 3];
                BitConverter.GetBytes((uint)1).CopyTo(InOptionValues, 0);
                BitConverter.GetBytes((uint)20000).CopyTo(InOptionValues, Marshal.SizeOf(0));
                BitConverter.GetBytes((uint)20000).CopyTo(InOptionValues, Marshal.SizeOf(0) * 2);

                this.AsyncState |= AsyncStates.Pending;
                this.Socket.IOControl(IOControlCode.KeepAliveValues, InOptionValues, null);
                this.Socket.BeginReceive(this.DataReceived, 0, this.DataReceived.Length, SocketFlags.None, out SocketError Result, this.OnReceived, this.Socket);
            }
            catch (Exception)
            {
                this.Dispose();
            }
        }
        protected void Send(byte[] Data)
        {
            try
            {
                if (this.Socket != null && this.IsRunning && this.Socket.Connected)
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
                        OnSendSuccess(this, new SendSuccessArgs(DataSend.Length));
                    }
                }
            }
            catch (Exception Exception)
            {
                OnSendException(this, new SendExceptionArgs(Exception));
            }
        }
        protected void HandleReceive()
        {
            try
            {
                if (this.ReceiveQueue != null && this.ReceiveQueue.Length > 0)
                {
                    lock (this.ReceiveQueue)
                    {
                        int BufferLength = this.ReceiveQueue.Length;
                        while (BufferLength > 0 && this.IsRunning)
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
                                        byte[] CurrentPacketData = new byte[CurrentPacketLength];
                                        this.ReceiveQueue.Dequeue(CurrentPacketData, 0, CurrentPacketLength);
                                        BufferLength = this.ReceiveQueue.Length;

                                        if (CurrentPacketData[0] == 170 && 
                                            CurrentPacketData[1] == 85 &&
                                            CurrentPacketData[CurrentPacketLength - 1] == 170 && 
                                            CurrentPacketData[CurrentPacketLength - 2] == 85)
                                        {
                                            using (var Packet = new NetworkPacketReader(CurrentPacketData))
                                            {
                                                byte[] MainData = Packet.ReadBytes();
                                                OnReceiveSuccess(this, new ReceiveSuccessArgs(CurrentPacketLength, MainData));
                                            }
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


        public void Dispose()
        {
            string Caller = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name;

            try
            {
                this.IsDisposed = true;
                this.IsRunning = false;

                this.Socket?.Shutdown(SocketShutdown.Both);
                this.Socket?.Close();
                this.Socket = null;

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

                this.ReceiveQueue?.Dispose();
                this.ReceiveQueue = null;
                this.DataReceived = null;
                this.OnReceived = null;
                this.OnSended = null;

                OnDisposeSuccess(this, new DisposeSuccessArgs(Caller));
            }
            catch (Exception Exception)
            {
                OnDisposeException(this, new DisposeExceptionArgs(Caller, Exception));
            }
        }
    }
}

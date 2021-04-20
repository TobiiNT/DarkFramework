using System;
using DarkNetwork.Connections.Events.Arguments;

namespace DarkNetwork.Connections.Events
{
    public class ClientEventHandler
    {
        public event EventHandler EventStartSuccess;

        public event EventHandler EventStartException;

        public event EventHandler EventConnectSuccess;

        public event EventHandler EventConnectException;

        public event EventHandler EventSendSuccess;

        public event EventHandler EventSendException;

        public event EventHandler EventReceiveSuccess;

        public event EventHandler EventReceiveException;

        public event EventHandler EventStopSuccess;

        public event EventHandler EventStopException;

        public event EventHandler EventDisposeSuccess;

        public event EventHandler EventDisposeException;

        protected void OnStartSuccess(object Sender, StartSuccessArgs Event) => EventStartSuccess?.Invoke(Sender, Event);
        protected void OnStartException(object Sender, StartExceptionArgs Event) => EventStartException?.Invoke(Sender, Event);
        protected void OnConnectSuccess(object Sender, ConnectSuccessArgs Event) => EventConnectSuccess?.Invoke(Sender, Event);
        protected void OnConnectException(object Sender, ConnectExceptionArgs Event) => EventConnectException?.Invoke(Sender, Event);
        protected void OnSendSuccess(object Sender, SendSuccessArgs Event) => EventSendSuccess?.Invoke(Sender, Event);
        protected void OnSendException(object Sender, SendExceptionArgs Event) => EventSendException?.Invoke(Sender, Event);
        protected void OnReceiveSuccess(object Sender, ReceiveSuccessArgs Event) => EventReceiveSuccess?.Invoke(Sender, Event);
        protected void OnReceiveException(object Sender, ReceiveExceptionArgs Event) => EventReceiveException?.Invoke(Sender, Event);
        protected void OnStopSuccess(object Sender, StopSuccessArgs Event) => EventStopSuccess?.Invoke(Sender, Event);
        protected void OnStopException(object Sender, StopExceptionArgs Event) => EventStopException?.Invoke(Sender, Event);
        protected void OnDisposeSuccess(object Sender, DisposeSuccessArgs Event) => EventDisposeSuccess?.Invoke(Sender, Event);
        protected void OnDisposeException(object Sender, DisposeExceptionArgs Event) => EventDisposeException?.Invoke(Sender, Event);
    }
}

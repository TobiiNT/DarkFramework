using DarkNetwork.Networks.Connections.Events.Arguments;
using System;

namespace DarkNetwork.Networks.Connections.Events
{
    public class ConnectionEventHandler
    {
        public event EventHandler EventStartSuccess = null;

        public event EventHandler EventStartException = null;

        public event EventHandler EventConnectSuccess = null;

        public event EventHandler EventConnectException = null;

        public event EventHandler EventSendSuccess = null;

        public event EventHandler EventSendException = null;

        public event EventHandler EventReceiveSuccess = null;

        public event EventHandler EventReceiveException = null;

        public event EventHandler EventDisposeSuccess = null;

        public event EventHandler EventDisposeException = null;

        protected void OnStartSuccess(object Sender, StartSuccessArgs Event) => EventStartSuccess?.Invoke(Sender, Event);
        protected void OnStartException(object Sender, StartExceptionArgs Event) => EventStartException?.Invoke(Sender, Event);
        protected void OnConnectSuccess(object Sender, ConnectSuccessArgs Event) => EventConnectSuccess?.Invoke(Sender, Event);
        protected void OnConnectException(object Sender, ConnectExceptionArgs Event) => EventConnectException?.Invoke(Sender, Event);
        protected void OnSendSuccess(object Sender, SendSuccessArgs Event) => EventSendSuccess?.Invoke(Sender, Event);
        protected void OnSendException(object Sender, SendExceptionArgs Event) => EventSendException?.Invoke(Sender, Event);
        protected void OnReceiveSuccess(object Sender, ReceiveSuccessArgs Event) => EventReceiveSuccess?.Invoke(Sender, Event);
        protected void OnReceiveException(object Sender, ReceiveExceptionArgs Event) => EventReceiveException?.Invoke(Sender, Event);
        protected void OnDisposeSuccess(object Sender, DisposeSuccessArgs Event) => EventDisposeSuccess?.Invoke(Sender, Event);
        protected void OnDisposeException(object Sender, DisposeExceptionArgs Event) => EventDisposeException?.Invoke(Sender, Event);
    }
}

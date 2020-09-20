using DarkNetwork.Networks.Connections.Events.Arguments;
using System;

namespace DarkNetwork.Networks.Connections.Events
{
    public class SocketEventHandler
    {
        public event EventHandler EventListenSuccess = null;

        public event EventHandler EventListenException = null;

        public event EventHandler EventAcceptSuccess = null;

        public event EventHandler EventAcceptException = null;
        
        public event EventHandler EventDisposeSuccess = null;

        public event EventHandler EventDisposeException = null;

        protected void OnListenSuccess(object Sender, ListenSuccessArgs Event) => EventListenSuccess?.Invoke(Sender, Event);
        protected void OnListenException(object Sender, ListenExceptionArgs Event) => EventListenException?.Invoke(Sender, Event);
        protected void OnAcceptSuccess(object Sender, AcceptSuccessArgs Event) => EventAcceptSuccess?.Invoke(Sender, Event);
        protected void OnAcceptException(object Sender, AcceptExceptionArgs Event) => EventAcceptException?.Invoke(Sender, Event);
        protected void OnDisposeSuccess(object Sender, DisposeSuccessArgs Event) => EventDisposeSuccess?.Invoke(Sender, Event);
        protected void OnDisposeException(object Sender, DisposeExceptionArgs Event) => EventDisposeException?.Invoke(Sender, Event);
    }
}

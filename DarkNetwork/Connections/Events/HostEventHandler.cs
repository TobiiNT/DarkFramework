using System;
using DarkNetwork.Connections.Events.Arguments;

namespace DarkNetwork.Connections.Events
{
    public class HostEventHandler
    {
        public event EventHandler EventListenSuccess;

        public event EventHandler EventListenException;

        public event EventHandler EventAcceptSuccess;

        public event EventHandler EventAcceptException;

        public event EventHandler EventStopSuccess;

        public event EventHandler EventStopException;

        public event EventHandler EventDisposeSuccess;

        public event EventHandler EventDisposeException;

        protected void OnListenSuccess(object Sender, ListenSuccessArgs Event) => EventListenSuccess?.Invoke(Sender, Event);
        protected void OnListenException(object Sender, ListenExceptionArgs Event) => EventListenException?.Invoke(Sender, Event);
        protected void OnAcceptSuccess(object Sender, AcceptSuccessArgs Event) => EventAcceptSuccess?.Invoke(Sender, Event);
        protected void OnAcceptException(object Sender, AcceptExceptionArgs Event) => EventAcceptException?.Invoke(Sender, Event);
        protected void OnStopSuccess(object Sender, StopSuccessArgs Event) => EventStopSuccess?.Invoke(Sender, Event);
        protected void OnStopException(object Sender, StopExceptionArgs Event) => EventStopException?.Invoke(Sender, Event);
        protected void OnDisposeSuccess(object Sender, DisposeSuccessArgs Event) => EventDisposeSuccess?.Invoke(Sender, Event);
        protected void OnDisposeException(object Sender, DisposeExceptionArgs Event) => EventDisposeException?.Invoke(Sender, Event);
    }
}

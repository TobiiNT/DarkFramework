using DarkSecurityNetwork.Events.Arguments;
using System;

namespace DarkSecurityNetwork.Events
{
    public class SecurityNetworkEvent
    {
        public event EventHandler EventChannelData = null;
        public event EventHandler EventSendData = null;
        public event EventHandler EventAuthException = null;
        public event EventHandler EventAuthSuccess = null;
        public event EventHandler EventAuthFailed = null;

        public void OnChannelData(object Sender, ChannelDataArgs Event) => EventChannelData?.Invoke(Sender, Event);
        public void OnSendData(object Sender, SendDataArgs Event) => EventSendData?.Invoke(Sender, Event);
        public void OnAuthException(object Sender, AuthExceptionArgs Event) => EventAuthException?.Invoke(Sender, Event);
        public void OnAuthSuccess(object Sender, AuthSuccessArgs Event) => EventAuthSuccess?.Invoke(Sender, Event);
        public void OnAuthFailed(object Sender, AuthFailedArgs Event) => EventAuthFailed?.Invoke(Sender, Event);

    }
}

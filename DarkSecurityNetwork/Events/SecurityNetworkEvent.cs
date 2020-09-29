using DarkSecurityNetwork.Events.Arguments;
using System;

namespace DarkSecurityNetwork.Events
{
    public class SecurityNetworkEvent
    {
        public event EventHandler EventChannelData;
        public event EventHandler EventSendData;
        public event EventHandler EventAuthException;
        public event EventHandler EventAuthSuccess;
        public event EventHandler EventAuthFailed;

        public void OnChannelData(object Sender, ChannelDataArgs Event) => EventChannelData?.Invoke(Sender, Event);
        public void OnSendData(object Sender, SendDataArgs Event) => EventSendData?.Invoke(Sender, Event);
        public void OnAuthException(object Sender, AuthExceptionArgs Event) => EventAuthException?.Invoke(Sender, Event);
        public void OnAuthSuccess(object Sender, AuthSuccessArgs Event) => EventAuthSuccess?.Invoke(Sender, Event);
        public void OnAuthFailed(object Sender, AuthFailedArgs Event) => EventAuthFailed?.Invoke(Sender, Event);

    }
}

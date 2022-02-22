using DarkSecurityNetwork.Events.Arguments;
using System;

namespace DarkSecurityNetwork.Interfaces
{
    public interface ISecurityNetworkEvent
    {
        event EventHandler EventChannelData;
        event EventHandler EventSendData;
        event EventHandler EventAuthException;
        event EventHandler EventAuthSuccess;
        event EventHandler EventAuthFailed;

        void OnChannelData(object Sender, ChannelDataArgs Event);
        void OnSendData(object Sender, SendDataArgs Event);
        void OnAuthException(object Sender, AuthExceptionArgs Event);
        void OnAuthSuccess(object Sender, AuthSuccessArgs Event);
        void OnAuthFailed(object Sender, AuthFailedArgs Event);
    }
}

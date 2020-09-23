using System;

namespace DarkSecurityNetwork.Interfaces
{
    public interface ISecurityNetworkConnection
    {
        void SendDataWithEncryption(byte[] Data);

        void EventAuthenticationSend(object Sender, EventArgs Arguments);
        void EventAuthenticationSuccess(object Sender, EventArgs Arguments);
        void EventAuthenticationFailed(object Sender, EventArgs Arguments);

        void EventCryptoAuthException(object Sender, EventArgs Arguments);
        void EventStartSuccess(object Sender, EventArgs Arguments);
        void EventStartException(object Sender, EventArgs Arguments);
        void EventConnectSuccess(object Sender, EventArgs Arguments);
        void EventConnectException(object Sender, EventArgs Arguments);
        void EventSendSuccess(object Sender, EventArgs Arguments);
        void EventSendException(object Sender, EventArgs Arguments);
        void EventReceiveSuccess(object Sender, EventArgs Arguments);
        void EventReceiveException(object Sender, EventArgs Arguments);
        void EventDisposeSuccess(object Sender, EventArgs Arguments);
        void EventDisposeException(object Sender, EventArgs Arguments);
    }
}

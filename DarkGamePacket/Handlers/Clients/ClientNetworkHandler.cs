using DarkGamePacket.Interfaces;
using System;
using System.Collections.Generic;

namespace DarkGamePacket.Handlers.Clients
{
    public class ClientNetworkHandler<MessageType> where MessageType : ICoreMessage
    {
        public delegate bool MessageHandler<T>(T msg) where T : MessageType;
        private readonly Dictionary<Type, List<Delegate>> _handlers = new Dictionary<Type, List<Delegate>>();

        public void Register<T>(MessageHandler<T> handler) where T : MessageType
        {
            if (handler == null) return;
            if (!_handlers.ContainsKey(typeof(T)))
            {
                _handlers.Add(typeof(T), new List<Delegate>());
            }
            _handlers[typeof(T)].Add(handler);
        }

        public bool OnMessage<T>(T req) where T : MessageType
        {
            var handlerList = _handlers[req.GetType()];
            var success = true;
            foreach (MessageHandler<T> handler in handlerList)
            {
                success = handler(req) && success;
            }
            return success;
        }
    }
}

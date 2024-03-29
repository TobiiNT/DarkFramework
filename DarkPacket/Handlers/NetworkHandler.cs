﻿using DarkPacket.Interfaces;
using System;
using System.Collections.Generic;

namespace DarkPacket.Handlers
{
    public class NetworkHandler<MessageType> where MessageType : ICoreMessage
    {
        public delegate bool MessageHandler<T>(uint ClientID, T msg) where T : MessageType;
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

        public bool OnMessage<T>(uint ClientID, T req) where T : MessageType
        {
            var handlerList = _handlers[req.GetType()];
            var success = true;
            foreach (MessageHandler<T> handler in handlerList)
            {
                success = handler(ClientID, req) && success;
            }
            return success;
        }
    }
}

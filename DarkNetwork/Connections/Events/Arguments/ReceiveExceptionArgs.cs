﻿using System;

namespace DarkNetwork.Networks.Connections.Events.Arguments
{
    public class ReceiveExceptionArgs : EventArgs
    {
        public ReceiveExceptionArgs(Exception Exception) => this.Exception = Exception;
        public Exception Exception { private set; get; }
    }
}
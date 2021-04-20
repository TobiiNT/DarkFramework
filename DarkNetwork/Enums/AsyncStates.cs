using System;

namespace DarkNetwork.Enums
{
    [Flags]
    public enum AsyncStates
    {
        Paused = 2,
        Pending = 1,
        Empty = 0
    }
}

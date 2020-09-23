using System;
using System.Collections.Generic;

namespace DarkThread.Interfaces
{
    public interface IThreadSafeDictionary<TKey, TValue> : IDisposable, IDictionary<TKey, TValue>
    {
        void MergeSafe(TKey key, TValue newValue);
        void RemoveSafe(TKey key);
    }
}

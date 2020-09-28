using System;
using System.Threading;

namespace DarkThreading.Structs
{
    public abstract class BaseLock : IDisposable
    {
        protected ReaderWriterLockSlim LockSlim { private set; get; }
        protected BaseLock(ReaderWriterLockSlim LockSlim) => this.LockSlim = LockSlim;
        public abstract void Dispose();
    }
}

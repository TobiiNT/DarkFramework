using System.Threading;

namespace DarkThreading.Structs
{
    public class ReadLock : BaseLock
    {
        public ReadLock(ReaderWriterLockSlim LockSlim) : base(LockSlim)
        {
            Locks.GetReadLock(LockSlim);
        }

        public override void Dispose()
        {
            Locks.ReleaseReadLock(LockSlim);
        }
    }
}

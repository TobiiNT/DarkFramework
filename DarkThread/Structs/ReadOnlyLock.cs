using System.Threading;

namespace DarkThreading.Structs
{
    public class ReadOnlyLock : BaseLock
    {
        public ReadOnlyLock(ReaderWriterLockSlim LockSlim) : base(LockSlim)
        {
            Locks.GetReadOnlyLock(LockSlim);
        }

        public override void Dispose()
        {
            Locks.ReleaseReadOnlyLock(LockSlim);
        }
    }
}

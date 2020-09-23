using System.Threading;

namespace DarkThread.Structs
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

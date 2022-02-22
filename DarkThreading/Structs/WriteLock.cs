using System.Threading;

namespace DarkThreading.Structs
{
    public class WriteLock : BaseLock
    {
        public WriteLock(ReaderWriterLockSlim LockSlim) : base(LockSlim)
        {
            Locks.GetWriteLock(LockSlim);
        }

        public override void Dispose()
        {
            Locks.ReleaseWriteLock(LockSlim);
        }
    }
}

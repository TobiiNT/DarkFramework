using System.Threading;

namespace DarkThread.Structs
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

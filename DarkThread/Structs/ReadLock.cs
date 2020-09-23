using System.Threading;

namespace DarkThread.Structs
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

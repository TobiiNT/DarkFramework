using System.Threading;

namespace DarkThreading.Structs
{
    public static class Locks
    {
        public static ReaderWriterLockSlim GetLockInstance()
        {
            return GetLockInstance(LockRecursionPolicy.SupportsRecursion);
        }

        public static ReaderWriterLockSlim GetLockInstance(LockRecursionPolicy RecursionPolicy)
        {
            return new ReaderWriterLockSlim(RecursionPolicy);
        }

        public static void GetReadLock(ReaderWriterLockSlim locks)
        {
            for (bool flag = false; !flag; flag = locks.TryEnterUpgradeableReadLock(1))
            {
            }
        }

        public static void GetReadOnlyLock(ReaderWriterLockSlim LockSlim)
        {
            for (bool flag = false; !flag; flag = LockSlim.TryEnterReadLock(1))
            {
            }
        }

        public static void GetWriteLock(ReaderWriterLockSlim LockSlim)
        {
            for (bool flag = false; !flag; flag = LockSlim.TryEnterWriteLock(1))
            {
            }
        }

        public static void ReleaseLock(ReaderWriterLockSlim LockSlim)
        {
            ReleaseWriteLock(LockSlim);
            ReleaseReadLock(LockSlim);
            ReleaseReadOnlyLock(LockSlim);
        }

        public static void ReleaseReadLock(ReaderWriterLockSlim LockSlim)
        {
            if (LockSlim.IsUpgradeableReadLockHeld)
            {
                LockSlim.ExitUpgradeableReadLock();
            }
        }

        public static void ReleaseReadOnlyLock(ReaderWriterLockSlim LockSlim)
        {
            if (LockSlim.IsReadLockHeld)
            {
                LockSlim.ExitReadLock();
            }
        }

        public static void ReleaseWriteLock(ReaderWriterLockSlim LockSlim)
        {
            if (LockSlim.IsWriteLockHeld)
            {
                LockSlim.ExitWriteLock();
            }
        }
    }
}

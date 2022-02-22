using System.Collections.Generic;
using System.Threading;

namespace DarkThreading
{
    public class UniqueIDFactory
    {
        private readonly List<ulong> ListID = new List<ulong>();

        private ulong LastID;

        public UniqueIDFactory(ulong LastID)
        {
            this.LastID = LastID;
            this.ListID.Add(this.LastID);
        }

        public ulong Register(ulong UID)
        {
            lock (ListID)
            {
                this.LastID = UID;

                while (!ListID.Contains(LastID))
                {
                    if (ListID.Contains(LastID))
                    {
                        var Temp = (long)this.LastID;

                        Interlocked.Increment(ref Temp);

                        this.LastID = (ulong)Temp;
                    }
                    else
                    {
                        ListID.Add(LastID);
                        break;
                    }
                }
            }

            return LastID;
        }

        public ulong GetNext()
        {
            lock (ListID)
            {
                var Temp = (long)this.LastID;

                Interlocked.Increment(ref Temp);

                this.LastID = (ulong)Temp;

                while (!ListID.Contains(LastID))
                {
                    if (ListID.Contains(LastID))
                    {
                        Temp = (long)this.LastID;

                        Interlocked.Increment(ref Temp);

                        this.LastID = (ulong)Temp;
                    }
                    else
                    {
                        ListID.Add(LastID);
                        break;
                    }
                }
            }

            return LastID;
        }

        public void Release(ulong UID)
        {
            lock (ListID)
            {
                if (ListID.Contains(UID))
                {
                    ListID.Remove(UID);
                    LastID = UID;
                }
            }
        }

        public bool Contains(ulong UID) => this.ListID.Contains(UID);
    }
}

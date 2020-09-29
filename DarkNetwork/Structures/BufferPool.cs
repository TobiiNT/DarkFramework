using System.Collections.Generic;

namespace DarkNetwork.Structures
{
    public class BufferPool
    {
        private List<BufferPool> Pools { get; }

        private int BufferSize { get; }
        private Queue<byte[]> FreeBuffers { get; }
        private int InitialCapacity { get; }

        public BufferPool(int InitialCapacity, int BufferSize)
        {
            this.Pools = new List<BufferPool>();
            this.InitialCapacity = InitialCapacity;
            this.BufferSize = BufferSize;
            this.FreeBuffers = new Queue<byte[]>(InitialCapacity);
            for (var i = 0; i < InitialCapacity; i++)
            {
                this.FreeBuffers.Enqueue(new byte[BufferSize]);
            }


            lock (Pools)
            {
                Pools.Add(this);
            }
        }

        public byte[] AcquireBuffer()
        {
            lock (this)
            {
                if (this.FreeBuffers.Count <= 0)
                {
                    for (var i = 0; i < this.InitialCapacity; i++)
                    {
                        this.FreeBuffers.Enqueue(new byte[this.BufferSize]);
                    }
                }
                return this.FreeBuffers.Dequeue();
            }
        }

        public void Free()
        {
            lock (Pools)
            {
                Pools.Remove(this);
            }
        }

        public void ReleaseBuffer(byte[] buffer)
        {
            if (buffer != null)
            {
                lock (this)
                {
                    this.FreeBuffers.Enqueue(buffer);
                }
            }
        }
    }
}

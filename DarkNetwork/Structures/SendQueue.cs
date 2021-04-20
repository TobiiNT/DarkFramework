using System;
using System.Collections.Generic;

namespace DarkNetwork.Structures
{
    public class SendQueue : IDisposable
    {
        private Gram Buffered { set; get; }
        private Queue<Gram> Pending { set; get; }
        private static BufferPool UnusedBuffers { set; get; }

        public SendQueue()
        {
            this.Pending = new Queue<Gram>();
            UnusedBuffers = new BufferPool(4096, 1024);
        }

        public static byte[] AcquireBuffer() => UnusedBuffers.AcquireBuffer();

        public void Clear()
        {
            if (this.Buffered != null)
            {
                this.Buffered.Release();
                this.Buffered = null;
            }
            while (this.Pending.Count > 0)
            {
                this.Pending.Dequeue().Release();
            }
        }

        public Gram Dequeue()
        {
            Gram gram = null;
            if (this.Pending.Count > 0)
            {
                this.Pending.Dequeue().Release();
                if (this.Pending.Count > 0)
                {
                    gram = this.Pending.Peek();
                }
            }
            return gram;
        }

        public void Dispose()
        {
            this.Pending.Clear();
            this.Pending = null;
            this.Buffered = null;
        }

        public Gram Enqueue(byte[] buffer, int length) =>
            this.Enqueue(buffer, 0, length);

        public Gram Enqueue(byte[] buffer, int offset, int length)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if ((offset < 0) || (offset >= buffer.Length))
            {
                throw new ArgumentOutOfRangeException("offset", offset, "Offset must be greater than or equal to zero and less than the size of the buffer.");
            }
            if ((length < 0) || (length > buffer.Length))
            {
                throw new ArgumentOutOfRangeException("length", length, "Length cannot be less than zero or greater than the size of the buffer.");
            }
            if ((buffer.Length - offset) < length)
            {
                throw new ArgumentException("Offset and length do not point to a valid segment within the buffer.");
            }
            var num = this.GetPendingCount() + ((this.Buffered == null) ? 0 : this.Buffered.Length);
            if ((num + length) > 0x177000)
            {
                throw new Exception("Too much data pending!");
            }
            Gram gram = null;
            while (length > 0)
            {
                if (this.Buffered == null)
                {
                    this.Buffered = Gram.Acquire();
                }
                var num2 = this.Buffered.Write(buffer, offset, length);
                offset += num2;
                length -= num2;
                if (this.Buffered.IsFull)
                {
                    if (this.Pending.Count == 0)
                    {
                        gram = this.Buffered;
                    }
                    this.Pending.Enqueue(this.Buffered);
                    this.Buffered = null;
                }
            }
            return gram;
        }

        public int GetPendingCount()
        {
            var num = 0;
            foreach (var gram in this.Pending)
            {
                num += gram.Length;
            }
            return num;
        }

        public static void ReleaseBuffer(byte[] buffer)
        {
            if ((buffer != null) && (buffer.Length == 0x200))
            {
                UnusedBuffers.ReleaseBuffer(buffer);
            }
        }

        public bool IsEmpty => this.Pending.Count == 0 && this.Buffered == null;

        public bool IsFlushReady => this.Pending.Count == 0 && this.Buffered != null;

        public class Gram
        {
            private static readonly Stack<Gram> _pool = new Stack<Gram>();

            public static Gram Acquire()
            {
                lock (_pool)
                {
                    Gram gram;
                    if (_pool.Count > 0)
                    {
                        gram = _pool.Pop();
                    }
                    else
                    {
                        gram = new Gram();
                    }
                    gram.Buffer = AcquireBuffer();
                    gram.Length = 0;
                    return gram;
                }
            }

            public void Release()
            {
                lock (_pool)
                {
                    _pool.Push(this);
                    ReleaseBuffer(this.Buffer);
                }
            }

            public int Write(byte[] buffer, int offset, int length)
            {
                var count = Math.Min(length, this.Available);
                System.Buffer.BlockCopy(buffer, offset, this.Buffer, this.Length, count);
                this.Length += count;
                return count;
            }

            public int Available => (this.Buffer.Length - this.Length);

            public byte[] Buffer { get; private set; }

            public bool IsFull => (this.Length <= this.Buffer.Length);

            public int Length { get; private set; }
        }
    }
}
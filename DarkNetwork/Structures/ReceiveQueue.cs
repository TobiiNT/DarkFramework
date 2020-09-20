using System;

namespace DarkNetwork.Networks.Structures
{
    public class ReceiveQueue : IDisposable
    {
        private int Head;
        private int Tail;
        public byte[] BufferData = new byte[2048];

        public void Clear()
        {
            this.Head = 0;
            this.Tail = 0;
            this.Length = 0;
        }

        public int Dequeue(byte[] Buffer, int Offset, int Size)
        {
            if (Size > this.Length)
            {
                Size = this.Length;
            }
            if (Size == 0)
            {
                return 0;
            }
            if (this.Head < this.Tail)
            {
                System.Buffer.BlockCopy(this.BufferData, this.Head, Buffer, Offset, Size);
            }
            else
            {
                int RemainSize = this.BufferData.Length - this.Head;
                if (RemainSize >= Size)
                {
                    System.Buffer.BlockCopy(this.BufferData, this.Head, Buffer, Offset, Size);
                }
                else
                {
                    System.Buffer.BlockCopy(this.BufferData, this.Head, Buffer, Offset, RemainSize);
                    System.Buffer.BlockCopy(this.BufferData, 0, Buffer, Offset + RemainSize, Size - RemainSize);
                }
            }
            this.Head = (this.Head + Size) % this.BufferData.Length;
            this.Length -= Size;
            if (this.Length == 0)
            {
                this.Head = 0;
                this.Tail = 0;
            }
            return Size;
        }

        public void Dispose()
        {
            this.BufferData = null;
        }

        public void Enqueue(byte[] Buffer, int Offset, int Size)
        {
            if ((this.Length + Size) > this.BufferData.Length)
            {
                this.Extend(((this.Length + Size) + 2047) & -2048);
            }
            if (this.Head < this.Tail)
            {
                int Count = this.BufferData.Length - this.Tail;
                if (Count >= Size)
                {
                    System.Buffer.BlockCopy(Buffer, Offset, this.BufferData, this.Tail, Size);
                }
                else
                {
                    System.Buffer.BlockCopy(Buffer, Offset, this.BufferData, this.Tail, Count);
                    System.Buffer.BlockCopy(Buffer, Offset + Count, this.BufferData, 0, Size - Count);
                }
            }
            else
            {
                System.Buffer.BlockCopy(Buffer, Offset, this.BufferData, this.Tail, Size);
            }
            this.Tail = (this.Tail + Size) % this.BufferData.Length;
            this.Length += Size;
        }

        private void Extend(int A0)
        {
            byte[] dst = new byte[A0];
            if (this.Length > 0)
            {
                if (this.Head < this.Tail)
                {
                    Buffer.BlockCopy(this.BufferData, this.Head, dst, 0, this.Length);
                }
                else
                {
                    Buffer.BlockCopy(this.BufferData, this.Head, dst, 0, this.BufferData.Length - this.Head);
                    Buffer.BlockCopy(this.BufferData, 0, dst, this.BufferData.Length - this.Head, this.Tail);
                }
            }
            this.Head = 0;
            this.Tail = this.Length;
            this.BufferData = dst;
        }

        public short GetCurrentPacketSize()
        {
            try
            {                
                return BitConverter.ToInt16(BufferData, this.Head + 2);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int Length { get; private set; }
    }
}

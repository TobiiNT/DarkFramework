using System;

namespace DarkNetwork.Structures
{
    public class ReceiveQueue : IDisposable
    {
        private int _Head;
        private int _Tail;
        private int _Length;
        public int Length => this._Length;

        public byte[] BufferData = new byte[2048];

        public void Clear()
        {
            this._Head = 0;
            this._Tail = 0;
            this._Length = 0;
        }

        public int Dequeue(byte[] Buffer, int Offset, int Size)
        {
            if (Size > this._Length)
            {
                Size = this._Length;
            }
            if (Size == 0)
            {
                return 0;
            }
            if (this._Head < this._Tail)
            {
                System.Buffer.BlockCopy(this.BufferData, this._Head, Buffer, Offset, Size);
            }
            else
            {
                var RemainSize = this.BufferData.Length - this._Head;
                if (RemainSize >= Size)
                {
                    System.Buffer.BlockCopy(this.BufferData, this._Head, Buffer, Offset, Size);
                }
                else
                {
                    System.Buffer.BlockCopy(this.BufferData, this._Head, Buffer, Offset, RemainSize);
                    System.Buffer.BlockCopy(this.BufferData, 0, Buffer, Offset + RemainSize, Size - RemainSize);
                }
            }
            this._Head = (this._Head + Size) % this.BufferData.Length;
            this._Length -= Size;
            if (this._Length == 0)
            {
                this._Head = 0;
                this._Tail = 0;
            }
            return Size;
        }

        public void Dispose()
        {
            this.BufferData = null;
        }

        public void Enqueue(byte[] Buffer, int Offset, int Size)
        {
            if ((this._Length + Size) > this.BufferData.Length)
            {
                this.Extend(((this._Length + Size) + 2047) & -2048);
            }
            if (this._Head < this._Tail)
            {
                var Count = this.BufferData.Length - this._Tail;
                if (Count >= Size)
                {
                    System.Buffer.BlockCopy(Buffer, Offset, this.BufferData, this._Tail, Size);
                }
                else
                {
                    System.Buffer.BlockCopy(Buffer, Offset, this.BufferData, this._Tail, Count);
                    System.Buffer.BlockCopy(Buffer, Offset + Count, this.BufferData, 0, Size - Count);
                }
            }
            else
            {
                System.Buffer.BlockCopy(Buffer, Offset, this.BufferData, this._Tail, Size);
            }
            this._Tail = (this._Tail + Size) % this.BufferData.Length;
            this._Length += Size;
        }

        private void Extend(int A0)
        {
            var dst = new byte[A0];
            if (this._Length > 0)
            {
                if (this._Head < this._Tail)
                {
                    Buffer.BlockCopy(this.BufferData, this._Head, dst, 0, this._Length);
                }
                else
                {
                    Buffer.BlockCopy(this.BufferData, this._Head, dst, 0, this.BufferData.Length - this._Head);
                    Buffer.BlockCopy(this.BufferData, 0, dst, this.BufferData.Length - this._Head, this._Tail);
                }
            }
            this._Head = 0;
            this._Tail = this._Length;
            this.BufferData = dst;
        }

        public short GetCurrentPacketSize()
        {
            try
            {                
                return BitConverter.ToInt16(BufferData, this._Head + 2);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        
    }
}

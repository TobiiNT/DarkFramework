using System;
using System.Text;

namespace DarkPacket.Readers
{
    public class PacketReader : IDisposable
    {
        protected byte[] Data { set; get; }
        protected int Length { set; get; }
        protected int Index { set; get; }

        public byte ReadByte()
        {
            if (this.Index >= this.Length)
            {
                return 0;
            }
            int CurrentIndex = this.Index;
            this.Index += 1;
            return this.Data[CurrentIndex];
        }
        public byte[] ReadBytes()
        {
            int BytesLength = ReadInt();
            byte[] OutputData = new byte[BytesLength];
            int CurrentIndex = this.Index;
            this.Index += BytesLength;
            Buffer.BlockCopy(this.Data, CurrentIndex, OutputData, 0, BytesLength);
            return OutputData;
        }
        public byte[] ReadRemains()
        {
            byte[] OutputData = new byte[this.Length - this.Index];
            int CurrentIndex = this.Index;
            this.Index += OutputData.Length;
            Buffer.BlockCopy(this.Data, CurrentIndex, OutputData, 0, OutputData.Length);
            return OutputData;
        }
        public short ReadShort()
        {
            if (this.Index + 1 >= this.Length)
            {
                return 0;
            }
            int CurrentIndex = this.Index;
            this.Index += 2;
            return BitConverter.ToInt16(this.Data, CurrentIndex);
        }
        public ushort ReadUShort()
        {
            if (this.Index + 1 >= this.Length)
            {
                return 0;
            }
            int CurrentIndex = this.Index;
            this.Index += 2;
            return BitConverter.ToUInt16(this.Data, CurrentIndex);
        }
        public int ReadInt()
        {
            if (this.Index + 3 >= this.Length)
            {
                return 0;
            }
            int CurrentIndex = this.Index;
            this.Index += 4;
            return BitConverter.ToInt32(this.Data, CurrentIndex);
        }
        public uint ReadUInt()
        {
            if (this.Index + 3 >= this.Length)
            {
                return 0;
            }
            int CurrentIndex = this.Index;
            this.Index += 4;
            return BitConverter.ToUInt32(this.Data, CurrentIndex);
        }
        public float ReadFloat()
        {
            if (this.Index + 3 >= this.Length)
            {
                return 0;
            }
            int CurrentIndex = this.Index;
            this.Index += 4;
            return BitConverter.ToSingle(this.Data, CurrentIndex);
        }
        public long ReadLong()
        {
            if (this.Index + 7 >= this.Length)
            {
                return 0;
            }
            int CurrentIndex = this.Index;
            this.Index += 8;
            return BitConverter.ToInt64(this.Data, CurrentIndex);
        }
        public ulong ReadULong()
        {
            if (this.Index + 7 >= this.Length)
            {
                return 0;
            }
            int CurrentIndex = this.Index;
            this.Index += 8;
            return BitConverter.ToUInt64(this.Data, CurrentIndex);
        }
        public double ReadDouble()
        {
            if (this.Index + 7 >= this.Length)
            {
                return 0;
            }
            int CurrentIndex = this.Index;
            this.Index += 8;
            return BitConverter.ToDouble(this.Data, CurrentIndex);
        }
        public string ReadString()
        {
            short StringLength = ReadShort();
            byte[] OutputData = new byte[StringLength];
            int CurrentIndex = this.Index;
            this.Index += StringLength;
            Buffer.BlockCopy(this.Data, CurrentIndex, OutputData, 0, StringLength);
            return Encoding.Unicode.GetString(OutputData);
        }
        public DateTime ReadDateTime()
        {
            int Ticks = ReadInt();
            return new DateTime(Ticks);
        }
        public void Dispose()
        {
            this.Data = null;
        }
    }
}

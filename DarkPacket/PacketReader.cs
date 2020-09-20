using System;
using System.Text;

namespace DarkPacket.Packets
{
    public class PacketReader : IDisposable
    {
        private byte[] Data { set; get; }
        private int Length { set; get; }
        private int Index { set; get; }

        public PacketReader(byte[] Data)
        {
            if (Data.Length < 8)
                throw new Exception($"Invalid Packet");

            this.Length = BitConverter.ToInt32(Data, 2);
            this.Data = new byte[this.Length];
            Buffer.BlockCopy(Data, 6, this.Data, 0, this.Length);
            this.Index = 0;
        }

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
            short BytesLength = ReadShort();
            byte[] OutputData = new byte[BytesLength];
            int CurrentIndex = this.Index;
            this.Index += BytesLength;
            Buffer.BlockCopy(this.Data, CurrentIndex, OutputData, 0, BytesLength);
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
        public void Dispose()
        {
            this.Data = null;
        }
    }
}

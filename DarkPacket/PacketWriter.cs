using System;
using System.IO;
using System.Text;

namespace DarkPacket.Packets
{
    public class PacketWriter : IDisposable
    {
        private MemoryStream DataStream { set; get; }
        public PacketWriter() => DataStream = new MemoryStream();

        public void WriteByte(byte Value) => this.DataStream.WriteByte(Value);
        public void WriteBytes(byte[] Value)
        {
            this.DataStream.Write(BitConverter.GetBytes(Value.Length), 0, 2);
            this.DataStream.Write(Value, 0, Value.Length);
        }
        public void WriteBytes(byte[] Value, int Length)
        {
            this.DataStream.Write(BitConverter.GetBytes(Length), 0, 2);
            this.DataStream.Write(Value, 0, Length);
        }
        public void WriteShort(short Value) => this.DataStream.Write(BitConverter.GetBytes(Value), 0, 2);
        public void WriteUShort(ushort Value) => this.DataStream.Write(BitConverter.GetBytes(Value), 0, 2);
        public void WriteInt(int Value) => this.DataStream.Write(BitConverter.GetBytes(Value), 0, 4);
        public void WriteUInt(uint Value) => this.DataStream.Write(BitConverter.GetBytes(Value), 0, 4);
        public void WriteFloat(float Value) => this.DataStream.Write(BitConverter.GetBytes(Value), 0, 4);
        public void WriteLong(long Value) => this.DataStream.Write(BitConverter.GetBytes(Value), 0, 8);
        public void WriteULong(ulong Value) => this.DataStream.Write(BitConverter.GetBytes(Value), 0, 8);
        public void WriteDouble(double Value) => this.DataStream.Write(BitConverter.GetBytes(Value), 0, 8);
        public void WriteString(string Value)
        {
            if (Value == null)
            {
                Value = string.Empty;
            }
            byte[] StringData = Encoding.Unicode.GetBytes(Value);
            this.DataStream.Write(BitConverter.GetBytes(StringData.Length), 0, 2);
            this.DataStream.Write(StringData, 0, StringData.Length);
        }
        public void WriteString(string Value, int Length)
        {
            if (Value == null)
            {
                Value = string.Empty;
            }
            byte[] StringData = Encoding.Unicode.GetBytes(Value);
            this.DataStream.Write(BitConverter.GetBytes(Math.Min(StringData.Length, Length)), 0, 2);
            this.DataStream.Write(StringData, 0, Math.Min(StringData.Length, Length));
        }
        public byte[] GetPacketData()
        {
            using (MemoryStream OutputStream = new MemoryStream())
            {
                OutputStream.WriteByte(170);
                OutputStream.WriteByte(85);
                OutputStream.Write(BitConverter.GetBytes(this.DataStream.Length), 0, 4);
                OutputStream.Write(this.DataStream.ToArray(), 0, (int)this.DataStream.Length);
                OutputStream.WriteByte(85);
                OutputStream.WriteByte(170);
                return OutputStream.ToArray();
            }
        }
        public void Dispose() => this.DataStream.Dispose();
    }
}

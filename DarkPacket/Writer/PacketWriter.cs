using System;
using System.IO;
using System.Text;
using DarkPacket.Interfaces;

namespace DarkPacket.Writer
{
    public class PacketWriter : IPacketWriter
    {
        private MemoryStream DataStream { set; get; }
        public PacketWriter() => DataStream = new MemoryStream();
        public PacketWriter(byte[] Data)
        {
            this.DataStream = new MemoryStream();
            this.DataStream.Write(Data, 0, Data.Length);
        }
        public void WriteBoolean(bool Value) => this.DataStream.WriteByte((byte)(Value ? 1 : 0));
        public void WriteByte(byte Value) => this.DataStream.WriteByte(Value);
        public void WriteBytes(byte[] Value)
        {
            this.DataStream.Write(BitConverter.GetBytes(Value.Length), 0, 4);
            this.DataStream.Write(Value, 0, Value.Length);
        }
        public void WriteBytes(byte[] Value, int Length)
        {
            this.DataStream.Write(BitConverter.GetBytes(Length), 0, 4);
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
            this.DataStream.Write(BitConverter.GetBytes(Math.Min(StringData.Length, Length * 2)), 0, 2);
            this.DataStream.Write(StringData, 0, Math.Min(StringData.Length, Length * 2));
        }
        public void WriteDateTime(DateTime DateTime)
        {
            this.DataStream.Write(BitConverter.GetBytes(DateTime.Ticks), 0, 4);
        }
        public virtual byte[] GetPacketData()
        {
            using (MemoryStream OutputStream = new MemoryStream())
            {
                OutputStream.Write(this.DataStream.ToArray(), 0, (int)this.DataStream.Length);
                return OutputStream.ToArray();
            }
        }
        public void Dispose() => this.DataStream.Dispose();
    }
}
